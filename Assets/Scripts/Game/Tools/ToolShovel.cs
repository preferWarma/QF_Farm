using Game.Data;

namespace Game.Tools
{
    // 锄头工具
    public class ToolShovel : ITool
    {
        public string Name => "Shovel";
        public float CostHours => 0.5f;
        public int ToolScope => Global.IsToolUpgraded[1] ? 2 : 1;
        
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
            
            if (showGrid[cellPos.x, cellPos.y] != null) return; // 已经有耕地了
            
            Global.Mouse.TimeNotEnough.gameObject.SetActive(false);
            if (Global.RestHours.Value < CostHours)   // 时间不够
            {
                Global.Mouse.TimeNotEnough.gameObject.SetActive(true);
                return;
            }

            AudioController.Instance.Sfx_DigSoil.Play();	// 播放开垦音效
            tilemap.SetTile(cellPos, pen);
            showGrid[cellPos.x, cellPos.y] = new SoilData();
            
            Global.RestHours.Value -= CostHours;
        }
    }
}