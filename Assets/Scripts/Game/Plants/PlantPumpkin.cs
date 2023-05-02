using Game.Data;
using QFramework;
using UnityEngine;

namespace Game.Plants
{
	public partial class PlantPumpkin : ViewController, IPlant
	{
		public int X { get; set; } = -1;
		public int Y { get; set; } = -1;
		public PlantSates Sate { get; private set; } = PlantSates.Seed;
		public int RipeDay { get; set; } = -1; // 成熟的日期
		public GameObject GameObject => gameObject;
		
		public void Grow(SoilData soilData)
		{
			if (soilData.Watered)
			{
				if (Sate == PlantSates.Seed) // 如果是种子
				{
					SetState(PlantSates.Small);
				}
				else if (Sate == PlantSates.Small) // 如果是幼苗
				{
					SetState(PlantSates.Ripe);
				}
			}
		}

		public void SetState(PlantSates newSate)
		{
			if (newSate == Sate) return;
			
			if (Sate == PlantSates.Small && newSate == PlantSates.Ripe)
			{
				RipeDay = Global.Days.Value;
			}
			
			Sate = newSate;
			
			GetComponent<SpriteRenderer>().sprite = newSate switch	// 切换表现
			{
				PlantSates.Seed => ResController.Instance.seedSprite,
				PlantSates.Small => ResController.Instance.smallPlantSprite,
				PlantSates.Ripe => ResController.Instance.ripeSprite,
				PlantSates.Old => ResController.Instance.oldSprite,
				_ => GetComponent<SpriteRenderer>().sprite
			};

			FindObjectOfType<GridController>().ShowGrid[X, Y].PlantSates = newSate;	// 同步到SoilData
		}

		
	}
}
