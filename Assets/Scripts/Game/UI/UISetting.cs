using DG.Tweening;
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
		}

		private void Start()
		{
			BtnQuit.onClick.AddListener(Application.Quit);
			BtnHide.onClick.AddListener(() =>
			{
				_isOpen = false;
				DoCloseOrOpen(SettingsRoot.gameObject, _isOpen);
			});
			ToggleShowChallenge.onValueChanged.AddListener(value => { DoCloseOrOpen(_uiChallenge.gameObject, value); });
			SoundSlider.onValueChanged.AddListener(value => { AudioController.Instance.BackGround.volume = value; });
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				_isOpen = !_isOpen;
				DoCloseOrOpen(SettingsRoot.gameObject, _isOpen);
			}
		}

		// 使用DoTween实现界面的打开和关闭动画
		private static void DoCloseOrOpen(GameObject obj, bool isOpen)
		{
			if (isOpen)
			{
				obj.transform.DOScale(Vector3.one, 0.3f)
					.SetEase(Ease.OutCubic)
					.startValue = Vector3.zero;
			}
			else
			{
				obj.transform.DOScale(Vector3.zero, 0.3f)
					.SetEase(Ease.InQuad)
					.startValue = Vector3.one;
			}
		}
	}
}
