using System.Collections.Generic;
using System.ToolBarSys;
using Game.Inventory;
using Game.Tools;
using QFramework;

// 游戏配置类
public static class Config
{
    // 游戏配置
    public const int InitMoney = 50; // 初始金钱
    public const int InitDays = 1; // 初始天数
    public const float InitRestHours = 10; // 初始剩余时间
    public const int InitSoilWidth = 5; // 初始土地宽度
    public const int InitSoilHeight = 4;    // 初始土地高度
    public const int InitDailyCost = 8; // 初始每日花费

    // 背包配置
    public const int InitPumpkinSeedCount = 5;
    public const int InitRadishSeedCount = 5;
    public const int InitPotatoSeedCount = 5;
    public const int InitTomatoSeedCount = 5;
    public const int InitBeanSeedCount = 5;
    
    // 工具配置
    public const float CdToolSeed = 0.3f;
    public const float CdToolShovel = 0.6f;
    public const float CdToolWateringCan = 0.5f;
    public const float CdToolHand = 0.3f;
    public const float CdToolFruit = 0.0f;

    public static List<Item> Items => Global.Interface.GetSystem<IToolBarSystem>().Items;

    public static Item CreateItem(string itemName, int initCount = 1)
    {
        return itemName switch
        {
            #region 固定物品
            
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
            
            #endregion
            
            #region 种子
            
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
            
            ItemNameCollections.SeedBean => new Item
            {
                name = ItemNameCollections.SeedBean,
                iconName = "SeedBean",
                Count = new BindableProperty<int>(initCount),
                canStack = true,
                isPlant = true,
                plantPrefabName = "PlantBean"
            }.Self(item => { item.Tool = new ToolSeed() { Item = item };}),
            
            #endregion
            
            #region 植物
            
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
            ItemNameCollections.Bean => new Item
            {
                name = ItemNameCollections.Bean,
                iconName = "Bean",
                Count = new BindableProperty<int>(initCount),
                canStack = true,
                isPlant = false,
                Tool = null,
                plantPrefabName = string.Empty
            },
            
            #endregion
            
            _ => null
        };
    }

    #region 升级相关

    public interface ILvBase
    {
        public float ToolSeedCostTime { get; }
        public float ToolShovelCostTime { get; }
        public float ToolWateringCanCostTime { get; }
        public float ToolHandCostTime { get; }
        
    }

    public static readonly Dictionary<int, ILvBase> LvDict = new()
    {
        {1, new Lv1()},
        {2, new Lv2()},
        {3, new Lv3()},
    };

    private class Lv1 : ILvBase
    {
        public float ToolSeedCostTime => 0.2f;
        public float ToolShovelCostTime => 0.5f;
        public float ToolWateringCanCostTime => 0.3f;
        public float ToolHandCostTime => 0.2f;
    }

    private class Lv2 : ILvBase
    {
        public float ToolSeedCostTime => 0.1f;
        public float ToolShovelCostTime => 0.3f;
        public float ToolWateringCanCostTime => 0.2f;
        public float ToolHandCostTime => 0.1f;
    }

    private class Lv3 : ILvBase
    {
        public float ToolSeedCostTime => 0.05f;
        public float ToolShovelCostTime => 0.15f;
        public float ToolWateringCanCostTime => 0.1f;
        public float ToolHandCostTime => 0.05f;
    }
    
    #endregion
}