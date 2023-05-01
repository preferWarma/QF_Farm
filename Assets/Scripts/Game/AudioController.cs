using UnityEngine;
using QFramework;

namespace Game
{
	public partial class AudioController : ViewController, ISingleton
	{
		public static AudioController Instance => MonoSingletonProperty<AudioController>.Instance;

		public void OnSingletonInit()
		{
			
		}
	}
}
