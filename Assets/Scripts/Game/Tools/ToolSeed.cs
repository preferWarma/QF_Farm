using Game.Data;
using Game.Plants;
using QFramework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Tools
{
    public class ToolSeed : ITool
    {
        public bool Selected()
        {
            return Global.CurrentTool == Constant.ToolSeedPumpkin ||
                   Global.CurrentTool == Constant.ToolSeedRadish;
        }

        public void Use(EasyGrid<SoilData> easyGrid, Tilemap tilemap, Vector3Int cellPos, TileBase pen)
        {
            if (easyGrid[cellPos.x, cellPos.y].HasPlant) return; // 已经有植物了
            
            GameObject plantObj = null;
            Vector3 tileWorldPos = tilemap.GetCellCenterWorld(cellPos);
            
            // 根据当前工具种植不同的植物
            if (Global.CurrentTool.Value == Constant.ToolSeedPumpkin && Global.PumpKinSeedCount.Value > 0)
            {
                plantObj = ResController.Instance.plantPrefab
                    .Instantiate()
                    .Position(tileWorldPos);
                Global.PumpKinSeedCount.Value--;
            }
            else if (Global.CurrentTool.Value == Constant.ToolSeedRadish && Global.RadishSeedCount.Value > 0)
            {
                plantObj = ResController.Instance.plantRadishPrefab
                    .Instantiate()
                    .Position(tileWorldPos);
                Global.RadishSeedCount.Value--;
            }
            if (!plantObj) return;
            AudioController.Instance.Sfx_PutSeed.Play(); // 播放种植音效
            var plant = plantObj.GetComponent<IPlant>();
            plant.X = cellPos.x;
            plant.Y = cellPos.y;
            plant.SetState(PlantSates.Seed);

            PlantController.Instance.PlantGrid[cellPos.x, cellPos.y] = plant;
            easyGrid[cellPos.x, cellPos.y].HasPlant = true;
        }
    }
}