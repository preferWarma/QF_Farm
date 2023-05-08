using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using QFramework;
using UnityEngine.Serialization;

namespace Game
{
	public partial class ResController : ViewController, ISingleton
	{
		public static ResController Instance => MonoSingletonProperty<ResController>.Instance;

		[Header("预制体")]
		public GameObject waterPrefab;	// 水
		public GameObject plantPumpkinPrefab;  // 南瓜植物
		public GameObject plantRadishPrefab; // 萝卜植物
		public GameObject plantPotatoPrefab; // 土豆植物
		
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
		
		[Header("土豆种子贴图")]
		public Sprite seedPotatoSprite;	// 种子贴图
		public Sprite smallPlantPotatoSprite; // 幼苗贴图
		public Sprite ripePotatoSprite;  // 成熟贴图
		public Sprite oldPotatoSprite;  // 摘取贴图

		[Header("贴图集合")]
		public List<Sprite> sprites = new();
		
		public Sprite LoadSprite(string spriteName)
		{
			return sprites.Single(sprite => sprite.name == spriteName);
		}


		public void OnSingletonInit()
		{
			
		}
	}
}
