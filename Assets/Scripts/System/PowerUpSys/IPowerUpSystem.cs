using System.Collections.Generic;
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
            SaveManager.Instance.Register(this, SaveType.Json);
            
            #region 强化项具体内容
            // 花洒强化
            PowerUps.Add(new PowerUp().WithKey("1")
                .WithPrice(10)
                .WithTitle("花洒范围+1")
                .WithDescription("强化($10)")
                .SetOnUnlock(up =>
                {
                    IsPowerUpUnlocked[ItemNameCollections.WateringCan] = true;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                })
                .SetCondition(up => !IsPowerUpUnlocked[ItemNameCollections.WateringCan] && Global.Money.Value >= up.Price)
            );
            
            // 铲子强化
            PowerUps.Add(new PowerUp().WithKey("2")
                .WithPrice(20)
                .WithTitle("锄头范围+1")
                .WithDescription("强化($20)")
                .SetOnUnlock(up =>
                {
                    IsPowerUpUnlocked[ItemNameCollections.Shovel] = true;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                })
                .SetCondition(up => !IsPowerUpUnlocked[ItemNameCollections.Shovel] && Global.Money.Value >= up.Price)
            );
            
            // 手强化
            PowerUps.Add(new PowerUp().WithKey("3")
                .WithPrice(30)
                .WithTitle("手范围+1")
                .WithDescription("强化($30)")
                .SetOnUnlock(up =>
                {
                    IsPowerUpUnlocked[ItemNameCollections.Hand] = true;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                })
                .SetCondition(up => !IsPowerUpUnlocked[ItemNameCollections.Hand] && Global.Money.Value >= up.Price)
            );
            
            // 种子强化
            PowerUps.Add(new PowerUp().WithKey("4")
                .WithPrice(40)
                .WithTitle("种子范围+1")
                .WithDescription("强化($40)")
                .SetOnUnlock(up =>
                {
                    IsPowerUpUnlocked[ItemNameCollections.Seed] = true;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                })
                .SetCondition(up => !IsPowerUpUnlocked[ItemNameCollections.Seed] && Global.Money.Value >= up.Price)
            );
            
            #endregion
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