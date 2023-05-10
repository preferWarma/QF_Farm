namespace Game.ChallengeSystem.ChallengeTasks
{
    public class HarvestOneRadish : Challenge
    {
        public override string Name => "收获一个萝卜";

        public override void OnStart()
        {
            StartDate = Global.Days.Value;
        }

        public override bool CheckFinish()
        {
            return StartDate != Global.Days.Value && ChallengeController.HarvestRadishCountInCurrentDay.Value >= 1;
        }

        public override void OnFinish()
        {
        }
    }
}