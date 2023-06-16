using System.ChallengeSys;
using System.Linq;
using System.PowerUpSys;
using System.SoilSys;
using System.ToolBarSys;
using Game.UI;
using Lyf.SaveSystem;
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Game
{
    public partial class GameController : ViewController, IController
    {
        private void Start()
        {
            // 注册相关事件
            RegisterOnToolChange();
            RegisterOnDaysChange();
            RegisterOnPlantHarvest();
            RegisterOnItemCountChange();
        }

        private void Update()
        {
            if(Global.Money.Value < 0) // 如果钱不够了，游戏结束
            {
                ActionKit.Delay(0.5f, () => SceneManager.LoadScene("Scenes/GameOver"))
                    .Start(this);
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
                var soilGrid = this.GetSystem<ISoilSystem>().SoilGrid;

                PlantController.Instance.PlantGrid.ForEach((x, y, plant) =>
                {
                    if (plant is null) return;
                    plant.Grow(soilGrid[x, y]);
                });

                soilGrid.ForEach((x,y,data) =>
                {
                    if (data is null) return;
                    data.Watered = false; // 过了一天，所有的土地都没有水
                    Global.GridController.Watering.SetTile(new Vector3Int(x, y), null); // 同步清空水的tilemap
                });
                
                Global.WaterRoot.transform.DestroyChildren();

                }).UnRegisterWhenGameObjectDestroyed(gameObject);
            
            // 强化相关
            Global.Days.Register(_ =>
            {
                PowerUpSystem.IntensifiedToday.Value = false;
            }).UnRegisterWhenGameObjectDestroyed(this);
            
            // 每日剩余时间相关
            Global.Days.Register(_ =>
            {
                AudioController.Instance.Sfx_NextDay.Play(); // 播放下一天音效
                Global.RestHours.Value = Random.Range(8, 12 + 1);
            }).UnRegisterWhenGameObjectDestroyed(this);
        }
        
        // 监听植物采摘
        private void RegisterOnPlantHarvest()
        {
            // 监听植物采摘
            Global.OnPlantHarvest.Register(plant =>
            {
                ChallengeSystem.HarvestCountInCurrentDay.Value++;
                ChallengeSystem.TotalFruitCount.Value++;

                UIMessageQueue.Push(ResController.Instance.LoadSprite(plant.PlantName), "+1");

                switch (plant.PlantName)
                {
                    // 根据植物类型增加不同的水果数量, 以及完成对应的挑战
                    case ItemNameCollections.Radish:
                        ChallengeSystem.HarvestRadishCountInCurrentDay.Value++;
                        ChallengeSystem.TotalRadishCount.Value++;
                        this.SendCommand(new AddItemCountCommand(ItemNameCollections.Radish, plant.SoilData.RipeCount));
                        break;
                    case ItemNameCollections.Pumpkin:
                        ChallengeSystem.TotalPumpkinCount.Value++;
                        ChallengeSystem.HarvestPumpkinCountInCurrentDay.Value++;
                        this.SendCommand(new AddItemCountCommand(ItemNameCollections.Pumpkin, plant.SoilData.RipeCount));
                        break;
                    case ItemNameCollections.Potato:
                        ChallengeSystem.HarvestPotatoCountInCurrentDay.Value++;
                        ChallengeSystem.TotalPotatoCount.Value++;
                        this.SendCommand(new AddItemCountCommand(ItemNameCollections.Potato, plant.SoilData.RipeCount));
                        break;
                    case ItemNameCollections.Tomato:
                        ChallengeSystem.HarvestTomatoInCurrentDay.Value++;
                        ChallengeSystem.TotalTomatoCount.Value++; 
                        this.SendCommand(new AddItemCountCommand(ItemNameCollections.Tomato, plant.SoilData.RipeCount));
                        break;
                    case ItemNameCollections.Bean:
                        ChallengeSystem.HarvestBeanInCurrentDay.Value++;
                        ChallengeSystem.TotalBeanCount.Value++;
                        this.SendCommand(new AddItemCountCommand(ItemNameCollections.Bean, plant.SoilData.RipeCount));
                        break;
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
        
        public IArchitecture GetArchitecture()
        {
            return Global.Interface;
        }

        private void OnApplicationQuit() // 退出游戏时保存数据
        {
            SaveManager.Instance.SaveAllRegister(SaveType.Json);
        }
    }
}