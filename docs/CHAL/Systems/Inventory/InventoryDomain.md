# CHAL.Systems.Inventory.InventoryDomain

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryDomain.cs`._

Purpose
- Defines InventoryDomain, a concrete implementation of IInventoryDomain that manages per-instance inventories.
- Exposes optional item-info hooks (ItemExists, ItemHasTag) to influence inventory logic.
- Emits OnSlotChanged events and provides core inventory operations (peek, add, move, remove, and slot management).

Public API
- Namespace: CHAL.Systems.Inventory
- Types
  - public sealed class InventoryDomain : IInventoryDomain
    - public Func<string, bool> ItemExists
      - Hook for querying whether an itemId exists
    - public Func<string, string, bool> ItemHasTag
      - Hook: (itemId, tag) => true/false
    - public event Action<string, int, ItemStack?> OnSlotChanged
      - Invoked when a slot changes: (instanceId, slotIndex, newStack)
    - public bool HasInstance(string instanceId)
      - Returns false if instanceId is null/empty or not present
    - public InventoryInstance GetInstance(string instanceId)
      - Returns the InventoryInstance or null if not found/invalid
    - public void RegisterInstance(InventoryInstance inst)
      - Registers or updates the given instance in the internal map
    - public ItemStack? Peek(string instanceId, int slotIndex)
      - Returns the stack at slotIndex or null if invalid/missing
    - public int SlotCount(string instanceId)
      - Returns number of slots for the given instance or 0 if not found
    - public void ClearAllSlots(string instanceId)
      - Clears all slot stacks for the instance, emitting OnSlotChanged per slot
    - public bool CanAccept(string instanceId, in ItemStack stack)
      - Checks if the given stack can be accepted by the inventory (considers existing stacks and empty slots, with filtering)
    - public bool TryAdd(string instanceId, in ItemStack stack, out TransactionResult result)
      - Attempts to add items into the inventory, updating slots and emitting OnSlotChanged
      - Fails with result.reason "InstanceNotFound" if missing
    - public bool TryMove(in MoveRequest req, out TransactionResult result)
      - Moves items between inventories according to MoveRequest
      - Multiple move modes (Move, Split, Merge, Swap) with filtering and slot logic
      - Emits OnSlotChanged for affected slots and records SlotDeltas
    - public bool TryRemove(string instanceId, int slotIndex, int amount, out TransactionResult result)
      - Removes items from a slot, updating slot and emitting OnSlotChanged
internal/private helpers (not public API surface)
- private static bool PassesFilter(Slot slot, string itemId)
  - Applies slot.Filter rules:
    - BlockedItemIds: fails if itemId is blocked
    - AllowedItemIds: requires itemId to be in the allowed list
    - BlockedItemTypes / AllowedItemTypes: validates item type via ItemTypeUtils.FromId
    - Note: includes debug logging via DebugManager for failures

Key Behavior & Side Effects
- Instance management
  - RegisterInstance adds/updates entries in _instances by inst.instanceID
  - HasInstance/GetInstance return status or instance/null for invalid IDs
- Slot change notifications
  - OnSlotChanged is invoked after any slot mutation (add, move, remove, or swap)
  - ClearAllSlots emits OnSlotChanged for every slot set to null
- Filtering and validation
  - PassesFilter enforces block/allow lists and type constraints for a slot and item
  - CanAccept uses PassesFilter to decide if existing stacks can be filled or empty slots used
- Add/move/remove semantics
  - TryAdd fills existing stacks of matching itemID first, then fills empty slots, all respecting slot filters
  - TryMove supports:
    - Auto-target selection when toInventory.slot < 0
    - Move: swap/merge behavior with filter checks and max stack enforcement
    - Merge: combines stacks if same itemID and space remains
    - Swap: exchanges stacks if both pass filter checks
  - TryRemove reduces stack counts up to requested amount and updates the source slot
- Logging
  - Uses DebugManager.Log to emit debug messages for filter failures and inventory actions

Constraints & Failure Modes
- Defensive checks
  - HasInstance/GetInstance return early on null/empty IDs
  - Peek/SlotCount return safe defaults when instance is absent
  - TryAdd/Move/Remove return success/failure with a populated TransactionResult
- Thread-safety
  - Internal _instances dictionary is not synchronized; concurrent access is not protected
- Error signaling
  - Several operations set result.reason (e.g., InstanceNotFound, NoSpace, FilterFailed, TargetOccupied, etc.)
  - On failures, actions may partially mutate state (e.g., partial fills) depending on flow
- Filtering behavior
  - PassesFilter may cause actions to fail even when a later operation could succeed if filters were relaxed

Unknowns
- Definitions and semantics of:
  - InventoryInstance (structure of slots, Slot type, and their fields)
  - ItemStack (itemID, count, WithCount, and nullable handling)
  - Slot type (Filter, maxStack, and its properties)
  - MoveRequest, MoveMode, TransactionResult, and related fields (SlotDeltas, success flag, reason)
  - IInventoryDomain interface (contract and expected usage)
  - Move/Filter related classes (DebugManager, ItemTypeUtils, etc.)
- Exact contents of related types (InventoryInstance.slots, Slot.Filter, etc.)
- Behavior of ItemExists, ItemHasTag hooks in Unity integration or external adapters

Notes
- This file is the single source of truth for InventoryDomain’s public surface as shown; external project parts define related types and runtime behavior.
