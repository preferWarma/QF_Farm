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
			
			// 强化流程控制
			PowerUpSystem.IntensifiedToday.RegisterWithInitValue(todayIntensified =>
			{
				Title.text = todayIntensified ? "<color=red>今日已强化, 请明天再来</color>" : "强化!!!";
			}).UnRegisterWhenGameObjectDestroyed(this);
			
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
						self.Button.GetComponentInChildren<Text>().text = $"强化(${tmp.Price})";
						self.Description.text = tmp.Description;
						SetShowCondition(Global.Money, self.Button, tmp.ShowObjCondition, tmp.ShowBtnCondition);
					});
			}

		}

		private void SetShowCondition(BindableProperty<int> money, Button btn, Func<bool> showObjCondition, Func<bool> showBtnCondition)
		{
			money.RegisterWithInitValue(value =>
			{
				btn.transform.parent.gameObject.SetActive(false);
				if (!showObjCondition()) return;
				btn.transform.parent.gameObject.SetActive(true);
				
				if (showBtnCondition() && !PowerUpSystem.IntensifiedToday.Value)
				{
					btn.GetComponentInChildren<Text>().color = new Color(0.2f, 0.2f, 0.2f);
					btn.interactable = true;
				}
				else
				{
					btn.GetComponentInChildren<Text>().color = Color.gray;
					btn.interactable = false;
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
		}

		public IArchitecture GetArchitecture()
		{
			return Global.Interface;
		}
	}
}
