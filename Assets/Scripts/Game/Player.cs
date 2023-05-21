using UnityEngine;
using QFramework;

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
		
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Q))	// 按下Q键，进入下一天
			{
				Global.Days.Value++;
			}
			
			var horizontal = Input.GetAxisRaw("Horizontal");
			var vertical = Input.GetAxisRaw("Vertical");
			var direction = new Vector2(horizontal, vertical).normalized;
			
			var targetVelocity = direction * moveSpeed;
			mRigidbody.velocity = Vector2.Lerp(mRigidbody.velocity, targetVelocity, 1-Mathf.Exp(-Time.deltaTime * smooth));	// 平滑移动
		}

		private void OnGUI()
		{
			// 显示提示信息
			IMGUIHelper.SetDesignResolution(720, 360);
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label(" 天数: " + Global.Days.Value);
			GUILayout.EndHorizontal();
			
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label($" 剩余小时: {Global.RestHours.Value :0.0}");
			GUILayout.EndHorizontal();

			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label(" <color=yellow>$金币: " + Global.Money.Value + "</color>");
			GUILayout.EndHorizontal();
		}

		private void OnDestroy()
		{
			Global.Player = null;
		}
	}
}
