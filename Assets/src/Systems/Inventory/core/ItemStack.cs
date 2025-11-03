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

/// <summary>
/// Creates a new ItemStack with the specified count.
/// </summary>
/// <param name="newCount">The new count for the ItemStack.</param>
/// <returns>A new instance of ItemStack with the updated count.</returns>
        public ItemStack WithCount(int newCount) => new ItemStack(itemID, newCount);

    }
}
