# CHAL.Systems.Inventory.Slot

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/Slot.cs`._

# Purpose
- Defines the `Slot` and `SlotFilter` classes for managing inventory slots and item filtering.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - **public sealed class Slot**
    - `int index`: The index of the slot.
    - `int maxStack`: The maximum stack size for items in the slot.
    - `SlotFilter Filter`: Optional filter for items that can be placed in the slot.
    - `ItemStack? stack`: The current item stack in the slot; null if empty.
    - **public Slot(int i, int mStack, SlotFilter filter = null)**: Constructor to initialize a slot with an index, maximum stack size, and an optional filter.

  - **[Serializable] public sealed class SlotFilter**
    - `List<ItemType> AllowedItemTypes`: List of allowed item types.
    - `List<string> AllowedItemIds`: List of allowed item IDs.
    - `List<string> AllowedTags`: List of allowed tags.
    - `List<ItemType> BlockedItemTypes`: List of blocked item types.
    - `List<string> BlockedItemIds`: List of blocked item IDs.
    - `List<string> BlockedTags`: List of blocked tags.
    - **public bool Allows(string itemId)**: Checks if the specified item ID is allowed based on defined criteria.
    - **public bool Passes(string itemId, Func<string, IReadOnlyCollection<string>> tagResolver = null)**: Determines if an item passes certain criteria based on its ID and optional tag resolver.

# Key Behavior & Side Effects
- The `Slot` class manages the state of an inventory slot, including its index, maximum stack size, and current item stack.
- The `SlotFilter` class provides methods to determine if an item is allowed or blocked based on its ID, type, and tags.

# Constraints & Failure Modes
- The `Passes` method in `SlotFilter` returns false if the `itemId` is null or whitespace.
- The filtering logic checks both allowed and blocked lists; if no allowed lists are configured, all items are permitted.

# Example
```csharp
var slot = new Slot(0, 10);
var filter = new SlotFilter
{
    AllowedItemIds = new List<string> { "item1", "item2" },
    BlockedItemTypes = new List<ItemType> { ItemType.Weapon }
};
slot.Filter = filter;
```

# Unknowns
- None.

