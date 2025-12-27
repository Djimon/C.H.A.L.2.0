using CHAL.Data;
using CHAL.Systems.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHAL.Systems.Inventory
{
    public sealed class Slot
    {
        public int index { get; }
        public int maxStack { get; internal set; } // aus Def oder Fallback-Regeln
        public SlotFilter Filter { get; internal set; } // optional
        public ItemStackRef? stack { get; internal set; } // null => leer



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

/// <summary>
/// Checks if the specified item ID is allowed based on defined criteria.
/// </summary>
/// <param name="itemId">The ID of the item to check.</param>
/// <returns>True if the item is allowed; otherwise, false.</returns>
        public bool Allows(string itemId)
        {
            // nutzt deine Allowed/Blocked (Ids/Types/Tags); hier nur der Call-Signatur-Vorschlag
            return Passes(itemId);
        }

/// <summary>
/// Determines if an item passes certain criteria based on its ID and optional tag resolver.
/// </summary>
/// <param name="itemId">The ID of the item to evaluate.</param>
/// <param name="tagResolver">An optional function to resolve tags for the item.</param>
/// <returns>True if the item passes the criteria; otherwise, false.</returns>
        public bool Passes(string itemId, Func<string, IReadOnlyCollection<string>> tagResolver = null)
        {
            if (string.IsNullOrWhiteSpace(itemId)) return false;

            var type = ItemTypeUtils.FromId(itemId); // deine Utility

            bool InIds(IReadOnlyCollection<string> set, string id) =>
                set != null && set.Any(s => string.Equals(s, id, StringComparison.OrdinalIgnoreCase));

            bool InTypes(IReadOnlyCollection<ItemType> set, ItemType t) =>
                set != null && set.Contains(t);

            bool InTags(IReadOnlyCollection<string> set, IReadOnlyCollection<string> tags) =>
                set != null && tags != null && set.Any(rule =>
                    tags.Any(tag => string.Equals(tag, rule, StringComparison.OrdinalIgnoreCase)));

            var tags = tagResolver?.Invoke(itemId);

            // 1) Blocked? -> sofort raus
            if (InIds(BlockedItemIds, itemId) ||
                InTypes(BlockedItemTypes, type) ||
                InTags(BlockedTags, tags))
                return false;

            // 2) Keine Allowed-Listen gesetzt? -> erlaubt
            bool anyAllowedConfigured =
                (AllowedItemIds != null && AllowedItemIds.Count > 0) ||
                (AllowedItemTypes != null && AllowedItemTypes.Count > 0) ||
                (AllowedTags != null && AllowedTags.Count > 0);

            if (!anyAllowedConfigured) return true;

            // 3) Mindestens eine Allowed-Regel muss matchen
            if (InIds(AllowedItemIds, itemId)) return true;
            if (InTypes(AllowedItemTypes, type)) return true;
            if (InTags(AllowedTags, tags)) return true;

            return false;
        }
    }

}
