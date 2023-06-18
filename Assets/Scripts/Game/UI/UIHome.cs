using System;
using QFramework;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.UI
{
	public partial class UIHome : ViewController
	{
		private void Awake()
		{
			RegisterGlobal();
			RegisterWork();
		}

		// 注册全局事件
		private void RegisterGlobal()
		{
			Global.Days.Register(_ =>
			{
				var cost = Random.Range(Global.DailyCost, Global.DailyCost + 6);
				Global.Money.Value -= cost;
				UIMessageQueue.Push($"昨日消耗$-${cost}");
			}).UnRegisterWhenGameObjectDestroyed(this);
		}

		// 注册打工事件
		private void RegisterWork()
		{
			Global.RestHours.Register(rest =>
			{
				if (rest < 1f)
				{
					BtnWork.interactable = false;
					BtnWork.GetComponentInChildren<Text>().text = "打工一小时起步";
				}
				else
				{
					BtnWork.interactable = true;
					BtnWork.GetComponentInChildren<Text>().text = "打工($1-2/h)";
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
			
			BtnWork.onClick.AddListener(() =>
			{
				var perHourIncome = Random.Range(1f, 2f);
				Global.Money.Value += Convert.ToInt32(perHourIncome * Global.RestHours.Value);
				UIMessageQueue.Push($"打工收入${perHourIncome * Global.RestHours.Value:0.0}");
				Global.RestHours.Value = 0;
				AudioController.Instance.Sfx_Trade.Play();
			});
		}
		
	}
}
