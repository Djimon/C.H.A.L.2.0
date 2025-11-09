# Assets/src/Systems/Inventory/core/ItemStack.cs

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/ItemStack.cs`._

# Purpose
- Defines the `ItemStack` struct for managing items in an inventory system.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public readonly struct ItemStack`
    - Public fields/properties:
      - `string itemID`: The identifier for the item.
      - `int count`: The quantity of the item.
    - Public methods:
      - `ItemStack(string id, int itemcount)`: Constructor to create an `ItemStack` with a specified item ID and count.
      - `ItemStack WithCount(int newCount)`: Creates a new `ItemStack` with the specified count.

# Key Behavior & Side Effects
- The `WithCount` method returns a new instance of `ItemStack` with an updated count, leaving the original instance unchanged.

# Constraints & Failure Modes
- No explicit guards or null handling are present in the code.
- The struct is immutable, ensuring thread safety when accessed concurrently.

# Example
```csharp
var stack = new ItemStack("item_001", 5);
var updatedStack = stack.WithCount(10);
```

# Unknowns
- No unknowns present in the file.
