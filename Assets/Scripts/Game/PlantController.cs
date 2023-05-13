using Game.Plants;
using QFramework;

namespace Game
{
	public enum PlantSates
	{
		Seed,	// 种子
		Small,	// 幼苗
		Ripe,	// 成熟
		Old	    // 枯萎(已摘取)
	}
	
	public partial class PlantController : ViewController, ISingleton
	{
		public static PlantController Instance => MonoSingletonProperty<PlantController>.Instance;

		public readonly EasyGrid<IPlant> PlantGrid = new(5, 4);

		public void OnSingletonInit()
		{
		}
	}
}
