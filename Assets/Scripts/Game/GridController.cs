using Game.Data;
using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;
// ReSharper disable Unity.InefficientPropertyAccess

namespace Game
{
	public partial class GridController : ViewController
	{
		[Header("笔刷")]
		public TileBase pen;	// 地形笔刷
		public TileBase canCultivateFieldPen;	// 可开垦区域笔刷笔刷

		public EasyGrid<SoilData> ShowGrid { get; } = new(5,4);

		private void Start()
		{
			// 画可以种植的区域
			ShowGrid.ForEach((x, y, _) =>
			{
				Ground.SetTile(new Vector3Int(x, y), canCultivateFieldPen);
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
