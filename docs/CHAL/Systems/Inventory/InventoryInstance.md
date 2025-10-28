# CHAL.Systems.Inventory.InventoryInstance

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryInstance.cs`._

# Purpose
- Defines the `InventoryInstance` class for managing inventory instances.
- Provides an enumeration `InventoryCapabilities` for inventory capability flags.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `InventoryInstance`
    - Public fields/properties:
      - `string instanceID` - Unique identifier for the inventory instance.
      - `InventoryDef InvDef` - Definition of the inventory.
      - `Slot[] slots` - Array of slots in the inventory.
      - `string ownerID` - Identifier for the owner of the inventory.
      - `InventoryCapabilities Caps` - Capabilities of the inventory.
      - `int SlotCount` - Gets the number of slots in the inventory.
    - Public methods:
      - `static InventoryInstance Create(string instanceId, InventoryDef def, string ownerId = null)` - Creates a new `InventoryInstance` with specified parameters.

  - public enum `InventoryCapabilities`
    - Flags:
      - `None` - No capabilities.
      - `ReadOnly` - Inventory is read-only.
      - `Hidden` - Inventory is hidden.
      - `Locked` - Inventory is locked.

# Key Behavior & Side Effects
- The `Create` method initializes an `InventoryInstance` with a specified ID, definition, and optional owner ID.
- It allocates slots based on the `cols` and `rows` properties of `InventoryDef`.
- Each slot is initialized with a default maximum stack size and an optional filter.

# Constraints & Failure Modes
- The `slots` array is initialized based on the `def.cols` and `def.rows` values.
- If `def.globalSlotFilter` is null, the slot filter will also be null.
- The `SlotCount` property safely handles null `slots` by returning 0.

# Example
```csharp
var inventoryDef = new InventoryDef { cols = 5, rows = 3, defaultMaxStackPerSlot = 10 };
var inventory = InventoryInstance.Create("inventory1", inventoryDef, "owner123");
```

# Unknowns
- The structure and properties of `InventoryDef` and `Slot` are not defined in this file.

