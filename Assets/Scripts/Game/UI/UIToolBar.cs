using System;
using System.Collections.Generic;
using System.Linq;
using Game.Inventory;
using Game.Tools;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	public partial class UIToolBar : ViewController, IController
	{
		private readonly List<UISlot> _toolbarSlots = new();	// 工具栏槽
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

		private void Start()
		{
			UISlot.SpriteLoader = ResController.Instance.LoadSprite;	// 设置图标加载器方法
			UISlot.OnUse = slot => SetCurrentTool(slot.ItemData?.Tool, slot.icon, slot.select);	// 设置使用方法
			
			_toolbarSlots.Add(ToolbarSlot1);
			_toolbarSlots.Add(ToolbarSlot2);
			_toolbarSlots.Add(ToolbarSlot3);
			_toolbarSlots.Add(ToolbarSlot4);
			_toolbarSlots.Add(ToolbarSlot5);
			_toolbarSlots.Add(ToolbarSlot6);
			_toolbarSlots.Add(ToolbarSlot7);
			_toolbarSlots.Add(ToolbarSlot8);
			_toolbarSlots.Add(ToolbarSlot9);
			_toolbarSlots.Add(ToolbarSlot10);
			
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
					if (_toolbarSlots[i])
						UISlot.OnUse?.Invoke(_toolbarSlots[i]);
				}
			}
		}

		private void SetCurrentTool(ITool tool, Image icon, Image selectImg)
		{
			if (tool == null)
			{
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
			_toolbarSlots.ForEach(slot => slot.select.Hide());
		}
		
		private void InitToolBarSlots()
		{
			var toolbarSystem = this.GetSystem<IToolBarSystem>();
			
			for (var i = 0; i < _toolbarSlots.Count; i++)
			{
				var slot = _toolbarSlots[i];
				if (i < toolbarSystem.Items.Count)
					slot.SetSlotData(toolbarSystem.Items[i], (i+1).ToString());
				else 
					slot.SetSlotData(null, string.Empty);
			}
		}

		public void AddItemSlot(Item item)
		{
			UISlot slot = null;
			var idx = -1;
			for (var i = 0; i < _toolbarSlots.Count; i++)
			{
				if (_toolbarSlots[i].ItemData != null) continue;
				idx = i;
				slot = _toolbarSlots[i];
				break;
			}
			if (!slot) return;
			slot.SetSlotData(item, (idx + 1).ToString());
		}
		
		public void RemoveItemSlot(Item item)
		{
			var slot = _toolbarSlots.FirstOrDefault(s => s.ItemData == item);
			if (!slot) return;
			slot.SetSlotData(null,string.Empty);
		}

		public void SetDefaultTool()
		{
			var toolbarSystem = this.GetSystem<IToolBarSystem>();
			var defaultTool = toolbarSystem.Items.Find(item => item.name == ItemNameCollections.Hand);
			SetCurrentTool(defaultTool?.Tool, _toolbarSlots[0].icon, _toolbarSlots[0].select);	// 设置默认工具
		}

		public IArchitecture GetArchitecture()
		{
			return Global.Interface;
		}
	}
}
