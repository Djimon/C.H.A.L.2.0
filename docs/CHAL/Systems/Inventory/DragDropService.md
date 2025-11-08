# CHAL.Systems.Inventory.DragDropService

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/DragDropService.cs`._

# Purpose
- Defines the `DragDropService` for managing drag-and-drop operations in an inventory system.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public sealed class DragDropService`
    - Public fields/properties:
      - `public bool HasFrom` - Indicates if there is an item being dragged.
      - `public ItemMoveObject From` - The item being dragged, used for ghost text/icon.
      - `public bool IsSplit` - Indicates if the item is split in half.
    - Public methods:
      - `public DragDropService(IInventoryDomain domain)` - Constructor that initializes the service with an inventory domain.
      - `public void BeginDrag(ItemMoveObject from, bool splitHalf)` - Initiates the drag operation for an item.
      - `public void Cancel()` - Cancels the current drag operation.
      - `public void TryDropOn(ItemMoveObject to)` - Attempts to drop the item onto the specified target object.

# Key Behavior & Side Effects
- `BeginDrag` sets the source item and whether it should be split, and triggers `OnBeginDrag` if the item stack is valid.
- `Cancel` resets the drag state and triggers `OnEndDrag`.
- `TryDropOn` checks if the item can be dropped on the target; if not, it cancels the operation.

# Constraints & Failure Modes
- `TryDropOn` does not proceed if there is no item being dragged (`_hasFrom` is false).
- If the item is dropped on the same slot and is split, the operation is canceled.
- If the move operation fails, it logs the reason and cancels the drag.

# Example
```csharp
var dragDropService = new DragDropService(inventoryDomain);
dragDropService.BeginDrag(itemMoveObject, true);
dragDropService.TryDropOn(targetItemMoveObject);
```

# Unknowns
- None.

