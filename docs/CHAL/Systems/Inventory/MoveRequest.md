# CHAL.Systems.Inventory.MoveRequest

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/MoveRequest.cs`._

# Purpose
- Defines the `MoveRequest` class for handling item movement between inventories.
- Provides the `ItemMoveObject` struct to represent inventory items and their slots.
- Enumerates `MoveMode` for different types of item movement operations.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `MoveRequest`
    - Public fields:
      - `ItemMoveObject fromInventory` - Source inventory item.
      - `ItemMoveObject toInventory` - Destination inventory item.
      - `int? amount` - Optional amount of items to move.
      - `MoveMode moveMode` - Mode of the move operation.
  - public struct `ItemMoveObject`
    - Public fields:
      - `string instanceID` - Unique identifier for the inventory item.
      - `int slot` - Slot index of the item in the inventory.
  - public enum `MoveMode`
    - Values:
      - `Move` - Move items.
      - `Merge` - Merge items.
      - `Swap` - Swap items.
      - `Split` - Split items.

# Key Behavior & Side Effects
- `MoveRequest` encapsulates the details required to perform an item move operation between inventories.

# Constraints & Failure Modes
- `amount` is nullable, indicating that it may not always be specified.
- No explicit error handling or constraints are defined in the provided code.

# Example
```csharp
var moveRequest = new MoveRequest
{
    fromInventory = new ItemMoveObject { instanceID = "item1", slot = 0 },
    toInventory = new ItemMoveObject { instanceID = "item2", slot = 1 },
    amount = 5,
    moveMode = MoveMode.Move
};
```

# Unknowns
- No information on how `MoveRequest` is used or validated in the broader system context.

