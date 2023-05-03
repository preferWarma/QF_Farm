using UnityEngine;
using QFramework;
using UnityEngine.Tilemaps;
// ReSharper disable Unity.InefficientPropertyAccess

namespace Game
{
	public partial class Player : ViewController
	{
		[Header("Tilemap相关")]
		[Tooltip("地块网格")] public Grid grid;
		[Tooltip("地块显示")] public Tilemap tilemap;

		private void Awake()
		{
			Global.Player = this;
		}
		
		private void Update()
		{

			if (Input.GetKeyDown(KeyCode.Q))	// 按下Q键，进入下一天
			{
				Global.Days.Value++;
			}
			
			// // 根据player的position值拿到tilemap的具体块
			// var cellPos = grid.WorldToCell(transform.position);
			// var showGrid = FindObjectOfType<GridController>().ShowGrid;
			//
			// var tileWorldPos = grid.CellToWorld(cellPos);
			// tileWorldPos.x += grid.cellSize.x * 0.5f;
			// tileWorldPos.y += grid.cellSize.y * 0.5f;
			//
			// if (Input.GetMouseButtonDown(1))	// 右键消除土地
			// {
			// 	if (cellPos.x is < 10 and >= 0 && cellPos.y is >= 0 and < 10)
			// 	{
			// 		if (showGrid[cellPos.x, cellPos.y] != null)
			// 		{
			// 			tilemap.SetTile(cellPos, null);
			// 			showGrid[cellPos.x, cellPos.y] = null;
			// 		}
			// 	}
			// }
		}

		private void OnGUI()
		{
			// 显示提示信息
			IMGUIHelper.SetDesignResolution(640,360);
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  天数: " + Global.Days.Value);
			GUILayout.EndHorizontal();
			
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  南瓜种子: " + Global.PumpKinSeedCount.Value);
			GUILayout.EndHorizontal();
			
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  胡萝卜种子: " + Global.RadishSeedCount.Value);
			GUILayout.EndHorizontal();
			
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  南瓜果子: " + Global.PumpkinCount.Value);
			GUILayout.EndHorizontal();
			
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  胡萝卜果子: " + Global.RadishCount.Value);
			GUILayout.EndHorizontal();
			
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  当天成熟并采摘的数量: " + Global.RipeAndHarvestCountInCurrentDay.Value);
			GUILayout.EndHorizontal();
			
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  浇水: E");
			GUILayout.EndHorizontal();
			
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  下一天: Q");
			GUILayout.EndHorizontal();GUILayout.Space(10);

			// GUI.Label(new Rect(10, 320, 300, 24), "切换工具: [1]手, [2]锄头, [3]水壶, [4]种子");
		}

		private void OnDestroy()
		{
			Global.Player = null;
		}
	}
}
