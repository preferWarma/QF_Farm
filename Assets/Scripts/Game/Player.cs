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
				var soilDatas = FindObjectOfType<GridController>().ShowGrid;
				
				PlantController.Instance.PlantGrid.ForEach((x, y, plant) =>
				{
					if (plant is null)	return;
					if (plant.Sate == PlantSates.Seed)	// 如果是种子
					{
						if (soilDatas[x, y].Watered)	// 如果已经浇水了, 切换为幼苗
						{
							plant.SetState(PlantSates.Small);
						}
					}
					else if (plant.Sate == PlantSates.Small)	// 如果是幼苗
					{
						if (soilDatas[x, y].Watered)	// 如果已经浇水了, 切换为成熟
						{
							plant.SetState(PlantSates.Ripe);
						}
					}
				});
 
				soilDatas.ForEach(data =>
				{
					if (data is null)	return;
					data.Watered = false;	// 过了一天，所有的土地都没有水
				});

				var waters = SceneManager.GetActiveScene().GetRootGameObjects()
					.Where(obj => obj.name.StartsWith("Water"));
				waters.ForEach(water => { water.DestroySelf(); });	// 过了一天，所有的水都消失了
				
			}).UnRegisterWhenGameObjectDestroyed(gameObject);
		}

		private void Update()
		{

			if (Input.GetKeyDown(KeyCode.Q))	// 按下Q键，进入下一天
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
					else if (!easyGrid[cellPos.x, cellPos.y].HasPlant) // 当前土地没有植物则种植
					{
						var plantObj = ResController.Instance.plantPrefab
							.Instantiate()
							.Position(tileWorldPos);
						var plant = plantObj.GetComponent<Plant>();
						plant.x = cellPos.x;
						plant.y = cellPos.y;
						
						PlantController.Instance.PlantGrid[cellPos.x, cellPos.y] = plant;
						easyGrid[cellPos.x, cellPos.y].HasPlant = true;
					}
					else // 当前土地有植物则判断是否成熟, 成熟则收获
					{
						if (easyGrid[cellPos.x, cellPos.y].PlantSates == PlantSates.Ripe)
						{
							// 摘取, 切换状态, 增加水果数量
							PlantController.Instance.PlantGrid[cellPos.x, cellPos.y].SetState(PlantSates.Old);
							Global.Fruits.Value++;
						}
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
			else if (Input.GetKeyDown(KeyCode.E))	// E键浇水
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
			
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  果子: " + Global.Fruits.Value);
			GUILayout.EndHorizontal();
		}
	}
}
