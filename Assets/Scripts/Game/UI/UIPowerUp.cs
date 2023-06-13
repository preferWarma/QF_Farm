using System;
using System.PowerUpSys;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public partial class UIPowerUp : ViewController, IController
	{
		private IPowerUpSystem mPowerUpSystem;
		
		private void Start()
		{
			mPowerUpSystem = this.GetSystem<IPowerUpSystem>();
			PowerUpItemTemplate.Hide();
			
			// 生成强化项
			foreach (var powerUp in mPowerUpSystem.PowerUps)
			{
				PowerUpItemTemplate.InstantiateWithParent(BtnRoot).Show()
					.Self(self =>
					{
						self.TextName.text = powerUp.Title;
						self.Price.text = $"价格{powerUp.Price}";
						var tmp = powerUp;
						self.Button.onClick.AddListener(() =>
						{
							tmp.OnUnlock();
						});
						self.Button.GetComponentInChildren<Text>().text = powerUp.Description;
						SetBtnShowCondition(Global.Money, self.gameObject, tmp.ShowCondition);// 设置按钮显示条件
					});
			}

			// 强化流程控制
			PowerUpSystem.IntensifiedToday.RegisterWithInitValue(todayIntensified =>
			{
				BtnRoot.GetComponentsInChildren<PowerUpItem>().ForEach(item =>
				{
					item.Button.interactable = !todayIntensified;
					item.Button.GetComponentInChildren<Text>().color = todayIntensified ? Color.gray : Color.black;
				});
				Title.text = todayIntensified ? "今日已强化, 请明天再来" : "强化!!!";
			}).UnRegisterWhenGameObjectDestroyed(this);

		}
		
		private void SetBtnShowCondition(BindableProperty<int> money, GameObject obj, Func<bool> showCondition)
		{
			money.RegisterWithInitValue(_ =>
			{
				obj.SetActive(showCondition());
			}).UnRegisterWhenGameObjectDestroyed(this);
		}

		public IArchitecture GetArchitecture()
		{
			return Global.Interface;
		}
	}
}
