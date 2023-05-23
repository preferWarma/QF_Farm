using QFramework;

namespace Game
{
	public partial class Home : ViewController
	{
		public void NextDay()
		{
			Global.Days.Value++;
		}
	}
}
