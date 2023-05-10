namespace Game.ChallengeSystem.ChallengeTasks
{
    public class Have100MoneyCurrently : Challenge
    {
        public override string Name => "当前拥有100金币";
        
        public override void OnStart()
        {
            StartDate = Global.Days.Value;    
        }

        public override bool CheckFinish()
        {
            return StartDate != Global.Days.Value && Global.Money.Value >= 100;
        }

        public override void OnFinish()
        {
            
        }
    }
}