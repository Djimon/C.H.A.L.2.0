# CHAL.Systems.Inventory.InventoryInstance

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryInstance.cs`._

# Purpose
- Defines the `InventoryInstance` class representing an instance of an inventory, including slots and associated data.
- Provides the `InventoryCapabilities` enum to define various capabilities of the inventory.

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
      - `static InventoryInstance Create(string instanceId, InventoryDef def, string ownerId = null)`: Creates a new instance of `InventoryInstance` with specified parameters.

  - [Flags] enum `InventoryCapabilities`
    - None = 0
    - ReadOnly = 1 << 0
    - Hidden = 1 << 1
    - Locked = 1 << 2

# Key Behavior & Side Effects
- The `Create` method initializes a new `InventoryInstance`, setting its properties and creating an array of `Slot` objects based on the inventory definition's dimensions.

# Constraints & Failure Modes
- The `slots` array is initialized based on `def.cols * def.rows`, which assumes `def` is valid and properly defined.
- The `ownerId` parameter in the `Create` method is optional and can be null.

# Example
```csharp
var inventoryDef = new InventoryDef { cols = 5, rows = 4, defaultMaxStackPerSlot = 10 };
var inventoryInstance = InventoryInstance.Create("inventory1", inventoryDef, "owner123");
```

# Unknowns
- The structure and properties of `InventoryDef` and `Slot` are not defined in this file.
