# CHAL.Systems.Inventory.ItemMoveObject

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
      - `ItemMoveObject fromInventory`: Source inventory item details.
      - `ItemMoveObject toInventory`: Destination inventory item details.
      - `int? amount`: Optional amount of items to move.
      - `MoveMode moveMode`: Mode of the move operation.
  - public struct `ItemMoveObject`
    - Public fields:
      - `string instanceID`: Unique identifier for the inventory item.
      - `int slot`: Slot index of the inventory item.
  - public enum `MoveMode`
    - Values:
      - `Move`: Standard move operation.
      - `Merge`: Combine items.
      - `Swap`: Exchange items between inventories.
      - `Split`: Divide items into smaller quantities.

# Key Behavior & Side Effects
- The `MoveRequest` class encapsulates the details necessary for processing item movements in an inventory system.

# Constraints & Failure Modes
- The `amount` field is nullable, allowing for cases where the amount is not specified.
- The `moveMode` field determines the behavior of the move operation and must be set appropriately.

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
- None.
