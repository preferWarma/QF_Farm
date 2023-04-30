using System;
using UnityEngine;
using QFramework;

namespace Game
{
	public partial class ChallengeController : ViewController
	{
		private void OnGUI()
		{
			IMGUIHelper.SetDesignResolution(640, 480);
			GUI.Label(new Rect(640 - 200, 0, 200, 24), "挑战列表");
			for (var i = 0; i < Global.Challenges.Count; i++)
			{
				var change = Global.Challenges[i];
				GUI.Label(new Rect(640 - 200, 20 + i * 20, 200, 24), change.Name + ": " + change.State);
			}
		}
	}
}
