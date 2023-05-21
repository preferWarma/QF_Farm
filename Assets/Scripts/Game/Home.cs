using UnityEngine;
using QFramework;

namespace Game
{
	public partial class Home : ViewController
	{
		public void NextDay()
		{
			ActionKit.Delay(0.5f, () =>
			{
				Global.Days.Value++;
			}).Start(this);
		}
	}
}
