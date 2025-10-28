# CHAL.Data.RemainData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

# Purpose
- Defines the `ItemDef` ScriptableObject for item definitions in the game.
- Provides data structures for item properties, including rarity, description, and associated data types.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class ItemDef : ScriptableObject**
    - Public fields/properties:
      - `string itemId`: Unique identifier for the item.
      - `ItemType itemType`: Type of the item (hidden in inspector).
      - `string description`: Description of the item.
      - `Sprite icon`: Icon representing the item.
      - `Rarity rarity`: Rarity level of the item (default is `Common`).
      - `int lootValue`: Value for loot calculations (default is 10).
      - `RemainData remainData`: Specific data for remains.
      - `RuneData runeData`: Specific data for runes.
      - `PartData partData`: Specific data for parts.
      - `ModuleData moduleData`: Specific data for modules.
    - Public methods:
      - `void OnValidate()`: Validates item properties and ensures type safety based on `itemId`.

  - **public class RemainData**
    - Public fields/properties:
      - `string remainType`: Type of remain (e.g., Insect, Beast).

  - **public static class RuneColors**
    - Public methods:
      - `static Color Get(RuneColorType type)`: Returns the color associated with the specified rune color type.

  - **public class RuneData**
    - Public fields/properties:
      - `string effectType`: Type of effect (e.g., "Armor+", "Lifesteal").
      - `RuneColorType runeColortType`: Type of rune color.
      - `Color runecolor`: Gets the color based on `runeColortType`.

  - **public class PartData**
    - Public fields/properties:
      - `string dnaType`: Type of DNA (e.g., "Weapon", "Armor").
      - `List<ItemDef> moduleFuel`: List of item definitions used as fuel for modules.

  - **public class ModuleData**
    - Public fields/properties:
      - `string effect`: Effect of the module.
      - `float modulePower`: Power of the module.

# Key Behavior & Side Effects
- `OnValidate()` method:
  - Sets `itemType` based on `itemId`.
  - Validates `itemId` format and logs a warning if invalid.
  - Ensures `lootValue` is non-negative.
  - Clears incompatible data fields based on the prefix of `itemId`.

# Constraints & Failure Modes
- `itemId` must follow the format `category:item` to be valid.
- `lootValue` cannot be negative; it defaults to 0 if set below.
- Type safety is enforced by nullifying incompatible data fields based on `itemId` prefix.

# Example
```csharp
ItemDef newItem = ScriptableObject.CreateInstance<ItemDef>();
newItem.itemId = "remains:gland";
newItem.description = "A gland from a creature.";
newItem.rarity = Rarity.Rare;
newItem.lootValue = 30;
```

# Unknowns
- The definitions and behaviors of `ItemType`, `Rarity`, and `RuneColorType` cannot be determined from this file.

