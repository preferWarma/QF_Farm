using System.ChallengeSys;
using System.SoilSys;
using System.ToolBarSys;
using Game;
using Game.Plants;
using Game.Tools;
using Lyf.SaveSystem;
using QFramework;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using SaveType = Lyf.SaveSystem.SaveType;

public class Global : Architecture<Global>, ISaveWithJson
{
    [Header("游戏状态")]
    public static readonly BindableProperty<int> Days = new(Config.InitDays); // 第几天, 从第1天开始
    public static readonly BindableProperty<float> RestHours = new(Config.InitRestHours);  // 当天剩余时间
    public static readonly BindableProperty<ITool> CurrentTool = new();  // 当前工具
        
    [Header("植物数量")]
    public static readonly BindableProperty<int> PumpkinCount = new(); // 当前拥有的南瓜数量
    public static readonly BindableProperty<int> RadishCount = new(); // 当前拥有的胡萝卜数量
    public static readonly BindableProperty<int> PotatoCount = new();   // 当前拥有的土豆数量
    public static readonly BindableProperty<int> TomatoCount = new();   // 当前拥有的西红柿数量
    public static readonly BindableProperty<int> BeanCount = new();   // 当前拥有的豆角数量

    [Header("货币")]
    public static readonly BindableProperty<int> Money = new(Config.InitMoney); // 当前拥有的金钱

    [Header("升级相关")]
    public static bool[] IsToolUpgraded = new bool[4]; // 工具是否升级(顺序按照工具的顺序)

    [Header("事件相关")]
    public static readonly EasyEvent<IPlant> OnPlantHarvest = new(); // 采摘植物事件
    public static readonly EasyEvent<Challenge> OnChallengeFinish = new(); // 挑战完成事件

    [Header("其他")]
    public static Player Player = null;
    public static MouseController Mouse = null;
    
    protected override void Init()
    {
        _self = this;   // 用于存储时候调用非静态方法
        
        RegisterSystem<IToolBarSystem>(new ToolBarSystem());
        RegisterSystem<ISoilSystem>(new SoilSystem());
        RegisterSystem<IChallengeSystem>(new ChallengeSystem());

        SaveManager.Instance.Register(this, SaveType.Json);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]   // 在start之前执行
    public static void DoLoadAll()
    {
        SaveManager.Instance.LoadAllRegister(SaveType.Json);
    }
    
    #region 存储相关

    private static Global _self;

    [MenuItem("Lyf/存档/保存所有注册数据")]
    public static void Save()
    {
        SaveManager.Instance.SaveAllRegister(SaveType.Json);
    }
    
    [MenuItem("Lyf/读档/读取所有注册数据")]
    public static void Load()
    {
        SaveManager.Instance.LoadAllRegister(SaveType.Json);
    }
    
    [MenuItem("Lyf/重置数据/加载所有默认数据")]
    public static void LoadDefaultData()
    {
        _self.ResetDefaultData();
        Interface.GetSystem<ISoilSystem>().ResetDefaultData();
        Object.FindObjectOfType<GridController>()?.Show();
        Interface.GetSystem<IChallengeSystem>().ResetDefaultData();
        Interface.GetSystem<IToolBarSystem>().ResetDefaultData();
        SaveManager.Instance.SaveAllRegister(SaveType.Json);
    }
    
    public string SAVE_FILE_NAME => "Global";
    private class SaveDataCollection    // 需要保存的数据
    {
        public int Days;
        public float RestHours;
        public int PumpkinCount;
        public int RadishCount;
        public int PotatoCount;
        public int TomatoCount;
        public int BeanCount;
        public int Money;
        public bool[] IsToolUpgraded;
    }
    
    public void SaveWithJson()
    {
        var saveData = new SaveDataCollection()
        {
            Days = Days.Value,
            RestHours = RestHours.Value,
            PumpkinCount = PumpkinCount.Value,
            RadishCount = RadishCount.Value,
            PotatoCount = PotatoCount.Value,
            TomatoCount = TomatoCount.Value,
            BeanCount = BeanCount.Value,
            Money = Money.Value,
            IsToolUpgraded = IsToolUpgraded
        };
        SaveManager.SaveWithJson(SAVE_FILE_NAME, saveData);
    }

    public void LoadWithJson()
    {
        var saveData = SaveManager.LoadWithJson<SaveDataCollection>(SAVE_FILE_NAME);
        if (saveData == null) return;
        Days.SetValueWithoutEvent(saveData.Days);
        RestHours.SetValueWithoutEvent(saveData.RestHours);
        PumpkinCount.Value = saveData.PumpkinCount;
        RadishCount.Value = saveData.RadishCount;
        PotatoCount.Value = saveData.PotatoCount;
        TomatoCount.Value = saveData.TomatoCount;
        BeanCount.Value = saveData.BeanCount;
        Money.SetValueWithoutEvent(saveData.Money);
        IsToolUpgraded = saveData.IsToolUpgraded;
    }
    
    private void ResetDefaultData()
    {
        Days.SetValueWithoutEvent(Config.InitDays);
        RestHours.SetValueWithoutEvent(Config.InitRestHours);
        Money.SetValueWithoutEvent(Config.InitMoney);
        IsToolUpgraded = new bool[4];
        PumpkinCount.Value = 0;
        RadishCount.Value = 0;
        PotatoCount.Value = 0;
        TomatoCount.Value = 0;
        BeanCount.Value = 0;
    }
    
    #endregion
}