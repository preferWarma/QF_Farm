using Game.Data;
using UnityEngine;
using QFramework;

namespace Game.Plants
{
	public partial class PlantTomato : ViewController, IPlant
	{
		public int X { get; set; }
        public int Y { get; set; }
        public PlantSates Sate { get; private set; }
        public int RipeDay { get; private set; } = -1;
        public GameObject GameObject => gameObject;

        private int mBigDay = 0;  // 阶段3生长天数

        private SpriteRenderer mSpriteRenderer;
        private GridController mGridController;

        private void Awake()
        {
            mSpriteRenderer = GetComponent<SpriteRenderer>();
            mGridController = FindObjectOfType<GridController>();
        }
        
        public void Grow(SoilData soilData)
        {
            if (!soilData.Watered) return; // 如果没有浇水, 不生长
            
            switch (Sate)
            {
                case PlantSates.Seed:
                {
                    SetState(PlantSates.Small);
                    break;
                }
                case PlantSates.Small:
                {
                    SetState(PlantSates.Middle);
                    break;
                }
                case PlantSates.Middle:
                    SetState(PlantSates.Big);
                    break;
                case PlantSates.Big:
                    mBigDay++;
                    if (mBigDay == 2)
                    {
                        SetState(PlantSates.Ripe);
                    }
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

            mSpriteRenderer.sprite = newSate switch // 切换表现
            {
                PlantSates.Seed => ResController.Instance.seedTomatoSprite,
                PlantSates.Small => ResController.Instance.smallPlantTomatoSprite,
                PlantSates.Middle => ResController.Instance.middlePlantTomatoSprite,
                PlantSates.Big => ResController.Instance.bigPlantTomatoSprite,
                PlantSates.Ripe => ResController.Instance.ripeTomatoSprite,
                PlantSates.Old => ResController.Instance.oldTomatoSprite,
                _ => mSpriteRenderer.sprite
            };

            mGridController.ShowGrid[X, Y].PlantSates = newSate; // 同步到SoilData
        }
	}
}
