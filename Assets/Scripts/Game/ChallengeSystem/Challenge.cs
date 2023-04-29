namespace Game.ChallengeSystem
{
    // 挑战
    public abstract class Challenge
    {
        public enum States
        {
            NotStart,   // 未开始
            Doing,      // 进行中
            Finished    // 已完成
        }
        
        public string Name;
        public States State = States.NotStart;  // 当前挑战状态
        
        public abstract void OnStart(); // 挑战开始时候要做的事情
        public abstract bool CheckFinish();
        public abstract void OnFinish();    // 挑战完成时候要做的事情
    }
}