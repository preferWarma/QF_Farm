namespace Game.ChallengeSystem
{
    public class RipeAndHarvestFiveInOneDay : Challenge
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
        }
    }
}