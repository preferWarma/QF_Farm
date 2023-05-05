using Game.Data;
using QFramework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Tools
{
    public class ToolHand : ITool
    {
        public bool Selected()
        {
            return Global.CurrentTool == Constant.ToolHand;
        }

        public void Use(EasyGrid<SoilData> easyGrid, Tilemap tilemap, Vector3Int cellPos, TileBase pen)
        {
            if (easyGrid[cellPos.x, cellPos.y].PlantSates != PlantSates.Ripe) return;   // 当前植物未成熟
            AudioController.Instance.Sfx_Harvest.Play();	// 播放收获音效
            Global.OnPlantHarvest.Trigger(PlantController.Instance.PlantGrid[cellPos.x, cellPos.y]);    // 触发收获事件
            Object.Destroy(PlantController.Instance.PlantGrid[cellPos.x, cellPos.y].GameObject); // 摘取后销毁, 简化流程,后期会改
            easyGrid[cellPos.x, cellPos.y].HasPlant = false;
            easyGrid[cellPos.x, cellPos.y].PlantSates = PlantSates.Seed; // 摘取后下一次变成种子(有待改进)
        }
    }
}