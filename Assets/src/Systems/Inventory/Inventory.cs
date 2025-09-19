using System;
using System.Collections.Generic;

namespace CHAL.Systems.Inventory
{
    [Serializable]
    public class Inventory
    {
        private readonly string _prefix;
        private List<InventoryItem> _items = new();

        public Inventory(string prefix)
        {
            _prefix = prefix;
        }

        public bool AddItem(string itemId, int amount = 1)
        {
            if (!itemId.StartsWith(_prefix))
                return false;

            int maxStack = InventoryRules.GetMaxStack(_prefix);
            int maxSlots = InventoryRules.GetMaxSlots(_prefix);

            // existierenden Stack suchen
            var entry = _items.Find(i => i.ItemId == itemId);
            if (entry != null)
            {
                int spaceLeft = maxStack - entry.Count;
                if (spaceLeft <= 0)
                    return false;

                int toAdd = Math.Min(amount, spaceLeft);
                entry.Count += toAdd;
                amount -= toAdd;
            }

            // neue Slots anlegen, falls noch Platz
            while (amount > 0 && _items.Count < maxSlots)
            {
                int toAdd = Math.Min(amount, maxStack);
                _items.Add(new InventoryItem { ItemId = itemId, Count = toAdd });
                amount -= toAdd;
            }

            // falls nicht alles reinpasst
            return amount == 0;
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
    }

    [Serializable]
    public class InventoryItem
    {
        public string ItemId;
        public int Count;
    }
}
