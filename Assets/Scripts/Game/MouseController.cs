using Game.Data;
using Game.Plants;
using UnityEngine;
using QFramework;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
// ReSharper disable Unity.InefficientPropertyAccess

namespace Game
{
	public partial class MouseController : ViewController
	{
		private Grid mgrid;
		private Camera mCamera;
		private SpriteRenderer mSpriteRenderer;
		private EasyGrid<SoilData> mshowGrid;
		private GridController mGridController;
		private Tilemap mTilemap;

		private void Awake()
		{
			Global.Mouse = this;
		}

		private void Start()
		{
			mGridController = FindObjectOfType<GridController>();
			mgrid = mGridController.GetComponent<Grid>();
			mCamera = Camera.main;
			mSpriteRenderer = GetComponent<SpriteRenderer>();
			mSpriteRenderer.enabled = false;	// 默认是隐藏的
			mshowGrid = mGridController.ShowGrid;
			mTilemap = mGridController.Tilemap;
		}
		
		private void Update()
		{
			var playerCellPos = mgrid.WorldToCell(Global.Player.transform.position);	// 获取玩家所在的格子位置
			var worldMousePoint = mCamera.ScreenToWorldPoint(Input.mousePosition);	// 获取鼠标所在的世界坐标
			var cellPosition = mgrid.WorldToCell(worldMousePoint);		// 获取鼠标所在的格子位置

			Icon.Position(worldMousePoint.x, worldMousePoint.y);	// 设置鼠标图标的位置
			
			if (Mathf.Abs(playerCellPos.x - cellPosition.x) <= 1 && Mathf.Abs(playerCellPos.y - cellPosition.y) <= 1)	// 鼠标在玩家周围
			{
				if (cellPosition.x is < 10 and >= 0 && cellPosition.y is < 10 and >= 0)	// 鼠标在地图内
				{
					DoOnMouse0(cellPosition);
					mSpriteRenderer.enabled = true;
					var gridCenterPosition = mgrid.GetCellCenterWorld(cellPosition); // 获取格子中心点的世界坐标
					gridCenterPosition -= mgrid.cellSize * 0.5f;
					transform.position = gridCenterPosition; // 将鼠标对应的格子位置显示出来
				}
			}
			else
			{
				mSpriteRenderer.enabled = false;
			}
		}

		private void OnDestroy()
		{
			Global.Mouse = null;
		}

		private void DoOnMouse0(Vector3Int cellPos)
		{
			if (!Input.GetMouseButton(0)) return; // 鼠标左键没有按下则不处理
			if (EventSystem.current.IsPointerOverGameObject()) return;	// 如果点击到了UI则不处理

			var tileWorldPos = mgrid.GetCellCenterWorld(cellPos);

			if (mshowGrid[cellPos.x, cellPos.y] == null)	// 无耕地
			{
				if (Global.CurrentTool.Value != Constant.ToolShovel) return; // 当前工具不是锄头则不开垦
					
				AudioController.Instance.Sfx_DigSoil.Play();	// 播放开垦音效
				mTilemap.SetTile(cellPos, mGridController.pen);
				mshowGrid[cellPos.x, cellPos.y] = new SoilData();
			}
			else // 有耕地
			{
				if (!mshowGrid[cellPos.x, cellPos.y].Watered) //当前土地没有浇水
				{
					if (Global.CurrentTool.Value == Constant.ToolWateringCan)	// 当前工具是水壶
					{
						AudioController.Instance.Sfx_Watering.Play(); // 播放浇水音效
						mshowGrid[cellPos.x, cellPos.y].Watered = true;
						ResController.Instance.waterPrefab
							.Instantiate()
							.Position(tileWorldPos);
					}
				}
				
				if (!mshowGrid[cellPos.x, cellPos.y].HasPlant) // 当前土地没有植物则种植
				{
					if (Global.CurrentTool.Value != Constant.ToolSeedPumpkin && Global.CurrentTool.Value != Constant.ToolSeedRadish) return; // 当前工具不是种子则不种植
					
					GameObject plantObj = null;
					if (Global.CurrentTool.Value == Constant.ToolSeedPumpkin && Global.PumpKinSeedCount.Value > 0)	// 根据当前工具种植不同的植物
					{
						plantObj = ResController.Instance.plantPrefab
						.Instantiate()
						.Position(tileWorldPos);
						Global.PumpKinSeedCount.Value--;
					}
					else if (Global.CurrentTool.Value == Constant.ToolSeedRadish && Global.RadishSeedCount.Value > 0)
					{
						plantObj = ResController.Instance.plantRadishPrefab
						.Instantiate()
						.Position(tileWorldPos);
						Global.RadishSeedCount.Value--;
					}
					if (plantObj == null) return;
					AudioController.Instance.Sfx_PutSeed.Play(); // 播放种植音效
					var plant = plantObj.GetComponent<IPlant>();
					plant.X = cellPos.x;
					plant.Y = cellPos.y;
					plant.SetState(PlantSates.Seed);

					PlantController.Instance.PlantGrid[cellPos.x, cellPos.y] = plant;
					mshowGrid[cellPos.x, cellPos.y].HasPlant = true;
				} 
				else // 当前土地有植物则判断是否成熟, 成熟则收获
				{
					if (Global.CurrentTool.Value != Constant.ToolHand) return; // 当前工具不是手则不收获
					if (mshowGrid[cellPos.x, cellPos.y].PlantSates != PlantSates.Ripe) return; // 不成熟则不收获
					// 摘取, 切换状态, 增加水果数量
					// PlantController.Instance.PlantGrid[cellPos.x, cellPos.y].SetState(PlantSates.Old);

					AudioController.Instance.Sfx_Harvest.Play(); // 播放收割音效
					Global.OnPlantHarvest.Trigger(PlantController.Instance.PlantGrid[cellPos.x, cellPos.y]); // 触发收获事件
					
					Destroy(PlantController.Instance.PlantGrid[cellPos.x, cellPos.y].GameObject); // 摘取后销毁, 简化流程,后期会改
					mshowGrid[cellPos.x, cellPos.y].HasPlant = false;
					mshowGrid[cellPos.x, cellPos.y].PlantSates = PlantSates.Seed; // 摘取后下一次变成种子(有待改进)
					// Global.PumpkinCount.Value++;
				}
			}
		}
	}
}
