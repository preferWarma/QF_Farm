using QFramework;

namespace Game.Tools
{
    public class ToolWateringCan : ITool
    {
        public string Name => "WateringCan";
        public int ToolScope { get; set; } = 1;

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
            if (showGrid[cellPos.x, cellPos.y].Watered) return; // 已经浇过水了
            AudioController.Instance.Sfx_Watering.Play();	// 播放浇水音效
            showGrid[cellPos.x, cellPos.y].Watered = true;
            ResController.Instance.waterPrefab
                .Instantiate()
                .Position(tilemap.GetCellCenterWorld(cellPos));
        }
    }
}