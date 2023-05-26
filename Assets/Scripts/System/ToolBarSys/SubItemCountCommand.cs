using QFramework;

namespace System.ToolBarSys
{
    public class SubItemCountCommand : AbstractCommand
    {
        private readonly string mItemName;  // 物品名
        private readonly int mSubCount; // 减少的数量

        public SubItemCountCommand(string itemName, int subCount)
        {
            mItemName = itemName;
            mSubCount = subCount;
        }
        
        protected override void OnExecute()
        {
            var item = Config.Items.Find(item => item.name == mItemName);
            if (item == null) return;
            item.Count.Value -= mSubCount;
            if (item.Count.Value <= 0)
            {
                Config.Items.Remove(item);
                ToolBarSystem.OnItemRemove.Trigger(item);
            }
            ToolBarSystem.OnItemCountChange.Trigger(item, item.Count);
        }
    }
}