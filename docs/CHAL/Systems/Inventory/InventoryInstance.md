# Assets/src/Systems/Inventory/core/InventoryInstance.cs

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryInstance.cs`._

# Purpose
- Defines the `InventoryInstance` class representing an instance of an inventory, containing slots and associated data.
- Provides a method to create a new `InventoryInstance` with specified parameters.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `InventoryInstance`
    - Public fields/properties:
      - `string instanceID`: Unique identifier for the inventory instance.
      - `InventoryDef InvDef`: Definition of the inventory.
      - `Slot[] slots`: Array of slots in the inventory.
      - `string ownerID`: Optional owner identifier for the inventory instance.
      - `InventoryCapabilities Caps`: Capabilities of the inventory.
      - `int SlotCount`: Gets the number of slots in the inventory.
    - Public methods:
      - `static InventoryInstance Create(string instanceId, InventoryDef def, string ownerId = null)`: Creates a new instance of `InventoryInstance`.

  - [Flags] enum `InventoryCapabilities`
    - None = 0
    - ReadOnly = 1 << 0
    - Hidden = 1 << 1
    - Locked = 1 << 2

# Key Behavior & Side Effects
- The `Create` method initializes a new `InventoryInstance` with a specified ID, definition, and optional owner ID.
- It populates the `slots` array based on the `cols` and `rows` properties of the `InventoryDef`.
- Each slot is initialized with a new `Slot` instance, using the `defaultMaxStackPerSlot` and an optional filter.

# Constraints & Failure Modes
- The `slots` array is initialized based on the `cols` and `rows` of the `InventoryDef`, which must be valid.
- If `def.globalSlotFilter` is null, the slot filter is set to null.

# Example
```csharp
var inventoryDef = new InventoryDef { cols = 5, rows = 4, defaultMaxStackPerSlot = 10 };
var inventory = InventoryInstance.Create("inventory1", inventoryDef, "owner1");
```

# Unknowns
- The structure and properties of `InventoryDef` and `Slot` are not defined in this file.
