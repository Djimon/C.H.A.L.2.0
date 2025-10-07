namespace CHAL.Systems.Inventory
{
    public class MoveRequest
    {
        public ItemMoveObject fromInventory;
        public ItemMoveObject toInventory;

        public int? amount;
        public MoveMode moveMode;      
    }

    public struct ItemMoveObject
    {
        public string instanceID;
        public int slot;
    }

    public enum MoveMode
    { 
        Move,
        Merge,
        Swap,
        Split
    }
}