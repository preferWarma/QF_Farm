using System;
using UnityEngine;
using QFramework;

namespace Game
{
	public partial class MouseController : ViewController
	{
		private Grid mgrid;
		private Camera mCamera;
		private SpriteRenderer mSpriteRenderer;
		
		private void Start()
		{
			mgrid = FindObjectOfType<GridController>().GetComponent<Grid>();
			mCamera = Camera.main;
			mSpriteRenderer = GetComponent<SpriteRenderer>();
			mSpriteRenderer.enabled = false;	// 默认是隐藏的
		}
		
		private void Update()
		{
			var playerCellPos = mgrid.WorldToCell(Global.Player.transform.position);	// 获取玩家所在的格子位置
			var worldMousePoint = mCamera.ScreenToWorldPoint(Input.mousePosition);
			var cellPosition = mgrid.WorldToCell(worldMousePoint);

			if (Mathf.Abs(playerCellPos.x - cellPosition.x) <= 1 && Mathf.Abs(playerCellPos.y - cellPosition.y) <= 1)
			{
				mSpriteRenderer.enabled = true;
				var gridCenterPosition = mgrid.GetCellCenterWorld(cellPosition); // 获取格子中心点的世界坐标
				gridCenterPosition -= mgrid.cellSize * 0.5f;
				transform.position = gridCenterPosition; // 将鼠标对应的格子位置显示出来
			}
			else
			{
				mSpriteRenderer.enabled = false;
			}
		}
	}
}
