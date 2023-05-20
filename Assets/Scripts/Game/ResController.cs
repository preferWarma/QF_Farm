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

		[Header("植物相关贴图")]
		public List<Sprite> plantSprites = new();	// 植物贴图集合


		[Header("工具贴图集合")]
		public List<Sprite> sprites = new();
		
		[Header("植物预制体集合")]
		public List<GameObject> plantPrefabs = new();

		public Sprite LoadSprite(string spriteName)
		{
			return sprites.Single(sprite => sprite.name == spriteName);
		}
		
		public Sprite LoadPlantSprite(string spriteName)
		{
			return plantSprites.Single(sprite => sprite.name == spriteName);
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
