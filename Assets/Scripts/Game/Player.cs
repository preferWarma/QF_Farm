using System.ToolBarSys;
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
		
		private Rigidbody2D _rigidbody;
		private Animator _animator;
		private bool _isMoving;
		private float _moveDirection;

		private void Awake()
		{
			Global.Player = this;
			_rigidbody = GetComponent<Rigidbody2D>();
			_animator = GetComponent<Animator>();
		}
		
		#region 作弊菜单
		
		[MenuItem("Lyf/游戏/金钱无限")]
		public static void MoneyMax()
		{
			Global.Money.Value = 999999;
		}
		
		[MenuItem("Lyf/游戏/今日时间无限")]
		public static void RestHoursMax()
		{
			Global.RestHours.Value = 999999;
		}
		
		[MenuItem("Lyf/游戏/全部果实数量MAX")]
		public static void FruitMax()
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

			_moveDirection = Input.GetAxis("Horizontal");
			_isMoving = Mathf.Abs(_moveDirection) > 0.01f;

			if (CameraController.CanMove)
			{
				Move();
			}

			UpdateAnimation();
		}

		private void Move()
		{
			var horizontal = Input.GetAxisRaw("Horizontal");
			var vertical = Input.GetAxisRaw("Vertical");
			var direction = new Vector2(horizontal, vertical).normalized;
			
			var targetVelocity = direction * moveSpeed;
			_rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, targetVelocity, 1-Mathf.Exp(-Time.deltaTime * smooth));	// 平滑移动
		}
		
		private void UpdateAnimation()	// 更新角色动画
		{
			var horizontal = Input.GetAxisRaw("Horizontal");
			var vertical = Input.GetAxisRaw("Vertical");
			if (horizontal == 0 && vertical == 0)
			{
				var info = _animator.GetCurrentAnimatorStateInfo(0);
				if (info.IsName("walk_right"))
				{
					_animator.Play("idle_right");
				}
				else if (info.IsName("walk_left"))
				{
					_animator.Play("idle_left");
				}
			}
			else
			{
				_animator.Play(horizontal >= 0 ? "walk_right" : "walk_left");
			}
		}

		private void OnDestroy()
		{
			Global.Player = null;
		}
	}
}
