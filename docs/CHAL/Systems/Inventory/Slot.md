# Assets/src/Systems/Inventory/core/Slot.cs

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/Slot.cs`._

# Purpose
- Defines the `Slot` and `SlotFilter` classes for managing inventory slots and item filtering.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - **public sealed class Slot**
    - `int index` - The index of the slot.
    - `int maxStack` - The maximum stack size for items in the slot.
    - `SlotFilter Filter` - Optional filter for items that can be placed in the slot.
    - `ItemStack? stack` - The current item stack in the slot; null if empty.
    - **public Slot(int i, int mStack, SlotFilter filter = null)** - Constructor to initialize a slot with an index, maximum stack size, and an optional filter.

  - **[Serializable] public sealed class SlotFilter**
    - `List<ItemType> AllowedItemTypes` - List of item types allowed in the slot.
    - `List<string> AllowedItemIds` - List of item IDs allowed in the slot.
    - `List<string> AllowedTags` - List of tags allowed in the slot.
    - `List<ItemType> BlockedItemTypes` - List of item types blocked from the slot.
    - `List<string> BlockedItemIds` - List of item IDs blocked from the slot.
    - `List<string> BlockedTags` - List of tags blocked from the slot.
    - **public bool Allows(string itemId)** - Checks if the specified item ID is allowed based on defined criteria.
    - **public bool Passes(string itemId, Func<string, IReadOnlyCollection<string>> tagResolver = null)** - Determines if an item passes certain criteria based on its ID and optional tag resolver.

# Key Behavior & Side Effects
- The `Slot` class initializes with an index, maximum stack size, and an optional filter.
- The `SlotFilter` class checks if an item is allowed based on its ID, types, and tags, considering both allowed and blocked lists.

# Constraints & Failure Modes
- The `Passes` method returns false if the item ID is null or whitespace.
- The `Passes` method evaluates against both allowed and blocked criteria; if no allowed criteria are set, all items are permitted.

# Example
```csharp
var slot = new Slot(0, 10);
var filter = new SlotFilter
{
    AllowedItemIds = new List<string> { "item1", "item2" },
    BlockedItemTypes = new List<ItemType> { ItemType.TypeA }
};
slot.Filter = filter;
```

# Unknowns
- None.

