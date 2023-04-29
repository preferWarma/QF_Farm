using QFramework;

namespace Game
{
	public partial class GameController : ViewController
	{
		private void Start()
		{
			Global.Fruits.Register(Achievement_1);
			Global.Days.Register(Achievement_2);
		}

		private void Achievement_1(int fruitCount)	// 成就1: 采集1个水果
		{
			// if (fruitCount >= 1)
			// {
			// 	// 1s后跳转到GamePass场景
			// 	ActionKit.Delay(1.0f, () =>
			// 	{
			// 		SceneManager.LoadScene("Scenes/GamePass");
			// 	}).Start(this);
			// }
		}

		private void Achievement_2(int _)	// 成就2: 一天结出两个果实并采摘
		{
		}
	}
}
