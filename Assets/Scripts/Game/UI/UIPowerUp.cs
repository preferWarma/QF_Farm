using System;
using QFramework;
using UnityEngine.UI;

namespace Game.UI
{
	public partial class UIPowerUp : ViewController
	{
		private void Start()
		{
			// 设置按钮显示条件
			SetBtnShowCondition(Global.Money, BtnUpgradeHand, money => money >= 10 && !Global.IsToolUpgraded[0]);
			SetBtnShowCondition(Global.Money, BtnUpgradeShovel, money => money >= 20 && !Global.IsToolUpgraded[1]);
			SetBtnShowCondition(Global.Money, BtnUpgradeWateringCan, money => money >= 30 && !Global.IsToolUpgraded[2]);
			
			// 注册按钮事件
			BtnUpgradeHand.onClick.AddListener(() =>
			{
				Global.IsToolUpgraded[0] = true;
				Global.Money.Value -= 10;
				Config.Hand.Tool.ToolScope++;	// 手的工作范围增加
				AudioController.Instance.Sfx_Trade.Play();
			});
			
			BtnUpgradeShovel.onClick.AddListener(() =>
			{
				Global.IsToolUpgraded[1] = true;
				Global.Money.Value -= 20;
				Config.Shovel.Tool.ToolScope++;	// 锄头的工作范围增加
				AudioController.Instance.Sfx_Trade.Play();
			});
			
			BtnUpgradeWateringCan.onClick.AddListener(() =>
			{
				Global.IsToolUpgraded[2] = true;
				Global.Money.Value -= 30;
				Config.WateringCan.Tool.ToolScope++;	// 水壶的工作范围增加
				AudioController.Instance.Sfx_Trade.Play();
			});
		}
		
		// 同UIShop.cs
		private void SetBtnShowCondition(BindableProperty<int> item, Button btn, Func<int, bool> showCondition)
		{
			item.RegisterWithInitValue(countValue =>
			{
				btn.gameObject.SetActive(showCondition(countValue));
			}).UnRegisterWhenGameObjectDestroyed(this);
		}
	}
}
