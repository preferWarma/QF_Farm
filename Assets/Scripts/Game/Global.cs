using QFramework;
using UnityEngine;

namespace Game
{
    public class Global : MonoBehaviour
    {
        public static BindableProperty<int> Days = new(1);
    }
}
