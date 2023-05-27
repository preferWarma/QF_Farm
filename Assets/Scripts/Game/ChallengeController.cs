using System.ChallengeSys;
using QFramework;
using UnityEngine;

namespace Game
{
	public partial class ChallengeController : ViewController
	{
		private void OnGUI()
		{
			IMGUIHelper.SetDesignResolution(640, 480);
			GUI.Label(new Rect(640 - 200, 0, 200, 24), "挑战列表");
			for (var i = 0; i < ChallengeSystem.ActiveChallenges.Count; i++)
			{
				var change = ChallengeSystem.ActiveChallenges[i];
				GUI.Label(new Rect(640 - 200, 20 + i * 20, 200, 24), change.Name + ": " + change.State);
			}

			for (var i = 0; i < ChallengeSystem.FinishedChallenges.Count; i++)
			{
				var change = ChallengeSystem.FinishedChallenges[i];
				GUI.Label(new Rect(640 - 200, 20 + (i + ChallengeSystem.ActiveChallenges.Count) * 20, 200, 24),
					"<color=green>" + change.Name + ": " + change.State + "</color>");
			}
		}
	}
}
