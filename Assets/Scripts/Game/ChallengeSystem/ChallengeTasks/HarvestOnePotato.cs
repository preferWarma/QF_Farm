namespace Game.ChallengeSystem.ChallengeTasks
{
    public class HarvestOnePotato : Challenge
    {
        public override string Name => "采摘一个土豆";
        
        public override void OnStart()
        {
            StartDate = Global.Days.Value;    
        }

        public override bool CheckFinish()
        {
            return StartDate != Global.Days.Value && ChallengeController.HarvestPotatoCountInCurrentDay.Value >= 1;
        }

        public override void OnFinish()
        {
            
        }
    }
}