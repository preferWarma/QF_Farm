using System.Collections.Generic;
using Game.ChallengeSystem;
using QFramework;
using UnityEngine;

namespace Game
{
    // 充当Model层
    public class Global : MonoBehaviour
    {
        public static readonly BindableProperty<int> Days = new(1); // 第几天
        public static readonly BindableProperty<int> Fruits = new(0); // 水果数量
        public static readonly BindableProperty<string> CurrentTool = new("手");  // 当前工具
        public static readonly BindableProperty<int> RipeAndHarvestCountInCurrentDay = new(0); // 当天成熟并采摘的植物数量
        public static readonly List<Challenge> Challenges = new()
        {
            new ChallengeHarvestFirstFruit(),   // 收获第一个果实挑战
            new RipeAndHarvestTwoInOneDay(), // 一天成熟并收获两个果实挑战
            new RipeAndHarvestFiveInOneDay() // 一天成熟并收获五个果实挑战
        }; // 挑战列表
        public static readonly List<Challenge> ActiveChallenges = new(); // 激活的挑战列表
        public static readonly List<Challenge> FinishedChallenges = new(); // 完成的挑战列表


        public static readonly EasyEvent<Plant> OnPlantHarvest = new(); // 采摘植物事件
        public static readonly EasyEvent<Challenge> OnChallengeFinish = new(); // 挑战完成事件
    }

    public static class Constant
    {
        public const string ToolHand = "hand";  // 手
        public const string ToolShovel = "shovel";  // 锄头
        public const string ToolWateringCan = "wateringCan";    // 水壶
        public const string ToolSeed = "seed";  // 种子
        
        public static string DisplayName(string toolName, Language language)   // 工具名字(方便以后更改语言)
        {
            if (language == Language.Chinese)
            {
                return toolName switch
                {
                    ToolHand => "手",
                    ToolShovel => "锄头",
                    ToolWateringCan => "水壶",
                    ToolSeed => "种子",
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
                    ToolSeed => "Seed",
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
