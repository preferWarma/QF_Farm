using System.Collections.Generic;
using QFramework;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

namespace Game.ChallengeSystem
{
    public class RipeAndHarvestTwoInOneDay : Challenge, IUnRegisterList
    {

        public override void OnStart()
        {
            Name = "一天成熟并收获两个果实";
            
            // 监听成熟的植物是否当天被采摘
            Global.OnPlantHarvest.Register(plant =>
            {
                if (plant.ripeDay == Global.Days.Value)
                {
                    Global.RipeAndHarvestCountInCurrentDay.Value++;
                }
            }).AddToUnregisterList(this);
        }

        public override bool CheckFinish()
        {
            return Global.RipeAndHarvestCountInCurrentDay.Value >= 2;
        }

        public override void OnFinish()
        {
            this.UnRegisterAll();   // 挑战完成时移除所有事件监听
            
            // ActionKit.Delay(1.0f, () =>
            // {
            //     SceneManager.LoadScene("Scenes/GamePass");
            // }).StartGlobal();
        }

        public List<IUnRegister> UnregisterList { get; } = new();
    }
}