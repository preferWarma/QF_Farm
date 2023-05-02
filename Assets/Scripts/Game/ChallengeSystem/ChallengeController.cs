 using Game.Plants;
 using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.ChallengeSystem
{
	public partial class ChallengeController : ViewController
	{
		private void Start()
		{
			// 挑战相关的事件注册
			RegisterOnChallengeFinish();
			RegisterOnPlantHarvest();
		}

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
		
		#region 挑战相关的事件注册
		
		private void RegisterOnChallengeFinish()
		{
			// 监听挑战完成
			Global.OnChallengeFinish.Register(challenge =>
			{
				AudioController.Instance.Sfx_Complete.Play(); // 播放挑战完成音效
				Global.FinishedChallenges.Add(challenge);
				Debug.Log($"完成挑战:{challenge.Name}");

				if (Global.Challenges.Count == Global.FinishedChallenges.Count) // 如果所有的挑战都完成了
				{
					ActionKit.Delay(1.0f, () => SceneManager.LoadScene("Scenes/GamePass"))
						.Start(this);
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
		}
		
		private void RegisterOnPlantHarvest()
		{
			// 监听植物采摘
			Global.OnPlantHarvest.Register(plant =>
			{
				Global.HarvestCountInCurrentDay.Value++;

				// 根据植物类型增加不同的水果数量
				if (plant as PlantRadish != null)
				{
					Global.RadishCount.Value++;
					Global.HarvestRadishCountInCurrentDay.Value++;
				}
				else if (plant as PlantPumpkin != null)
				{
					Global.PumpkinCount.Value++;
				}

				if (plant.RipeDay == Global.Days.Value) // 如果是当天成熟的植物被采摘
				{
					Global.RipeAndHarvestCountInCurrentDay.Value++;
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
		}
		
		#endregion
	}
}
