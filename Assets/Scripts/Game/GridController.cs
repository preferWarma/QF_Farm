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

		public EasyGrid<SoilData> ShowGrid { get; } = new(10,10);

		private void Start()
		{
			ShowGrid[0, 0] = new SoilData();
			ShowGrid[1, 1] = new SoilData();
			ShowGrid[2, 2] = new SoilData();
			
			ShowGrid.ForEach((x, y, data) =>
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
			ShowGrid.ForEach((x, y, _) =>
			{
				var tileWorldPos = grid.CellToWorld(new Vector3Int(x, y, 0));
				var leftTopPos = tileWorldPos + new Vector3(0, grid.cellSize.y, 0);
				var rightTopPos = tileWorldPos + new Vector3(grid.cellSize.x, grid.cellSize.y, 0);
				var rightBottomPos = tileWorldPos + new Vector3(grid.cellSize.x, 0, 0);
				
				Debug.DrawLine(tileWorldPos, leftTopPos, Color.red);
				Debug.DrawLine(leftTopPos, rightTopPos, Color.red);
				Debug.DrawLine(rightTopPos, rightBottomPos, Color.red);
				Debug.DrawLine(rightBottomPos, tileWorldPos, Color.red);
			});
		}
	}
}
