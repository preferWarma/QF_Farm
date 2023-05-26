using System;
using System.ToolBarSys;
using Game.Inventory;
using Game.Plants;
using QFramework;
using UnityEngine;

namespace Game.Tools
{
    public class ToolSeed : ITool, IController
    {
        public string Name => "Seed";
        public float CostHours => 0.2f;
        public int ToolScope => Global.IsToolUpgraded[3] ? 2 : 1;
        public Item Item { get; set; }  // 与背包中的物品对应
        
        public bool Selected()
        {
            return Global.CurrentTool.Value.Name == Name;
        }

        public void Use(ToolNeedData needData)
        {
            if (Item.Count.Value <= 0) return; // 没有种子了,直接返回
            
            var showGrid = needData.ShowGrid;
            var cellPos = needData.CellPos;
            var tilemap = needData.Tilemap;
            if (showGrid[cellPos.x, cellPos.y] == null) return; // 该格子无耕地
            if (showGrid[cellPos.x, cellPos.y].HasPlant) return; // 已经有植物了
            
            Global.Mouse.TimeNotEnough.gameObject.SetActive(false);
            if (Global.RestHours.Value < CostHours)   // 时间不够
            {
                Global.Mouse.TimeNotEnough.gameObject.SetActive(true);
                return;
            }

            var tileWorldPos = tilemap.GetCellCenterWorld(cellPos);
            var plantObj = ResController.Instance.LoadPrefab(Item.plantPrefabName)
                .Instantiate()
                .Position(tileWorldPos);
            if (!plantObj)
            {
                Debug.LogError("植物生成失败,请检查植物预制体是否存在");
                return;
            }
            
            AudioController.Instance.Sfx_PutSeed.Play(); // 播放种植音效
            
            this.SendCommand(new SubItemCountCommand(Item.name, 1));    // 种子数量减1
            
            var plant = plantObj.GetComponent<IPlant>();
            plant.X = cellPos.x;    // 设置植物的位置, 与耕地的位置一致
            plant.Y = cellPos.y;
            plant.SetState(PlantSates.Seed);

            PlantController.Instance.PlantGrid[cellPos.x, cellPos.y] = plant;
            showGrid[cellPos.x, cellPos.y].HasPlant = true;
            showGrid[cellPos.x, cellPos.y].PlantPrefabName = Item.plantPrefabName;

            Global.RestHours.Value -= CostHours;
        }

        public IArchitecture GetArchitecture()
        {
            return Global.Interface;
        }
    }
}