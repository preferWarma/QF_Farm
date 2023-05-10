using Game.ChallengeSystem;
using Game.Plants;
using Game.Tools;
using QFramework;
using UnityEngine;

namespace Game
{
    // 充当Model层
    public static class Global
    {
        [Header("游戏状态")]
        public static readonly BindableProperty<int> Days = new(1); // 第几天, 从第1天开始
        public static readonly BindableProperty<ITool> CurrentTool = new(Config.Hand.Tool);  // 当前工具, 默认为手
        
        [Header("植物数量")]
        public static readonly BindableProperty<int> PumpkinCount = new(); // 当前拥有的南瓜数量
        public static readonly BindableProperty<int> RadishCount = new(); // 当前拥有的胡萝卜数量
        public static readonly BindableProperty<int> PotatoCount = new();   // 当前拥有的土豆数量

        [Header("货币")]
        public static readonly BindableProperty<int> Money = new(); // 当前拥有的金钱
        
        [Header("事件相关")]
        public static readonly EasyEvent<IPlant> OnPlantHarvest = new(); // 采摘植物事件
        public static readonly EasyEvent<Challenge> OnChallengeFinish = new(); // 挑战完成事件

        [Header("其他")]
        public static Player Player = null;
        public static MouseController Mouse = null;
    }
}
