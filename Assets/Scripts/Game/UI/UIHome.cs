using QFramework;
using UnityEngine.UI;

namespace Game.UI
{
	public partial class UIHome : ViewController
	{
		private float totalHours_1 = 100f;
		private float currentHours_1 = 0f;
		
		private void Start()
		{
			BtnCreateFirst.onClick.AddListener(() =>
			{
				if (currentHours_1 + Global.RestHours.Value >= totalHours_1)
				{
					Global.RestHours.Value -= totalHours_1 - currentHours_1;
					currentHours_1 = totalHours_1;
				}
				else
				{
					currentHours_1 += Global.RestHours.Value;
					Global.RestHours.Value = 0; // 消耗剩余时间
				}
				
				if (currentHours_1 >= totalHours_1)
				{
					currentHours_1 = totalHours_1;
					BtnCreateFirst.GetComponentInChildren<Text>().text = "项目1已完成";
					BtnCreateFirst.interactable = false;
				}
				else
				{
					BtnCreateFirst.GetComponentInChildren<Text>().text =
						$"项目1进行中...{currentHours_1 :0.0}/{totalHours_1 :0.0}";
				}
			});
		}
	}
}
