using System;
using Game.Data;
using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;

namespace Game
{
	public partial class GridController : ViewController
	{
		public TileBase pen;	// 地形笔刷
		
		private EasyGrid<SoilData> showGrid = new(10,10);	// 需要显示的Grid
		public EasyGrid<SoilData> ShowGrid => showGrid;

		private void Start()
		{
			showGrid[0, 0] = new SoilData();
			showGrid[1, 1] = new SoilData();
			showGrid[2, 2] = new SoilData();
			
			showGrid.ForEach((x, y, data) =>
			{
				if (data != null)
				{
					Tilemap.SetTile(new Vector3Int(x, y), pen);
				}
			});
		}
	}
}
