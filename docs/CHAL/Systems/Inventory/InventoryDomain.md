# CHAL.Systems.Inventory.InventoryDomain

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryDomain.cs`._

1) Purpose
- Defines InventoryDomain, a domain service managing InventoryInstance objects by ID.
- Provides optional adapters for item existence and tags via ItemExists and ItemHasTag.
- Emits OnSlotChanged when a slot's stack changes.

2) Public API
- Namespace/module: CHAL.Systems.Inventory
- Type: public sealed class InventoryDomain : IInventoryDomain
  - Public fields/properties
    - public Func<string, bool> ItemExists
    - public Func<string, string, bool> ItemHasTag
  - Public events
    - public event Action<string, int, ItemStack?> OnSlotChanged
  - Public methods
    - public bool HasInstance(string instanceId)
    - public InventoryInstance GetInstance(string instanceId)
    - public void RegisterInstance(InventoryInstance inst)
    - public ItemStack? Peek(string instanceId, int slotIndex)
    - public int SlotCount(string instanceId)
    - public void ClearAllSlots(string instanceId)
    - public bool CanAccept(string instanceId, in ItemStack stack)
    - public bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result)
    - public bool TryMove(in MoveRequest req, out TransactionResult result)
    - public bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result)
    - internal bool TryGetInstance(string inventoryID, out InventoryInstance inst)

3) Key Behavior & Side Effects
- HasInstance
  - Returns false if instanceId is null/empty; otherwise checks internal store.
- GetInstance
  - Returns null if instanceId is null/empty; otherwise retrieves from store.
- RegisterInstance
  - Stores/overwrites the given InventoryInstance by its instanceID.
- Peek
  - Returns null if instance not found or slotIndex out of range; otherwise returns the slot's stack.
- SlotCount
  - Returns 0 if instance not found; otherwise returns number of slots in the inventory.
- ClearAllSlots
  - For each slot in the instance, sets stack to null and raises OnSlotChanged(instanceId, i, null).
- PassesFilter (private)
  - Evaluates a Slot's Filter against an itemId:
    - Denies if itemId is in BlockedItemIds.
    - Denies if itemId not in AllowedItemIds (when any AllowedItemIds provided).
    - Evaluates BlockedItemTypes and AllowedItemTypes against ItemTypeUtils.FromId(itemId).
    - Logs filter failures via DebugManager.
- CanAccept
  - Returns false if instance missing or stack.count <= 0.
  - Tries to fill existing same-item stacks (respecting slot.Filter) and then empty slots (respecting filters) to fit the entire stack.
- TryAdd
  - If instance not found, returns false with reason "InstanceNotFound".
  - Phase 1: Fill existing stacks with same itemID if possible (respecting PassesFilter and maxStack).
  - Phase 2: Fill empty slots that pass the filter (respecting maxStack).
  - Updates slot stacks, records deltas, logs, and invokes OnSlotChanged for affected slots.
  - Returns true only if entire stack was placed; otherwise sets result.reason to "NoSpace".
- TryMove
  - Validates source and destination instances; returns "InstanceNotFound" if missing.
  - If toInventory.slot < 0, searches for a fitting target slot in destination (empty or same-item with space), recursively calling TryMove when a candidate is found.
  - Otherwise moves between specific slots:
    - Gathers moving amount (supports Split and Move modes).
    - Applies PassesFilter on target and source as applicable.
    - Move: can swap or place into empty slot; handles merging into same-item stacks and swapping when permitted.
    - Merge: requires same item and available space; aggregates counts up to maxStack.
    - Swap: exchanges stacks if both pass filters.
  - Emits OnSlotChanged for affected slots and records SlotDeltas.
  - Returns success only when a valid operation completes; otherwise sets appropriate reason (e.g., FilterFailed, TargetOccupied, MaxStackReached, etc.).
- TryRemove
  - Validates instance and slot index; checks for existing stack and positive amount.
  - Reduces or clears the source stack by the requested amount.
  - Emits OnSlotChanged and records SlotDeltas on removal.
- TryGetInstance
  - Internal helper: returns false if inventoryID is null/empty; otherwise looks up instance in the dictionary.

4) Constraints & Failure Modes
- Guards
  - HasInstance/GetInstance explicitly handle null/empty IDs.
  - TryAdd/TryMove/TryRemove return explicit failure reasons in result when the operation cannot proceed (e.g., InstanceNotFound, NoSpace, SourceEmpty, InvalidAmount, TargetOccupied, FilterFailed, etc.).
- Null/empty handling
  - Peek, GetInstance, TryGetInstance guard against null or empty inputs; 0-slot inventories are handled gracefully.
- Threading/async
  - No explicit threading/async semantics; all operations are synchronous and rely on shared state in _instances.
- Logging/Debug
  - Uses DebugManager for filter and notable flow messages; side effects depend on DebugManager configuration.

5) Example
- Not provided (not clearly derivable from the file alone without additional type definitions).

6) Unknowns
- Definitions and public surface of:
  - InventoryInstance, ItemStack, MoveRequest, TransactionResult, Slot, Filter, MoveMode, MoveTarget, ItemTypeUtils, DebugManager, and IInventoryDomain.
- Exact shapes of InventoryInstance.slots, Slot.Filter, ItemStack fields (itemID, count, maxStack), and how Create/Instantiate InventoryInstance objects should be constructed.
- Any external effects of ItemExists/ItemHasTag beyond being declared; their runtime usage is not shown here.
