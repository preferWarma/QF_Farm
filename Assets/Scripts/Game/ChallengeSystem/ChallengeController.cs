 using System.Collections.Generic;
 using Game.Plants;
 using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.ChallengeSystem
{
	public partial class ChallengeController : ViewController
	{
		[Header("挑战要求相关")]
		[Header("和当日有关")]
		public static readonly BindableProperty<int> RipeAndHarvestCountInCurrentDay = new(); // 当天成熟并采摘的植物数量
		public static readonly BindableProperty<int> HarvestCountInCurrentDay = new(); // 当天采摘的植物数量
		public static readonly BindableProperty<int> HarvestRadishCountInCurrentDay = new(); // 当天采摘的萝卜数量

		[Header("和累计有关")]
		public static readonly BindableProperty<int> TotalPumpkinCount = new(); // 累计采摘的南瓜数量
		public static readonly BindableProperty<int> TotalRadishCount = new(); // 累计采摘的胡萝卜数量

		[Header("挑战列表")]
		public static readonly List<Challenge> Challenges = new()
		{
			new ChallengeHarvestOneFruit(),   // 收获第一个果实挑战
			new RipeAndHarvestTwoInOneDay(), // 一天成熟并收获两个果实挑战
			new RipeAndHarvestFiveInOneDay(), // 一天成熟并收获五个果实挑战
			new HarvestOneRadish(), // 收获一个萝卜挑战
			new HarvestTenFruitsTotal(), // 累计收获10个果实挑战
			new HasTenFruitsCurrently(), // 当前拥有十个以上的果实挑战
		}; // 挑战列表
		public static readonly List<Challenge> ActiveChallenges = new(); // 激活的挑战列表
		public static readonly List<Challenge> FinishedChallenges = new(); // 完成的挑战列表
		
		private void Start()
		{
			InitValueOnStart();	// 初始化挑战相关的值
			
			// 挑战相关的事件注册
			RegisterOnChallengeFinish();

			// 开局随机添加一个挑战
			var randomItem = Challenges.GetRandomItem();
			ActiveChallenges.Add(randomItem);
		}

		private void OnGUI()
		{
			IMGUIHelper.SetDesignResolution(640, 480);
			GUI.Label(new Rect(640 - 200, 0, 200, 24), "挑战列表");
			for (var i = 0; i < ActiveChallenges.Count; i++)
			{
				var change = ActiveChallenges[i];
				GUI.Label(new Rect(640 - 200, 20 + i * 20, 200, 24), change.Name + ": " + change.State);
			}

			for (var i = 0; i < FinishedChallenges.Count; i++)
			{
				var change = FinishedChallenges[i];
				GUI.Label(new Rect(640 - 200, 20 + (i + ActiveChallenges.Count) * 20, 200, 24),
					"<color=green>" + change.Name + ": " + change.State + "</color>");
			}
		}
		
		#region 挑战相关的事件注册
		
		// 监听挑战完成
		private void RegisterOnChallengeFinish()
		{
			// 监听挑战完成
			Global.OnChallengeFinish.Register(challenge =>
			{
				AudioController.Instance.Sfx_Complete.Play(); // 播放挑战完成音效
				FinishedChallenges.Add(challenge);
				Debug.Log($"完成挑战:{challenge.Name}");

				if (Challenges.Count == FinishedChallenges.Count) // 如果所有的挑战都完成了
				{
					ActionKit.Delay(1.0f, () => SceneManager.LoadScene("Scenes/GamePass"))
						.Start(this);
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
		}
		
		#endregion

		private void InitValueOnStart()
		{
			RipeAndHarvestCountInCurrentDay.Value = 0;	// 重置当天成熟并采摘的植物数量
			HarvestCountInCurrentDay.Value = 0;	// 重置当天采摘的植物数量
			HarvestRadishCountInCurrentDay.Value = 0;	// 重置当天采摘的萝卜数量
			TotalPumpkinCount.Value = 0;	// 重置累计采摘的南瓜数量
			TotalRadishCount.Value = 0;	// 重置累计采摘的胡萝卜数量
			
			ActiveChallenges.Clear(); // 清空激活的挑战列表
			FinishedChallenges.Clear(); // 清空完成的挑战列表
			
			// 开局时所有的挑战都是未开始的
			foreach (var challenge in Challenges)
			{
				challenge.State = Challenge.States.NotStart;
			}
		}
	}
}
