using System.Collections.Generic;
using System.SoilSys;
using Game;
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
        public static Dictionary<string, bool> IsPowerUpUnlocked = new() // 强化项是否解锁
        {
            {ItemNameCollections.WateringCan, false},
            {ItemNameCollections.Shovel, false},
            {ItemNameCollections.Hand, false},
            {ItemNameCollections.Seed, false},
        };
        [Tooltip("今天是否已经强化过, 控制强化流程，每天只能强化一次")]
        public static readonly BindableProperty<bool> IntensifiedToday = new();
        public List<IPowerUp> PowerUps { get; set; } = new();
        
        protected override void OnInit()
        {
            ToolPowerUp();
            SoilPowerUp(5, 200, 5);
            SoilPowerUp(6,500,10);
            SoilPowerUp(7,1000,20);
            SoilPowerUp(8,2000,40);
            
            if (!_hasRegistered)
            {
                SaveManager.Instance.Register(this, SaveType.Json);
                _hasRegistered = true;
            }
        }

        // 做一个简单封装,使得可以链式调用
        private IPowerUp Add(IPowerUp up)
        {
            PowerUps.Add(up);
            return up;
        }
        
        // 工具升级
        private void ToolPowerUp()
        {
            // 花洒强化
            Add(new PowerUp().WithKey("1")
                .WithPrice(10)
                .WithTitle("花洒强化")
                .WithDescription("花洒使用范围+1")
                .SetOnUnlock(up =>
                {
                    IsPowerUpUnlocked[ItemNameCollections.WateringCan] = true;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                })
                .SetCondition(up => !up.UnLocked && Global.Money.Value >= up.Price)
            );
            
            // 铲子强化
            Add(new PowerUp().WithKey("2")
                .WithPrice(20)
                .WithTitle("锄头强化")
                .WithDescription("锄头使用范围+1")
                .SetOnUnlock(up =>
                {
                    IsPowerUpUnlocked[ItemNameCollections.Shovel] = true;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                })
                .SetCondition(up => !up.UnLocked && Global.Money.Value >= up.Price)
            );
            
            // 手强化
            Add(new PowerUp().WithKey("3")
                .WithPrice(30)
                .WithTitle("手强化")
                .WithDescription("手使用范围+1")
                .SetOnUnlock(up =>
                {
                    IsPowerUpUnlocked[ItemNameCollections.Hand] = true;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                })
                .SetCondition(up => !up.UnLocked && Global.Money.Value >= up.Price)
            );
            
            // 种子强化
            Add(new PowerUp().WithKey("4")
                .WithPrice(40)
                .WithTitle("种子强化")
                .WithDescription("种子使用范围+1")
                .SetOnUnlock(up =>
                {
                    IsPowerUpUnlocked[ItemNameCollections.Seed] = true;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                })
                .SetCondition(up => !up.UnLocked && Global.Money.Value >= up.Price)
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
                })
                .SetCondition(up => !up.UnLocked && Global.Money.Value >= up.Price)
            );
        }

        #region 存储相关
        
        private static bool _hasRegistered;
        
        public string SAVE_FILE_NAME => "PowerUp";
        
        private class SaveDataCollection
        {
            public Dictionary<string, bool> IsPowerUpUnlocked;
        }
        
        public void SaveWithJson()
        {
            var saveData = new SaveDataCollection
            {
                IsPowerUpUnlocked = IsPowerUpUnlocked
            };
            SaveManager.SaveWithJson(SAVE_FILE_NAME, saveData);
        }

        public void LoadWithJson()
        {
            var saveData = SaveManager.LoadWithJson<SaveDataCollection>(SAVE_FILE_NAME);
            if (saveData == null) return;
            IsPowerUpUnlocked = saveData.IsPowerUpUnlocked;
        }

        public void ResetDefaultData()
        {
            IsPowerUpUnlocked = new Dictionary<string, bool>
            {
                {ItemNameCollections.WateringCan, false},
                {ItemNameCollections.Shovel, false},
                {ItemNameCollections.Hand, false},
                {ItemNameCollections.Seed, false},
            };
        }
        
        #endregion
    }
    
}