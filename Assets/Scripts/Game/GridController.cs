using System.SoilSys;
using Game.Plants;
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

		private ISoilSystem mSoilSystem;

		public EasyGrid<SoilData> ShowGrid => mSoilSystem?.SoilGrid;

		private void Awake()
		{
			mSoilSystem = this.GetSystem<ISoilSystem>();
		}
		
		private void Start()
		{
			mSoilSystem.LoadWithJson();
			Show();
			
			ShowGrid.ForEach((x, y, _) =>
			{
				Ground.SetTile(new Vector3Int(x, y), canCultivateFieldPen);
			});
		}
		
		public void Show()
		{
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
						.Instantiate()
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
		}
	}
}
