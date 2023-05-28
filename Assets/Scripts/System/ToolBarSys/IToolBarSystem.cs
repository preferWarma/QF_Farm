using System.Collections.Generic;
using Game.Inventory;
using Lyf.SaveSystem;
using QFramework;
using UnityEngine;

namespace System.ToolBarSys
{
    public interface IToolBarSystem : ISystem, ISaveWithJson
    {
        List<Item> Items { get; }
        List<ToolBarSlot> ToolBarSlots { get; } // 引入的目的是为了方便存储
        int MaxCount { get; }
        void ResetDefaultData();
        
        // AddItem
        // RemoveItem
        // ItemCount++
        // ItemCount--
        // IsEmpty
    }

    // 工具栏系统,管理工具栏数据
    public class ToolBarSystem : AbstractSystem, IToolBarSystem
    {
        public static readonly EasyEvent<Item> OnItemAdd = new();	// 物品添加事件
        public static readonly EasyEvent<Item> OnItemRemove = new();	// 物品移除事件
        public static readonly EasyEvent<Item, int> OnItemCountChange = new();	// 物品数量改变事件
        public int MaxCount => 10;

        public List<ToolBarSlot> ToolBarSlots { get; } = new(); // 工具栏槽, 为了存储设计的
        
        public List<Item> Items { get; private set; } = new() // 所有物品数据库
        {
            Config.CreateItem(ItemNameCollections.Hand),
            Config.CreateItem(ItemNameCollections.Shovel),
            Config.CreateItem(ItemNameCollections.WateringCan),
            Config.CreateItem(ItemNameCollections.SeedPumpkin, Config.InitPumpkinSeedCount),
            Config.CreateItem(ItemNameCollections.SeedRadish, Config.InitRadishSeedCount),
            Config.CreateItem(ItemNameCollections.SeedPotato, Config.InitPotatoSeedCount),
            Config.CreateItem(ItemNameCollections.SeedTomato, Config.InitTomatoSeedCount),
            Config.CreateItem(ItemNameCollections.SeedBean, Config.InitBeanSeedCount),
        };
        
        protected override void OnInit()
        {
            SaveManager.Instance.Register(this, SaveType.Json);
        }
        
        #region 存储相关

        public string SAVE_FILE_NAME => "ToolBarData";
        
        [Serializable]
        private class SaveDataCollection
        {
            public ToolBarSlot[] ToolBarSlots;  // 包含物品ID和数量
        }
        
        public void SaveWithJson()
        {
            ToolBarSlots.Clear();
            foreach (var item in Items)
            {
                ToolBarSlots.Add(new ToolBarSlot(item.name, item.Count));
            }
            
            var saveData = new SaveDataCollection
            {
                ToolBarSlots = ToolBarSlots.ToArray()
            };
            SaveManager.SaveWithJson(SAVE_FILE_NAME, saveData);
        }

        public void LoadWithJson()
        {
            var saveData = SaveManager.LoadWithJson<SaveDataCollection>(SAVE_FILE_NAME);
            if (saveData == null)
            {
                Debug.LogError("加载工具栏数据失败");
                return;
            }
            Items.Clear();
            for (var i = 0; i < saveData.ToolBarSlots.Length; i++)
            {
                ToolBarSlots.Add(saveData.ToolBarSlots[i]);
                Items.Add(Config.CreateItem(ToolBarSlots[i].ItemID, ToolBarSlots[i].Count));
            }
        }
        
        public void ResetDefaultData()
        {
             Items = new List<Item>
             {
                Config.CreateItem(ItemNameCollections.Hand),
                Config.CreateItem(ItemNameCollections.Shovel),
                Config.CreateItem(ItemNameCollections.WateringCan),
                Config.CreateItem(ItemNameCollections.SeedPumpkin, Config.InitPumpkinSeedCount),
                Config.CreateItem(ItemNameCollections.SeedRadish, Config.InitRadishSeedCount),
                Config.CreateItem(ItemNameCollections.SeedPotato, Config.InitPotatoSeedCount),
                Config.CreateItem(ItemNameCollections.SeedTomato, Config.InitTomatoSeedCount),
                Config.CreateItem(ItemNameCollections.SeedBean, Config.InitBeanSeedCount),
            };
        }
        
        #endregion
    }
}