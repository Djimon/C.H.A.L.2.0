# CHAL.Systems.Inventory.InventoryDef

_Automatically generated/updated from `Assets/src/Systems/Inventory/core/InventoryDef.cs`._

# Purpose
- Defines the `InventoryDef` class as a ScriptableObject for inventory configuration.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - public class `InventoryDef` [extends ScriptableObject]
    - Public fields/properties:
      - `PlayerInventoryType TypeId` - Identifier for the inventory type.
      - `string NameKey` - Key for the inventory name.
      - `int cols` - Number of columns in the inventory (minimum 1).
      - `int rows` - Number of rows in the inventory (minimum 1).
      - `int defaultMaxStackPerSlot` - Default maximum stack size per slot (default is 250).
      - `SlotFilter globalSlotFilter` - Filter applied to the inventory slots.

# Key Behavior & Side Effects
- None explicitly defined in the file.

# Constraints & Failure Modes
- `cols` and `rows` must be greater than or equal to 1 due to the `[Min(1)]` attribute.

# Example
```csharp
InventoryDef inventoryDef = ScriptableObject.CreateInstance<InventoryDef>();
inventoryDef.TypeId = PlayerInventoryType.SomeType;
inventoryDef.NameKey = "InventoryName";
inventoryDef.cols = 5;
inventoryDef.rows = 4;
inventoryDef.defaultMaxStackPerSlot = 100;
```

# Unknowns
- The definition and behavior of `PlayerInventoryType` and `SlotFilter` cannot be determined from this file.

