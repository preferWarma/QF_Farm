using QFramework;
using UnityEngine;

namespace Game
{
    // 充当Model层
    public class Global : MonoBehaviour
    {
        public static BindableProperty<int> Days = new(1);
        public static BindableProperty<int> Fruits = new(0);
    }
}
