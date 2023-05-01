using System;
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
			SeedBtn.onClick.AddListener(() => {SetCurrentTool(Constant.ToolSeed, SeedBtnSelect);});
			WatercanBtn.onClick.AddListener(() => {SetCurrentTool(Constant.ToolWateringCan, WatercanBtnSelect);});
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
				SetCurrentTool(Constant.ToolSeed, SeedBtnSelect);
			}
			
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
				SetCurrentTool(Constant.ToolWateringCan, WatercanBtnSelect);
			}
		}
		
		private void SetCurrentTool(string toolName, Image selectImg)
		{
			Global.CurrentTool.Value = toolName;
			HideAllSelect();
			selectImg.Show();
			Global.Player.CurrentToolIcon.sprite = toolName switch
			{
				Constant.ToolHand => HandBtnImage.sprite,
				Constant.ToolShovel => ShovelBtnImage.sprite,
				Constant.ToolSeed => SeedBtnImage.sprite,
				Constant.ToolWateringCan => WatercanBtnImage.sprite,
				_ => throw new ArgumentOutOfRangeException(nameof(toolName), toolName, null)
			};	// 在Player上显示当前工具
		}

		private void HideAllSelect()
		{
			HandBtnSelect.Hide();
			ShovelBtnSelect.Hide();
			SeedBtnSelect.Hide();
			WatercanBtnSelect.Hide();
		}
	}
}
