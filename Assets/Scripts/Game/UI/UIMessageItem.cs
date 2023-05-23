using QFramework;

namespace Game.UI
{
	public partial class UIMessageItem : ViewController
	{
		public void SetAlpha(float alpha)
		{
			Icon.ColorAlpha(alpha);
			TextWithIcon.ColorAlpha(alpha);
		}
	}
}
