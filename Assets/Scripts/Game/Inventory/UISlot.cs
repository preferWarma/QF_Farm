using System;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Inventory
{
    // 背包槽
    public class UISlot : MonoBehaviour
    {
        [Header("背包槽基本属性")]
        [Tooltip("图标")] public Image icon;
        [Tooltip("选择框")] public Image select;
        [Tooltip("快捷键")] public Text shotCut;

        [Tooltip("当前背包槽的数据, 如果为空则表示此背包槽未被使用")] 
        public ISlotData SlotData { get; private set; }

        private void Awake()
        {
            icon.sprite = null;
            select.Hide();
        }

        public void SetSlotData(ISlotData newSlotData, string newShotCut)
        {
            SlotData = newSlotData;
            icon.sprite = SlotData.Icon;
            shotCut.text = newShotCut;
        }
    }
    
    // 背包槽数据
    public class SlotData : ISlotData
    {
        public Sprite Icon { get; set; }
        public Action OnSelect { get; set; }
    }
}
