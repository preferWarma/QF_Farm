using QFramework;

namespace Game.UI
{
	public partial class UIShop : ViewController
	{
		private void Start()
		{
			RegisterRadish();
			RegisterPumpkin();
			BtnBuyPumpkin.onClick.AddListener(() =>
			{
				Global.PumpkinCount.Value--;
				Global.PumpKinSeedCount.Value += 2;
				
				AudioController.Instance.Sfx_Trade.Play();
			});
			BtnBuyRadish.onClick.AddListener(() =>
			{
				Global.RadishCount.Value--;
				Global.RadishSeedCount.Value += 2;
				
				AudioController.Instance.Sfx_Trade.Play();
			});
		}

		private void RegisterPumpkin()
		{
			Global.PumpkinCount.RegisterWithInitValue(count =>
			{
				if (count >= 1)
				{
					BtnBuyPumpkin.Show();
				}
				else
				{
					BtnBuyPumpkin.Hide();
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
		}

		private void RegisterRadish()
		{
			Global.RadishCount.RegisterWithInitValue(count =>
			{
				if (count >= 1)
				{
					BtnBuyRadish.Show();
				}
				else
				{
					BtnBuyRadish.Hide();
				}
			}).UnRegisterWhenGameObjectDestroyed(this);
		}
	}
}
