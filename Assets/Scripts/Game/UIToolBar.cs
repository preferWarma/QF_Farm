using System.Collections.Generic;
using System.Linq;
using Game.Tools;
using UnityEngine;
using QFramework;
using UnityEngine.UI;

namespace Game
{
	public partial class UIToolBar : ViewController
	{
		private List<UISlot> ToolbarSlots = new();
		private Dictionary<int, ITool> ToolDic = new()	// 背包槽与工具的映射
		{
			{0, Constant.ToolHand},
			{1, Constant.ToolShovel},
			{2, Constant.ToolSeedPumpkin},
			{3, Constant.ToolWateringCan},
			{4, Constant.ToolSeedRadish},
			{5, Constant.ToolSeedPotato}
		};
		private Dictionary<int, KeyCode> ShotCutDic = new()
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
			
			for (var i = 0; i < ToolDic.Count; i++)
			{
				var i1 = i;	// 闭包, 不能直接使用i, 否则注册的事件都是最后一个i
				ToolbarSlots[i].GetComponent<Button>().onClick.AddListener(() =>
				{
					SetCurrentTool(ToolDic[i1], ToolbarSlots[i1].icon, ToolbarSlots[i1].select);
				});
				ToolbarSlots[i].shotCut.text = (i + 1).ToString();
			}
		}

		private void Update()
		{
			for (var i = 0; i < ShotCutDic.Count; i++)
			{
				var shotCut = ShotCutDic[i];
				if (Input.GetKeyDown(shotCut))
				{
					SetCurrentTool(ToolDic[i], ToolbarSlots[i].icon, ToolbarSlots[i].select);
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
			ToolbarSlots.ForEach(slot => slot.select.Hide());
		}
	}
}
