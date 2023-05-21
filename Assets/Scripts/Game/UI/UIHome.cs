using QFramework;

namespace Game.UI
{
	public partial class UIHome : ViewController
	{
		private void Start()
		{
			BtnCreateFirst.onClick.AddListener(() =>
			{
				Global.RestHours.Value = 0;	// 消耗剩余时间
			});
		}
	}
}
