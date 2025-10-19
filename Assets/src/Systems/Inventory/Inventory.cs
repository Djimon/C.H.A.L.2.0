using System;
using System.Collections.Generic;
using System.Linq;

namespace CHAL.Systems.Inventory
{
    [Serializable]
    public class Inventory
    {
        public readonly string invID; //=ItemPrefix
        private List<InventoryItem> _items = new();

        public Inventory(string prefix)
        {
            invID = prefix;
        }

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

        public int GetItemCount(string itemId)
        {
            var entry = _items.Find(i => i.ItemId == itemId);
            return entry?.Count ?? 0;
        }

        public List<InventoryItem> GetAllItems() => _items;

        public Dictionary<string, int> ToDictionary()
        {
            return _items.ToDictionary(i => i.ItemId, i => i.Count);
        }

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
