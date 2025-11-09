# Assets/src/Systems/Inventory/core/InventoryDomain.cs

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryDomain.cs`._

# Purpose
- Defines the `InventoryDomain` class, which manages inventory instances and their operations.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `sealed class InventoryDomain : IInventoryDomain`
    - Public fields/properties:
      - `Func<string, bool> ItemExists;`
      - `Func<string, string, bool> ItemHasTag;`
      - `event Action<string, int, ItemStack?> OnSlotChanged;`
    - Public methods:
      - `bool HasInstance(string instanceId);`
      - `InventoryInstance GetInstance(string instanceId);`
      - `void RegisterInstance(InventoryInstance inst);`
      - `ItemStack? Peek(string instanceId, int slotIndex);`
      - `int SlotCount(string instanceId);`
      - `void ClearAllSlots(string instanceId);`
      - `bool CanAccept(string instanceId, in ItemStack stack);`
      - `bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result);`
      - `bool TryMove(in MoveRequest req, out TransactionResult result);`
      - `bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result);`
      - `internal bool TryGetInstance(string inventoryID, out InventoryInstance inst);`

# Key Behavior & Side Effects
- `OnSlotChanged` event is invoked whenever a slot's item stack changes.
- Methods handle null checks and ensure valid indices for inventory operations.
- `TryAdd` and `TryMove` methods include filtering logic to determine if items can be added or moved based on slot filters.

# Constraints & Failure Modes
- Methods return false and set a reason in `TransactionResult` for various failure conditions, such as:
  - Instance not found
  - Index out of range
  - Source empty
  - Invalid amount
  - No valid target slot
  - Max stack reached
  - Filter failed
- Handles null or empty strings for instance IDs.

# Example
```csharp
var inventoryDomain = new InventoryDomain();
var instance = new InventoryInstance("exampleID");
inventoryDomain.RegisterInstance(instance);
bool exists = inventoryDomain.HasInstance("exampleID");
```

# Unknowns
- The implementation details of `InventoryInstance`, `ItemStack`, `TransactionResult`, and `MoveRequest` are not provided in this file.
- The behavior of `ItemTypeUtils.FromId` and `slot.Filter.Allows` is not defined in this file.

