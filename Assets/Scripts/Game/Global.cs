using QFramework;
using UnityEngine;

namespace Game
{
    // 充当Model层
    public class Global : MonoBehaviour
    {
        public static BindableProperty<int> Days = new(1); // 第几天
        public static BindableProperty<int> Fruits = new(0); // 水果数量
        public static BindableProperty<string> CurrentTool = new("手");  // 当前工具
    }
}
