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
			if (Input.GetKeyDown(KeyCode.J))
			{
				// 根据player的position值拿到tilemap的具体块, 消除对应块
				var cellPos = grid.WorldToCell(transform.position);
				tilemap.SetTile(cellPos, null);
			}
		}
	}
}
