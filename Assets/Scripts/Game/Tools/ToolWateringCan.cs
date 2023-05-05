using Game.Data;
using QFramework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Tools
{
    public class ToolWateringCan : ITool
    {
        public bool Selected()
        {
            return Global.CurrentTool == Constant.ToolWateringCan;
        }

        public void Use(EasyGrid<SoilData> easyGrid, Tilemap tilemap, Vector3Int cellPos, TileBase pen)
        {
            if (easyGrid[cellPos.x, cellPos.y].Watered) return; // 已经浇过水了
            AudioController.Instance.Sfx_Watering.Play();	// 播放浇水音效
            easyGrid[cellPos.x, cellPos.y].Watered = true;
            ResController.Instance.waterPrefab
                .Instantiate()
                .Position(tilemap.GetCellCenterWorld(cellPos));
        }
    }
}