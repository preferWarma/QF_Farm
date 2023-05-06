using Game.Tools;
using UnityEngine;
using QFramework;
using UnityEngine.UI;

namespace Game
{
	public partial class UIToolBar : ViewController
	{
		private void Start()
		{
			SetCurrentTool(Constant.ToolHand, HandBtnSelect);
			
			HandBtn.onClick.AddListener(() => {SetCurrentTool(Constant.ToolHand, HandBtnSelect);});
			ShovelBtn.onClick.AddListener(() => {SetCurrentTool(Constant.ToolShovel, ShovelBtnSelect);});
			SeedPumpkinBtn.onClick.AddListener(() => {SetCurrentTool(Constant.ToolSeedPumpkin, SeedBtnPumpkinSelect);});
			WatercanBtn.onClick.AddListener(() => {SetCurrentTool(Constant.ToolWateringCan, WatercanBtnSelect);});
			SeedRadishBtn.onClick.AddListener(() => {SetCurrentTool(Constant.ToolSeedRadish, SeedBtnRadishSelect);});
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				SetCurrentTool(Constant.ToolHand, HandBtnSelect);
			}
		
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				SetCurrentTool(Constant.ToolShovel, ShovelBtnSelect);
			}

			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
				SetCurrentTool(Constant.ToolSeedPumpkin, SeedBtnPumpkinSelect);
			}
			
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				SetCurrentTool(Constant.ToolWateringCan, WatercanBtnSelect);
			}

			if (Input.GetKeyDown(KeyCode.Alpha5))
			{
				SetCurrentTool(Constant.ToolSeedRadish, SeedBtnRadishSelect);
			}
		}
		
		private void SetCurrentTool(ITool tool, Image selectImg)
		{
			Global.CurrentTool.Value = tool;	// 设置当前工具
			HideAllSelect();
			selectImg.Show();
			Sprite showIcon = null;

			// 获取当前工具的图标
			if (tool == Constant.ToolHand) showIcon = HandBtnImage.sprite;
			if (tool == Constant.ToolShovel) showIcon = ShovelBtnImage.sprite;
			if (tool == Constant.ToolWateringCan) showIcon = WatercanBtnImage.sprite;
			if (tool == Constant.ToolSeedPumpkin) showIcon = SeedBtnPumpkinImage.sprite;
			if (tool == Constant.ToolSeedRadish) showIcon = SeedBtnRadishImage.sprite;
			
			Global.Mouse.Icon.sprite = showIcon;	// 设置鼠标图标
		}

		private void HideAllSelect()
		{
			HandBtnSelect.Hide();
			ShovelBtnSelect.Hide();
			WatercanBtnSelect.Hide();
			SeedBtnPumpkinSelect.Hide();
			SeedBtnRadishSelect.Hide();
		}
	}
}
