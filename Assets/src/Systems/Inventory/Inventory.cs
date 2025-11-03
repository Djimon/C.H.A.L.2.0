using System;
using System.Collections.Generic;
using System.Linq;

namespace CHAL.Systems.Inventory
{
    [Serializable]
/// <summary>
/// Represents an inventory that holds a collection of items.
/// </summary>
    public class Inventory
    {
        public readonly string invID; //=ItemPrefix
        private List<InventoryItem> _items = new();

        public Inventory(string prefix)
        {
            invID = prefix;
        }

/// <summary>
/// Adds an item to the inventory with a specified amount.
/// Returns true if the item was successfully added; otherwise, false.
/// </summary>
/// <param name="itemId">The ID of the item to add.</param>
/// <param name="amount">The number of items to add (default is 1).</param>
/// <returns>True if the item was added; otherwise, false.</returns>
        public bool AddItem(string itemId, int amount = 1)
        {
            DebugManager.Log($"TEST: {itemId.StartsWith(invID)}");
            if (!itemId.StartsWith(invID))
                return false;

            int maxStack = InventoryRules.GetMaxStack(invID);
            int maxSlots = InventoryRules.GetMaxSlots(invID);

            // existierenden Stack suchen
            var entry = _items.Find(i => i.ItemId == itemId);
            if (entry != null)
            {
                int spaceLeft = maxStack - entry.Count;
                if (spaceLeft <= 0)
                {
                    DebugManager.DebugLog($"max stacks ({maxStack}) reached for {itemId}");
                    return false;
                }

                int toAdd = Math.Min(amount, spaceLeft);
                entry.Count += toAdd;
                amount -= toAdd;

                if (amount > 0)
                {
                    DebugManager.DebugLog($"not all items could be added ({amount} left over)");
                    return false;
                }

                DebugManager.Log($"Added Item {itemId}({toAdd}) to Inventory {invID}.", DebugManager.EDebugLevel.Test, "Inventory");
                return true;
            }

            // nur neuer Slot, wenn ItemId noch nicht existiert
            if (_items.Count < maxSlots)
            {
                int toAdd = Math.Min(amount, maxStack);
                _items.Add(new InventoryItem { ItemId = itemId, Count = toAdd });
                amount -= toAdd;

                if (amount > 0)
                {
                    DebugManager.DebugLog($"max stack size reached, {amount} left over");
                    return false;
                }

                DebugManager.Log($"Added Item {itemId}({toAdd}) to Inventory {invID}.", DebugManager.EDebugLevel.Test, "Inventory");
                return true;
            }

            DebugManager.DebugLog($"max Slots ({maxSlots}) reached");
            return false;
        }

/// <summary>
/// Removes a specified amount of an item from the inventory.
/// Returns true if the item was successfully removed; otherwise, false.
/// </summary>
/// <param name="itemId">The ID of the item to remove.</param>
/// <param name="amount">The amount of the item to remove (default is 1).</param>
/// <returns>True if the item was removed; otherwise, false.</returns>
        public bool RemoveItem(string itemId, int amount = 1)
        {
            var entry = _items.Find(i => i.ItemId == itemId);
            if (entry == null || entry.Count < amount)
                return false;

            entry.Count -= amount;
            if (entry.Count <= 0)
                _items.Remove(entry);

            return true;
        }

/// <summary>
/// Gets the count of a specific item in the inventory.
/// </summary>
/// <param name="itemId">The ID of the item to count.</param>
/// <returns>The count of the specified item.</returns>
        public int GetItemCount(string itemId)
        {
            var entry = _items.Find(i => i.ItemId == itemId);
            return entry?.Count ?? 0;
        }

/// <summary>
/// Retrieves all items from the inventory.
/// </summary>
/// <returns>A list of inventory items.</returns>
        public List<InventoryItem> GetAllItems() => _items;

/// <summary>
/// Converts the inventory items to a dictionary of item IDs and their counts.
/// </summary>
/// <returns>A dictionary where keys are item IDs and values are their counts.</returns>
        public Dictionary<string, int> ToDictionary()
        {
            return _items.ToDictionary(i => i.ItemId, i => i.Count);
        }

/// <summary>
/// Initializes the inventory from a dictionary of item IDs and counts.
/// </summary>
/// <param name="dict">A dictionary where keys are item IDs and values are their counts.</param>
        public void FromDictionary(Dictionary<string, int> dict)
        {
            _items = dict.Select(kv => new InventoryItem
            {
                ItemId = kv.Key,
                Count = kv.Value
            }).ToList();
        }
    }

    [Serializable]
    public class InventoryItem
    {
        public string ItemId;
        public int Count;
    }
}
