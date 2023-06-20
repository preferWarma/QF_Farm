using System;
using System.Collections.Generic;
using System.SoilSys;
using DG.Tweening;
using Game.UI;
using QFramework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Plants
{
	public partial class Plant : ViewController, IPlant, IController
	{
		public int X { get; set; } = -1;
		public int Y { get; set; } = -1;
		public int RipeDay { get; private set; } = -1; // 成熟的日期
		public GameObject GameObject => gameObject;

		public PlantSates Sate	// 与土壤数据同步
		{
			get => mSoilSystem.SoilGrid[X, Y].PlantSate;
			private set => mSoilSystem.SoilGrid[X, Y].PlantSate = value;
		}
		
		[Range(0,1)] public float aliveRate = 1;	// 存活率
		[SerializeField] public string plantName = "未知植物";	// 供外部设置
		public string PlantName => plantName;
		public List<PlantStateInfo> stateInfos = new ();	// 植物各阶段生长状态
		public RipeCountWeight ripeCountWeight = new RipeCountWeight(1,1);	// 成熟数量
		public SoilData SoilData => mSoilSystem.SoilGrid[X, Y];	// 对应的土壤数据

		private int mCurrentStateDay	// 当前阶段已经生长的天数
		{
			get => mSoilSystem.SoilGrid[X, Y].CurrentStateDay;
			set => mSoilSystem.SoilGrid[X, Y].CurrentStateDay = value;
		}
		private SpriteRenderer mSpriteRenderer;
		private GridController mGridController;
		private ISoilSystem mSoilSystem;
		private bool _tweenPlaying;
 
		private void Awake()
		{
			mSpriteRenderer = GetComponent<SpriteRenderer>();
			mGridController = FindObjectOfType<GridController>();
			mSoilSystem = this.GetSystem<ISoilSystem>();
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.CompareTag("Player")) return;
			if (_tweenPlaying) return;
			if (mSoilSystem.SoilGrid[X, Y] == null) return;
			if (Sate == PlantSates.Seed) return;
			
			_tweenPlaying = true;
			DOTween.Sequence()
				.Append(transform.DORotate(Vector3.back * 5, 0.2f).SetEase(Ease.OutCubic))
				.Append(transform.DORotate(Vector3.forward * 5, 0.2f).SetEase(Ease.OutCubic))
				.Append(transform.DORotate(Vector3.zero, 0.1f).SetEase(Ease.OutCubic))
				.OnComplete(() =>
				{
					_tweenPlaying = false;
				});
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
				
				if (stateInfos[curIdx + 1].sate == PlantSates.Ripe)	// 成熟的时候需要做更多的事情
				{
					RipeDay = Global.Days.Value;

					if (Random.Range(0f, 1.0f) > aliveRate)	// 意外死亡
					{
						UIMessageQueue.Push(GetComponent<SpriteRenderer>().sprite, "死亡");
						RemoveSelf();
						return;
					}

					var ripeCount = Random.Range(ripeCountWeight.minCount, ripeCountWeight.maxCount + 1) +
					                + Config.LvDict[Global.HarvestLevel].AnotherRipeCount;
					soilData.RipeCount = ripeCount;
					UIMessageQueue.Push(GetComponent<SpriteRenderer>().sprite, $"成熟+{ripeCount}");
				}
			}
		}

		public void SetState(PlantSates newSate)
		{
			Sate = newSate;
			
			var newStateInfo = stateInfos.Find(info => info.sate == newSate);
			if (newStateInfo == null) return;

			if (!newStateInfo.showSoilDig)
			{
				this.ClearSoilDigState(mGridController);
			}
			mSpriteRenderer.sprite = newStateInfo.sprite;
		}

		public void RemoveSelf()
		{
			Global.GridController.ShowGrid[X, Y] = null;
			Destroy(gameObject);
			Global.GridController.Watering.SetTile(new Vector3Int(X, Y), null);
			PlantController.Instance.PlantGrid[X, Y] = null;
		}

		public IArchitecture GetArchitecture()
		{
			return Global.Interface;
		}

		private void OnDestroy()
		{
			mSpriteRenderer = null;
			mGridController = null;
			mSoilSystem = null;
		}
	}

	[Serializable]
	public class RipeCountWeight
	{
		public int minCount;
		public int maxCount;

		public RipeCountWeight(int minCount, int maxCount)
		{
			this.minCount = minCount;
			this.maxCount = maxCount;
		}
	}
}
