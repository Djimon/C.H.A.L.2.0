# Assets/src/Systems/Heroes/HeroLoadoutService.cs

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroLoadoutService.cs`._

# Purpose
- Defines the `HeroLoadoutService` for managing hero gear and module socketing in an inventory system.

# Public API
- Namespace: `CHAL.Systems.Hero`
- Types
  - `public static class HeroLoadoutService`
    - Public methods:
      - `public static bool TryEquipGear(InventoryDomain inventory, string heroId, string fromInstanceId, int fromSlotIndex, int heroGearSlotIndex, out string failReason)`
        - Attempts to equip gear from a specified inventory slot to a hero's gear slot.
      - `public static bool TryUnequipGear(InventoryDomain inventory, GameManager gm, string heroId, int heroGearSlotIndex, out string failReason)`
        - Attempts to unequip gear from a hero's gear slot to the player's gear inventory.
      - `public static bool TrySocketModule(InventoryDomain inventory, GameManager gm, string heroId, string fromInstanceId, int fromSlotIndex, int heroSocketSlotIndex, out string failReason)`
        - Attempts to socket a module from a specified inventory slot to a hero's socket slot.
      - `public static bool TryUnsocketModule(InventoryDomain inventory, GameManager gm, string heroId, int heroSocketSlotIndex, out string failReason)`
        - Attempts to unsocket a module from a hero's socket slot to the player's module inventory.

# Key Behavior & Side Effects
- Each method checks for valid inputs and inventory states, returning a failure reason if any checks fail.
- `TryEquipGear` and `TrySocketModule` handle replacing existing gear/modules by moving them to the player's inventory if necessary.
- All methods ensure that the appropriate inventory instances exist before performing operations.

# Constraints & Failure Modes
- Methods return `false` and set `failReason` if:
  - Inventory or GameManager is null.
  - Hero ID or instance IDs are empty or invalid.
  - Source slots are empty or do not contain the expected item type.
  - Target slots are out of range or cannot accept the item.
- Performance considerations are not explicitly mentioned.

# Example
```csharp
var success = HeroLoadoutService.TryEquipGear(inventory, "hero123", "gearInstance", 0, 1, out var reason);
if (!success) {
    Debug.Log($"Failed to equip gear: {reason}");
}
```

# Unknowns
- None.

