using QFramework;
using UnityEngine;

namespace Game.UI
{
	public partial class UISetting : ViewController
	{
		private bool _isOpen;

		private void Start()
		{
			SoundSlider.value = 0.5f;
			BtnQuit.onClick.AddListener(Application.Quit);
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				_isOpen = !_isOpen;
				SettingsRoot.gameObject.SetActive(_isOpen);
			}
			
			UpdateSound();
		}

		private void UpdateSound()
		{
			AudioController.Instance.BackGround.volume = SoundSlider.value;
		}

	}
}
