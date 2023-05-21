using QFramework;
using UnityEngine;

namespace Game.Tools
{
    public class ToolHand : ITool
    {
        public string Name => "Hand";
        public float CostHours => 0.1f;
        public int ToolScope => Global.IsToolUpgraded[0] ? 2 : 1;

        public bool Selected()
        {
            return Global.CurrentTool.Value.Name == Name;
        }

        public void Use(ToolNeedData needData)
        {
            var showGrid = needData.ShowGrid;
            var cellPos = needData.CellPos;

            if (showGrid[cellPos.x, cellPos.y] == null) return; // 没有耕地
            if (showGrid[cellPos.x, cellPos.y].PlantSates != PlantSates.Ripe) return;   // 当前植物未成熟
            if (Global.RestHours.Value < CostHours)   // 时间不够
            {
                Global.Mouse.TimeNotEnough.gameObject.SetActive(true);
                return;
            }

            Global.Mouse.TimeNotEnough.gameObject.SetActive(false);

            AudioController.Instance.Sfx_Harvest.Play();	// 播放收获音效
            Global.OnPlantHarvest.Trigger(PlantController.Instance.PlantGrid[cellPos.x, cellPos.y]);    // 触发收获事件
            Object.Destroy(PlantController.Instance.PlantGrid[cellPos.x, cellPos.y].GameObject); // 摘取后销毁, 简化流程,后期会改
            showGrid[cellPos.x, cellPos.y] = null; // 摘取后清空耕地,下次可以重新开垦
            PlantController.Instance.PlantGrid[cellPos.x, cellPos.y] = null;    // 同步清空植物
            
            Global.RestHours.Value -= CostHours;
        }
    }
}