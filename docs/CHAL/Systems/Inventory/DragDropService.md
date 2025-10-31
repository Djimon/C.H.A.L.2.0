# CHAL.Systems.Inventory.DragDropService

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/DragDropService.cs`._

# DragDropService.cs

## Purpose
- Defines the `DragDropService` class for handling drag-and-drop operations in an inventory system.

## Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public sealed class DragDropService`
    - Public fields/properties:
      - `public bool HasFrom` - Indicates if there is an item being dragged.
      - `public ItemMoveObject From` - The source item being dragged.
      - `public bool IsSplit` - Indicates if the drag operation is a split.
    - Public methods:
      - `public DragDropService(IInventoryDomain domain)` - Constructor that initializes the service with an inventory domain.
      - `public event Action<ItemStack,bool> OnBeginDrag` - Event triggered when dragging begins.
      - `public event Action OnEndDrag` - Event triggered when dragging ends.
      - `public void BeginDrag(ItemMoveObject from, bool splitHalf)` - Starts the drag operation.
      - `public void Cancel()` - Cancels the drag operation.
      - `public void TryDropOn(ItemMoveObject to)` - Attempts to drop the item on a target.

## Key Behavior & Side Effects
- `BeginDrag` initializes the drag operation and triggers `OnBeginDrag` if an item stack is available.
- `Cancel` resets the drag state and triggers `OnEndDrag`.
- `TryDropOn` checks if the drop target is the same as the source; if so, it cancels the operation if splitting is attempted. It also attempts to move the item and logs success or failure.

## Constraints & Failure Modes
- Drag operation can only proceed if an item is set as the source (`_hasFrom`).
- If the drop target is the same as the source and a split is attempted, the operation is canceled.
- If the move operation fails, it logs the reason and cancels the drag.

## Example
```csharp
var dragDropService = new DragDropService(inventoryDomain);
dragDropService.BeginDrag(itemMoveObject, true);
dragDropService.TryDropOn(targetItemMoveObject);
```

## Unknowns
- The implementation details of `IInventoryDomain`, `ItemMoveObject`, `ItemStack`, and `MoveRequest` are not provided in this file.

