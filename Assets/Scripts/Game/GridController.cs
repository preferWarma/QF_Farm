using Game.Data;
using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;
// ReSharper disable Unity.InefficientPropertyAccess

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

		private void Update()
		{
			var grid = FindObjectOfType<Grid>();
			showGrid.ForEach((x, y, _) =>
			{
				var tileWorldPos = grid.CellToWorld(new Vector3Int(x, y, 0));
				var leftBottomPos = tileWorldPos;
				var leftTopPos = tileWorldPos + new Vector3(0, grid.cellSize.y, 0);
				var rightTopPos = tileWorldPos + new Vector3(grid.cellSize.x, grid.cellSize.y, 0);
				var rightBottomPos = tileWorldPos + new Vector3(grid.cellSize.x, 0, 0);
				
				Debug.DrawLine(leftBottomPos, leftTopPos, Color.red);
				Debug.DrawLine(leftTopPos, rightTopPos, Color.red);
				Debug.DrawLine(rightTopPos, rightBottomPos, Color.red);
				Debug.DrawLine(rightBottomPos, leftBottomPos, Color.red);
			});
		}
	}
}
