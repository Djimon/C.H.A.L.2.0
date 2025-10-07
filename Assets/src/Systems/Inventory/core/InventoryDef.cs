namespace CHAL.Systems.Inventory
{
    public class InventoryDef 
    {
        public string TypeId;
        public string NameKey;
        public int cols;
        public int rows;
        public int defaultMaxStackPerSlot = 99;
        public SlotFilter globalSlotFilter;
    }

}