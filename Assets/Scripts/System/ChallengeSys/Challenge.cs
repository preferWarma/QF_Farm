namespace System.ChallengeSys
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
        public int StartDate;    // 挑战开始的日期
        
        public abstract void OnStart(); // 挑战开始时候要做的事情
        public abstract bool CheckFinish();
        public abstract void OnFinish();    // 挑战完成时候要做的事情
    }
    
    // 通用挑战
    public class GenericChallenge : Challenge
    {
        public override string Name => mName;
        private Action<GenericChallenge> mOnStart;
        private Func<GenericChallenge, bool> mCheckFinish;
        private Action<GenericChallenge> mOnFinish;
        private string mName;

        public override void OnStart()
        {
            mOnStart?.Invoke(this);
        }

        public override bool CheckFinish()
        {
            return mCheckFinish.Invoke(this);
        }

        public override void OnFinish()
        {
            mOnFinish?.Invoke(this);
        }
        
        public GenericChallenge OnStart(Action<GenericChallenge> onStart)
        {
            mOnStart = onStart;
            return this;
        }
        
        public GenericChallenge CheckFinish(Func<GenericChallenge, bool> checkFinish)
        {
            mCheckFinish = checkFinish;
            return this;
        }
        
        public GenericChallenge OnFinish(Action<GenericChallenge> onFinish)
        {
            mOnFinish = onFinish;
            return this;
        }
        
        public GenericChallenge SetName(string name)
        {
            mName = name;
            return this;
        }
        
    }
}