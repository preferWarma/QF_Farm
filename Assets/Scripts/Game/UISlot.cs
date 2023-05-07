using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // 背包槽
    public class UISlot : MonoBehaviour
    {
        [Header("背包槽基本属性")]
        [Tooltip("图标")] public Image icon;
        [Tooltip("选择框")] public Image select;
        [Tooltip("快捷键")] public Text shotCut;
    }
}
