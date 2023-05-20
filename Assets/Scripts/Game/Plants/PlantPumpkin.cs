using Game.Data;
using QFramework;
using UnityEngine;

namespace Game.Plants
{
	public partial class PlantPumpkin : ViewController, IPlant
	{
		public int X { get; set; } = -1;
		public int Y { get; set; } = -1;
		public PlantSates Sate { get; private set; }
		public int RipeDay { get; private set; } = -1; // 成熟的日期
		public GameObject GameObject => gameObject;

		private SpriteRenderer mSpriteRenderer;
		private GridController mGridController;

		private void Awake()
		{
			mSpriteRenderer = GetComponent<SpriteRenderer>();
			mGridController = FindObjectOfType<GridController>();
		}

		public void Grow(SoilData soilData)
		{
			if (!soilData.Watered) return;	// 如果没有浇水, 不生长

			switch (Sate)
			{
				// 如果是种子
				case PlantSates.Seed:
					SetState(PlantSates.Small);
					break;
				// 如果是幼苗
				case PlantSates.Small:
					SetState(PlantSates.Mid);
					break;
				case PlantSates.Mid:
					SetState(PlantSates.Big);
					break;
				case PlantSates.Big:
					SetState(PlantSates.Ripe);
					break;
			}
		}

		public void SetState(PlantSates newSate)
		{
			if (newSate == Sate) return;
			Sate = newSate;
            
			if (newSate == PlantSates.Small)
			{
				this.ClearSoilDigState(mGridController);    // 清除耕地开垦状态
			}
            
			if (newSate == PlantSates.Ripe)
			{
				RipeDay = Global.Days.Value;
			}
			
			mSpriteRenderer.sprite = newSate switch	// 切换表现
			{
				PlantSates.Seed => ResController.Instance.LoadPlantSprite(PlantSpriteNameCollections.SeedPumpkin),
				PlantSates.Small => ResController.Instance.LoadPlantSprite(PlantSpriteNameCollections.SmallPumpkin),
				PlantSates.Mid => ResController.Instance.LoadPlantSprite(PlantSpriteNameCollections.MidPumpkin),
				PlantSates.Big => ResController.Instance.LoadPlantSprite(PlantSpriteNameCollections.BigPumpkin),
				PlantSates.Ripe => ResController.Instance.LoadPlantSprite(PlantSpriteNameCollections.RipePumpkin),
				
				_ => mSpriteRenderer.sprite
			};

			mGridController.ShowGrid[X, Y].PlantSates = newSate;	// 同步到SoilData
		}
	}
}
