using UnityEngine;
using QFramework;

namespace Game
{
	public partial class ResController : ViewController, ISingleton
	{
		[Header("预制体")]
		public GameObject waterPrefab;	// 水
		public GameObject plantPrefab;  // 植物
		public GameObject plantRadishPrefab; // 萝卜植物
		
		[Header("默认种子贴图")]
		public Sprite seedSprite;	// 种子贴图
		public Sprite smallPlantSprite; // 幼苗贴图
		public Sprite ripeSprite;  // 成熟贴图
		public Sprite oldSprite;  // 摘取贴图
		
		[Header("胡萝卜种子贴图")]
		public Sprite seedRadishSprite;	// 种子贴图
		public Sprite smallPlantRadishSprite; // 幼苗贴图
		public Sprite ripeRadishSprite;  // 成熟贴图
		public Sprite oldRadishSprite;  // 摘取贴图

		public static ResController Instance => MonoSingletonProperty<ResController>.Instance;

		public void OnSingletonInit()
		{
			
		}
	}
}
