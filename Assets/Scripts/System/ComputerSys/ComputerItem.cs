using QFramework;
using UnityEngine;

namespace System.ComputerSys
{
    public class ComputerItem
    {
        public string Name;
        public float TotalHours;
        public float RestHours;
        public readonly BindableProperty<bool> IsFinished = new();
        public GameObject ItemObj;
        [Tooltip("完成该项目后每日收益")] public float Price;

        private Action _onFinish;
        private Func<ComputerItem, bool> _showCondition;


        public bool ShowCondition()
        {
            return _showCondition?.Invoke(this) ?? !IsFinished.Value;
        }
        

        public void OnFinish()
        {
            _onFinish?.Invoke();
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
            RestHours = totalHours;
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

        #endregion
    }
}