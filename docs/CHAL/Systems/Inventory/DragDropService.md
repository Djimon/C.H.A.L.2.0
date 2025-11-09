# Assets/src/Systems/Inventory/core/DragDropService.cs

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/DragDropService.cs`._

# Purpose
- Defines the `DragDropService` class for managing drag-and-drop operations in an inventory system.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public sealed class `DragDropService`
    - Public fields/properties:
      - `bool HasFrom` - Indicates if there is an item being dragged.
      - `ItemMoveObject From` - The source item being dragged.
      - `bool IsSplit` - Indicates if the item is split in half during the drag.
    - Public methods:
      - `DragDropService(IInventoryDomain domain)` - Constructor that initializes the service with an inventory domain.
      - `void BeginDrag(ItemMoveObject from, bool splitHalf)` - Initiates the drag operation for an item.
      - `void Cancel()` - Cancels the current drag operation.
      - `void TryDropOn(ItemMoveObject to)` - Attempts to drop an item onto the specified target object.

# Key Behavior & Side Effects
- `BeginDrag` sets the source item and whether it should be split, and triggers `OnBeginDrag` if the item stack is valid.
- `Cancel` resets the drag state and triggers `OnEndDrag`.
- `TryDropOn` checks if the drop target is the same as the source; if so, it cancels the operation if splitting is requested. It attempts to move the item and cancels if the move fails.

# Constraints & Failure Modes
- `TryDropOn` will not proceed if there is no item being dragged (`_hasFrom` is false).
- If the source and target are the same and splitting is requested, the operation is canceled.
- The move operation can fail, in which case the drag is canceled.

# Example
```csharp
var dragDropService = new DragDropService(inventoryDomain);
dragDropService.BeginDrag(itemMoveObject, true);
dragDropService.TryDropOn(targetItemMoveObject);
```

# Unknowns
- The implementation details of `IInventoryDomain` and `MoveRequest` are not provided in this file.

