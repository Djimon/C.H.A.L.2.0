# CHAL.Systems.Inventory.InventoryCapabilities

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryInstance.cs`._

# Purpose
- Defines the `InventoryInstance` class representing an instance of an inventory, including slots and associated data.
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
      - `int SlotCount`: Returns the number of slots in the inventory.
    - Public methods:
      - `static InventoryInstance Create(string instanceId, InventoryDef def, string ownerId = null)`: Creates a new instance of `InventoryInstance`.

  - [Flags] enum `InventoryCapabilities`
    - Values:
      - `None`: No capabilities.
      - `ReadOnly`: Inventory is read-only.
      - `Hidden`: Inventory is hidden.
      - `Locked`: Inventory is locked.

# Key Behavior & Side Effects
- The `Create` method initializes a new `InventoryInstance` with a specified ID, definition, and optional owner ID.
- It populates the `slots` array based on the `InventoryDef` dimensions and initializes each slot with a filter.

# Constraints & Failure Modes
- The `slots` array is initialized based on `def.cols` and `def.rows`; if `def` is invalid, it may lead to unexpected behavior.
- The `ownerId` parameter in `Create` is optional and defaults to `null`.

# Example
```csharp
var inventoryDef = new InventoryDef { cols = 5, rows = 4, defaultMaxStackPerSlot = 10 };
var inventoryInstance = InventoryInstance.Create("inventory1", inventoryDef, "owner123");
```

# Unknowns
- The structure and properties of `InventoryDef` and `Slot` are not defined in this file.

