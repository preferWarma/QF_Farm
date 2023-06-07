using System.Collections.Generic;
using System.Linq;
using System.ToolBarSys;
using Game.Inventory;
using Game.Tools;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public partial class UIToolBar : ViewController, IController
	{
		public List<UISlot> ToolbarSlots { get; } = new();	// 工具栏槽
		private readonly Dictionary<int, KeyCode> _shotCutDict = new()	// 背包槽位置与快捷键的映射
		{
			{0, KeyCode.Alpha1},
			{1, KeyCode.Alpha2},
			{2, KeyCode.Alpha3},
			{3, KeyCode.Alpha4},
			{4, KeyCode.Alpha5},
			{5, KeyCode.Alpha6},
			{6, KeyCode.Alpha7},
			{7, KeyCode.Alpha8},
			{8, KeyCode.Alpha9},
			{9, KeyCode.Alpha0},
		};

		public Image harvestCollection;
		private IToolBarSystem mToolBarSystem;

		private void Awake()
		{
			mToolBarSystem = this.GetSystem<IToolBarSystem>();
			// HarvestCollection.Hide();
		}

		private void Start()
		{
			UISlot.SpriteLoader = ResController.Instance.LoadSprite;	// 设置图标加载器方法
			UISlot.OnUse = slot => SetCurrentTool(slot.ItemData?.Tool, slot.icon, slot.select);	// 设置使用方法
			ToolBarSystem.OnItemAdd.Register(AddItemSlot).UnRegisterWhenGameObjectDestroyed(this);	// 注册添加物品槽方法
			ToolBarSystem.OnItemRemove.Register(item =>
			{
				RemoveItemSlot(item);
				SetDefaultTool();
			}).UnRegisterWhenGameObjectDestroyed(this);	// 注册移除物品槽方法
			
			ToolbarSlots.Add(ToolbarSlot1);
			ToolbarSlots.Add(ToolbarSlot2);
			ToolbarSlots.Add(ToolbarSlot3);
			ToolbarSlots.Add(ToolbarSlot4);
			ToolbarSlots.Add(ToolbarSlot5);
			ToolbarSlots.Add(ToolbarSlot6);
			ToolbarSlots.Add(ToolbarSlot7);
			ToolbarSlots.Add(ToolbarSlot8);
			ToolbarSlots.Add(ToolbarSlot9);
			ToolbarSlots.Add(ToolbarSlot10);
			ToolbarSlots.Add(ToolbarSlot11);
			ToolbarSlots.Add(ToolbarSlot12);
			ToolbarSlots.Add(ToolbarSlot13);
			
			InitToolBarSlots();
			SetDefaultTool();
		}

		private void Update()
		{
			// 设置工具对应的快捷键
			for (var i = 0; i < _shotCutDict.Count; i++)
			{
				var shotCut = _shotCutDict[i];
				if (Input.GetKeyDown(shotCut))
				{
					if (ToolbarSlots[i])
						UISlot.OnUse?.Invoke(ToolbarSlots[i]);
				}
			}
		}

		#region ToolBar逻辑相关
		
		private void SetCurrentTool(ITool tool, Image icon, Image selectImg)
		{
			if (tool == null)	// 是果实而不是可用工具
			{
				Global.CurrentTool.Value = Global.ToolFruit;
				HideAllSelect();
				selectImg.Show();
				Global.Mouse.Icon.sprite = icon.sprite;
			}
			else
			{
				Global.CurrentTool.Value = tool; // 设置当前工具
				HideAllSelect();
				selectImg.Show();
				Global.Mouse.Icon.sprite = icon.sprite; // 设置鼠标图标
			}
		}

		private void HideAllSelect()
		{
			ToolbarSlots.ForEach(slot => slot.select.Hide());
		}
		
		private void InitToolBarSlots()
		{
			var toolbarSystem = this.GetSystem<IToolBarSystem>();
			
			for (var i = 0; i < ToolbarSlots.Count; i++)
			{
				var slot = ToolbarSlots[i];
				if (i < toolbarSystem.Items.Count)
					slot.SetSlotData(toolbarSystem.Items[i], (i+1).ToString());
				else 
					slot.SetSlotData(null, string.Empty);
			}
		}

		private void AddItemSlot(Item item)
		{
			UISlot slot = null;
			var idx = -1;
			for (var i = 0; i < ToolbarSlots.Count; i++)
			{
				if (ToolbarSlots[i].ItemData != null) continue;
				idx = i;
				slot = ToolbarSlots[i];
				break;
			}
			if (!slot) return;
			slot.SetSlotData(item, (idx + 1).ToString());
		}

		private void RemoveItemSlot(Item item)
		{
			var slot = ToolbarSlots.FirstOrDefault(s => s.ItemData == item);
			if (!slot) return;
			slot.SetSlotData(null,string.Empty);
		}

		public void SetDefaultTool()
		{
			var defaultTool = mToolBarSystem.Items.Find(item => item.name == ItemNameCollections.Hand);
			SetCurrentTool(defaultTool?.Tool, ToolbarSlots[0].icon, ToolbarSlots[0].select);	// 设置默认工具
		}
		
		#endregion

		public IArchitecture GetArchitecture()
		{
			return Global.Interface;
		}
	}
}
