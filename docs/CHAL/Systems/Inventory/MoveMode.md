# CHAL.Systems.Inventory.MoveMode

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/MoveRequest.cs`._

# Purpose
- Defines the `MoveRequest` class for handling item movement between inventories.
- Provides the `ItemMoveObject` struct to represent inventory items and their slots.
- Enumerates possible movement modes with the `MoveMode` enum.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `MoveRequest`
    - Public fields:
      - `ItemMoveObject fromInventory`: Source inventory item details.
      - `ItemMoveObject toInventory`: Destination inventory item details.
      - `int? amount`: Optional amount of items to move.
      - `MoveMode moveMode`: Mode of the move operation.
  - public struct `ItemMoveObject`
    - Public fields:
      - `string instanceID`: Unique identifier for the inventory item.
      - `int slot`: Slot index of the item in the inventory.
  - public enum `MoveMode`
    - Values:
      - `Move`: Standard item movement.
      - `Merge`: Combine items.
      - `Swap`: Exchange items between inventories.
      - `Split`: Divide items into separate quantities.

# Key Behavior & Side Effects
- The `MoveRequest` class encapsulates the details necessary for executing an item move operation, including source and destination inventories, the amount to move, and the type of move.

# Constraints & Failure Modes
- The `amount` field is nullable, indicating that it may not always be specified.
- No explicit error handling is defined within the structures or enums.

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
