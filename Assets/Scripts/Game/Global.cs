using System.Collections.Generic;
using Game.ChallengeSystem;
using Game.Plants;
using QFramework;
using UnityEngine;

namespace Game
{
    // 充当Model层
    public class Global : MonoBehaviour
    {
        public static readonly BindableProperty<int> Days = new(1); // 第几天
        public static readonly BindableProperty<string> CurrentTool = new(Constant.ToolHand);  // 当前工具
        
        [Header("植物数量")]
        public static readonly BindableProperty<int> PumpkinCount = new(0); // 南瓜数量
        public static readonly BindableProperty<int> RadishCount = new(0); // 胡萝卜数量

        [Header("植物种子数量")] 
        public static readonly BindableProperty<int> PumpKinSeedCount = new(5); // 南瓜种子数量
        public static readonly BindableProperty<int> RadishSeedCount = new(5);  // 胡萝卜种子

        [Header("挑战要求相关")]
        public static readonly BindableProperty<int> RipeAndHarvestCountInCurrentDay = new(0); // 当天成熟并采摘的植物数量
        public static readonly BindableProperty<int> HarvestCountInCurrentDay = new(0); // 当天采摘的植物数量
        public static readonly BindableProperty<int> HarvestRadishCountInCurrentDay = new(0); // 当天采摘的萝卜数量

        [Header("挑战列表")]
        public static readonly List<Challenge> Challenges = new()
        {
            new ChallengeHarvestOneFruit(),   // 收获第一个果实挑战
            new RipeAndHarvestTwoInOneDay(), // 一天成熟并收获两个果实挑战
            new RipeAndHarvestFiveInOneDay(), // 一天成熟并收获五个果实挑战
            new HarvestOneRadish(), // 收获一个萝卜挑战
        }; // 挑战列表
        public static readonly List<Challenge> ActiveChallenges = new(); // 激活的挑战列表
        public static readonly List<Challenge> FinishedChallenges = new(); // 完成的挑战列表
        
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
