using CHAL.Systems.Items;

namespace CHAL.Systems.Inventory
{
    public sealed class Slot
    {
        public int index { get; }
        public int maxStack { get; internal set; } // aus Def oder Fallback-Regeln
        public SlotFilter Filter { get; internal set; } // optional
        public ItemStack? stack { get; internal set; } // null => leer


        public Slot(int i, int mStack, SlotFilter filter = null)
        {
            index = i;
            maxStack = mStack;
            Filter = filter;
            stack = null;
        }
    }

    public sealed class SlotFilter
    {
        public ItemType[] AllowedItemTypes;
        public string[] AllowedItemIds;
        public string[] AllowedTags;

        public ItemType[] BlockedItemTypes;
        public string[] BlockedItemIds;
        public string[] BlockedTags;
    }

}