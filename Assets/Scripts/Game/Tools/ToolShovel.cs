using Game.Data;

namespace Game.Tools
{
    // 锄头工具
    public class ToolShovel : ITool
    {
        public bool Selected()
        {
            return Global.CurrentTool == Constant.ToolShovel;
        }

        public void Use(ToolNeedData needData)
        {
            var showGrid = needData.ShowGrid;
            var cellPos = needData.CellPos;
            var tilemap = needData.Tilemap;
            var pen = needData.Pen;
            
            if (showGrid[cellPos.x, cellPos.y] != null) return; // 已经有耕地了
            AudioController.Instance.Sfx_DigSoil.Play();	// 播放开垦音效
            tilemap.SetTile(cellPos, pen);
            showGrid[cellPos.x, cellPos.y] = new SoilData();
            
        }
    }
}