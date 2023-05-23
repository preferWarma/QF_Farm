 using System.Collections.Generic;
 using Game.UI;
 using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.ChallengeSystem
{
	public partial class ChallengeController : ViewController
	{
		[Tooltip("挑战要求相关")]
		[Header("和当日有关")]
		public static readonly BindableProperty<int> RipeAndHarvestCountInCurrentDay = new(); // 当天成熟并采摘的植物数量
		public static readonly BindableProperty<int> HarvestCountInCurrentDay = new(); // 当天采摘的植物数量
		public static readonly BindableProperty<int> HarvestRadishCountInCurrentDay = new(); // 当天采摘的萝卜数量
		public static readonly BindableProperty<int> HarvestPotatoCountInCurrentDay = new(); // 当天采摘的土豆数量
		public static readonly BindableProperty<int> HarvestTomatoInCurrentDay = new(); // 当天采摘的番茄数量
		public static readonly BindableProperty<int> HarvestBeanInCurrentDay = new(); // 当天采摘的豆角数量

		[Header("和累计有关")]
		public static readonly BindableProperty<int> TotalFruitCount = new(); // 累计采摘的果实数量
		public static readonly BindableProperty<int> TotalPumpkinCount = new(); // 累计采摘的南瓜数量
		public static readonly BindableProperty<int> TotalRadishCount = new(); // 累计采摘的胡萝卜数量

		[Header("挑战列表")]
		public static readonly List<Challenge> Challenges = new(); // 挑战列表
		public static readonly List<Challenge> ActiveChallenges = new(); // 激活的挑战列表
		public static readonly List<Challenge> FinishedChallenges = new(); // 完成的挑战列表

		private void Awake()
		{
			Challenges.Add(new GenericChallenge().SetName("收获第一个果实").OnStart(challenge =>
			{
				challenge.StartDate = Global.Days.Value;
			}).CheckFinish(challenge => challenge.StartDate != Global.Days.Value && TotalFruitCount.Value >= 1));
			
			Challenges.Add(new GenericChallenge().SetName("一天成熟并收获两个果实").OnStart(challenge =>
			{
				challenge.StartDate = Global.Days.Value;
			}).CheckFinish(challenge => challenge.StartDate != Global.Days.Value && RipeAndHarvestCountInCurrentDay.Value >= 2));
			
			Challenges.Add(new GenericChallenge().SetName("一天成熟并收获五个果实").OnStart(challenge =>
			{
				challenge.StartDate = Global.Days.Value;
			}).CheckFinish(challenge => challenge.StartDate != Global.Days.Value && RipeAndHarvestCountInCurrentDay.Value >= 5));
			
			Challenges.Add(new GenericChallenge().SetName("收获一个萝卜").OnStart(challenge =>
			{
				challenge.StartDate = Global.Days.Value;
			}).CheckFinish(challenge => challenge.StartDate != Global.Days.Value && HarvestRadishCountInCurrentDay.Value >= 1));
			
			Challenges.Add(new GenericChallenge().SetName("累计收获10个果实").OnStart(challenge =>
			{
				challenge.StartDate = Global.Days.Value;
			}).CheckFinish(challenge => challenge.StartDate != Global.Days.Value && TotalFruitCount.Value >= 10));
			
			Challenges.Add(new GenericChallenge().SetName("当前拥有十个以上的果实").OnStart(challenge =>
			{
				challenge.StartDate = Global.Days.Value;
			}).CheckFinish(challenge => challenge.StartDate != Global.Days.Value && TotalFruitCount.Value >= 10));
			
			Challenges.Add(new GenericChallenge().SetName("收获一个土豆").OnStart(challenge =>
			{
				challenge.StartDate = Global.Days.Value;
			}).CheckFinish(challenge => challenge.StartDate != Global.Days.Value && HarvestPotatoCountInCurrentDay.Value >= 1));
			
			Challenges.Add(new GenericChallenge().SetName("当前拥有100金币").OnStart(challenge =>
			{
				challenge.StartDate = Global.Days.Value;
			}).CheckFinish(challenge => challenge.StartDate != Global.Days.Value && Global.Money.Value >= 100));
			
			Challenges.Add(new GenericChallenge().SetName("采摘一个番茄").OnStart(challenge =>
			{
				challenge.StartDate = Global.Days.Value;
			}).CheckFinish(challenge => challenge.StartDate != Global.Days.Value && HarvestTomatoInCurrentDay.Value >= 1));
			
			Challenges.Add(new GenericChallenge().SetName("采摘一个豆荚").OnStart(challenge =>
			{
				challenge.StartDate = Global.Days.Value;
			}).CheckFinish(challenge => challenge.StartDate != Global.Days.Value && HarvestBeanInCurrentDay.Value >= 1));
		}

		private void Start()
		{
			InitValueOnStart();	// 初始化挑战相关的值
			
			// 挑战相关的事件注册
			RegisterOnChallengeFinish();
			RegisterOnDaysChange();

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
			Global.OnChallengeFinish.Register(challenge =>
			{
				AudioController.Instance.Sfx_Complete.Play(); // 播放挑战完成音效
				FinishedChallenges.Add(challenge);
				UIMessageQueue.Push(null, $"完成挑战:{challenge.Name}, <color=yellow>金币+100</color>");

				if (Challenges.Count == FinishedChallenges.Count) // 如果所有的挑战都完成了
				{
					ActionKit.Delay(1.0f, () => SceneManager.LoadScene("Scenes/GamePass"))
						.Start(this);
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
		}

		// 监听天数变化
		private void RegisterOnDaysChange()
		{
			Global.Days.Register(_ =>
			{
				RipeAndHarvestCountInCurrentDay.Value = 0; // 每天开始时，重置当天成熟且采摘的水果数量
				HarvestCountInCurrentDay.Value = 0; // 每天开始时，重置当天采摘的水果数量
				HarvestRadishCountInCurrentDay.Value = 0; // 每天开始时，重置当天采摘的萝卜数量
				HarvestPotatoCountInCurrentDay.Value = 0; // 每天开始时，重置当天采摘的土豆数量
				HarvestTomatoInCurrentDay.Value = 0; // 每天开始时，重置当天采摘的番茄数量
				HarvestBeanInCurrentDay.Value = 0; // 每天开始时，重置当天采摘的豆荚数量
			}).UnRegisterWhenGameObjectDestroyed(this);
		}
		
		#endregion

		private void InitValueOnStart()
		{
			RipeAndHarvestCountInCurrentDay.Value = 0;	// 重置当天成熟并采摘的植物数量
			HarvestCountInCurrentDay.Value = 0;	// 重置当天采摘的植物数量
			HarvestRadishCountInCurrentDay.Value = 0;	// 重置当天采摘的萝卜数量
			HarvestPotatoCountInCurrentDay.Value = 0;	// 重置当天采摘的土豆数量
			HarvestTomatoInCurrentDay.Value = 0;	// 重置当天采摘的番茄数量
			HarvestBeanInCurrentDay.Value = 0;	// 重置当天采摘的豆荚数量
			
			TotalPumpkinCount.Value = 0;	// 重置累计采摘的南瓜数量
			TotalRadishCount.Value = 0;	// 重置累计采摘的胡萝卜数量
			TotalFruitCount.Value = 0;	// 重置累计采摘的土豆数量
			
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
