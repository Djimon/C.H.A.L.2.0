# CHAL.Systems.Inventory.MoveMode

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/MoveRequest.cs`._

# Purpose
- Defines the `MoveRequest` class for handling item movement requests in an inventory system.
- Provides the `ItemMoveObject` struct for specifying inventory item details.
- Enumerates `MoveMode` for different types of item movement operations.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `MoveRequest`
    - Public fields:
      - `ItemMoveObject fromInventory`: Source inventory details for the move.
      - `ItemMoveObject toInventory`: Destination inventory details for the move.
      - `int? amount`: Optional amount of items to move.
      - `MoveMode moveMode`: Mode of the move operation.
  - public struct `ItemMoveObject`
    - Public fields:
      - `string instanceID`: Unique identifier for the inventory item.
      - `int slot`: Slot index of the item in the inventory.
  - public enum `MoveMode`
    - Values:
      - `Move`: Standard move operation.
      - `Merge`: Combine items.
      - `Swap`: Exchange items between inventories.
      - `Split`: Divide items into separate quantities.

# Key Behavior & Side Effects
- `MoveRequest` encapsulates the details necessary for processing item movements in an inventory system.

# Constraints & Failure Modes
- `amount` is nullable, allowing for cases where the quantity is not specified.
- No explicit error handling or constraints are defined in the provided code.

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
- No information on how `MoveRequest` is utilized or processed within the inventory system.

