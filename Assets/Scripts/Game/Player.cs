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
		[Tooltip("地块网格")] public Grid grid;
		[Tooltip("地块显示")] public Tilemap tilemap;

		private void Start()
		{
			Global.Days.Register(day =>
			{
				Global.RipeAndHarvestCountInCurrentDay.Value = 0;	// 每天开始时，重置成熟的水果数量
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
			
			if (Input.GetKeyDown(KeyCode.Space))
			{
				SceneManager.LoadScene("Scenes/GamePass");
			}

			if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				Global.CurrentTool.Value = Constant.ToolHand;
			}
			
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				Global.CurrentTool.Value = Constant.ToolShovel;
			}

			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				Global.CurrentTool.Value = Constant.ToolWateringCan;
			}
			
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				Global.CurrentTool.Value = Constant.ToolSeed;
			}
			
			// 根据player的position值拿到tilemap的具体块
			var cellPos = grid.WorldToCell(transform.position);
			var showGrid = FindObjectOfType<GridController>().ShowGrid;
			
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
					if (showGrid[cellPos.x, cellPos.y] == null)	// 无耕地
					{
						if (Global.CurrentTool.Value != Constant.ToolShovel) return; // 当前工具不是锄头则不开垦
						tilemap.SetTile(cellPos, FindObjectOfType<GridController>().pen);
						showGrid[cellPos.x, cellPos.y] = new SoilData();
					}
					// 耕地已经开垦, 判断是否种植了
					else if (!showGrid[cellPos.x, cellPos.y].HasPlant) // 当前土地没有植物则种植
					{
						if (Global.CurrentTool.Value != Constant.ToolSeed) return; // 当前工具不是种子则不种植
						var plantObj = ResController.Instance.plantPrefab
							.Instantiate()
							.Position(tileWorldPos);
						var plant = plantObj.GetComponent<Plant>();
						plant.x = cellPos.x;
						plant.y = cellPos.y;
						plant.SetState(PlantSates.Seed);
						
						PlantController.Instance.PlantGrid[cellPos.x, cellPos.y] = plant;
						showGrid[cellPos.x, cellPos.y].HasPlant = true;
					}
					else // 当前土地有植物则判断是否成熟, 成熟则收获
					{
						if (Global.CurrentTool.Value != Constant.ToolHand) return; // 当前工具不是手则不收获
						if (showGrid[cellPos.x, cellPos.y].PlantSates != PlantSates.Ripe) return;	// 不成熟则不收获
						// 摘取, 切换状态, 增加水果数量
						// PlantController.Instance.PlantGrid[cellPos.x, cellPos.y].SetState(PlantSates.Old);

						Global.OnPlantHarvest.Trigger(PlantController.Instance.PlantGrid[cellPos.x, cellPos.y]);	// 触发收获事件
						
						Destroy(PlantController.Instance.PlantGrid[cellPos.x, cellPos.y].gameObject);	// 摘取后销毁, 简化流程,后期会改
						showGrid[cellPos.x, cellPos.y].HasPlant = false;
						showGrid[cellPos.x, cellPos.y].PlantSates = PlantSates.Seed;// 摘取后下一次变成种子(有待改进)
						Global.Fruits.Value++;
					}
					
				}
			}

			else if (Input.GetMouseButtonDown(1))	// 右键消除土地
			{
				if (cellPos.x is < 10 and >= 0 && cellPos.y is >= 0 and < 10)
				{
					if (showGrid[cellPos.x, cellPos.y] != null)
					{
						tilemap.SetTile(cellPos, null);
						showGrid[cellPos.x, cellPos.y] = null;
					}
				}
			}
			else if (Input.GetKeyDown(KeyCode.E))	// E键浇水
			{
				if (cellPos.x is >= 10 or < 0 || cellPos.y is < 0 or >= 10) return;	// 超出范围
				if (showGrid[cellPos.x, cellPos.y] == null) return;	// 无耕地
				if (showGrid[cellPos.x, cellPos.y].Watered) return;	// 已经浇过水了
				if (Global.CurrentTool.Value != Constant.ToolWateringCan) return;	// 当前工具不是水壶
				showGrid[cellPos.x, cellPos.y].Watered = true;
				ResController.Instance.waterPrefab
					.Instantiate()
					.Position(tileWorldPos);
			}
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
			GUILayout.Label("  果子: " + Global.Fruits.Value);
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
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("  摘取: 鼠标左键");
			GUILayout.EndHorizontal();
			
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  消除: 鼠标右键");
			GUILayout.EndHorizontal();GUILayout.Space(10);

			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label($"  当前工具: {Constant.DisplayName(Global.CurrentTool.Value, Language.Chinese)}");
			GUILayout.EndHorizontal();GUILayout.Space(10);
			
			GUI.Label(new Rect(10, 320, 300, 24), "切换工具: [0]手, [1]锄头, [2]水壶, [3]种子");
		}
	}
}
