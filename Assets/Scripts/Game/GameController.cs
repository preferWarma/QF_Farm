using System.Linq;
using Game.ChallengeSystem;
using Game.Plants;
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

            RegisterOnChallengeFinish();
            RegisterOnPlantHarvest();
            RegisterOnToolChange();
            RegisterOnDaysChange();
        }

        private void Update()
        {
            UpdateChallenge();
        }

        private void UpdateChallenge()
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
                        Global.OnChallengeFinish.Trigger(challenge); // 触发挑战完成事件
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
                Global.ActiveChallenges.Add(randomItem); // 完成挑战时再随机添加一个未开始的挑战
            }
        }
        
        #region 监听注册的相关函数

        private void RegisterOnChallengeFinish()
        {
            // 监听挑战完成
            Global.OnChallengeFinish.Register(challenge =>
            {
                AudioController.Instance.Sfx_Complete.Play(); // 播放挑战完成音效
                Global.FinishedChallenges.Add(challenge);
                Debug.Log($"完成挑战:{challenge.Name}");

                if (Global.Challenges.Count == Global.FinishedChallenges.Count) // 如果所有的挑战都完成了
                {
                    ActionKit.Delay(1.0f, () => SceneManager.LoadScene("Scenes/GamePass"))
                        .Start(this);
                }
            }).UnRegisterWhenGameObjectDestroyed(this);
        }

        private void RegisterOnPlantHarvest()
        {
            // 监听植物采摘
            Global.OnPlantHarvest.Register(plant =>
            {
                Global.HarvestCountInCurrentDay.Value++;

                // 根据植物类型增加不同的水果数量
                if (plant as PlantRadish != null)
                {
                    Global.RadishCount.Value++;
                    Global.HarvestRadishCountInCurrentDay.Value++;
                }
                else if (plant as PlantPumpkin != null)
                {
                    Global.PumpkinCount.Value++;
                }

                if (plant.RipeDay == Global.Days.Value) // 如果是当天成熟的植物被采摘
                {
                    Global.RipeAndHarvestCountInCurrentDay.Value++;
                }
            }).UnRegisterWhenGameObjectDestroyed(this);
        }

        private void RegisterOnToolChange()
        {
            // 监听工具切换
            Global.CurrentTool.Register(_ => { AudioController.Instance.Sfx_SwitchTool.Play(); })
                .UnRegisterWhenGameObjectDestroyed(this);
        }

        private void RegisterOnDaysChange()
        {
            // 监听天数变化
            Global.Days.Register(_ =>
            {
                AudioController.Instance.Sfx_NextDay.Play(); // 播放下一天音效

                Global.RipeAndHarvestCountInCurrentDay.Value = 0; // 每天开始时，重置当天成熟且采摘的水果数量
                Global.HarvestCountInCurrentDay.Value = 0; // 每天开始时，重置当天采摘的水果数量
                Global.HarvestRadishCountInCurrentDay.Value = 0; // 每天开始时，重置当天采摘的萝卜数量

                var soilDatas = FindObjectOfType<GridController>().ShowGrid;

                PlantController.Instance.PlantGrid.ForEach((x, y, plant) =>
                {
                    if (plant is null) return;
                    plant.Grow(soilDatas[x, y]);
                });

                soilDatas.ForEach(data =>
                {
                    if (data is null) return;
                    data.Watered = false; // 过了一天，所有的土地都没有水
                });

                var waters = SceneManager.GetActiveScene().GetRootGameObjects()
                    .Where(obj => obj.name.StartsWith("Water"));
                waters.ForEach(water => { water.DestroySelf(); }); // 过了一天，所有的水都消失了
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        #endregion
    }
}