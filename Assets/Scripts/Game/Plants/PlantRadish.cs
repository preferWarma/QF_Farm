using Game.Data;
using QFramework;
using UnityEngine;

namespace Game.Plants
{
    public partial class PlantRadish : ViewController, IPlant
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PlantSates Sate { get; private set; } = PlantSates.Seed;
        public int RipeDay { get; set; } = -1;
        public GameObject GameObject => gameObject;
		
        private int mSmallDay = 0;
		
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
                    mSmallDay++;
                    if (mSmallDay == 2)
                    {
                        SetState(PlantSates.Ripe);
                    }
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
                PlantSates.Seed => ResController.Instance.seedRadishSprite,
                PlantSates.Small => ResController.Instance.smallPlantRadishSprite,
                PlantSates.Ripe => ResController.Instance.ripeRadishSprite,
                PlantSates.Old => ResController.Instance.oldRadishSprite,
                _ => GetComponent<SpriteRenderer>().sprite
            };

            FindObjectOfType<GridController>().ShowGrid[X, Y].PlantSates = newSate;	// 同步到SoilData
        }
    }
}