namespace Game.ChallengeSystem.ChallengeTasks
{
    public class HasTenFruitsCurrently : Challenge
    {
        public override string Name => "当前拥有十个以上的果实";
        
        public override void OnStart()
        {
            StartDate = Global.Days.Value;
        }

        public override bool CheckFinish()
        {
            if (StartDate == Global.Days.Value) return false;
            return Global.PumpkinCount.Value + Global.RadishCount.Value >= 10;
        }

        public override void OnFinish()
        {
            
        }
    }
}