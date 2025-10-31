# CHAL.Systems.Inventory.ItemStack

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/ItemStack.cs`._

# Purpose
- Defines the `ItemStack` structure for managing items in an inventory system.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public readonly struct ItemStack`
    - Public fields/properties:
      - `string itemID`: Identifier for the item.
      - `int count`: Quantity of the item.
    - Public methods:
      - `ItemStack(string id, int itemcount)`: Constructor to initialize an `ItemStack` with an item ID and count.
      - `ItemStack WithCount(int newCount)`: Returns a new `ItemStack` with the specified count.

# Key Behavior & Side Effects
- The `WithCount` method creates a new instance of `ItemStack` with a modified count, leaving the original instance unchanged.

# Constraints & Failure Modes
- No explicit null or empty handling is present for `itemID` or `count`.
- The struct is immutable, ensuring thread safety when accessed concurrently.

# Example
```csharp
var stack = new ItemStack("item_001", 5);
var updatedStack = stack.WithCount(10);
```

# Unknowns
- No information on how `ItemStack` interacts with other components of the inventory system.
