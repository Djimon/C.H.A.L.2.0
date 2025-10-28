# Assets/src/Systems/Inventory/core/InventoryDef.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines an inventory configuration as a ScriptableObject.
- Provides fields for inventory type, name, dimensions, and stacking rules.

## Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public class InventoryDef : ScriptableObject`
    - Public fields/properties:
      - `PlayerInventoryType TypeId` - Identifier for the inventory type.
      - `string NameKey` - Key for the inventory name.
      - `int cols` - Number of columns in the inventory (minimum 1).
      - `int rows` - Number of rows in the inventory (minimum 1).
      - `int defaultMaxStackPerSlot` - Maximum stack size per slot (default 250).
      - `SlotFilter globalSlotFilter` - Filter applied to slots globally.

## Key Behavior & Side Effects
- None explicitly defined beyond field definitions.

## Constraints & Failure Modes
- `cols` and `rows` must be greater than or equal to 1 due to `[Min(1)]` attribute.

## Example
```csharp
var inventoryDef = ScriptableObject.CreateInstance<InventoryDef>();
inventoryDef.TypeId = PlayerInventoryType.SomeType;
inventoryDef.NameKey = "InventoryName";
inventoryDef.cols = 4;
inventoryDef.rows = 5;
inventoryDef.defaultMaxStackPerSlot = 100;
```

## Unknowns
- Specifics of `PlayerInventoryType` and `SlotFilter` are not defined in this file.
```
