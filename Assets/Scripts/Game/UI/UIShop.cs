using System;
using QFramework;
using UnityEngine.UI;

namespace Game.UI
{
	public partial class UIShop : ViewController, IController
	{
		private void Start()
		{
			// 注册购买植物种子按钮的方法
			RegisterBuySeed(BtnBuyPumpkinSeed, Global.Money,ItemNameCollections.SeedPumpkin , 1);
			RegisterBuySeed(BtnBuyRadishSeed, Global.Money, ItemNameCollections.SeedRadish, 2);
			RegisterBuySeed(BtnBuyPotatoSeed, Global.Money, ItemNameCollections.SeedPotato, 3);
			RegisterBuySeed(BtnBuyTomatoSeed, Global.Money, ItemNameCollections.SeedTomato, 4);
			RegisterBuySeed(BtnBuyBeanSeed, Global.Money, ItemNameCollections.SeedBean, 5);
			
			// 注册出售植物按钮的方法
			RegisterSellPlant(BtnSellPumpkin, ItemNameCollections.Pumpkin, Global.Money, 2);
			RegisterSellPlant(BtnSellRadish, ItemNameCollections.Radish, Global.Money, 4);
			RegisterSellPlant(BtnSellPotato, ItemNameCollections.Potato, Global.Money, 6);
			RegisterSellPlant(BtnSellTomato, ItemNameCollections.Tomato, Global.Money, 8);
			RegisterSellPlant(BtnSellBean, ItemNameCollections.Bean, Global.Money, 10);
			
			// 买按钮的显示条件
			SetBtnShowCondition(Global.Money, BtnBuyPumpkinSeed, money => money >= 1);
			SetBtnShowCondition(Global.Money, BtnBuyRadishSeed, money => money >= 2);
			SetBtnShowCondition(Global.Money, BtnBuyPotatoSeed, money => money >= 3);
			SetBtnShowCondition(Global.Money, BtnBuyTomatoSeed, money => money >= 4);
			SetBtnShowCondition(Global.Money, BtnBuyBeanSeed, money => money >= 5);
			
			// 卖按钮的显示条件
			SetBtnShowCondition(Global.PumpkinCount, BtnSellPumpkin, count => count > 0);
			SetBtnShowCondition(Global.RadishCount, BtnSellRadish, count => count > 0);
			SetBtnShowCondition(Global.PotatoCount, BtnSellPotato, count => count > 0);
			SetBtnShowCondition(Global.TomatoCount, BtnSellTomato, count => count > 0);
			SetBtnShowCondition(Global.BeanCount, BtnSellBean, count => count > 0);
		}
		
		/// <summary>
		/// 设置按钮显示条件
		/// </summary>
		/// <param name="item"> 显示条件的对象 </param>
		/// <param name="btn"> 当前设置的按钮 </param>
		/// <param name="showCondition"> 显示的条件 </param>
		private void SetBtnShowCondition(BindableProperty<int> item, Button btn, Func<int, bool> showCondition)
		{
			item.RegisterWithInitValue(countValue =>
			{
				btn.gameObject.SetActive(showCondition(countValue));
			}).UnRegisterWhenGameObjectDestroyed(this);
		}

		/// <summary>
		/// 注册购买植物种子按钮的方法
		/// </summary>
		/// <param name="btnBuy"> 当前设置的按钮 </param>
		/// <param name="money"> 货币对象 </param>
		/// <param name="itemName"> 购买的物品名 </param>
		/// <param name="buyPrice"> 购买单价 </param>
		private void RegisterBuySeed(Button btnBuy, BindableProperty<int> money, string itemName, int buyPrice)
		{
			btnBuy.onClick.AddListener(() =>
			{
				money.Value -= buyPrice;
				this.SendCommand(new AddItemCountCommand(itemName, 1));
				UIMessageQueue.Push(ResController.Instance.LoadSprite(itemName), $"+1\t金币-{buyPrice}");
				AudioController.Instance.Sfx_Trade.Play();
			});
		}

		/// <summary>
		/// 注册出售植物按钮的方法
		/// </summary>
		/// <param name="btnSell"> 当前设置的按钮 </param>
		/// <param name="fruitName"> 需要出售的植物的名称 </param>
		/// <param name="money"> 货币对象 </param>
		/// <param name="sellPrice"> 出售单价 </param>
		private void RegisterSellPlant(Button btnSell, string fruitName, BindableProperty<int> money, int sellPrice)
		{
			btnSell.onClick.AddListener(() =>
			{
				money.Value += sellPrice;
				this.SendCommand(new SubItemCountCommand(fruitName, 1));
				UIMessageQueue.Push(ResController.Instance.LoadSprite(fruitName), $"-1\t金币+{sellPrice}");
				AudioController.Instance.Sfx_Trade.Play();
			});
		}

		public IArchitecture GetArchitecture()
		{
			return Global.Interface;
		}
	}
}
