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
        public static Item CreateItem(string itemName, int initCount = 1)
        {
            return itemName switch
            {
                ItemNameCollections.Hand => new Item
                {
                    name = ItemNameCollections.Hand,
                    iconName = "Hand",
                    Count = new BindableProperty<int>(initCount),
                    canStack = false,
                    Tool = new ToolHand(),
                    isPlant = false,
                    plantPrefabName = string.Empty,
                },
                ItemNameCollections.Shovel => new Item
                {
                    name = ItemNameCollections.Shovel,
                    iconName = "Shovel",
                    Count = new BindableProperty<int>(initCount),
                    canStack = false,
                    Tool = new ToolShovel(),
                    isPlant = false,
                    plantPrefabName = string.Empty
                },
                ItemNameCollections.WateringCan => new Item
                {
                    name = ItemNameCollections.WateringCan,
                    iconName = "WateringCan",
                    Count = new BindableProperty<int>(initCount),
                    canStack = false,
                    Tool = new ToolWateringCan(),
                    isPlant = false,
                    plantPrefabName = string.Empty
                },
                
                ItemNameCollections.SeedPumpkin => new Item
                {
                    name = ItemNameCollections.SeedPumpkin,
                    iconName = "SeedPumpkin",
                    Count = new BindableProperty<int>(initCount),
                    canStack = true,
                    isPlant = true,
                    plantPrefabName = "PlantPumpkin"
                }.Self(item => { item.Tool = new ToolSeed { Item = item }; }), // 这里的目的是为了让Item和ToolSeed互相引用
                
                ItemNameCollections.SeedRadish => new Item
                {
                    name = ItemNameCollections.SeedRadish,
                    iconName = "SeedRadish",
                    Count = new BindableProperty<int>(initCount),
                    canStack = true,
                    isPlant = true,
                    plantPrefabName = "PlantRadish"
                }.Self(item => { item.Tool = new ToolSeed { Item = item }; }),
                
                ItemNameCollections.SeedPotato => new Item
                {
                    name = ItemNameCollections.SeedPotato,
                    iconName = "SeedPotato",
                    Count = new BindableProperty<int>(initCount),
                    canStack = true,
                    isPlant = true,
                    plantPrefabName = "PlantPotato"
                }.Self(item => { item.Tool = new ToolSeed { Item = item }; }),
                
                ItemNameCollections.SeedTomato => new Item
                {
                    name = ItemNameCollections.SeedTomato,
                    iconName = "SeedTomato",
                    Count = new BindableProperty<int>(initCount),
                    canStack = true,
                    isPlant = true,
                    plantPrefabName = "PlantTomato"
                }.Self(item => { item.Tool = new ToolSeed { Item = item }; }),
                
                ItemNameCollections.Pumpkin => new Item
                {
                    name = ItemNameCollections.Pumpkin,
                    iconName = "Pumpkin",
                    Count = new BindableProperty<int>(initCount),
                    canStack = true,
                    isPlant = false,
                    Tool = null,
                    plantPrefabName = string.Empty
                },
                
                _ => null
            };
        }

        public static readonly List<Item> Items = new() // 所有物品
        {
            CreateItem(ItemNameCollections.Hand),
            CreateItem(ItemNameCollections.Shovel),
            CreateItem(ItemNameCollections.WateringCan),
            CreateItem(ItemNameCollections.SeedPumpkin, 5),
            CreateItem(ItemNameCollections.SeedRadish, 5),
            CreateItem(ItemNameCollections.SeedPotato, 5),
            CreateItem(ItemNameCollections.SeedTomato, 5),
        };

        [Tooltip("有些地方会使用到的单独的物品引用")]
        public static readonly Item Hand = Items.Find(item => item.iconName == "Hand"); // 手 
        public static readonly Item Shovel = Items.Find(item => item.iconName == "Shovel"); // 铲子
        public static readonly Item WateringCan = Items.Find(item => item.iconName == "WateringCan"); // 水壶
        public static readonly Item SeedPumpkin = Items.Find(item => item.iconName == "SeedPumpkin"); // 南瓜种子
        public static readonly Item SeedRadish = Items.Find(item => item.iconName == "SeedRadish"); // 萝卜种子
        public static readonly Item SeedPotato = Items.Find(item => item.iconName == "SeedPotato"); // 土豆种子
        public static readonly Item SeedTomato = Items.Find(item => item.iconName == "SeedTomato"); // 西红柿种子
    }
}