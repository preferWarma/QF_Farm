using UnityEngine;
using QFramework;

// ReSharper disable Unity.InefficientPropertyAccess

namespace Game
{
	public partial class Player : ViewController
	{
		private void Awake()
		{
			Global.Player = this;
		}
		
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Q))	// 按下Q键，进入下一天
			{
				Global.Days.Value++;
			}
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
			GUILayout.Label(" <color=yellow>$金币: " + Global.Money.Value + "</color>");
			GUILayout.EndHorizontal();
		}

		private void OnDestroy()
		{
			Global.Player = null;
		}
	}
}
