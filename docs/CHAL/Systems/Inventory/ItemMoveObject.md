# CHAL.Systems.Inventory.ItemMoveObject

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/MoveRequest.cs`._

# Purpose
- Defines the `MoveRequest` class for handling item movement within inventories.
- Provides the `ItemMoveObject` struct to represent inventory items and their slots.
- Enumerates `MoveMode` for different types of item movement operations.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `MoveRequest`
    - Public fields/properties:
      - `ItemMoveObject fromInventory` - Source inventory item.
      - `ItemMoveObject toInventory` - Destination inventory item.
      - `int? amount` - Optional amount of items to move.
      - `MoveMode moveMode` - Mode of the move operation.
  - public struct `ItemMoveObject`
    - Public fields/properties:
      - `string instanceID` - Unique identifier for the inventory item.
      - `int slot` - Slot index of the item in the inventory.
  - public enum `MoveMode`
    - Values:
      - `Move` - Move items from one inventory to another.
      - `Merge` - Combine items from two inventories.
      - `Swap` - Exchange items between two inventories.
      - `Split` - Divide items between inventories.

# Key Behavior & Side Effects
- `MoveRequest` encapsulates the details necessary for processing item movements between inventories.

# Constraints & Failure Modes
- `amount` is nullable, indicating that it may not always be specified.
- No explicit threading or async handling noted.

# Example
```csharp
var moveRequest = new MoveRequest
{
    fromInventory = new ItemMoveObject { instanceID = "item123", slot = 1 },
    toInventory = new ItemMoveObject { instanceID = "item456", slot = 2 },
    amount = 5,
    moveMode = MoveMode.Move
};
```

# Unknowns
- No information on how `MoveRequest` is utilized or processed within the broader system.

