using System;
using QFramework;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.UI
{
	public partial class UIHome : ViewController
	{
		private float totalHours_1 = 100f;
		private BindableProperty<float> currentHours_1 = new(0f);
		private bool isFinished_1 = false;
		
		private void Awake()
		{
			RegisterGlobal();
			RegisterProject();
			RegisterWork();
		}

		// 注册全局事件
		private void RegisterGlobal()
		{
			Global.Days.Register(_ =>
			{
				var cost = Random.Range(5, 8 + 1);
				Global.Money.Value -= cost;	// 每天扣5块钱
				UIMessageQueue.Push(null,$"昨日消耗$-${cost}");
			
				if (isFinished_1)
				{
					var earn = Random.Range(5,8+1);
					Global.Money.Value += earn;
					UIMessageQueue.Push(null,$"项目1昨天收入+${earn}元");
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
		}

		// 注册项目事件
		private void RegisterProject()
		{
			currentHours_1.RegisterWithInitValue(cur =>
			{
				if (cur >= totalHours_1)
				{
					BtnCreateFirst.GetComponentInChildren<Text>().text = "项目1已完成(+$(5-8)/天)";
					BtnCreateFirst.interactable = false;
					isFinished_1 = true;
				}
				else
				{
					BtnCreateFirst.GetComponentInChildren<Text>().text =
						$"项目1进行中...{cur :0.0}/{totalHours_1 :0.0}";
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
			
			BtnCreateFirst.onClick.AddListener(() =>
			{
				if (currentHours_1 + Global.RestHours.Value >= totalHours_1)
				{
					Global.RestHours.Value -= totalHours_1 - currentHours_1;
					currentHours_1.Value = totalHours_1;
				}
				else
				{
					currentHours_1.Value += Global.RestHours.Value;
					Global.RestHours.Value = 0; // 消耗剩余时间
				}
				AudioController.Instance.Sfx_Trade.Play();
			});
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
				UIMessageQueue.Push(null,$"打工收入${perHourIncome * Global.RestHours.Value:0.0}");
				Global.RestHours.Value = 0;
				AudioController.Instance.Sfx_Trade.Play();
			});
		}
		
	}
}
