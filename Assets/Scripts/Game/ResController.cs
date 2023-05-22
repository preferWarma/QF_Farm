using System.Collections.Generic;
using System.Linq;
using Game.Plants;
using UnityEngine;
using QFramework;

namespace Game
{
	public partial class ResController : ViewController, ISingleton
	{
		public static ResController Instance => MonoSingletonProperty<ResController>.Instance;

		[Header("预制体")]
		public GameObject waterPrefab;	// 水

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
