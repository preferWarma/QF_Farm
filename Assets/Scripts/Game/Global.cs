using QFramework;
using UnityEngine;

namespace Game
{
    // 充当Model层
    public class Global : MonoBehaviour
    {
        public static BindableProperty<int> Days = new(1); // 第几天
        public static BindableProperty<int> Fruits = new(0); // 水果数量
        public static BindableProperty<string> CurrentTool = new("手");  // 当前工具
        public static int RipeAndHarvestCountInCurrentDay = 0; // 当天成熟并采摘的植物数量
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
