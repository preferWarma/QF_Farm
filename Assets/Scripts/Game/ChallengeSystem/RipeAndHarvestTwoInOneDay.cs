using System.Collections.Generic;
using QFramework;

namespace Game.ChallengeSystem
{
    public class RipeAndHarvestTwoInOneDay : Challenge, IUnRegisterList
    {
        public override string Name => "一天成熟并收获两个果实";

        public override void OnStart()
        {
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