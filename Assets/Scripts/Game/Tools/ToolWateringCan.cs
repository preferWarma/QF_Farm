﻿using QFramework;
using UnityEngine;

namespace Game.Tools
{
    public class ToolWateringCan : ITool
    {
        public string Name => ItemNameCollections.WateringCan;
        public float CostHours => Config.LvDict[Global.ToolCostLevel].ToolWateringCanCostTime;
        public float CdTime { get; set; } = Config.LvDict[1].ToolWateringCanCdTime;
        public float InitCdTime => Config.LvDict[Global.ToolCdLevel].ToolWateringCanCdTime;
        public int ToolScope => Global.ToolCostLevel;
        public bool Selected()
        {
            return Global.CurrentTool.Value.Name == Name;
        }

        public void Use(ToolNeedData needData)
        {
            var showGrid = needData.ShowGrid;
            var cellPos = needData.CellPos;
            var tilemap = needData.Tilemap;
            
            if (showGrid[cellPos.x, cellPos.y] == null) return; // 没有耕地
            if (showGrid[cellPos.x, cellPos.y].Watered) return; // 已经浇过水了
            
            Global.Mouse.TimeNotEnough.gameObject.SetActive(false);
            if (Global.RestHours.Value < CostHours)   // 时间不够
            {
                Global.Mouse.TimeNotEnough.gameObject.SetActive(true);
                return;
            }

            AudioController.Instance.Sfx_Watering.Play();	// 播放浇水音效
            showGrid[cellPos.x, cellPos.y].Watered = true;
            ResController.Instance.waterPrefab
                .InstantiateWithParent(Global.WaterRoot.transform)
                .Position(tilemap.GetCellCenterWorld(cellPos));
            
            Global.RestHours.Value -= CostHours;
            
            // Tilemap方面
            Global.GridController.Watering.SetTile(new Vector3Int(cellPos.x, cellPos.y),
                Global.GridController.WaterTile);
            
            // 以下特效方面
            var wateringFx = Global.Mouse.Watering_Fx;   // 挖掘特效
            var wateringPos = tilemap.GetCellCenterWorld(cellPos);   // 获取挖掘特效位置
            wateringFx.gameObject.transform.Position(wateringPos.x, wateringPos.y);   // 设置挖掘特效位置,z轴不变
            wateringFx.Play();   // 播放挖掘特效
            CameraController.Shake(ShakeType.Light);
            MouseController.RotateIcon();
        }
    }
}