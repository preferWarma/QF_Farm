using System.PowerUpSys;
using System.SoilSys;
using QFramework;

namespace Game.Tools
{
    // 锄头工具
    public class ToolShovel : ITool
    {
        public string Name => ItemNameCollections.Shovel;
        public float CostHours => Global.ToolShovelCostTime;
        public float CdTime { get; set; } = Config.CdToolShovel;
        public float InitCdTime => Config.CdToolShovel;
        public int ToolScope => Global.Level;

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

            // 以下特效方面
            var digFx = Global.Mouse.Dig_Fx;   // 挖掘特效
            var digPos = tilemap.GetCellCenterWorld(cellPos);   // 获取挖掘特效位置
            digFx.gameObject.transform.Position(digPos.x, digPos.y);   // 设置挖掘特效位置,z轴不变
            digFx.Play();   // 播放挖掘特效
            MouseController.RotateIcon();   // 旋转鼠标图标
            CameraController.Shake(ShakeType.Heavy);   // 震动镜头
        }
    }
}