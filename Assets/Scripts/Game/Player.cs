using Game.Data;
using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;

namespace Game
{
	public partial class Player : ViewController
	{
		[Header("Tilemap相关")]
		public Grid grid;
		public Tilemap tilemap;
		
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				// 根据player的position值拿到tilemap的具体块
				var cellPos = grid.WorldToCell(transform.position);
				
				var easyGrid = FindObjectOfType<GridController>().ShowGrid;

				if (cellPos.x is < 10 and >= 0 && cellPos.y is >= 0 and < 10)
				{
					if (easyGrid[cellPos.x, cellPos.y] == null)
					{
						tilemap.SetTile(cellPos, FindObjectOfType<GridController>().pen);
						easyGrid[cellPos.x, cellPos.y] = new SoilData();
					}
				}
			}
		}
	}
}
