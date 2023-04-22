using UnityEngine;
using QFramework;

namespace Game
{
	public partial class ResController : ViewController, ISingleton
	{
		public GameObject seedPrefab;
		public GameObject waterPrefab;

		public static ResController Instance => MonoSingletonProperty<ResController>.Instance;

		public void OnSingletonInit()
		{
			
		}
	}
}
