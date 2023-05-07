using System.Collections.Generic;
using Game.Tools;
using UnityEngine;
using QFramework;
using UnityEngine.UI;

namespace Game
{
	public partial class UIToolBar : ViewController
	{
		private List<UISlot> ToolbarSlots = new();

		private Dictionary<int, ITool> ToolIdx = new()	// 背包槽与工具的映射
		{
			{0, Constant.ToolHand},
			{1, Constant.ToolShovel},
			{2, Constant.ToolSeedPumpkin},
			{3, Constant.ToolSeedRadish},
			{4, Constant.ToolWateringCan},
			{5, Constant.ToolSeedPotato}
		};

		private void Start()
		{
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
			
			SetCurrentTool(Constant.ToolHand, ToolbarSlots[0].icon, ToolbarSlots[0].select);
			
			for (var i = 0; i < ToolIdx.Count; i++)
			{
				var i1 = i;	// 闭包, 不能直接使用i, 否则注册的事件都是最后一个i
				ToolbarSlots[i].GetComponent<Button>().onClick.AddListener(() =>
				{
					SetCurrentTool(ToolIdx[i1], ToolbarSlots[i1].icon, ToolbarSlots[i1].select);
				});
				ToolbarSlots[i].shotCut.text = (i + 1).ToString();
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				SetCurrentTool(Constant.ToolHand, ToolbarSlots[0].icon, ToolbarSlots[0].select);
			}
		
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				SetCurrentTool(Constant.ToolShovel, ToolbarSlots[1].icon, ToolbarSlots[1].select);
			}

			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				SetCurrentTool(Constant.ToolSeedPumpkin, ToolbarSlots[2].icon, ToolbarSlots[2].select);
			}
			
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				SetCurrentTool(Constant.ToolWateringCan, ToolbarSlots[3].icon, ToolbarSlots[3].select);
			}

			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				SetCurrentTool(Constant.ToolSeedRadish, ToolbarSlots[4].icon, ToolbarSlots[4].select);
			}
			
			if (Input.GetKeyDown(KeyCode.Alpha6))
			{
				SetCurrentTool(Constant.ToolSeedPotato, ToolbarSlots[5].icon, ToolbarSlots[5].select);
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
			foreach (var slot in ToolbarSlots)
			{
				slot.select.Hide();
			}
		}
	}
}
