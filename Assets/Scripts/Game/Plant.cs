using UnityEngine;
using QFramework;

namespace Game
{
	public partial class Plant : ViewController
	{
		public int x;
		public int y;
		private PlantSates mSate = PlantSates.Seed;
		public PlantSates Sate => mSate;
		public int ripeDay = -1;	// 成熟的日期
		
		public void SetState(PlantSates newSate)
		{
			if (newSate == mSate) return;
			
			if (mSate == PlantSates.Small && newSate == PlantSates.Ripe)
			{
				ripeDay = Global.Days.Value;
			}
			
			mSate = newSate;
			
			
			GetComponent<SpriteRenderer>().sprite = newSate switch	// 切换表现
			{
				PlantSates.Seed => ResController.Instance.seedSprite,
				PlantSates.Small => ResController.Instance.smallPlantSprite,
				PlantSates.Ripe => ResController.Instance.ripeSprite,
				PlantSates.Old => ResController.Instance.oldSprite,
				_ => GetComponent<SpriteRenderer>().sprite
			};

			FindObjectOfType<GridController>().ShowGrid[x, y].PlantSates = newSate;	// 同步到SoilData
		}

	}
}
