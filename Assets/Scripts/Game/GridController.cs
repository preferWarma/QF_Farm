using System.Collections.Generic;
using System.Linq;
using System.SoilSys;
using Game.Plants;
using Game.UI;
using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;

// ReSharper disable Unity.InefficientPropertyAccess

namespace Game
{
	public partial class GridController : ViewController, IController
	{
		[Header("笔刷")]
		public TileBase pen;	// 地形笔刷
		public TileBase canCultivateFieldPen;	// 可开垦区域笔刷笔刷
		public EasyGrid<SoilData> ShowGrid => mSoilSystem?.SoilGrid;
		
		[Tooltip("用于合并成熟消息")]
		public readonly Dictionary<string, int> RipeCountToday = new()
		{
			{ItemNameCollections.Pumpkin, 0},
			{ItemNameCollections.Radish, 0},
			{ItemNameCollections.Tomato, 0},
			{ItemNameCollections.Potato, 0},
			{ItemNameCollections.Bean, 0},
		};

		private ISoilSystem mSoilSystem;

		private void Awake()
		{
			mSoilSystem = this.GetSystem<ISoilSystem>();
			Global.GridController = this;
		}
		
		private void Start()
		{
			Show();
			Global.Days.Register(_ =>
			{
				foreach (var keyValuePair in RipeCountToday.Where(keyValuePair => keyValuePair.Value != 0))
				{
					UIMessageQueue.Push(ResController.Instance.LoadSprite(keyValuePair.Key), $"成熟 + {keyValuePair.Value}");
				}
				// 将今天的成熟的数量清零
				foreach (var key in RipeCountToday.Keys.ToList())
				{
					RipeCountToday[key] = 0;
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
		}
		
		public void Show()
		{
			// 画背景
			ShowGrid.ForEach((x, y, _) =>
			{
				BackGround.SetTile(new Vector3Int(x, y), canCultivateFieldPen);
			});
			
			// 画土壤
			ShowGrid.ForEach((x, y, soilData) =>
			{
				if (soilData == null)
				{
					Soil.SetTile(new Vector3Int(x, y), null);
					if (PlantController.Instance.PlantGrid[x, y] == null) return;
					PlantController.Instance.PlantGrid[x, y].GameObject.DestroySelf();
					PlantController.Instance.PlantGrid[x, y] = null;
					return;
				}
				Soil.SetTile(new Vector3Int(x, y), pen);
				if (soilData.HasPlant)
				{
					var plantObj = ResController.Instance.LoadPrefab(soilData.PlantPrefabName)
						.InstantiateWithParent(Global.PlantsRoot.transform)
						.Position(Soil.layoutGrid.GetCellCenterWorld(new Vector3Int(x, y)));
					var plant = plantObj.GetComponent<IPlant>();
					PlantController.Instance.PlantGrid[x, y] = plant;
					plant.X = x;
					plant.Y = y;
					plant.SetState(soilData.PlantSate);
				}

				if (soilData.Watered)
				{
					ResController.Instance.waterPrefab
						.Instantiate()
						.Position(Soil.layoutGrid.GetCellCenterWorld(new Vector3Int(x, y)));
				}
			});
		}

		public IArchitecture GetArchitecture()
		{
			return Global.Interface;
		}

		private void OnDestroy()
		{
			mSoilSystem = null;
			Global.GridController = null;
		}
	}
}
