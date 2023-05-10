using Game.Data;
using Game.Tools;
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
			mTilemap = mGridController.Soil;
		}
		
		private void LateUpdate()
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

			var toolNeedData = new ToolNeedData
			{
				ShowGrid = mshowGrid,
				CellPos = cellPos,
				Tilemap = mTilemap,
				Pen = mGridController.pen
			};
			
			if (Global.CurrentTool.Value.Selected())
			{
				Global.CurrentTool.Value.Use(toolNeedData);
			}
		}
	}
}
