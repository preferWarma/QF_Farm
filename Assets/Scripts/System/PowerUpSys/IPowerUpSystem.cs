using System.Collections.Generic;
using Game;
using Lyf.SaveSystem;
using QFramework;

namespace System.PowerUpSys
{
    public interface IPowerUpSystem : ISystem, ISaveWithJson
    {
        List<IPowerUp> PowerUps { get; set; }
        void ResetDefaultData();
    }
    
    public class PowerUpSystem : AbstractSystem, IPowerUpSystem
    {
        public static bool[] IsToolUpgraded = new bool[4]; // 工具是否升级(顺序按照工具的顺序)
        public List<IPowerUp> PowerUps { get; set; } = new();
        
        protected override void OnInit()
        {
            PowerUps.Add(new PowerUp().WithKey("10")
                .WithPrice(10)
                .WithTitle("花洒范围+1")
                .WithDescription("强化($10)")
                .WithKey("wateringCanUp")
                .SetOnUnlock(up =>
                {
                    IsToolUpgraded[0] = true;
                    Global.Money.Value -= up.Price;
                    AudioController.Instance.Sfx_Trade.Play();
                })
                .SetCondition(_ => IsToolUpgraded[0] == false && Global.Money.Value >= 10)
            );
        }

        public string SAVE_FILE_NAME => "PowerUp";
        public void SaveWithJson()
        {
            var saveData = new bool[4];
            for (var i = 0; i < 4; i++)
            {
                saveData[i] = IsToolUpgraded[i];
            }
            SaveManager.SaveWithJson(SAVE_FILE_NAME, saveData);
        }

        public void LoadWithJson()
        {
            var saveData = SaveManager.LoadWithJson<bool[]>(SAVE_FILE_NAME);
            if (saveData == null) return;
            for (var i = 0; i < 4; i++)
            {
                IsToolUpgraded[i] = saveData[i];
            }
        }

        public void ResetDefaultData()
        {
            IsToolUpgraded = new bool[4];
        }
    }
}