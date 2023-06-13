using System.Linq;
using System.PowerUpSys;
using DG.Tweening;
using Game.UI;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Tools
{
    public class ToolHand : ITool
    {
        public string Name => "Hand";
        public float CostHours => 0.1f;
        public float CdTime { get; set; } = Config.CdToolHand;
        public float InitCdTime => Config.CdToolHand;
        public int ToolScope => PowerUpSystem.IsPowerUpUnlocked[ItemNameCollections.Hand] ? 2 : 1;
        
        public bool Selected()
        {
            return Global.CurrentTool.Value.Name == Name;
        }

        public void Use(ToolNeedData needData)
        {
            var showGrid = needData.ShowGrid;
            var cellPos = needData.CellPos;

            if (showGrid[cellPos.x, cellPos.y] == null) return; // 没有耕地
            if (showGrid[cellPos.x, cellPos.y].PlantSate != PlantSates.Ripe) return; // 当前植物未成熟

            Global.Mouse.TimeNotEnough.gameObject.SetActive(false);
            if (Global.RestHours.Value < CostHours) // 时间不够
            {
                Global.Mouse.TimeNotEnough.gameObject.SetActive(true);
                return;
            }

            AudioController.Instance.Sfx_Harvest.Play(); // 播放收获音效
            var plantName = PlantController.Instance.PlantGrid[cellPos.x, cellPos.y].PlantName; // 做个缓存, 因为收割后会清空该植物
            
            Global.OnPlantHarvest.Trigger(PlantController.Instance.PlantGrid[cellPos.x, cellPos.y]); // 触发收获事件
            Object.Destroy(PlantController.Instance.PlantGrid[cellPos.x, cellPos.y].GameObject); // 摘取后销毁
            showGrid[cellPos.x, cellPos.y] = null; // 摘取后清空耕地,下次可以重新开垦
            PlantController.Instance.PlantGrid[cellPos.x, cellPos.y] = null; // 同步清空植物

            Global.RestHours.Value -= CostHours;

            HarvestAnimation(needData, plantName); // 收割动画
            MouseController.RotateIcon();
            CameraController.Shake(ShakeType.Middle);
        }

        private void HarvestAnimation(ToolNeedData toolNeedData, string plantName) // 收割动画
        {
            var toolBar = Object.FindObjectOfType<UIToolBar>();
            var plantScreenPos =
                RectTransformUtility.WorldToScreenPoint(Camera.main, toolNeedData.CellPos); // 获取植物的屏幕坐标
            var targetPos = GetFruitPosInToolBar(toolBar, plantName);

            toolBar.harvestCollection.InstantiateWithParent(toolBar.transform)
                .Position(plantScreenPos)
                .Show()
                .Self(self =>
                {
                    self.sprite = ResController.Instance.LoadSprite(plantName);
                    // 一个简单的动画效果
                    DoAnimation(self, targetPos);
                });
        }

        private Vector3 GetFruitPosInToolBar(UIToolBar uiToolBar, string plantName) // 获取水果在工具栏的位置
        {
            var slots = uiToolBar.ToolbarSlots;
            
            foreach (var slot in slots.Where(slot => slot.ItemData?.name == plantName))
            {
                return slot.transform.position;
            }

            return new Vector3();   // 工具栏中一定找得到, 防止报错
        }

        private void DoAnimation(Image self, Vector3 targetPos)
        {
            self.transform.localScale = Vector3.one * Random.Range(0.2f, 0.7f);
            self.transform.DOScale(Vector3.one, 0.5f);
            var pos1 = self.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 1, 0).normalized * 128; // 随机抛出位置
            DOTween.Sequence()
                .Append(self.transform.DOMove(pos1, 0.3f).SetEase(Ease.OutCubic))
                .Append(self.transform.DOMove(targetPos, 0.2f).SetEase(Ease.InCubic).OnComplete(self.DestroyGameObj));
        }
    }
}