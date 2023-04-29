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
			Global.OnChallengeFinish.Register(challenge =>Debug.Log($"完成挑战:{challenge.Name}"));
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
