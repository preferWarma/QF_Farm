using System;
using System.Collections.Generic;
using Game.Inventory;
using Game.Tools;
using QFramework;

// 游戏配置类
public static class Config
{
    public static List<Item> Items => Global.Interface.GetSystem<IToolBarSystem>().Items;

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
            ItemNameCollections.Radish => new Item
            {
                name = ItemNameCollections.Radish,
                iconName = "Radish",
                Count = new BindableProperty<int>(initCount),
                canStack = true,
                isPlant = false,
                Tool = null,
                plantPrefabName = string.Empty
            },
            ItemNameCollections.Potato => new Item
            {
                name = ItemNameCollections.Potato,
                iconName = "Potato",
                Count = new BindableProperty<int>(initCount),
                canStack = true,
                isPlant = false,
                Tool = null,
                plantPrefabName = string.Empty
            },
            ItemNameCollections.Tomato => new Item
            {
                name = ItemNameCollections.Tomato,
                iconName = "Tomato",
                Count = new BindableProperty<int>(initCount),
                canStack = true,
                isPlant = false,
                Tool = null,
                plantPrefabName = string.Empty
            },
            
                
            _ => null
        };
    }
}