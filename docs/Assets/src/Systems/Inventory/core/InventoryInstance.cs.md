# Assets/src/Systems/Inventory/core/InventoryInstance.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `InventoryInstance` class for managing inventory instances.
- Provides an enumeration `InventoryCapabilities` for inventory capability flags.

## Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public class InventoryInstance`
    - Public fields/properties:
      - `string instanceID` - Unique identifier for the inventory instance.
      - `InventoryDef InvDef` - Definition of the inventory.
      - `Slot[] slots` - Array of slots in the inventory.
      - `string ownerID` - Identifier for the owner of the inventory.
      - `InventoryCapabilities Caps` - Capabilities of the inventory.
      - `int SlotCount` - Gets the number of slots in the inventory.
    - Public methods:
      - `static InventoryInstance Create(string instanceId, InventoryDef def, string ownerId = null)` - Creates a new `InventoryInstance` with specified parameters.

  - `public enum InventoryCapabilities`
    - Flags:
      - `None` - No capabilities.
      - `ReadOnly` - Inventory is read-only.
      - `Hidden` - Inventory is hidden.
      - `Locked` - Inventory is locked.

## Key Behavior & Side Effects
- `Create` method initializes an `InventoryInstance` with a specified ID, definition, and optional owner ID.
- Allocates slots based on the dimensions defined in `InventoryDef`.
- Each slot is initialized with a default maximum stack size and an optional filter.

## Constraints & Failure Modes
- `slots` is initialized based on `def.cols * def.rows`; ensure `def` is valid.
- `SlotCount` returns 0 if `slots` is null.
- No explicit error handling for invalid parameters in `Create`.

## Example
```csharp
var inventory = InventoryInstance.Create("inv1", inventoryDef, "owner123");
```

## Unknowns
- The structure and properties of `InventoryDef` and `Slot` are not defined in this file.
```
