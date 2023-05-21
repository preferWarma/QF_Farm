using System;
using System.Linq;
using Game.ChallengeSystem;
using Game.Plants;
using Game.UI;
using QFramework;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Game
{
    public partial class GameController : ViewController, IController
    {
        private void Awake()
        {
            FindObjectOfType<UIToolBar>();
        }

        private void Start()
        {
            InitValueOnStart();
            
            // 注册相关事件
            RegisterOnToolChange();
            RegisterOnDaysChange();
            RegisterOnPlantHarvest();
            RegisterOnItemCountChange();
        }

        private void Update()
        {
            UpdateChallenge();
        }

        private void UpdateChallenge()
        {
            var hasFinishedChallenge = false;

            // 检查激活列表中是否有挑战完成或开始
            foreach (var challenge in ChallengeController.ActiveChallenges)
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
                ChallengeController.ActiveChallenges.RemoveAll(challenge => challenge.State == Challenge.States.Finished);
            }

            if (ChallengeController.ActiveChallenges.Count == 0 && ChallengeController.FinishedChallenges.Count != ChallengeController.Challenges.Count)
            {
                var randomItem = ChallengeController.Challenges.Where(challenge1 => challenge1.State == Challenge.States.NotStart)
                    .ToList().GetRandomItem();
                ChallengeController.ActiveChallenges.Add(randomItem); // 完成挑战时再随机添加一个未开始的挑战
            }
        }
        
        #region 监听注册的相关函数
        
        // 监听工具切换
        private void RegisterOnToolChange()
        {
            Global.CurrentTool.Register(_ => { AudioController.Instance.Sfx_SwitchTool.Play(); })
                .UnRegisterWhenGameObjectDestroyed(this);
        }

        // 监听天数变化
        private void RegisterOnDaysChange()
        {
            // 土地浇水相关
            Global.Days.Register(_ =>
            {
                AudioController.Instance.Sfx_NextDay.Play(); // 播放下一天音效
                
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
            
            // 每日剩余时间相关
            Global.Days.Register(_ =>
            {
                Global.RestHours.Value = Random.Range(8, 12 + 1);
            }).UnRegisterWhenGameObjectDestroyed(this);
        }
        
        // 监听植物采摘
        private void RegisterOnPlantHarvest()
        {
            // 监听植物采摘
            Global.OnPlantHarvest.Register(plant =>
            {
                ChallengeController.HarvestCountInCurrentDay.Value++;
                ChallengeController.TotalFruitCount.Value++;

                // 根据植物类型增加不同的水果数量
                if (plant is PlantRadish)
                {
                    ChallengeController.HarvestRadishCountInCurrentDay.Value++;
                    ChallengeController.TotalRadishCount.Value++;
                    this.SendCommand(new AddItemCountCommand(ItemNameCollections.Radish, 1));
                }
                else if (plant is PlantPumpkin)
                {
                    ChallengeController.TotalPumpkinCount.Value++;
                    this.SendCommand(new AddItemCountCommand(ItemNameCollections.Pumpkin, 1));
                }
                else if (plant is PlantPotato)
                {
                    ChallengeController.HarvestPotatoCountInCurrentDay.Value++;
                    this.SendCommand(new AddItemCountCommand(ItemNameCollections.Potato, 1));
                }
                else if (plant is PlantTomato)
                {
                    ChallengeController.HarvestTomatoInCurrentDay.Value++;
                    this.SendCommand(new AddItemCountCommand(ItemNameCollections.Tomato, 1));
                }
                else if (plant is PlantBean)
                {
                    ChallengeController.HarvestBeanInCurrentDay.Value++;
                    this.SendCommand(new AddItemCountCommand(ItemNameCollections.Bean, 1));
                }

                if (plant.RipeDay == Global.Days.Value) // 如果是当天成熟的植物被采摘
                {
                    ChallengeController.RipeAndHarvestCountInCurrentDay.Value++;
                }
            }).UnRegisterWhenGameObjectDestroyed(this);
        }
        
        // 监听物品栏数量变化
        private void RegisterOnItemCountChange()
        {
            ToolBarSystem.OnItemCountChange.Register((item, count) =>
            {
                if (item.name == ItemNameCollections.Pumpkin)
                {
                    Global.PumpkinCount.Value = count;
                }
                else if (item.name == ItemNameCollections.Radish)
                {
                    Global.RadishCount.Value = count;
                }
                else if (item.name == ItemNameCollections.Potato)
                {
                    Global.PotatoCount.Value = count;
                }
                else if (item.name == ItemNameCollections.Tomato)
                {
                    Global.TomatoCount.Value = count;
                }
                else if (item.name == ItemNameCollections.Bean)
                {
                    Global.BeanCount.Value = count;
                }
            }).UnRegisterWhenGameObjectDestroyed(this);
        }

        #endregion

        private void InitValueOnStart()
        {
            Global.Days.Value = 1;  // 开局第一天
        }

        public IArchitecture GetArchitecture()
        {
            return Global.Interface;
        }
    }
}