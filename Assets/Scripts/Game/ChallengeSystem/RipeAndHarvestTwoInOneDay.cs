namespace Game.ChallengeSystem
{
    public class RipeAndHarvestTwoInOneDay : Challenge
    {
        public override string Name => "一天成熟并收获两个果实";

        public override void OnStart()
        {
            StartDate = Global.Days.Value;
        }

        public override bool CheckFinish()
        {
            return StartDate != Global.Days.Value && ChallengeController.RipeAndHarvestCountInCurrentDay.Value >= 2;
        }

        public override void OnFinish()
        {
        }
    }
}