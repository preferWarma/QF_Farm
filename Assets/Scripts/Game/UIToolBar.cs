using System.Collections.Generic;
using Game.Inventory;
using Game.Tools;
using UnityEngine;
using QFramework;
using UnityEngine.UI;

namespace Game
{
	public partial class UIToolBar : ViewController
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
		};

		private void Start()
		{
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
			
			SetCurrentTool(Constant.ToolHand, _toolbarSlots[0].icon, _toolbarSlots[0].select);	// 设置默认工具
			
			InitToolBarSlots();
		}

		private void Update()
		{
			// 设置工具对应的快捷键
			for (var i = 0; i < _shotCutDict.Count; i++)
			{
				var shotCut = _shotCutDict[i];
				if (Input.GetKeyDown(shotCut))
				{
					_toolbarSlots[i].SlotData?.OnSelect?.Invoke();
				}
			}
		}

		private void SetCurrentTool(ITool tool, Image icon, Image selectImg)
		{
			Global.CurrentTool.Value = tool;	// 设置当前工具
			HideAllSelect();
			selectImg.Show();
			Global.Mouse.Icon.sprite = icon.sprite;	// 设置鼠标图标
		}

		private void HideAllSelect()
		{
			_toolbarSlots.ForEach(slot => slot.select.Hide());
		}
		
		private void InitToolBarSlots()
		{
			for (var i = 0; i < _toolbarSlots.Count; i++)
			{
				var slot = _toolbarSlots[i];
				if (i >= Config.Items.Count) break;	// 如果配置表没有对应的物品, 则结束
				
				var i1 = i;	// 闭包, 不能直接使用i, 否则注册的事件都是最后一个i
				var newSlotData = new SlotData
				{
					Icon = ResController.Instance.LoadSprite(Config.Items[i1].iconName),	// 动态加载图标
					OnSelect = () => { SetCurrentTool(Config.Items[i1].Tool, slot.icon, slot.select); }	// 添加执行事件
				};
				slot.SetSlotData(newSlotData, (i+1).ToString());
				_toolbarSlots[i].GetComponent<Button>().onClick.AddListener(() => slot.SlotData?.OnSelect?.Invoke());	// 注册事件到Button
			}
		}
	}
}
