namespace System.ToolBarSys
{
    public class ToolBarSlot
    {
        public ToolBarSlot(string itemID, int count)
        {
            ItemID = itemID;
            Count = count;
        }

        public string ItemID;
        public int Count;
    }
}