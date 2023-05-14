using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using QFramework;

namespace Game
{
	public partial class ResController : ViewController, ISingleton
	{
		public static ResController Instance => MonoSingletonProperty<ResController>.Instance;

		[Header("预制体")]
		public GameObject waterPrefab;	// 水

		[Header("南瓜贴图")]
		public Sprite seedSprite;	// 种子贴图
		public Sprite smallPlantSprite; // 幼苗贴图
		public Sprite ripeSprite;  // 成熟贴图
		public Sprite oldSprite;  // 摘取贴图
		
		[Header("胡萝卜贴图")]
		public Sprite seedRadishSprite;	// 种子贴图
		public Sprite smallPlantRadishSprite; // 幼苗贴图
		public Sprite ripeRadishSprite;  // 成熟贴图
		public Sprite oldRadishSprite;  // 摘取贴图
		
		[Header("土豆贴图")]
		public Sprite seedPotatoSprite;	// 种子贴图
		public Sprite smallPlantPotatoSprite; // 幼苗贴图
		public Sprite ripePotatoSprite;  // 成熟贴图
		public Sprite oldPotatoSprite;  // 摘取贴图
		
		[Header("西红柿贴图")]
		public Sprite seedTomatoSprite;	// 种子贴图
		public Sprite smallPlantTomatoSprite; // 幼苗1贴图
		public Sprite middlePlantTomatoSprite; // 幼苗2贴图
		public Sprite bigPlantTomatoSprite; // 幼苗3贴图
		public Sprite ripeTomatoSprite;  // 成熟贴图
		public Sprite oldTomatoSprite;  // 摘取贴图
		

		[Header("工具贴图集合")]
		public List<Sprite> sprites = new();
		
		[Header("植物预制体集合")]
		public List<GameObject> plantPrefabs = new();

		public Sprite LoadSprite(string spriteName)
		{
			return sprites.Single(sprite => sprite.name == spriteName);
		}
		
		public GameObject LoadPrefab(string prefabName)
		{
			return plantPrefabs.Single(prefab => prefab.name == prefabName);
		}

		public void OnSingletonInit()
		{
			
		}
	}
}
