using QFramework;

namespace System.ToolBarSys
{
    public class AddItemCountCommand : AbstractCommand
    {
        private readonly string mItemName;  // 物品名
        private readonly int mAddCount; // 增加的数量
        
        public AddItemCountCommand(string itemName, int addCount)
        {
            mItemName = itemName;
            mAddCount = addCount;
        }

        protected override void OnExecute()
        {
            var item = Config.Items.Find(item => item.name == mItemName);
            if (item == null)
            {
                item = Config.CreateItem(mItemName, mAddCount);
                Config.Items.Add(item);
                ToolBarSystem.OnItemAdd.Trigger(item);
            }
            else
            {
                item.Count.Value += mAddCount;
            }
            ToolBarSystem.OnItemCountChange.Trigger(item, item.Count);
            
        }
    }
}