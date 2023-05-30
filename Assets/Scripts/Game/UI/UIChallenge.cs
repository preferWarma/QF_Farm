using System.ChallengeSys;
using QFramework;
using UnityEngine.UI;

namespace Game.UI
{
	public partial class UIChallenge : ViewController, IController
	{
		public Text challengeItemPrefab;
		
		private IChallengeSystem mChallengeSystem;

		private void Awake()
		{
			mChallengeSystem = this.GetSystem<IChallengeSystem>();
		}

		private void Update()
		{
			UpdateView();
		}


		private void UpdateView()
		{
			UIChallengeContentRoot.DestroyChildren();
			
			foreach (var activeChallenge in mChallengeSystem.ActiveChallenges)
			{
				challengeItemPrefab.InstantiateWithParent(UIChallengeContentRoot)
					.Self(self =>
					{
						self.text = $"<color=yellow>[进行中]</color> {activeChallenge.Name}";
					}).Show();
			}
			
			foreach (var finishedChallenge in mChallengeSystem.FinishedChallenges)
			{
				challengeItemPrefab.InstantiateWithParent(UIChallengeContentRoot)
					.Self(self =>
					{
						self.text = $"<color=green>[已完成]</color> {finishedChallenge.Name}";
					}).Show();
			}
			
		}
		
		public IArchitecture GetArchitecture()
		{
			return Global.Interface;
		}
	}
}
