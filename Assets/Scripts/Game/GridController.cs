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
	}
}
