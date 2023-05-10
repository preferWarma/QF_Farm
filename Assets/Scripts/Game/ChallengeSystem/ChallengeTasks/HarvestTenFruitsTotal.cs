namespace Game.ChallengeSystem.ChallengeTasks
{
    public class HarvestTenFruitsTotal : Challenge
    {
        public override string Name => "累计收获10个果实";
        
        public override void OnStart()
        {
            StartDate = Global.Days.Value;
        }

        public override bool CheckFinish()
        {
            if (StartDate == Global.Days.Value) return false;
            return ChallengeController.TotalPumpkinCount.Value + ChallengeController.TotalRadishCount.Value >= 10;
        }

        public override void OnFinish()
        {
            
        }
    }
}