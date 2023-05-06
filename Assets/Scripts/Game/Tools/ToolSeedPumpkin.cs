using Game.Plants;
using QFramework;
using UnityEngine;

namespace Game.Tools
{
    public class ToolSeedPumpkin : ITool
    {
        public string Name => "SeedPumpkin";

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
            
            if (showGrid[cellPos.x, cellPos.y].HasPlant) return; // 已经有植物了
            
            GameObject plantObj = null;
            var tileWorldPos = tilemap.GetCellCenterWorld(cellPos);
            
            // 根据当前工具种植不同的植物
            if (Global.PumpKinSeedCount.Value > 0)
            {
                plantObj = ResController.Instance.plantPumpkinPrefab
                    .Instantiate()
                    .Position(tileWorldPos);
                Global.PumpKinSeedCount.Value--;
            }
            if (!plantObj) return;
            AudioController.Instance.Sfx_PutSeed.Play(); // 播放种植音效
            var plant = plantObj.GetComponent<IPlant>();
            plant.X = cellPos.x;
            plant.Y = cellPos.y;
            plant.SetState(PlantSates.Seed);

            PlantController.Instance.PlantGrid[cellPos.x, cellPos.y] = plant;
            showGrid[cellPos.x, cellPos.y].HasPlant = true;
        }
        
    }
}