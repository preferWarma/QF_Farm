namespace Game.ChallengeSystem
{
    public class RipeAndHarvestFiveInOneDay : Challenge
    {
        public override string Name => "一天成熟并收获五个果实";

        public override void OnStart()
        {
            StartDate = Global.Days.Value;
        }

        public override bool CheckFinish()
        {
            return StartDate != Global.Days.Value && ChallengeController.RipeAndHarvestCountInCurrentDay.Value >= 5;
        }

        public override void OnFinish()
        {
        }
    }
}