using System;
using QFramework;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.UI
{
	public partial class UIHome : ViewController
	{
		private const float TotalHours1 = 100f;
		private readonly BindableProperty<float> _currentHours1 = new();
		private bool _isFinished1;
		private bool _project1Registered;
		
		private void Awake()
		{
			RegisterGlobal();
			RegisterWork();
		}

		private void Update()
		{
			if (Global.HasComputer && !_project1Registered)
			{
				RegisterProject();
				_project1Registered = true;
			}
		}

		// 注册全局事件
		private void RegisterGlobal()
		{
			Global.Days.Register(_ =>
			{
				var cost = Random.Range(5, 8 + 1);
				Global.Money.Value -= cost;	// 每天扣5块钱
				UIMessageQueue.Push($"昨日消耗$-${cost}");
			
				if (_isFinished1)
				{
					var earn = Random.Range(5,8+1);
					Global.Money.Value += earn;
					UIMessageQueue.Push($"项目1昨天收入+${earn}元");
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
		}

		// 注册项目事件
		private void RegisterProject()
		{
			_currentHours1.RegisterWithInitValue(cur =>
			{
				if (cur >= TotalHours1)
				{
					BtnCreateFirst.GetComponentInChildren<Text>().text = "项目1已完成(+$(5-8)/天)";
					BtnCreateFirst.interactable = false;
					_isFinished1 = true;
				}
				else
				{
					BtnCreateFirst.GetComponentInChildren<Text>().text =
						$"项目1进行中...{cur :0.0}/{TotalHours1 :0.0}";
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
			
			BtnCreateFirst.onClick.AddListener(() =>
			{
				if (_currentHours1 + Global.RestHours.Value >= TotalHours1)
				{
					Global.RestHours.Value -= TotalHours1 - _currentHours1;
					_currentHours1.Value = TotalHours1;
				}
				else
				{
					_currentHours1.Value += Global.RestHours.Value;
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
				UIMessageQueue.Push($"打工收入${perHourIncome * Global.RestHours.Value:0.0}");
				Global.RestHours.Value = 0;
				AudioController.Instance.Sfx_Trade.Play();
			});
		}
		
	}
}
