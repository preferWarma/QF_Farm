using System;
using System.ToolBarSys;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public partial class UIShop : ViewController, IController
	{
		[Header("控制游戏流程")]
		public static readonly BindableProperty<bool> CanShowRadishSeed = new();
		public static readonly BindableProperty<bool> CanShowPotatoSeed = new();
		public static readonly BindableProperty<bool> CanShowTomatoSeed = new();
		public static readonly BindableProperty<bool> CanShowBeanSeed = new();
		
		// 避免单独创建新的函数
		private static readonly BindableProperty<bool> CanShowPumpkinSeed = new(true);
		private static readonly BindableProperty<bool> CanShowComputer = new(true);

		private void Start()
		{
			// 固定物品
			RegisterBuyComputer(BtnBuyComputer, Global.Money, ItemNameCollections.Computer, 500);
			SetBtnShowCondition(Global.Money, BtnBuyComputer, money => money >= 500 && !Global.HasComputer,CanShowComputer);
			
			// 购买型物品
			CreateBuyItem("南瓜种子", ItemNameCollections.SeedPumpkin, 
				6, "存活率90%,成熟时间4天\n成熟果实售价10元/个", CanShowPumpkinSeed);
			CreateBuyItem("胡萝卜种子", ItemNameCollections.SeedRadish,
				8, "存活率80%,成熟时间5天\n成熟果实售价12元/个", CanShowRadishSeed);
			CreateBuyItem("土豆种子", ItemNameCollections.SeedPotato,
				15, "存活率75%,成熟时间5天\n成熟果实售价30元/个", CanShowPotatoSeed);
			CreateBuyItem("番茄种子", ItemNameCollections.SeedTomato,
				20, "存活率60%,成熟时间5天\n成熟果实售价50元/个", CanShowTomatoSeed);
			CreateBuyItem("豆荚种子", ItemNameCollections.SeedBean,
				25, "存活率50%,成熟时间7天\n成熟果实售价100元/个", CanShowBeanSeed);
			
			// 出售型物品
			CreateSellItem("南瓜", ItemNameCollections.Pumpkin, 10, "成熟果实售价10元/个", Global.PumpkinCount);
			CreateSellItem("胡萝卜", ItemNameCollections.Radish, 12, "成熟果实售价12元/个", Global.RadishCount);
			CreateSellItem("土豆", ItemNameCollections.Potato, 30, "成熟果实售价30元/个", Global.PotatoCount);
			CreateSellItem("番茄", ItemNameCollections.Tomato, 50, "成熟果实售价50元/个", Global.TomatoCount);
			CreateSellItem("豆荚", ItemNameCollections.Bean, 100, "成熟果实售价100元/个", Global.BeanCount);
		}

		
		/// <summary>
		/// 创建购买型物品
		/// </summary>
		/// <param name="uiShowName">在UI上显示的名字</param>
		/// <param name="itemName">物品名</param>
		/// <param name="price">价格</param>
		/// <param name="description">描述</param>
		/// <param name="seedCanShow">是否可见</param>
		private void CreateBuyItem(string uiShowName, string itemName, int price, string description, BindableProperty<bool> seedCanShow)
		{
			var item = ShopItemTemplate.InstantiateWithParent(BtnRoot.transform);
			item.Icon.sprite = ResController.Instance.LoadSprite(itemName);
			item.Name.text = uiShowName;
			item.Description.text = description;
			item.Button.GetComponentInChildren<Text>().text = $"购买(${price})";
			SetBtnShowCondition(Global.Money, item.Button, money => money >= price, seedCanShow);
			RegisterBuy(item.Button, Global.Money, itemName, price);
		}
		
		/// <summary>
		/// 创建出售型物品
		/// </summary>
		/// <param name="uiShowName">在UI上显示的名字</param>
		/// <param name="itemName">物品名</param>
		/// <param name="price">价格</param>
		/// <param name="description">描述</param>
		/// <param name="fruitCount">数量</param>
		private void CreateSellItem(string uiShowName, string itemName, int price, string description, BindableProperty<int> fruitCount)
		{
			var item = ShopItemTemplate.InstantiateWithParent(BtnRoot.transform);
			item.Icon.sprite = ResController.Instance.LoadSprite(itemName);
			item.Name.text = uiShowName;
			item.Description.text = description;
			item.Button.GetComponentInChildren<Text>().text = $"出售(${price})";
			SetBtnShowCondition(fruitCount, item.Button, count => count > 0);
			RegisterSell(item.Button, itemName, Global.Money, price);
		}
		
		/// <summary>
		/// 设置按钮显示条件
		/// </summary>
		/// <param name="item"> 显示条件对象 </param>
		/// <param name="btn"> 当前设置的按钮 </param>
		/// <param name="showCondition"> 显示的条件 </param>
		private void SetBtnShowCondition(BindableProperty<int> item, Button btn, Func<int, bool> showCondition)
		{
			item.RegisterWithInitValue(countValue =>
			{
				btn.transform.parent.gameObject.SetActive(showCondition(countValue));
			}).UnRegisterWhenGameObjectDestroyed(this);
		}

		/// <summary>
		/// 设置按钮显示条件
		/// </summary>
		/// <param name="item"> 显示条件对象 </param>
		/// <param name="btn"> 当前设置的按钮 </param>
		/// <param name="showCondition"> 显示的条件 </param>
		/// <param name="isCreated"> 是否已经创建 </param>
		private void SetBtnShowCondition(BindableProperty<int> item, Button btn, Func<int, bool> showCondition, BindableProperty<bool> isCreated)
		{
			isCreated.RegisterWithInitValue(v => btn.transform.parent.gameObject.SetActive(v));
			
			item.RegisterWithInitValue(countValue =>
			{
				if (showCondition(countValue))
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

		/// <summary>
		/// 注册购买按钮的方法
		/// </summary>
		/// <param name="btnBuy"> 当前设置的按钮 </param>
		/// <param name="money"> 货币对象 </param>
		/// <param name="itemName"> 购买的物品名 </param>
		/// <param name="buyPrice"> 购买单价 </param>
		private void RegisterBuy(Button btnBuy, BindableProperty<int> money, string itemName, int buyPrice)
		{
			btnBuy.onClick.AddListener(() =>
			{
				money.Value -= buyPrice;
				this.SendCommand(new AddItemCountCommand(itemName, 1));
				UIMessageQueue.Push(ResController.Instance.LoadSprite(itemName), $"+1\t金币-{buyPrice}");
				AudioController.Instance.Sfx_Trade.Play();
			});
		}

		private void RegisterBuyComputer(Button btnBuy, BindableProperty<int> money, string itemName, int buyPrice)
		{
			btnBuy.onClick.AddListener(() =>
			{
				Global.HasComputer = true;
				money.Value -= buyPrice;
				UIMessageQueue.Push(ResController.Instance.LoadSprite(itemName), $"已解锁,请回家查看\t金币-{buyPrice}");
				AudioController.Instance.Sfx_Trade.Play();
			});
		}

		/// <summary>
		/// 注册出售按钮的方法
		/// </summary>
		/// <param name="btnSell"> 当前设置的按钮 </param>
		/// <param name="itemName"> 需要出售的物品的名称 </param>
		/// <param name="money"> 货币对象 </param>
		/// <param name="sellPrice"> 出售单价 </param>
		private void RegisterSell(Button btnSell, string itemName, BindableProperty<int> money, int sellPrice)
		{
			btnSell.onClick.AddListener(() =>
			{
				money.Value += sellPrice;
				this.SendCommand(new SubItemCountCommand(itemName, 1));
				UIMessageQueue.Push(ResController.Instance.LoadSprite(itemName), $"-1\t金币+{sellPrice}");
				AudioController.Instance.Sfx_Trade.Play();
			});
		}

		public IArchitecture GetArchitecture()
		{
			return Global.Interface;
		}
	}
}
