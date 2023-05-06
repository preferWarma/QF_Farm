using Game.Tools;

namespace Game
{
    public static class Constant
    {
        // public const string ToolHand = "hand";  // 手
        // public const string ToolShovel = "shovel";  // 锄头
        // public const string ToolWateringCan = "wateringCan";    // 水壶
        // public const string ToolSeedPumpkin = "seedPumpkin";  // 种子
        // public const string ToolSeedRadish = "seedRadish";  // 萝卜种子(逻辑有待完善)
        
        public static readonly ITool ToolHand = new ToolHand();
        public static readonly ITool ToolShovel = new ToolShovel();
        public static readonly ITool ToolWateringCan = new ToolWateringCan();
        public static readonly ITool ToolSeedPumpkin = new ToolSeedPumpkin();
        public static readonly ITool ToolSeedRadish = new ToolSeedRadish();
        
        public static string DisplayName(string toolName, Language language)   // 工具名字(方便以后更改语言)
        {
            return language switch
            {
                Language.Chinese when toolName == ToolHand.Name => "手",
                Language.Chinese when toolName == ToolShovel.Name => "铲子",
                Language.Chinese when toolName == ToolWateringCan.Name => "水壶",
                Language.Chinese when toolName == ToolSeedPumpkin.Name => "南瓜种子",
                Language.Chinese when toolName == ToolSeedRadish.Name => "胡萝卜种子",
                
                Language.English when toolName == ToolHand.Name => "Hand",
                Language.English when toolName == ToolShovel.Name => "Shovel",
                Language.English when toolName == ToolWateringCan.Name => "Watering Can",
                Language.English when toolName == ToolSeedPumpkin.Name => "Pumpkin Seed",
                Language.English when toolName == ToolSeedRadish.Name => "Radish Seed",
                _ => string.Empty
            };
        }
    }
}