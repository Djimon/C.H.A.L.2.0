using System;

namespace CHAL.Systems.Inventory
{
    public readonly struct ItemStackRef
    {
        public string itemID { get; }
        public int count { get; }
        public string instanceId { get; } // null/empty => no instance

        public bool IsEmpty => string.IsNullOrWhiteSpace(itemID) || count <= 0;
        public bool IsInstanced => !string.IsNullOrWhiteSpace(instanceId);

        public ItemStackRef(string itemID, int count, string instanceId = null)
        {
            this.itemID = itemID ?? string.Empty;
            this.count = Math.Max(0, count);
            this.instanceId = instanceId;
        }

        /// <summary>
        /// Creates a new ItemStack with the specified count.
        /// </summary>
        /// <param name="newCount">The new count for the ItemStack.</param>
        /// <returns>A new instance of ItemStack with the updated count.</returns>
        public ItemStackRef WithCount(int newCount) => new ItemStackRef(itemID, newCount, instanceId);
        public ItemStackRef WithInstance(string newInstanceId) => new ItemStackRef(itemID, count, newInstanceId);

        public override string ToString()
            => IsInstanced ? $"{itemID} x{count} (inst:{instanceId})" : $"{itemID} x{count}";


    }
}
