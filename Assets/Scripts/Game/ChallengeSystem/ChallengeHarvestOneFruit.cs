namespace Game.ChallengeSystem
{
    public class ChallengeHarvestFirstFruit : Challenge
    {
        public override string Name => "完成第一个果实的收获";
        public override void OnStart()
        {
            StartDate = Global.Days.Value;
        }

        public override bool CheckFinish()
        {
            return StartDate != Global.Days.Value && Global.HarvestCountInCurrentDay.Value > 0;
        }

        public override void OnFinish()
        {
        }
    }
}