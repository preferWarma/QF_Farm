using System.Collections.Generic;
using Game.Inventory;
using Game.Tools;
using QFramework;
using UnityEngine;

namespace Game
{
    // 游戏配置类
    public static class Config
    {
        public static readonly List<Item> Items = new()  // 所有物品
        {
            new Item
            {
                name = "手",
                iconName = "Hand",
                Count = new BindableProperty<int>(1),
                canStack = false,
                Tool = new ToolHand(),
                isPlant = false,
                plantPrefabName = string.Empty,
            },
            new Item
            {
                name = "铲子",
                iconName = "Shovel",
                Count = new BindableProperty<int>(1),
                canStack = false,
                Tool = new ToolShovel(),
                isPlant = false,
                plantPrefabName = string.Empty
            },
            new Item
            {
                name = "水壶",
                iconName = "WateringCan",
                Count = new BindableProperty<int>(1),
                canStack = false,
                Tool = new ToolWateringCan(),
                isPlant = false,
                plantPrefabName = string.Empty
            },
            new Item
            {
                name = "南瓜种子",
                iconName = "SeedPumpkin",
                Count = new BindableProperty<int>(5),
                canStack = true,
                isPlant = true,
                plantPrefabName = "PlantPumpkin"
            }.Self(item => { item.Tool = new ToolSeed { Item = item }; }),   // 这里的目的是为了让Item和ToolSeed互相引用
            
            new Item
            {
                name = "萝卜种子",
                iconName = "SeedRadish",
                Count = new BindableProperty<int>(5),
                canStack = true,
                isPlant = true,
                plantPrefabName = "PlantRadish"
            }.Self(item => { item.Tool = new ToolSeed { Item = item }; }),
            
            new Item
            {
                name = "土豆种子",
                iconName = "SeedPotato",
                Count = new BindableProperty<int>(5),
                canStack = true,
                Tool = new ToolSeed(),
                isPlant = true,
                plantPrefabName = "PlantPotato"
            }.Self(item => {item.Tool = new ToolSeed { Item = item }; }),
        };
        
        [Tooltip("有些地方会使用到的单独的物品引用")]
        public static readonly Item Hand = Items.Find(item => item.iconName == "Hand"); // 手 
    }
}