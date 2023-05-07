using Game.Plants;
using QFramework;
using UnityEngine;

namespace Game.Tools
{
    public class ToolSeedPotato : ITool
    {
        public string Name => "SeedPotato";

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
            
            if (Global.PotatoSeedCount.Value > 0)
            {
                plantObj = ResController.Instance.plantPotatoPrefab
                    .Instantiate()
                    .Position(tileWorldPos);
                Global.PotatoSeedCount.Value--;
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