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
/// <summary>
/// Creates a new ItemStackRef with the specified instance ID.
/// </summary>
/// <param name="newInstanceId">The new instance ID to set.</param>
/// <returns>A new ItemStackRef object.</returns>
        public ItemStackRef WithInstance(string newInstanceId) => new ItemStackRef(itemID, count, newInstanceId);

/// <summary>
/// Returns a string representation of the object, including item ID and count.
/// </summary>
/// <returns>A formatted string with item details.</returns>
        public override string ToString()
            => IsInstanced ? $"{itemID} x{count} (inst:{instanceId})" : $"{itemID} x{count}";


    }
}
