using System;
using Game.Data;
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

			Global.Days.Register(_ => Show());
		}

		// 画开垦了的区域
		private void Show()
		{
			ShowGrid.ForEach((x, y, _) =>
			{
				if (_ == null) return;
				Soil.SetTile(new Vector3Int(x, y), pen);
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
