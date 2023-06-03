using System.SoilSys;
using DG.Tweening;
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
		private Grid mGrid;
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
			mGrid = mGridController.GetComponent<Grid>();
			mCamera = Camera.main;
			mSpriteRenderer = GetComponent<SpriteRenderer>();
			mSpriteRenderer.enabled = false;	// 默认是隐藏的
			mshowGrid = mGridController.ShowGrid;
			mTilemap = mGridController.Soil;
			
			Global.Days.Register(_ => TimeNotEnough.gameObject.SetActive(false))
				.UnRegisterWhenGameObjectDestroyed(this);	// 天数改变时隐藏时间不够提示
			Global.CurrentTool.Register(tool =>
			{
				TimeNotEnough.gameObject.SetActive(tool.CostHours > Global.RestHours.Value);
			}).UnRegisterWhenGameObjectDestroyed(this);
		}
		
		private void LateUpdate()
		{
			Global.CurrentTool.Value.CdTime -= Time.deltaTime;
			
			var playerCellPos = mGrid.WorldToCell(Global.Player.transform.position);	// 获取玩家所在的格子位置
			var worldMousePoint = mCamera.ScreenToWorldPoint(Input.mousePosition);	// 获取鼠标所在的世界坐标
			var mouseCellPos = mGrid.WorldToCell(worldMousePoint);		// 获取鼠标所在的格子位置

			Icon.Alpha(1.0f);
			Icon.Position(worldMousePoint.x, worldMousePoint.y);	// 设置鼠标图标的位置
			if (TimeNotEnough.gameObject.activeSelf)
			{
				TimeNotEnough.transform.position = Icon.transform.position;
			}
			if (Global.CurrentTool.Value == null) return;	// 如果选择的是植物果实则不处理
			if (InToolRange(playerCellPos, mouseCellPos, Global.CurrentTool.Value.ToolScope))	// 在工具周围内
			{
				if (mouseCellPos.x < mshowGrid.Width && mouseCellPos.x >= 0 &&
				    mouseCellPos.y < mshowGrid.Height && mouseCellPos.y >= 0)	// 鼠标在地图内
				{
					DoOnMouse0(mouseCellPos);
					mSpriteRenderer.enabled = true;
					var gridCenterPosition = mGrid.GetCellCenterWorld(mouseCellPos); // 获取格子中心点的世界坐标
					gridCenterPosition -= mGrid.cellSize * 0.5f;
					transform.position = gridCenterPosition; // 将鼠标对应的格子位置显示出来
				}
			}
			else
			{
				Icon.Alpha(0.5f);
				mSpriteRenderer.enabled = false;
			}
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
				if (Global.CurrentTool.Value.CdTime <= 0)
				{
					Global.CurrentTool.Value.Use(toolNeedData);
					Global.CurrentTool.Value.CdTime = Global.CurrentTool.Value.InitCdTime;
				}
			}
		}

		// 检测工具是否在范围内
		private bool InToolRange(Vector3Int playerCellPos, Vector3Int mouseCellPos, int range)
		{
			return Mathf.Abs(playerCellPos.x - mouseCellPos.x) <= range && Mathf.Abs(playerCellPos.y - mouseCellPos.y) <= range;
		}
		
		public static void RotateIcon()	// 旋转图标
		{
			var randomRotation = RandomUtility.Choose(-360, 360);
			
			Global.Mouse.Icon.transform.DORotate(new Vector3(0, 0, randomRotation), 0.3f, RotateMode.FastBeyond360)	// 旋转图标
				.SetEase(Ease.OutCubic);	// 设置旋转的缓动
		}
		
		private void OnDestroy()
        {
         	Global.Mouse = null;
        }
	}
}
