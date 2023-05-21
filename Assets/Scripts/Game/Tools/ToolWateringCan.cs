﻿using QFramework;

namespace Game.Tools
{
    public class ToolWateringCan : ITool
    {
        public string Name => "WateringCan";
        public float CostHours => 0.1f;
        public int ToolScope => Global.IsToolUpgraded[2] ? 2 : 1;

        public bool Selected()
        {
            return Global.CurrentTool.Value.Name == Name;
        }

        public void Use(ToolNeedData needData)
        {
            var showGrid = needData.ShowGrid;
            var cellPos = needData.CellPos;
            var tilemap = needData.Tilemap;

            Global.Mouse.TimeNotEnough.gameObject.SetActive(false);
            if (showGrid[cellPos.x, cellPos.y] == null) return; // 没有耕地
            if (showGrid[cellPos.x, cellPos.y].Watered) return; // 已经浇过水了
            if (Global.RestHours.Value < CostHours)   // 时间不够
            {
                Global.Mouse.TimeNotEnough.gameObject.SetActive(true);
                return;
            }

            AudioController.Instance.Sfx_Watering.Play();	// 播放浇水音效
            showGrid[cellPos.x, cellPos.y].Watered = true;
            ResController.Instance.waterPrefab
                .Instantiate()
                .Position(tilemap.GetCellCenterWorld(cellPos));
            
            Global.RestHours.Value -= CostHours;
        }
    }
}