using System;
using QFramework;
using UnityEditor;
using UnityEngine;

namespace Game.UI
{
	public partial class UIMessageQueue : ViewController
	{
		private static UIMessageQueue mInstance;
		
		public static void Push(Sprite icon, string message)
		{
			mInstance.UIMessageTemplate.InstantiateWithParent(mInstance.MessageRoot)
				.Self(self =>
				{
					
					self.Icon.Hide();
					self.Icon.sprite = icon;
					self.TextWithIcon.text = message;
					if (icon)
					{
						self.Icon.Show();
					}
					self.TextWithIcon.Show();
					self.SetAlpha(0);
					ActionKit.Sequence()
						.Lerp(0, 1f, 0.5f, self.SetAlpha)	// 0.3秒内渐变到1
						.Delay(2f)
						.Lerp(1, 0f, 1.5f, self.SetAlpha)	// 1.5秒内渐变到0
						.Start(self, self.DestroyGameObj);
				}).Show();
		}

		private void Awake()
		{
			mInstance = this;
			UIMessageTemplate.Hide();
		}

		private void OnDestroy()
		{
			mInstance = null;
		}
	}
}
