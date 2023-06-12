using System;
using System.PowerUpSys;
using QFramework;
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
							tmp.Unlock();
						});
						self.Button.GetComponentInChildren<Text>().text = powerUp.Description;
					});
			}

			// // 设置按钮显示条件
			// SetBtnShowCondition(Global.Money, BtnUpgradeHand, money => money >= 10 && !PowerUpSystem.IsToolUpgraded[0]);
			// SetBtnShowCondition(Global.Money, BtnUpgradeShovel, money => money >= 20 && !PowerUpSystem.IsToolUpgraded[1]);
			// SetBtnShowCondition(Global.Money, BtnUpgradeWateringCan, money => money >= 30 && !PowerUpSystem.IsToolUpgraded[2]);
			// SetBtnShowCondition(Global.Money, BtnUpgradeSeed, money => money >= 40 && !PowerUpSystem.IsToolUpgraded[3]);
			
			// // 注册按钮事件
			// BtnUpgradeHand.onClick.AddListener(() =>
			// {
			// 	PowerUpSystem.IsToolUpgraded[0] = true;
			// 	Global.Money.Value -= 10;
			// 	AudioController.Instance.Sfx_Trade.Play();
			// });
			//
			// BtnUpgradeShovel.onClick.AddListener(() =>
			// {
			// 	PowerUpSystem.IsToolUpgraded[1] = true;
			// 	Global.Money.Value -= 20;
			// 	AudioController.Instance.Sfx_Trade.Play();
			// });
			//
			// BtnUpgradeWateringCan.onClick.AddListener(() =>
			// {
			// 	PowerUpSystem.IsToolUpgraded[2] = true;
			// 	Global.Money.Value -= 30;
			// 	AudioController.Instance.Sfx_Trade.Play();
			// });
			//
			// BtnUpgradeSeed.onClick.AddListener(() =>
			// {
			// 	PowerUpSystem.IsToolUpgraded[3] = true;	// 种子工具升级
			// 	Global.Money.Value -= 40;
			// 	AudioController.Instance.Sfx_Trade.Play();
			// });
		}
		
		// 同UIShop.cs
		private void SetBtnShowCondition(BindableProperty<int> item, Button btn, Func<int, bool> showCondition)
		{
			item.RegisterWithInitValue(countValue =>
			{
				btn.gameObject.SetActive(showCondition(countValue));
			}).UnRegisterWhenGameObjectDestroyed(this);
		}

		public IArchitecture GetArchitecture()
		{
			return Global.Interface;
		}
	}
}
