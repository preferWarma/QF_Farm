using System.Collections.Generic;
using Game.Inventory;

namespace Game
{
    // 游戏配置类
    public static class Config
    {
        public static readonly Item Hand = new Item
        {
            name = "手",
            iconName = "Hand",
            count = 1,
            canStack = false,
            Tool = Constant.ToolHand,
            isPlant = false,
            plantPrefabName = string.Empty
        };
        public static readonly Item Shovel = new Item
        {
            name = "铲子",
            iconName = "Shovel",
            count = 1,
            canStack = false,
            Tool = Constant.ToolShovel,
            isPlant = false,
            plantPrefabName = string.Empty
        };
        public static readonly Item WateringCan = new Item
        {
            name = "水壶",
            iconName = "WateringCan",
            count = 1,
            canStack = false,
            Tool = Constant.ToolWateringCan,
            isPlant = false,
            plantPrefabName = string.Empty
        };
        public static readonly Item SeedPumpkin = new Item
        {
            name = "南瓜种子",
            iconName = "SeedPumpkin",
            count = 1,
            canStack = true,
            Tool = Constant.ToolSeedPumpkin,
            isPlant = true,
            plantPrefabName = "PlantPumpkin"
        };
        public static readonly Item SeedRadish = new Item
        {
            name = "萝卜种子",
            iconName = "SeedRadish",
            count = 1,
            canStack = true,
            Tool = Constant.ToolSeedRadish,
            isPlant = true,
            plantPrefabName = "PlantRadish"
        };
        public static readonly Item SeedPotato = new Item
        {
            name = "土豆种子",
            iconName = "SeedPotato",
            count = 1,
            canStack = true,
            Tool = Constant.ToolSeedPotato,
            isPlant = true,
            plantPrefabName = "PlantPotato"
        };
        
        public static List<Item> Items = new()  // 所有物品
        {
            Hand,
            Shovel,
            SeedPumpkin,
            WateringCan,
            SeedRadish,
            SeedPotato
        };
    }
}