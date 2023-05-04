using Game.ChallengeSystem;
using Game.Plants;
using QFramework;
using UnityEngine;

namespace Game
{
    // 充当Model层
    public static class Global
    {
        public static readonly BindableProperty<int> Days = new(1); // 第几天, 从第1天开始
        public static readonly BindableProperty<string> CurrentTool = new(Constant.ToolHand);  // 当前工具, 默认为手
        
        [Header("植物数量")]
        public static readonly BindableProperty<int> PumpkinCount = new(); // 当前拥有的南瓜数量
        public static readonly BindableProperty<int> RadishCount = new(); // 当前拥有的胡萝卜数量
        
        [Header("植物种子数量")] 
        public static readonly BindableProperty<int> PumpKinSeedCount = new(5); // 南瓜种子数量
        public static readonly BindableProperty<int> RadishSeedCount = new(5);  // 胡萝卜种子
        
        [Header("事件相关")]
        public static readonly EasyEvent<IPlant> OnPlantHarvest = new(); // 采摘植物事件
        public static readonly EasyEvent<Challenge> OnChallengeFinish = new(); // 挑战完成事件

        [Header("其他")]
        public static Player Player = null;
        public static MouseController Mouse = null;
    }

    public static class Constant
    {
        public const string ToolHand = "hand";  // 手
        public const string ToolShovel = "shovel";  // 锄头
        public const string ToolWateringCan = "wateringCan";    // 水壶
        public const string ToolSeedPumpkin = "seedPumpkin";  // 种子
        public const string ToolSeedRadish = "seedRadish";  // 萝卜种子(逻辑有待完善)
        
        public static string DisplayName(string toolName, Language language)   // 工具名字(方便以后更改语言)
        {
            if (language == Language.Chinese)
            {
                return toolName switch
                {
                    ToolHand => "手",
                    ToolShovel => "锄头",
                    ToolWateringCan => "水壶",
                    ToolSeedPumpkin => "南瓜种子",
                    ToolSeedRadish => "萝卜种子",
                    _ => toolName
                };
            }
            if (language == Language.English)
            {
                return toolName switch
                {
                    ToolHand => "Hand",
                    ToolShovel => "Shovel",
                    ToolWateringCan => "Watering Can",
                    ToolSeedPumpkin => "Pumpkin Seed",
                    ToolSeedRadish => "Radish Seed",
                    _ => toolName
                };
            }

            return string.Empty;
        }
    }
    
    public enum Language
    {
        Chinese,
        English
    }
}
