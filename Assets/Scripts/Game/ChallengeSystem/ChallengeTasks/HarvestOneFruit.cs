namespace Game.ChallengeSystem.ChallengeTasks
{
    public class ChallengeHarvestOneFruit : Challenge
    {
        public override string Name => "完成一个果实的收获";
        public override void OnStart()
        {
            StartDate = Global.Days.Value;
        }

        public override bool CheckFinish()
        {
            return StartDate != Global.Days.Value && ChallengeController.HarvestCountInCurrentDay.Value > 0;
        }

        public override void OnFinish()
        {
        }
    }
}