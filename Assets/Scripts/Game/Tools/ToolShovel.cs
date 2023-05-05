using Game.Data;
using QFramework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game.Tools
{
    // 锄头工具
    public class ToolShovel : ITool
    {
        public bool Selected()
        {
            return Global.CurrentTool == Constant.ToolShovel;
        }

        public void Use(EasyGrid<SoilData> easyGrid, Tilemap tilemap, Vector3Int cellPos, TileBase pen)
        {
            if (easyGrid[cellPos.x, cellPos.y] != null) return; // 已经有耕地了
            AudioController.Instance.Sfx_DigSoil.Play();	// 播放开垦音效
            tilemap.SetTile(cellPos, pen);
            easyGrid[cellPos.x, cellPos.y] = new SoilData();
        }
    }
}