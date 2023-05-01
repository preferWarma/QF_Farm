namespace Game.ChallengeSystem
{
    // 挑战基类
    public abstract class Challenge
    {
        public enum States
        {
            NotStart,   // 未开始
            Doing,      // 进行中
            Finished    // 已完成
        }
        
        public abstract string Name { get;} // 挑战名称
        public States State = States.NotStart;  // 当前挑战状态
        protected int StartDate;    // 挑战开始的日期
        
        public abstract void OnStart(); // 挑战开始时候要做的事情
        public abstract bool CheckFinish();
        public abstract void OnFinish();    // 挑战完成时候要做的事情
    }
}