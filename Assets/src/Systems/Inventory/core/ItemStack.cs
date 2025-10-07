using System.Collections.Generic;

namespace CHAL.Systems.Inventory
{
    public readonly struct ItemStack
    {
        public string itemID { get; }
        public int count { get; }

        public ItemStack(string id, int itemcount)
        {
            itemID = id;
            count = itemcount;
        }

        public ItemStack WithCount(int newCount) => new ItemStack(itemID, newCount);

    }
}
