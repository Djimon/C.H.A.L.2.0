using CHAL.Data;
using CHAL.Systems.Items;
using System;
using System.Collections.Generic;

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

    [Serializable]
    public sealed class SlotFilter
    {
        public List<ItemType> AllowedItemTypes;
        public List<string> AllowedItemIds;
        public List<string> AllowedTags;

        public List<ItemType> BlockedItemTypes;
        public List<string> BlockedItemIds;
        public List<string> BlockedTags;
    }

}