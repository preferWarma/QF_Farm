namespace System.PowerUpSys
{
    public interface IPowerUp
    {
        string Key { get; set; }    // 身份标识
        string Title { get; set; }  // 标题
        string Description { get; set; }    // 描述
        bool UnLocked { get; set; } // 是否解锁
        int Price { get; set; } // 解锁价格
        
        bool ShowCondition();   // 显示条件
        void OnUnlock();  // 解锁
    }
    
    public class PowerUp : IPowerUp
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool UnLocked { get; set; }
        public int Price { get; set; }

        private Func<PowerUp, bool> _condition;  // 显示条件
        private Action<PowerUp> _onUnlock;   // 解锁回调

        public bool ShowCondition()
        {
            return !UnLocked && (_condition == null || _condition.Invoke(this));
        }

        public void OnUnlock()
        {
            UnLocked = true;
            PowerUpSystem.IntensifiedToday.Value = true;
            _onUnlock?.Invoke(this);
        }

        #region 链式封装

        public PowerUp WithPrice(int price)
        {
            Price = price;
            return this;
        }

        public PowerUp WithTitle(string title)
        {
            Title = title;
            return this;
        }

        public PowerUp WithKey(string key)
        {
            Key = key;
            return this;
        }
        
        public PowerUp WithDescription(string description)
        {
            Description = description;
            return this;
        }
        
        public PowerUp SetCondition(Func<PowerUp, bool> condition)
        {
            _condition = condition;
            return this;
        }
        
        public PowerUp SetOnUnlock(Action<PowerUp> onUnlock)
        {
            _onUnlock = onUnlock;
            return this;
        }
        
        #endregion
        
    }
}