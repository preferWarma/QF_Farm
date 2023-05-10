using Game.ChallengeSystem;
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
			IMGUIHelper.SetDesignResolution(720,480);
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  天数: " + Global.Days.Value);
			GUILayout.EndHorizontal();
			
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  南瓜果子: " + Global.PumpkinCount.Value);
			GUILayout.EndHorizontal();

			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  胡萝卜果子: " + Global.RadishCount.Value);
			GUILayout.EndHorizontal();

			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  土豆果子: " + Global.PotatoCount.Value);
			GUILayout.EndHorizontal();

			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Label("  当天成熟并采摘的数量: " + ChallengeController.RipeAndHarvestCountInCurrentDay.Value);
			GUILayout.EndHorizontal();
		}

		private void OnDestroy()
		{
			Global.Player = null;
		}
	}
}
