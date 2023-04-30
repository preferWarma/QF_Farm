namespace Game.ChallengeSystem
{
    public class ChallengeHarvestFirstFruit : Challenge
    {
        public override string Name => "完成第一个果实的收获";
        public override void OnStart()
        {
        }

        public override bool CheckFinish()
        {
            return Global.Fruits.Value > 0;
        }

        public override void OnFinish()
        {
        }
    }
}