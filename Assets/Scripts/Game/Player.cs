using System;
using System.ToolBarSys;
using Game.UI;
using UnityEngine;
using QFramework;
using UnityEditor;

// ReSharper disable Unity.InefficientPropertyAccess

namespace Game
{
	public partial class Player : ViewController
	{
		public float moveSpeed = 5f;
		[Tooltip("移动平滑度")] public float smooth = 10f;
		
		private Rigidbody2D mRigidbody;
		
		private void Awake()
		{
			Global.Player = this;
			mRigidbody = GetComponent<Rigidbody2D>();
		}
		
		#region 作弊菜单
		
		[MenuItem("Lyf/游戏/金钱无限")]
		public static void MoneyMAX()
		{
			Global.Money.Value = 999999;
		}
		
		[MenuItem("Lyf/游戏/今日时间无限")]
		public static void RestHoursMAX()
		{
			Global.RestHours.Value = 999999;
		}
		
		[MenuItem("Lyf/游戏/全部果实数量MAX")]
		public static void FruitMAX()
		{
			Global.Interface.SendCommand(new AddItemCountCommand(ItemNameCollections.Pumpkin, 999));
			Global.Interface.SendCommand(new AddItemCountCommand(ItemNameCollections.Radish, 999));
			Global.Interface.SendCommand(new AddItemCountCommand(ItemNameCollections.Tomato, 999));
			Global.Interface.SendCommand(new AddItemCountCommand(ItemNameCollections.Bean, 999));
			Global.Interface.SendCommand(new AddItemCountCommand(ItemNameCollections.Potato, 999));
		}
		
		#endregion
		
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				Global.Days.Value++;
			}
			
			var horizontal = Input.GetAxisRaw("Horizontal");
			var vertical = Input.GetAxisRaw("Vertical");
			var direction = new Vector2(horizontal, vertical).normalized;
			
			var targetVelocity = direction * moveSpeed;
			mRigidbody.velocity = Vector2.Lerp(mRigidbody.velocity, targetVelocity, 1-Mathf.Exp(-Time.deltaTime * smooth));	// 平滑移动
		}

		private void OnDestroy()
		{
			Global.Player = null;
		}
	}
}
