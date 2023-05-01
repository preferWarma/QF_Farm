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
			for (var i = 0; i < Global.ActiveChallenges.Count; i++)
			{
				var change = Global.ActiveChallenges[i];
				GUI.Label(new Rect(640 - 200, 20 + i * 20, 200, 24), change.Name + ": " + change.State);
			}

			for (var i = 0; i < Global.FinishedChallenges.Count; i++)
			{
				var change = Global.FinishedChallenges[i];
				GUI.Label(new Rect(640 - 200, 20 + (i + Global.ActiveChallenges.Count) * 20, 200, 24),
					"<color=green>" + change.Name + ": " + change.State + "</color>");
			}
		}
	}
}
