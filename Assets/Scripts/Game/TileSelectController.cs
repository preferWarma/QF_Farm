using UnityEngine;
using QFramework;

namespace Game
{
	public partial class TileSelectController : ViewController, ISingleton
	{
		public static TileSelectController Instance => MonoSingletonProperty<TileSelectController>.Instance;

		
		
		public void OnSingletonInit()
		{
			
		}
	}
}
