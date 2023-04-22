using System;
using System.Linq;
using Game.Data;
using UnityEngine;
using QFramework;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
// ReSharper disable Unity.InefficientPropertyAccess

namespace Game
{
	public partial class Player : ViewController
	{
		[Header("Tilemap相关")]
		public Grid grid;
		public Tilemap tilemap;

		private void Start()
		{
			Global.Days.Register(day =>
			{
				var seeds = SceneManager.GetActiveScene().GetRootGameObjects()
					.Where(obj => obj.name.StartsWith("Seed"));
				seeds.ForEach(seed =>
				{
					var tilePos = grid.WorldToCell(seed.transform.position);
					var tileData = FindObjectOfType<GridController>().ShowGrid[tilePos.x, tilePos.y];
					if (tileData is not { Watered: true })	return;
					ResController.Instance.smallPlantPrefab
						.Instantiate()
						.Position(seed.transform.position);
					seed.DestroySelf();
				});
			}).UnRegisterWhenGameObjectDestroyed(gameObject);
		}

		private void Update()
		{

			if (Input.GetKeyDown(KeyCode.Q))
			{
				Global.Days.Value++;
			}
			
			// 根据player的position值拿到tilemap的具体块
			var cellPos = grid.WorldToCell(transform.position);
			var easyGrid = FindObjectOfType<GridController>().ShowGrid;
			
			var tileWorldPos = grid.CellToWorld(cellPos);
			tileWorldPos.x += grid.cellSize.x * 0.5f;
			tileWorldPos.y += grid.cellSize.y * 0.5f;

			if (cellPos.x is < 10 and >= 0 && cellPos.y is >= 0 and < 10)
			{
				TileSelectController.Instance.Position(cellPos);	// 显示当前板块
				TileSelectController.Instance.Show();
			}
			else
			{
				TileSelectController.Instance.Hide();	// 如果超出范围就隐藏
			}
			
			
			if (Input.GetMouseButtonDown(0))	// 左键开垦和种植
			{

				if (cellPos.x is < 10 and >= 0 && cellPos.y is >= 0 and < 10)
				{
					if (easyGrid[cellPos.x, cellPos.y] == null)	// 无耕地
					{
						// 开垦
						tilemap.SetTile(cellPos, FindObjectOfType<GridController>().pen);
						easyGrid[cellPos.x, cellPos.y] = new SoilData();
					}
					// 耕地已经开垦, 判断是否种植了
					else if (!easyGrid[cellPos.x, cellPos.y].HasPlant) // 当前没有种子, 则放种子
					{
						ResController.Instance.seedPrefab
							.Instantiate()
							.Position(tileWorldPos);
						easyGrid[cellPos.x, cellPos.y].HasPlant = true;
					}
					
				}
			}

			else if (Input.GetMouseButtonDown(1))	// 右键消除土地
			{
				if (cellPos.x is < 10 and >= 0 && cellPos.y is >= 0 and < 10)
				{
					if (easyGrid[cellPos.x, cellPos.y] != null)
					{
						tilemap.SetTile(cellPos, null);
						easyGrid[cellPos.x, cellPos.y] = null;
					}
				}
			}
			else if (Input.GetKeyDown(KeyCode.E))	// W键浇水
			{
				if (cellPos.x is < 10 and >= 0 && cellPos.y is >= 0 and < 10)
				{
					if (easyGrid[cellPos.x, cellPos.y] != null)
					{
						if (!easyGrid[cellPos.x, cellPos.y].Watered)
						{
							easyGrid[cellPos.x, cellPos.y].Watered = true;
							ResController.Instance.waterPrefab
								.Instantiate()
								.Position(tileWorldPos);
						}
					}
				}
			}
		}

		private void OnGUI()
		{
			IMGUIHelper.SetDesignResolution(640,360);
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  天数: " + Global.Days.Value);
			GUILayout.EndHorizontal();
		}
	}
}
