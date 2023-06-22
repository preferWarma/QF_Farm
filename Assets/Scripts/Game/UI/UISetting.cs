using QFramework;
using UnityEngine;

namespace Game.UI
{
	public partial class UISetting : ViewController
	{
		private bool _isOpen;
		private UIChallenge _uiChallenge;

		private void Awake()
		{
			_uiChallenge = FindObjectOfType<UIChallenge>();
			SoundSlider.value = 0.5f;
			_isOpen = gameObject.activeSelf;
		}

		private void Start()
		{
			BtnQuit.onClick.AddListener(Application.Quit);
			BtnHide.onClick.AddListener(() =>
			{
				_isOpen = false;
				UIShowOrHideController.DoCloseOrOpen(SettingsRoot.gameObject, _isOpen);
			});
			ToggleShowChallenge.onValueChanged.AddListener(value => { UIShowOrHideController.DoCloseOrOpen(_uiChallenge.gameObject, value); });
			SoundSlider.onValueChanged.AddListener(value => { AudioController.Instance.BackGround.volume = value; });
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				_isOpen = !_isOpen;
				UIShowOrHideController.DoCloseOrOpen(SettingsRoot.gameObject, _isOpen);
			}
		}
	}
}
