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
				AudioController.Instance.Sfx_Complete.Play();	// 播放挑战完成音效
				Global.FinishedChallenges.Add(challenge);
				Debug.Log($"完成挑战:{challenge.Name}");
				
				if (Global.Challenges.All(challenge1 => challenge1.State == Challenge.States.Finished))	// 如果所有的挑战都完成了
				{
					ActionKit.Delay(1.0f, () => SceneManager.LoadScene("Scenes/GamePass"))
						.Start(this);
				}
			});
			
			// 监听成熟的植物是否当天被采摘
			Global.OnPlantHarvest.Register(plant =>
			{
				Global.HarvestCountInCurrentDay.Value++;
				if (plant.ripeDay == Global.Days.Value)
				{
					Global.RipeAndHarvestCountInCurrentDay.Value++;
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
			
			// 监听工具切换
			Global.CurrentTool.Register(_ =>
			{
				AudioController.Instance.Sfx_SwitchTool.Play();
			});
		}

		private void Update()
		{
			var hasFinishedChallenge = false;
			
			// 检查激活列表中是否有挑战完成或开始
			foreach (var challenge in Global.ActiveChallenges)
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
						hasFinishedChallenge = true;
					}
				}
			}

			if (hasFinishedChallenge)
			{
				Global.ActiveChallenges.RemoveAll(challenge => challenge.State == Challenge.States.Finished);
			}
			
			if (Global.ActiveChallenges.Count == 0 && Global.FinishedChallenges.Count != Global.Challenges.Count)
			{
				var randomItem = Global.Challenges.Where(challenge1 => challenge1.State == Challenge.States.NotStart)
					.ToList().GetRandomItem();
				Global.ActiveChallenges.Add(randomItem);	// 完成挑战时再随机添加一个未开始的挑战
			}
		}
	}
}
