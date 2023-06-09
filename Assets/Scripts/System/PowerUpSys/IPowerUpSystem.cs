﻿using System.Collections.Generic;
using System.SoilSys;
using Game;
using Game.UI;
using Lyf.SaveSystem;
using QFramework;
using UnityEngine;

namespace System.PowerUpSys
{
    public interface IPowerUpSystem : ISystem, ISaveWithJson
    {
        List<IPowerUp> PowerUps { get; }
        void ResetDefaultData();
    }
    
    public class PowerUpSystem : AbstractSystem, IPowerUpSystem
    {
        private static Dictionary<string, bool> _isPowerUpUnlocked = new() // 强化项是否解锁
        {
            {ItemNameCollections.Soil5X5, false},
            {ItemNameCollections.Soil6X6, false},
            {ItemNameCollections.Soil7X7, false},
            {ItemNameCollections.Soil8X8, false},
            {ItemNameCollections.Soil9X9, false},
            {ItemNameCollections.Soil10X10, false},
        };
        [Tooltip("今天是否已经强化过, 控制强化流程，每天只能强化一次")]
        public static readonly BindableProperty<bool> IntensifiedToday = new();

        public List<IPowerUp> PowerUps { get; } = new();
        
        protected override void OnInit()
        {
            // 添加强化项
            ToolRangeAndCostPowerUp();
            ToolCdPowerUp();
            HarvestPowerUp();
            SoilPowerUp(5, 500, 5);
            SoilPowerUp(6,1000,10);
            SoilPowerUp(7,1500,20);
            SoilPowerUp(8,3000,40);
            SoilPowerUp(9,5000, 80);
            SoilPowerUp(10,10000,100);

            SaveManager.Instance.Register(this, SaveType.Json);
        }
        
        // 工具范围升级
        private void ToolRangeAndCostPowerUp()
        {
            var item1 = Add(new PowerUp().WithKey("Tool_lv2")
                .WithPrice(1000)
                .WithTitle("工具lv2")
                .WithDescription("工具消耗减少,使用范围+1")
                .SetOnUnlock(up =>
                {
                    Global.ToolCostLevel = 2;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                    UIMessageQueue.Push("操作升级到lv2");
                })
                .SetObjShowCondition(_ => Global.ToolCostLevel == 1)
            );
            
            var item2 = Add(new PowerUp().WithKey("Tool_lv3")
                .WithPrice(2000)
                .WithTitle("工具lv3")
                .WithDescription("工具消耗大幅减少,使用范围+1")
                .SetOnUnlock(up =>
                {
                    Global.ToolCostLevel = 3;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                    UIMessageQueue.Push("操作升级到lv3");
                })
                .SetObjShowCondition(_ => Global.ToolCostLevel == 2)
            );

        }

        private void ToolCdPowerUp()
        {
            var item1 = Add(new PowerUp().WithKey("CD_lv2")
                .WithPrice(1000)
                .WithTitle("冷却lv2")
                .WithDescription("工具CD减少")
                .SetOnUnlock(up =>
                {
                    Global.ToolCdLevel = 2;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                    UIMessageQueue.Push("CD冷却升级到lv2");
                })
                .SetObjShowCondition(up => Global.ToolCdLevel == 1)
            );

            var item2 = Add(new PowerUp().WithKey("CD_lv3")
                .WithPrice(2000)
                .WithTitle("冷却lv3")
                .WithDescription("工具CD大幅减少")
                .SetOnUnlock(up =>
                {
                    Global.ToolCdLevel = 3;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                    UIMessageQueue.Push("CD冷却升级到lv3");
                })
                .SetObjShowCondition(up => Global.ToolCdLevel == 2)
            );
        }

        // 土地升级
        private void SoilPowerUp(int size, int powerUpCost, int addDailyCost)
        {
            Add(new PowerUp().WithKey($"soil{size}")
                .WithPrice(powerUpCost)
                .WithTitle($"土地{size}x{size}")
                .WithDescription($"土地扩展为{size}x{size}\n每日金币消耗+{addDailyCost}")
                .SetOnUnlock(up =>
                {
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                    Global.DailyCost += addDailyCost;
                    Global.Interface.GetSystem<ISoilSystem>().ResetSoil(size, size);
                    _isPowerUpUnlocked[ItemNameCollections.GetSoilNameBySize(size)] = true;
                })
                .SetObjShowCondition(up =>
                {
                    // 解锁小土地后才能解锁大土地
                    for (var i = 5; i < size; i++)
                    {
                        if (!_isPowerUpUnlocked[ItemNameCollections.GetSoilNameBySize(i)])
                            return false;
                    }

                    return !_isPowerUpUnlocked[ItemNameCollections.GetSoilNameBySize(size)];
                })
            );
        }

        // 植物收获升级
        private void HarvestPowerUp()
        {
            var item1 = Add(new PowerUp().WithKey("Harvest_lv2")
                .WithPrice(2000)
                .WithTitle("收获lv2")
                .WithDescription("每次收获果实额外+1")
                .SetOnUnlock(up =>
                {
                    Global.HarvestLevel = 2;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                    UIMessageQueue.Push("每次收获果实额外+1");
                })
                .SetObjShowCondition(up => Global.HarvestLevel == 1)
            );
            var item2 = Add(new PowerUp().WithKey("Harvest_lv3")
                .WithPrice(2000)
                .WithTitle("收获lv3")
                .WithDescription("每次收获果实额外+2")
                .SetOnUnlock(up =>
                {
                    Global.HarvestLevel = 3;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                    UIMessageQueue.Push("每次收获果实额外+2");
                })
                .SetObjShowCondition(up => Global.HarvestLevel == 2)
            );
        }
        
        // 简单链式封装
        private IPowerUp Add(IPowerUp up)
        {
            PowerUps.Add(up);
            return up;
        }
        

        #region 存储相关
        public string SAVE_FILE_NAME => "PowerUp";
        
        private class SaveDataCollection
        {
            public Dictionary<string, bool> IsPowerUpUnlocked;
        }
        
        public void SaveWithJson()
        {
            var saveData = new SaveDataCollection
            {
                IsPowerUpUnlocked = _isPowerUpUnlocked
            };
            SaveManager.SaveWithJson(SAVE_FILE_NAME, saveData);
        }

        public void LoadWithJson()
        {
            var saveData = SaveManager.LoadWithJson<SaveDataCollection>(SAVE_FILE_NAME);
            if (saveData == null) return;
            _isPowerUpUnlocked = saveData.IsPowerUpUnlocked;
        }

        public void ResetDefaultData()
        {
            _isPowerUpUnlocked = new Dictionary<string, bool>
            {
                {ItemNameCollections.Soil5X5, false},
                {ItemNameCollections.Soil6X6, false},
                {ItemNameCollections.Soil7X7, false},
                {ItemNameCollections.Soil8X8, false},
                {ItemNameCollections.Soil9X9, false},
                {ItemNameCollections.Soil10X10, false},
            };
        }
        
        #endregion
    }
    
}