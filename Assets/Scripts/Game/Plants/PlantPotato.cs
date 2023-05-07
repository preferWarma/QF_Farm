using Game.Data;
using QFramework;
using UnityEngine;

namespace Game.Plants
{
    public partial class PlantPotato : ViewController, IPlant
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PlantSates Sate { get; private set; } = PlantSates.Seed;
        public int RipeDay { get; private set; } = -1;
        public GameObject GameObject => gameObject;

        private int mSeedDay = 0;   // 种子生长天数
        private int mSmallDay = 0;  // 幼苗生长天数

        public void Grow(SoilData soilData)
        {
            if (!soilData.Watered) return; // 如果没有浇水, 不生长
            if (Sate == PlantSates.Seed) // 如果是种子
            {
                mSeedDay++;
                if (mSeedDay == 2)
                {
                    SetState(PlantSates.Small);
                }
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

        public void SetState(PlantSates newSate)
        {
            if (newSate == Sate) return;

            if (newSate == PlantSates.Ripe)
            {
                RipeDay = Global.Days.Value;
            }

            Sate = newSate;

            GetComponent<SpriteRenderer>().sprite = newSate switch // 切换表现
            {
                PlantSates.Seed => ResController.Instance.seedPotatoSprite,
                PlantSates.Small => ResController.Instance.smallPlantPotatoSprite,
                PlantSates.Ripe => ResController.Instance.ripePotatoSprite,
                PlantSates.Old => ResController.Instance.oldPotatoSprite,
                _ => GetComponent<SpriteRenderer>().sprite
            };

            FindObjectOfType<GridController>().ShowGrid[X, Y].PlantSates = newSate; // 同步到SoilData
        }
    }
}