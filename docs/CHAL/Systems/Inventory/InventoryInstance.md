# CHAL.Systems.Inventory.InventoryInstance

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryInstance.cs`._

```csharp
// Documentation for: Assets/src/Systems/Inventory/core/InventoryInstance.cs

1) Purpose
- Defines InventoryInstance with basic inventory data: ID, definition, owned slot array, owner, and capabilities.
- Provides a static Create helper to instantiate and populate an InventoryInstance from an InventoryDef.
- Exposes public fields for external access and a computed SlotCount property.

2) Public API
- Namespace/module
  - CHAL.Systems.Inventory

- Types

  - public class InventoryInstance
    - public string instanceID
      - Unique identifier for this inventory instance
    - public InventoryDef InvDef
      - Reference to the inventory definition used to configure this instance
    - public Slot[] slots
      - Array of Slot objects, one per slot in the inventory
    - public string ownerID
      - Optional owner identifier
    - public InventoryCapabilities Caps
      - Flags describing capabilities (None by default)

    - public int SlotCount => slots?.Length ?? 0
      - Number of slots; 0 if slots is null

    - public static InventoryInstance Create(string instanceId, InventoryDef def, string ownerId = null)
      - Creates and returns a new InventoryInstance
      - Initializes:
        - instanceID = instanceId
        - InvDef = def
        - ownerID = ownerId
        - Caps = InventoryCapabilities.None
        - slots = new Slot[def.cols * def.rows]
      - For each slot index i:
        - filter = def.globalSlotFilter != null ? def.globalSlotFilter : null
        - slots[i] = new Slot(i, def.defaultMaxStackPerSlot, filter)

  - public enum InventoryCapabilities [Flags]
    - None = 0
      - No special capabilities
    - ReadOnly = 1 << 0
      - Inventory is read-only
    - Hidden = 1 << 1
      - Inventory is hidden
    - Locked = 1 << 2
      - Inventory is locked

3) Key Behavior & Side Effects
- Create flow:
  - Allocates slots array sized cols * rows from def
  - Computes a shared filter value: def.globalSlotFilter if not null, otherwise null
  - Constructs each slot with parameters: index, def.defaultMaxStackPerSlot, and the computed filter
  - Returns a fully initialized InventoryInstance
- SlotCount reflects current slots array length
- Caps is explicitly initialized to InventoryCapabilities.None on creation
- No validation for null def; passing a null def will raise at runtime when accessing def.cols/def.rows

4) Constraints & Failure Modes
- If def is null during Create, NullReferenceException occurs when accessing def.cols/def.rows/etc.
- No null checks for fields; public fields can be mutated by external code
- No thread-safety guarantees; Create performs a simple, synchronous construction
- Potential extremely large arrays if def.cols or def.rows are large (limits depend on runtime)

5) Example
- Minimal usage pattern (exact types depend on locally defined InventoryDef/Slot):
```csharp
using CHAL.Systems.Inventory;

InventoryDef def = GetInventoryDefSomehow();
InventoryInstance inv = InventoryInstance.Create("inst-001", def, "owner-42");
```

6) Unknowns
- Definition and members of InventoryDef (beyond the fields accessed here)
- Definition and behavior of Slot (constructor signature is used here)
- How InventoryInstance and its fields are used elsewhere (e.g., mutability semantics, serialization)
- Any additional behavior tied to Caps beyond storage of the flag
- Expected thread-safety or lifecycle management for InventoryInstance instances
```
