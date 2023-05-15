using System;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Inventory
{
    // 背包槽
    public class UISlot : MonoBehaviour
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

        private void Update()
        {
            // if (ItemData == null)
            // {
            //     gameObject.SetActive(false);
            // }
        }

#if UNITY_EDITOR
        // 编辑器模式下, 用于更新UI
        private void OnValidate()
        {
            if (transform.Find("Icon") != null)
            {
                icon = transform.Find("Icon").GetComponent<Image>();
            }
            if (transform.Find("Select") != null)
            {
                select = transform.Find("Select").GetComponent<Image>();
            }
            if (transform.Find("ShotCut") != null)
            {
                shotCut = transform.Find("ShotCut").GetComponent<Text>();
            }
            if (transform.Find("Count") != null)
            {
                count = transform.Find("Count").GetComponent<Text>();
            }

            button = GetComponent<Button>();
        }
#endif

        public void SetSlotData(Item newItemData, string newShotCut)
        {
            ItemData = newItemData;
            icon.sprite = SpriteLoader?.Invoke(newItemData.iconName);
            shotCut.text = newShotCut;

            if (newItemData.canStack)
            {
                // 注册数量显示, 使物品数量发生变化时, UI也会发生变化
                newItemData.Count.RegisterWithInitValue(cnt =>
                {
                    count.text = cnt.ToString();
                }).UnRegisterWhenGameObjectDestroyed(this);
            }
        }
    }
}
