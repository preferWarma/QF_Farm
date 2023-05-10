using UnityEngine;

namespace Game.Tools
{
    public class ToolHand : ITool
    {
        public string Name => "Hand";

        public bool Selected()
        {
            return Global.CurrentTool.Value.Name == Name;
        }

        public void Use(ToolNeedData needData)
        {
            var showGrid = needData.ShowGrid;
            var cellPos = needData.CellPos;
            var tilemap = needData.Tilemap;
            var pen = needData.Pen;
            
            if (showGrid[cellPos.x, cellPos.y] == null) return; // 没有耕地
            if (showGrid[cellPos.x, cellPos.y].PlantSates != PlantSates.Ripe) return;   // 当前植物未成熟
            AudioController.Instance.Sfx_Harvest.Play();	// 播放收获音效
            Global.OnPlantHarvest.Trigger(PlantController.Instance.PlantGrid[cellPos.x, cellPos.y]);    // 触发收获事件
            Object.Destroy(PlantController.Instance.PlantGrid[cellPos.x, cellPos.y].GameObject); // 摘取后销毁, 简化流程,后期会改
            showGrid[cellPos.x, cellPos.y].HasPlant = false;
            showGrid[cellPos.x, cellPos.y].PlantSates = PlantSates.Seed; // 摘取后下一次变成种子(有待改进)
        }
    }
}