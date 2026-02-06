# Inventory

## Responsibilities
- Store live inventory instances and slots.
- Provide inventory routing and acceptance rules.
- Bridge between runtime and persistence snapshots.

## Key Types
- `InventoryDomain`
- `InventoryInstance`
- `InventoryDef`
- `ItemStackRef`

## Routing
- ItemId prefixes map to `PlayerInventoryType` and inventory instance ids.
- Routing maps are built by GameManager at startup.

## Persistence
- `MapDomainToProfile` produces `InventorySnapshot` lists.
- `MapProfileToDomain` restores instance payloads and slot contents.

## References
- `Systems/Inventory/InventoryDomain.cs` (API: [InventoryDomain](../../CHAL/Systems/Inventory/InventoryDomain.md))
- `Systems/Inventory/InventoryInstance.cs` (API: [InventoryInstance](../../CHAL/Systems/Inventory/InventoryInstance.md))
- `Systems/Inventory/ItemStackRef.cs` (API: [ItemStackRef](../../CHAL/Systems/Inventory/ItemStackRef.md))
- `Data/Enums/InventroyType.cs` (API: [InventroyType](../../CHAL/Data/InventroyType.md))
- `Core/GameManager.cs` (API: [GameManager](../../CHAL/Core/GameManager.md))

## Related
- [Data Pipeline](../DataPipeline.md)
- [Save and Load](../SaveLoad.md)
- [UI](UI.md)
