# CHAL.Data.PartData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

# Purpose
- Defines the `ItemDef` ScriptableObject for item definitions in the game.
- Provides data structures for item properties, including rarity, description, and associated data types.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class ItemDef : ScriptableObject**
    - Public fields/properties:
      - `string itemId` - Unique identifier for the item.
      - `ItemType itemType` - Type of the item (hidden in inspector).
      - `string description` - Description of the item.
      - `Sprite icon` - Icon representing the item.
      - `Rarity rarity` - Rarity level of the item.
      - `int lootValue` - Value for loot calculations.
      - `RemainData remainData` - Data specific to remains.
      - `RuneData runeData` - Data specific to runes.
      - `PartData partData` - Data specific to parts.
      - `ModuleData moduleData` - Data specific to modules.
    - Public methods:
      - `void OnValidate()` - Validates item properties and ensures type safety.

  - **[System.Serializable] public class RemainData**
    - Public fields/properties:
      - `string remainType` - Type of remain (e.g., Insect, Beast).

  - **[System.Serializable] public class RuneData**
    - Public fields/properties:
      - `string effectType` - Type of effect (e.g., "Armor+", "Lifesteal").
      - `RuneColorType runeColortType` - Color type of the rune.
      - `Color runecolor` - Gets the color based on `runeColortType`.

  - **[System.Serializable] public class PartData**
    - Public fields/properties:
      - `string dnaType` - Type of DNA (e.g., "Weapon", "Armor").
      - `List<ItemDef> moduleFuel` - List of item definitions used as fuel for modules.

  - **[System.Serializable] public class ModuleData**
    - Public fields/properties:
      - `string effect` - Effect of the module.
      - `float modulePower` - Power of the module.

  - **public static class RuneColors**
    - Public methods:
      - `static Color Get(RuneColorType type)` - Returns the color associated with the specified rune color type.

# Key Behavior & Side Effects
- `OnValidate()` method is called to validate the `itemId` and adjust properties based on its prefix.
- Logs a warning if `itemId` is invalid.
- Ensures `lootValue` is non-negative.
- Clears specific data fields based on the prefix of `itemId` to enforce type safety.

# Constraints & Failure Modes
- `itemId` must follow the format 'category:item' to be valid.
- `lootValue` must be set to 0 or greater.
- Specific data fields are set to null based on the `itemId` prefix, enforcing type constraints.

# Example
```csharp
ItemDef item = ScriptableObject.CreateInstance<ItemDef>();
item.itemId = "remains:gland";
item.description = "A gland from a creature.";
item.rarity = Rarity.Common;
item.lootValue = 10;
```

# Unknowns
- The definitions and behaviors of `ItemType`, `Rarity`, and `RuneColorType` cannot be determined from this file.

