using System;
using Game;
using Game.ChallengeSystem;
using Game.Plants;
using Game.Tools;
using QFramework;
using UnityEngine;

public class Global : Architecture<Global>
{
    [Header("游戏状态")]
    public static readonly BindableProperty<int> Days = new(1); // 第几天, 从第1天开始

    public static readonly BindableProperty<int> RestHours = new(10);  // 当天剩余时间
    public static readonly BindableProperty<ITool> CurrentTool = new();  // 当前工具
        
    [Header("植物数量")]
    public static readonly BindableProperty<int> PumpkinCount = new(); // 当前拥有的南瓜数量
    public static readonly BindableProperty<int> RadishCount = new(); // 当前拥有的胡萝卜数量
    public static readonly BindableProperty<int> PotatoCount = new();   // 当前拥有的土豆数量
    public static readonly BindableProperty<int> TomatoCount = new();   // 当前拥有的西红柿数量
    public static readonly BindableProperty<int> BeanCount = new();   // 当前拥有的豆角数量

    [Header("货币")]
    public static readonly BindableProperty<int> Money = new(60); // 当前拥有的金钱

    [Header("升级相关")]
    public static readonly bool[] IsToolUpgraded = new bool[4]; // 工具是否升级(顺序按照工具的顺序)

    [Header("事件相关")]
    public static readonly EasyEvent<IPlant> OnPlantHarvest = new(); // 采摘植物事件
    public static readonly EasyEvent<Challenge> OnChallengeFinish = new(); // 挑战完成事件

    [Header("其他")]
    public static Player Player = null;
    public static MouseController Mouse = null;
        
        
    protected override void Init()
    {
        RegisterSystem<IToolBarSystem>(new ToolBarSystem());
    }
}