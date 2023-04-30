using System.Collections.Generic;
using QFramework;

namespace Game.ChallengeSystem
{
    public class RipeAndHarvestFiveInOneDay : Challenge, IUnRegisterList
    {
        public override string Name => "一天成熟并收获五个果实";

        public override void OnStart()
        {
        }

        public override bool CheckFinish()
        {
            return Global.RipeAndHarvestCountInCurrentDay.Value >= 5;
        }

        public override void OnFinish()
        {
            this.UnRegisterAll();   // 挑战完成时移除所有事件监听
        }

        public List<IUnRegister> UnregisterList { get; } = new();
    }
}