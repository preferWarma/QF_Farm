using UnityEngine;
using QFramework;

namespace Game
{
	public partial class ResController : ViewController, ISingleton
	{
		public GameObject seedPrefab;	// 种子
		public GameObject waterPrefab;	// 水
		public GameObject smallPlantPrefab; // 幼苗
		public GameObject ripePrefab;  // 成熟

		public static ResController Instance => MonoSingletonProperty<ResController>.Instance;

		public void OnSingletonInit()
		{
			
		}
	}
}
