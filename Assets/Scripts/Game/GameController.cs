using System;
using System.Linq;
using Game.ChallengeSystem;
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
	public partial class GameController : ViewController
	{
		private void Start()
		{
			// 开局随机添加一个挑战
			var randomItem = Global.Challenges.GetRandomItem();
			Global.ActiveChallenges.Add(randomItem);
			
			Global.OnChallengeFinish.Register(challenge =>
			{
				Global.ActiveChallenges.Remove(challenge);
				Global.FinishedChallenges.Add(challenge);
				Debug.Log($"完成挑战:{challenge.Name}");
				
				var randomItem1 = Global.Challenges.Where(challenge1 => challenge1.State == Challenge.States.Doing)
					.ToList().GetRandomItem();
				Global.ActiveChallenges.Add(randomItem1);	// 完成挑战时再随机添加一个未完成的挑战
				
				if (Global.Challenges.All(challenge1 => challenge1.State == Challenge.States.Finished))
				{
					ActionKit.Delay(1.0f, () => SceneManager.LoadScene("Scenes/GamePass"))
						.Start(this);
				}
			});
			
			// 监听成熟的植物是否当天被采摘
			Global.OnPlantHarvest.Register(plant =>
			{
				if (plant.ripeDay == Global.Days.Value)
				{
					Global.RipeAndHarvestCountInCurrentDay.Value++;
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
		}

		private void Update()
		{
			// 检查是否有挑战完成或开始
			foreach (var challenge in Global.Challenges.Where(challenge => challenge.State != Challenge.States.Finished))
			{
				if (challenge.State == Challenge.States.NotStart)
				{
					challenge.OnStart();
					challenge.State = Challenge.States.Doing;
				}
				
				else if (challenge.State == Challenge.States.Doing)
				{
					if (challenge.CheckFinish())
					{
						challenge.OnFinish();
						challenge.State = Challenge.States.Finished;
						Global.OnChallengeFinish.Trigger(challenge);	// 触发挑战完成事件
					}
				}
			}
		}
	}
}
