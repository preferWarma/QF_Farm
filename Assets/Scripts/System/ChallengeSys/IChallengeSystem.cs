using System.Collections.Generic;
using System.Linq;
using Game;
using Game.UI;
using Lyf.SaveSystem;
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System.ChallengeSys
{
    public interface IChallengeSystem : ISystem, ISaveWithJson
    {
	    List<Challenge> Challenges { get; }
	    List<Challenge> ActiveChallenges { get; }
	    List<Challenge> FinishedChallenges { get; }
	    void ResetDefaultData();
    }

    public class ChallengeSystem : AbstractSystem, IChallengeSystem
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
        
        [Tooltip("挑战相关")]
        public List<Challenge> Challenges { get; } = new(); // 挑战列表
        public List<Challenge> ActiveChallenges { get; } = new(); // 激活的挑战列表
        public List<Challenge> FinishedChallenges { get; } = new(); // 完成的挑战列表
        
        protected override void OnInit()
        {
	        SaveManager.Instance.Register(this, SaveType.Json);
	        
            InitChallengeList();
            RegisterOnDaysChange();
            RegisterOnChallengeFinish();
            
            ActionKit.OnUpdate.Register(UpdateChallenge);	// 添加到Update中,每帧更新挑战
        }

        // 初始化挑战列表
        private void InitChallengeList()
        {
	        Challenges.Clear();
	        ActiveChallenges.Clear();
	        FinishedChallenges.Clear();
	        
	        #region 挑战列表添加
	        
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
			
			#endregion
        }
        
        // 挑战的更新
        private void UpdateChallenge()
        {
	        var hasFinishedChallenge = false;

	        // 检查激活列表中是否有挑战完成或开始
	        foreach (var challenge in ActiveChallenges)
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
				        Global.OnChallengeFinish.Trigger(challenge); // 触发挑战完成事件
				        hasFinishedChallenge = true;
			        }
		        }
	        }

	        if (hasFinishedChallenge)
	        {
		        ActiveChallenges.RemoveAll(challenge => challenge.State == Challenge.States.Finished);
	        }

	        if (ActiveChallenges.Count == 0 && FinishedChallenges.Count != Challenges.Count)
	        {
		        var randomItem = Challenges.Where(challenge1 => challenge1.State == Challenge.States.NotStart)
			        .ToList().GetRandomItem();
		        ActiveChallenges.Add(randomItem); // 完成挑战时再随机添加一个未开始的挑战
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
				        .StartGlobal();
		        }
	        });
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
	        });
        }
		
        #endregion
        
        #region 存储相关
        
        [Serializable]
        private class SaveDataCollection
        {
	        public Dictionary<string, Challenge.States> ChallengeStates;
	        public int TotalFruitCount;
	        public int TotalPumpkinCount;
	        public int TotalRadishCount;
        }
        
        public string SAVE_FILE_NAME => "Challenge";
        public void SaveWithJson()
        {
	        var saveData = new SaveDataCollection
	        {
		        ChallengeStates = new Dictionary<string, Challenge.States>(),
		        TotalFruitCount =  TotalFruitCount.Value,
		        TotalPumpkinCount = TotalPumpkinCount.Value,
		        TotalRadishCount = TotalRadishCount.Value,
		        
	        };
	        foreach (var challenge in Challenges)
	        {
		        saveData.ChallengeStates.Add(challenge.Name, challenge.State);
	        }
	        SaveManager.SaveWithJson(SAVE_FILE_NAME, saveData);
        }

        public void LoadWithJson()
        {
	        var saveData = SaveManager.LoadWithJson<SaveDataCollection>(SAVE_FILE_NAME);
	        if (saveData == null)
	        {
		        Debug.LogError("加载挑战数据失败");
		        return;
	        }
	        TotalFruitCount.SetValueWithoutEvent(TotalFruitCount);
	        TotalPumpkinCount.SetValueWithoutEvent(TotalPumpkinCount);
	        TotalRadishCount.SetValueWithoutEvent(TotalRadishCount);
	        
	        foreach (var challenge in Challenges)
	        {
		        if (saveData.ChallengeStates.TryGetValue(challenge.Name, out var state))
		        {
			        challenge.State = state;
			        if (state == Challenge.States.Finished)
			        {
				        if (!FinishedChallenges.Contains(challenge))
							FinishedChallenges.Add(challenge);
			        }
			        else if (state == Challenge.States.Doing)
			        {
				        if (!ActiveChallenges.Contains(challenge))
							ActiveChallenges.Add(challenge);
			        }
		        }
	        }
        }

        public void ResetDefaultData()
        {
	        InitChallengeList();
	        TotalFruitCount.SetValueWithoutEvent(0);
	        TotalPumpkinCount.SetValueWithoutEvent(0);
	        TotalRadishCount.SetValueWithoutEvent(0);
        }

        #endregion
    }
}