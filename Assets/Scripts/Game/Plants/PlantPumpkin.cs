using System.Collections.Generic;
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
		
		public string plantName => ItemNameCollections.Pumpkin;
		public List<PlantStateInfo> stateInfos = new ();
		
		private int mCurrentStateDay = 0;	// 当前状态的生长天数
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
			if (Sate == PlantSates.Ripe) return;	// 如果已经成熟, 不再生长
			mCurrentStateDay++;
			
			var currentStateInfo = stateInfos.Find(info => info.sate == Sate);
			if (mCurrentStateDay >= currentStateInfo.growDay)	// 生长天数到了, 可以切换为下一个状态
			{
				var curIdx = stateInfos.IndexOf(currentStateInfo);
				SetState(stateInfos[curIdx + 1].sate);
				mCurrentStateDay = 0;	// 重置生长天数
			}

			// switch (Sate)
			// {
			// 	// 如果是种子
			// 	case PlantSates.Seed:
			// 		SetState(PlantSates.Small);
			// 		break;
			// 	// 如果是幼苗
			// 	case PlantSates.Small:
			// 		SetState(PlantSates.Mid);
			// 		break;
			// 	case PlantSates.Mid:
			// 		SetState(PlantSates.Big);
			// 		break;
			// 	case PlantSates.Big:
			// 		SetState(PlantSates.Ripe);
			// 		break;
			// }
		}

		public void SetState(PlantSates newSate)
		{
			if (newSate == Sate) return;
			Sate = newSate;
			
			var newStateInfo = stateInfos.Find(info => info.sate == newSate);
			if (newStateInfo == null) return;

			if (!newStateInfo.showSoilDig)
			{
				this.ClearSoilDigState(mGridController);
			}
			mSpriteRenderer.sprite = newStateInfo.sprite;
			
            
			if (newSate == PlantSates.Ripe)
			{
				RipeDay = Global.Days.Value;
			}
			
			mGridController.ShowGrid[X, Y].PlantSates = newSate;	// 同步到SoilData
		}
	}
}
