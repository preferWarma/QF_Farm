﻿namespace Game.ChallengeSystem
{
    public class RipeAndHarvestTwoInOneDay : Challenge
    {
        public override string Name => "一天成熟并收获两个果实";

        public override void OnStart()
        {
        }

        public override bool CheckFinish()
        {
            return Global.RipeAndHarvestCountInCurrentDay.Value >= 2;
        }

        public override void OnFinish()
        {
        }
    }
}