using UnityEngine;
using QFramework;

namespace Game
{
	public partial class TimeController : ViewController
	{
		public static float Seconds;	// 游玩时长

		private void Start()
		{
			Seconds = 0f;
		}

		private void Update()
		{
			Seconds += Time.deltaTime;
		}

		private void OnGUI()
		{
			IMGUIHelper.SetDesignResolution(640, 360);
			GUI.Label(new Rect(640-50, 360 - 20, 640 - 100,360-40), $"{(int)Seconds}s");
		}

		private void OnDestroy()
		{
			Debug.Log($"总共用时{(int)Seconds}s");
		}
	}
}
