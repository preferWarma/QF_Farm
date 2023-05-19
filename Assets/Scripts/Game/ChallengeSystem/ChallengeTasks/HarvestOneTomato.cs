namespace Game.ChallengeSystem.ChallengeTasks
{
    public class HarvestOneTomato : Challenge
    {
        public override string Name => "采摘一个番茄";

        public override void OnStart()
        {
            StartDate = Global.Days.Value;
        }

        public override bool CheckFinish()
        {
            return StartDate != Global.Days.Value && ChallengeController.HarvestTomatoInCurrentDay.Value >= 1;
        }

        public override void OnFinish()
        {
        }

        
    }
}