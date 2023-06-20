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
        [Tooltip("挑战要求相关")] [Header("和当日有关")]
        public static readonly BindableProperty<int> HarvestCountInCurrentDay = new(); // 当天采摘的植物数量
        public static readonly BindableProperty<int> HarvestPumpkinCountInCurrentDay = new(); // 当天采摘的果实数量
        public static readonly BindableProperty<int> HarvestRadishCountInCurrentDay = new(); // 当天采摘的萝卜数量
        public static readonly BindableProperty<int> HarvestPotatoCountInCurrentDay = new(); // 当天采摘的土豆数量
        public static readonly BindableProperty<int> HarvestTomatoInCurrentDay = new(); // 当天采摘的番茄数量
        public static readonly BindableProperty<int> HarvestBeanInCurrentDay = new(); // 当天采摘的豆角数量

        [Header("和累计有关")]
        public static readonly BindableProperty<int> TotalFruitCount = new(); // 累计采摘的果实数量
        public static readonly BindableProperty<int> TotalPumpkinCount = new();
        public static readonly BindableProperty<int> TotalRadishCount = new(); 
        public static readonly BindableProperty<int> TotalPotatoCount = new(); 
        public static readonly BindableProperty<int> TotalTomatoCount = new(); 
        public static readonly BindableProperty<int> TotalBeanCount = new();

        [Tooltip("挑战列表")] public List<Challenge> Challenges { get; } = new(); // 挑战列表
        public List<Challenge> ActiveChallenges { get; } = new(); // 激活的挑战列表
        public List<Challenge> FinishedChallenges { get; } = new(); // 完成的挑战列表

        protected override void OnInit()
        {
            SaveManager.Instance.Register(this, SaveType.Json);
            
            InitChallengeList();
            RegisterOnDaysChange();
            RegisterOnChallengeFinish();

            ActionKit.OnUpdate.Register(UpdateChallenge); // 添加到Update中,每帧更新挑战
        }

        // 初始化挑战列表
        private void InitChallengeList()
        {
            Challenges.Clear();
            ActiveChallenges.Clear();
            FinishedChallenges.Clear();
            
            // 添加挑战
            AddPumpkinChallenges();
            AddRadishChallenges();
            AddPotatoChallenges();
            AddTomatoChallenges();
            AddBeanChallenges();
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
                    .ToList()
                    .First();
                ActiveChallenges.Add(randomItem); // 完成挑战时再随机添加一个未开始的挑战
            }
        }

        #region 挑战添加列表

        private void AddPumpkinChallenges()
        {
            Challenges.Add(new GenericChallenge().SetName("采摘1个南瓜")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => HarvestPumpkinCountInCurrentDay.Value >= 1)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 100);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有10个南瓜")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ =>  Global.PumpkinCount.Value >= 10)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 100);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有20个南瓜")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.PumpkinCount.Value >= 20)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 100);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有30个南瓜")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.PumpkinCount.Value >= 30)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 100);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有50个南瓜")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.PumpkinCount.Value >= 50)
                .OnFinish(challenge =>
                {
                    UIShop.CanShowRadishSeed.Value = true;
                    ShowMessageWhenFinished(challenge.Name, 100);
                    UIMessageQueue.Push("已解锁<color=orange>胡萝卜种子</color>, 请前往商店查看");
                }));
        }

        private void AddRadishChallenges()
        {
            Challenges.Add(new GenericChallenge().SetName("采摘一个萝卜")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => HarvestRadishCountInCurrentDay.Value >= 1)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 100);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有10个萝卜")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.RadishCount.Value >= 10)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 100);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有20个萝卜")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.RadishCount.Value >= 20)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 100);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有30个萝卜")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.RadishCount.Value >= 30)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 100);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有50个萝卜")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.RadishCount.Value >= 50)
                .OnFinish(challenge =>
                {
                    UIShop.CanShowPotatoSeed.Value = true;
                    ShowMessageWhenFinished(challenge.Name, 100);
                    UIMessageQueue.Push("已解锁<color=orange>土豆种子</color>, 请前往商店查看");
                }));
            
        }
        
        private void AddPotatoChallenges()
        {
            Challenges.Add(new GenericChallenge().SetName("收获一个土豆")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => HarvestPotatoCountInCurrentDay.Value >= 1)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 150);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有10个土豆")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.PotatoCount.Value >= 10)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 150);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有20个土豆")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.PotatoCount.Value >= 20)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 150);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有30个土豆")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.PotatoCount.Value >= 30)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 150);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有50个土豆")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.PotatoCount.Value >= 50)
                .OnFinish(challenge =>
                {
                    UIShop.CanShowPotatoSeed.Value = true;
                    ShowMessageWhenFinished(challenge.Name, 150);
                    UIMessageQueue.Push("已解锁<color=red>番茄种子</color>, 请前往商店查看");
                }));
        }
        
        private void AddTomatoChallenges()
        {
            Challenges.Add(new GenericChallenge().SetName("采摘一个番茄")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => HarvestTomatoInCurrentDay.Value >= 1)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 150);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有10个番茄")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.TomatoCount.Value >= 10)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 150);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有20个番茄")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.TomatoCount.Value >= 20)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 150);
                }));
            
            Challenges.Add(new GenericChallenge().SetName("拥有30个番茄")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.TomatoCount.Value >= 30)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 150);
                }));
            
             Challenges.Add(new GenericChallenge().SetName("拥有50个番茄")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(_ => Global.TomatoCount.Value >= 50)
                .OnFinish(challenge =>
                {
                    UIShop.CanShowBeanSeed.Value = true;
                    ShowMessageWhenFinished(challenge.Name, 150);
                    UIMessageQueue.Push("已解锁<color=green>豆荚种子</color>, 请前往商店查看");
                })); 
        }
        
        private void AddBeanChallenges()
        {
            Challenges.Add(new GenericChallenge().SetName("采摘一个豆荚")
                .OnStart(challenge => { challenge.StartDate = Global.Days.Value; })
                .CheckFinish(challenge =>
                    challenge.StartDate != Global.Days.Value && HarvestBeanInCurrentDay.Value >= 1)
                .OnFinish(challenge =>
                {
                    ShowMessageWhenFinished(challenge.Name, 100);
                }));
        }

        private void ShowMessageWhenFinished(string name, int price)
        {
            Global.Money.Value += price;
            UIMessageQueue.Push($"完成挑战:{name}, <color=yellow>金币+{price}</color>");
        }

        #endregion

        #region 挑战相关的事件注册

        // 监听挑战完成
        private void RegisterOnChallengeFinish()
        {
            Global.OnChallengeFinish.Register(challenge =>
            {
                AudioController.Instance.Sfx_Complete.Play(); // 播放挑战完成音效
                FinishedChallenges.Add(challenge);

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
                HarvestCountInCurrentDay.Value = 0; // 每天开始时，重置当天采摘的水果数量
                HarvestRadishCountInCurrentDay.Value = 0; // 每天开始时，重置当天采摘的萝卜数量
                HarvestPotatoCountInCurrentDay.Value = 0; // 每天开始时，重置当天采摘的土豆数量
                HarvestTomatoInCurrentDay.Value = 0; // 每天开始时，重置当天采摘的番茄数量
                HarvestBeanInCurrentDay.Value = 0; // 每天开始时，重置当天采摘的豆荚数量
            });
        }

        #endregion

        #region 存储相关
        public string SAVE_FILE_NAME => "Challenge";

        private class SaveDataCollection
        {
            public Dictionary<string, Challenge.States> ChallengeStates;
            public int TotalFruitCount;
            public int TotalPumpkinCount;
            public int TotalRadishCount;
            public int TotalPotatoCount;
            public int TotalTomatoCount;
            public int TotalBeanCount;
        }
        
        public void SaveWithJson()
        {
            var saveData = new SaveDataCollection
            {
                ChallengeStates = new Dictionary<string, Challenge.States>(),
                TotalFruitCount = TotalFruitCount.Value,
                TotalPumpkinCount = TotalPumpkinCount.Value,
                TotalRadishCount = TotalRadishCount.Value,
                TotalPotatoCount = TotalPotatoCount.Value,
                TotalTomatoCount = TotalTomatoCount.Value,
                TotalBeanCount = TotalBeanCount.Value,
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

            TotalFruitCount.SetValueWithoutEvent(saveData.TotalFruitCount);
            TotalPumpkinCount.SetValueWithoutEvent(saveData.TotalPumpkinCount);
            TotalRadishCount.SetValueWithoutEvent(saveData.TotalRadishCount);
            TotalPotatoCount.SetValueWithoutEvent(saveData.TotalPotatoCount);
            TotalTomatoCount.SetValueWithoutEvent(saveData.TotalTomatoCount);
            TotalBeanCount.SetValueWithoutEvent(saveData.TotalBeanCount);

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
            TotalPotatoCount.SetValueWithoutEvent(0);
            TotalTomatoCount.SetValueWithoutEvent(0);
            TotalBeanCount.SetValueWithoutEvent(0);
        }

        #endregion
    }
}