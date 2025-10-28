# Assets/src/Systems/Inventory/core/MoveRequest.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `MoveRequest` class for handling item movement requests in an inventory system.
- Provides the `ItemMoveObject` struct to represent inventory item details.
- Enumerates `MoveMode` for different types of item movement operations.

## Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - **public class** `MoveRequest`
    - **public ItemMoveObject** `fromInventory` - Source inventory details for the move.
    - **public ItemMoveObject** `toInventory` - Destination inventory details for the move.
    - **public int?** `amount` - Optional amount of items to move.
    - **public MoveMode** `moveMode` - Mode of the move operation.
  
  - **public struct** `ItemMoveObject`
    - **public string** `instanceID` - Unique identifier for the inventory item.
    - **public int** `slot` - Slot index of the item in the inventory.

  - **public enum** `MoveMode`
    - `Move` - Move items from one inventory to another.
    - `Merge` - Combine items from two inventories.
    - `Swap` - Exchange items between two inventories.
    - `Split` - Divide items between inventories.

## Key Behavior & Side Effects
- The `MoveRequest` class encapsulates the details necessary for processing item movements within an inventory system.

## Constraints & Failure Modes
- The `amount` field is nullable, allowing for cases where the amount is not specified.
- No explicit error handling or constraints are defined within the provided code.

## Example
```csharp
var moveRequest = new MoveRequest
{
    fromInventory = new ItemMoveObject { instanceID = "item123", slot = 0 },
    toInventory = new ItemMoveObject { instanceID = "item456", slot = 1 },
    amount = 5,
    moveMode = MoveMode.Move
};
```

## Unknowns
- No information on how `MoveRequest` is utilized or processed within the inventory system.
```
