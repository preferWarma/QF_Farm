using Game.Plants;
using QFramework;

namespace Game
{
	public enum PlantSates
	{
		Seed,	// 种子
		Small,	// 幼苗1
		Mid,	// 幼苗2
		Big,	// 幼苗3
		Ripe,	// 成熟
		Old	    // 枯萎(已摘取)
	}
	
	public partial class PlantController : ViewController, ISingleton
	{
		public static PlantController Instance => MonoSingletonProperty<PlantController>.Instance;

		// TODO 跟随土壤宽高变化
		public EasyGrid<IPlant> PlantGrid = new(Config.InitSoilWidth, Config.InitSoilHeight);

		public void OnSingletonInit()
		{
		}
	}
}
