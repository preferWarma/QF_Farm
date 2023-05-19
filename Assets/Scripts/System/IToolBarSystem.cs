using System.Collections.Generic;
using Game.Inventory;
using Game.Tools;
using QFramework;

namespace System
{
    public interface IToolBarSystem : ISystem
    {
        List<Item> Items { get; }
        int MaxCount { get; }
        
        // AddItem
        // RemoveItem
        // ItemCount++
        // ItemCount--
        // IsEmpty
    }

    // 工具栏系统,管理工具栏数据
    public class ToolBarSystem : AbstractSystem, IToolBarSystem
    {
        public List<Item> Items { get; } = new() // 所有物品
        {
            Config.CreateItem(ItemNameCollections.Hand),
            Config.CreateItem(ItemNameCollections.Shovel),
            Config.CreateItem(ItemNameCollections.WateringCan),
            Config.CreateItem(ItemNameCollections.SeedPumpkin, 5),
            Config.CreateItem(ItemNameCollections.SeedRadish, 5),
            Config.CreateItem(ItemNameCollections.SeedPotato, 5),
            Config.CreateItem(ItemNameCollections.SeedTomato, 5),
        };

        public int MaxCount => 10;

        protected override void OnInit()
        {
        }
        
    }
}