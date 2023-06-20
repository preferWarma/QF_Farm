using Game.UI;
using QFramework;
using UnityEngine;

namespace System.ComputerSys
{
    public class ComputerItem
    {
        public string Name;
        public float TotalHours;
        public BindableProperty<float> CurrentHours = new();
        public readonly BindableProperty<bool> IsFinished = new();
        public GameObject ItemObj;
        [Tooltip("完成该项目后每日收益")] public int Price;

        private Action _onFinish;
        private Func<ComputerItem, bool> _showCondition;


        public bool ShowCondition()
        {
            return _showCondition?.Invoke(this) ?? !IsFinished.Value;
        }
        

        public void OnFinish()
        {
            _onFinish?.Invoke();
            Global.Days.Register(_ =>
            {
                Global.Money.Value += Price;
                UIMessageQueue.Push(Name.Replace("制作",string.Empty) + "带来每日收益" + Price + "元");
            });
        }

        #region 简单链式封装
        
        public ComputerItem WithName(string name)
        {
            Name = name;
            return this;
        }
        
        public ComputerItem WithTotalHours(float totalHours)
        {
            TotalHours = totalHours;
            return this;
        }

        public ComputerItem WithOnFinish(Action onFinish)
        {
            _onFinish = onFinish;
            return this;
        }
        
        public ComputerItem WithShowCondition(Func<ComputerItem, bool> showCondition)
        {
            _showCondition = showCondition;
            return this;
        }
        
        public ComputerItem WithPrice(int price)
        {
            Price = price;
            return this;
        }

        #endregion
    }
}