using System.Collections.Generic;
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
        };
        [Tooltip("今天是否已经强化过, 控制强化流程，每天只能强化一次")]
        public static readonly BindableProperty<bool> IntensifiedToday = new();
        public List<IPowerUp> PowerUps { get; } = new();
        
        protected override void OnInit()
        {
            // 添加强化项
            ToolRangeAndCostPowerUp();
            ToolCdPowerUp();
            SoilPowerUp(5, 200, 5);
            SoilPowerUp(6,500,10);
            SoilPowerUp(7,1000,20);
            SoilPowerUp(8,2000,40);

            SaveManager.Instance.Register(this, SaveType.Json);
        }

        // 简单链式封装
        private IPowerUp Add(IPowerUp up)
        {
            PowerUps.Add(up);
            return up;
        }
        
        // 工具范围升级
        private void ToolRangeAndCostPowerUp()
        {
            Add(new PowerUp().WithKey("Tool_lv2")
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
                .SetCondition(up => Global.ToolCostLevel == 1 && Global.Money.Value >= up.Price)
            );
            
            Add(new PowerUp().WithKey("Tool_lv3")
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
                .SetCondition(up => Global.ToolCostLevel == 2 && Global.Money.Value >= up.Price)
            );

        }

        private void ToolCdPowerUp()
        {
            Add(new PowerUp().WithKey("CD_lv2")
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
                .SetCondition(up => Global.ToolCdLevel == 1 && Global.Money.Value >= up.Price)
            );

            Add(new PowerUp().WithKey("CD_lv3")
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
                .SetCondition(up => Global.ToolCdLevel == 2 && Global.Money.Value >= up.Price)
            );
        }

        // 土地升级
        private void SoilPowerUp(int size,int powerUpCost, int addDailyCost)
        {
            Add(new PowerUp().WithKey("soil")
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
                .SetCondition(up =>
                {
                    // 解锁小土地后才能解锁大土地
                    for (var i = 5; i < size; i++)
                    {
                        if (!_isPowerUpUnlocked[ItemNameCollections.GetSoilNameBySize(i)])
                            return false;
                    }

                    return !_isPowerUpUnlocked[ItemNameCollections.GetSoilNameBySize(size)] &&
                           Global.Money.Value >= up.Price;
                })
            );
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
            };
        }
        
        #endregion
    }
    
}