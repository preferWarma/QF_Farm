using UnityEngine;
using QFramework;

namespace Game
{
	public partial class ResController : ViewController, ISingleton
	{
		[Header("GameObjects")]
		public GameObject waterPrefab;	// 水
		public GameObject plantPrefab;  // 植物
		
		[Header("Sprites")]
		public Sprite seedSprite;	// 种子贴图
		public Sprite smallPlantSprite; // 幼苗贴图
		public Sprite ripeSprite;  // 成熟贴图
		public Sprite oldSprite;  // 摘取贴图

		public static ResController Instance => MonoSingletonProperty<ResController>.Instance;

		public void OnSingletonInit()
		{
			
		}
	}
}
