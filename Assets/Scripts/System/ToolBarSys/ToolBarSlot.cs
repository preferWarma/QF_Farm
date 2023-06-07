namespace System.ToolBarSys
{
    public class ToolBarSlot
    {
        // 用于存储工具栏设计的类
        public ToolBarSlot(string itemID, int count)
        {
            ItemID = itemID;
            Count = count;
        }

        public string ItemID;
        public int Count;
    }
}