using System;
using UnityEngine;

namespace Game.Inventory
{
    public interface ISlotData
    {
        public Sprite Icon { get; set; }	// 图标
        public Action OnSelect { get; set; }	// 选择的事件逻辑
    }
}