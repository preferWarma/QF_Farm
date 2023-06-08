using System;
using Game.UI;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Inventory
{
    // 背包槽
    public class UISlot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [Tooltip("图标加载器方法")] public static Func<string, Sprite> SpriteLoader;
        [Tooltip("与该工具对应的使用方法")] public static Action<UISlot> OnUse;
        
        [Header("背包槽基本属性")]
        [Tooltip("图标")] public Image icon;
        [Tooltip("选择框")] public Image select;
        [Tooltip("快捷键")] public Text shotCut;
        [Tooltip("物品数量显示")] public Text count;
        [Tooltip("对应按钮")] public Button button;

        [Tooltip("当前背包槽的数据, 如果为空则表示此背包槽未被使用")] 
        public Item ItemData { get; private set; }

        private bool _dragging; // 是否正在拖拽
        private Transform _originalParent;  // 拖拽前的父节点

        private void Awake()
        {
            icon.sprite = null;
            select.Hide();
            shotCut.text = string.Empty;
            count.text = string.Empty;  // 默认隐藏数量显示(即非可堆叠物品)
            button.onClick.AddListener(() =>
            {
                OnUse?.Invoke(this);
            });
        }
        
        public void SetSlotData(Item newItemData, string newShotCut)
        {
            if (newItemData == null)    // 如果传入的物品为空, 则表示清空该背包槽
            {
                ItemData = null;
                icon.sprite = SpriteLoader?.Invoke("UIMask");
                shotCut.text = string.Empty;
                count.text = string.Empty;
            }
            else
            {
                ItemData = newItemData;
                icon.sprite = SpriteLoader?.Invoke(newItemData.iconName);
                shotCut.text = newShotCut;

                if (newItemData.canStack)
                {
                    // 注册数量显示, 使物品数量发生变化时, UI也会发生变化
                    newItemData.Count.RegisterWithInitValue(cnt => { count.text = cnt.ToString(); })
                        .UnRegisterWhenGameObjectDestroyed(this);
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (ItemData != null && ItemData.Count.Value > 0)
            {
                _dragging = true;
                // 把当前物品作为父节点的最后一个子节点, 使其显示在最上层
                transform.SetAsLastSibling();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_dragging) return;
            var mousePos = Input.mousePosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, mousePos, null,
                    out var localPoint))
            {
                icon.LocalPosition(localPoint);
            }
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_dragging) return;
            _dragging = false;
            icon.LocalPosition(Vector3.zero);   // 拖拽结束后, 图标回到原位

            var toolbarSlots = FindObjectOfType<UIToolBar>().ToolbarSlots;
            foreach (var slot in toolbarSlots)
            {
                // 如果拖拽结束时, 鼠标在工具栏背包槽的范围内, 则表示拖拽成功
                if (RectTransformUtility.RectangleContainsScreenPoint(slot.transform as RectTransform, Input.mousePosition))
                {
                    if (slot == this) return;   // 如果拖拽到了自己身上, 则不做任何操作
                    // 交换两个背包槽的数据
                    var temp = slot.ItemData;
                    slot.SetSlotData(ItemData, slot.shotCut.text);
                    SetSlotData(temp, shotCut.text);
                    
                    slot.select.Show(); // 交换后, 选中交换到的背包槽
                    select.Hide();  // 同时隐藏当前开始的选中框
                }
            }
        }
    }
}
