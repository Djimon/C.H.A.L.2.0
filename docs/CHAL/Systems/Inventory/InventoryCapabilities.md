# CHAL.Systems.Inventory.InventoryCapabilities

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryInstance.cs`._

Purpose
- Defines InventoryInstance class that models a runtime inventory with an identifier, definition, owner, capability flags, and per-slot storage.
- Exposes core data: instanceID, InvDef, slots, ownerID, Caps.
- Provides SlotCount as a convenience for the number of slots (0 if slots is null).
- Provides a static factory Create to initialize a new InventoryInstance from an InventoryDef.

Public API
- Namespace: CHAL.Systems.Inventory

- Types
  - public class InventoryInstance
    - public string instanceID
      - Role: unique identifier of this inventory instance
    - public InventoryDef InvDef
      - Role: the inventory definition used to configure this instance
    - public Slot[] slots
      - Role: per-slot data storage for the inventory
    - public string ownerID
      - Role: identifier of the owner (optional)
    - public InventoryCapabilities Caps
      - Role: capability flags for this inventory
    - public int SlotCount
      - Getter: returns slots?.Length ?? 0
    - public static InventoryInstance Create(string instanceId, InventoryDef def, string ownerId = null)
      - Creates a new InventoryInstance with:
        - instanceID = instanceId
        - InvDef = def
        - ownerID = ownerId
        - Caps = InventoryCapabilities.None
        - slots = new Slot[def.cols * def.rows]
        - For each slot index i, creates Slot(i, def.defaultMaxStackPerSlot, filter) where:
          - filter = def.globalSlotFilter if non-null, else null
        - Returns the constructed InventoryInstance

  - public enum InventoryCapabilities [Flags]
    - public None = 0
    - public ReadOnly = 1 << 0
    - public Hidden = 1 << 1
    - public Locked = 1 << 2

Key Behavior & Side Effects
- Create(string instanceId, InventoryDef def, string ownerId = null)
  - Allocates slots to a length of def.cols * def.rows.
  - Initializes each slot with:
    - index = i
    - maxStackPerSlot = def.defaultMaxStackPerSlot
    - filter = def.globalSlotFilter if not null; otherwise null
  - Sets Caps to InventoryCapabilities.None.
  - Returns the new InventoryInstance.
- SlotCount is derived from the current slots array length or 0 if slots is null.
- No defensive checks on def; passing a null def will cause a NullReferenceException when accessing def.cols/def.rows.
- All fields are public; external code can mutate instanceID, InvDef, slots, ownerID, and Caps after creation.

Constraints & Failure Modes
- Guards: None for null def; null def leads to runtime failures when accessing def members.
- Threading/async: Not specified; no synchronization guarantees.
- Validation: No validation on def fields (cols, rows, defaultMaxStackPerSlot, globalSlotFilter) beyond their use in Create.
- Mutability: Public fields allow external modification after creation (including slots content).

Unknowns
- Definitions of InventoryDef, Slot, and their members (e.g., cols, rows, defaultMaxStackPerSlot, globalSlotFilter) are not present in this file.
- Semantics of Slot constructor (Slot(int index, int maxStack, object filter)) are inferred from usage here.
- Behavior of InventoryCapabilities interplay with other systems is not defined beyond the enum in this file.
