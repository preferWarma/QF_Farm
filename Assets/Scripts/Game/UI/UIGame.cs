using QFramework;

namespace Game.UI
{
	public partial class UIGame : ViewController
	{
		private void Start()
		{
			Global.Days.RegisterWithInitValue(day =>
			{
				DayText.text = $"第 {day} 天";
			}).UnRegisterWhenGameObjectDestroyed(this);

			Global.RestHours.RegisterWithInitValue(restHour =>
			{
				RestTimeText.text = $"{restHour : 0.0} 小时";
			}).UnRegisterWhenGameObjectDestroyed(this);

			Global.Money.RegisterWithInitValue(money =>
			{
				Money.text = $"$ {money}";
			}).UnRegisterWhenGameObjectDestroyed(this);
		}
	}
}
