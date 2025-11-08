# CHAL.Data.ItemDef

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

# Purpose
- Defines item definitions in the game, including properties and attributes.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class ItemDef : ScriptableObject**
    - Public fields/properties:
      - `string itemId`: Unique identifier for the item.
      - `ItemType itemType`: Type of the item (hidden in inspector).
      - `string description`: Description of the item.
      - `Sprite icon`: Icon representing the item.
      - `Rarity rarity`: Rarity level of the item (default is Common).
      - `int lootValue`: Value for soft cap/budget (default is 10).
      - `RemainData remainData`: Data specific to remains.
      - `RuneData runeData`: Data specific to runes.
      - `PartData partData`: Data specific to parts.
      - `ModuleData moduleData`: Data specific to modules.
      - `GearData gearData`: Data specific to gear.
    - Public methods:
      - `void OnValidate()`: Validates item properties and ensures type safety.
  - **[System.Serializable] public class RemainData**
    - Public fields/properties:
      - `string remainType`: Type of remain (e.g., Insect, Beast).
  - **[System.Serializable] public class RuneData**
    - Public fields/properties:
      - `string effectType`: Type of effect (e.g., "Armor+", "Lifesteal").
      - `RuneColorType runeColortType`: Type of rune color.
      - `Color runecolor`: Gets the color associated with the rune color type.
  - **[System.Serializable] public class PartData**
    - Public fields/properties:
      - `string dnaType`: Type of DNA (e.g., "Weapon", "Armor").
      - `List<ItemDef> moduleFuel`: List of item definitions used as fuel for modules.
  - **[System.Serializable] public class ModuleData**
    - Public fields/properties:
      - `string effect`: Effect of the module.
      - `float modulePower`: Power of the module.
  - **[System.Serializable] public class GearData**
    - Public fields/properties:
      - `GearType slotType`: Type of gear slot (e.g., Head, Chest).
      - `string[] tags`: Tags associated with the gear.
      - `RuneColorType runeSocketType`: Type of rune socket (optional).

# Key Behavior & Side Effects
- `OnValidate()` method ensures:
  - `itemType` is set based on `itemId`.
  - Validates `itemId` format and logs a warning if invalid.
  - Ensures `lootValue` is non-negative.
  - Clears type-specific data based on `itemType`.

# Constraints & Failure Modes
- `itemId` must follow the format 'category:item'.
- `lootValue` must be set to a non-negative integer.
- Type-specific data fields are cleared based on the `itemType`.

# Example
```csharp
ItemDef itemDef = ScriptableObject.CreateInstance<ItemDef>();
itemDef.itemId = "remains:gland";
itemDef.description = "A gland from a creature.";
itemDef.icon = someSpriteReference;
itemDef.rarity = Rarity.Rare;
itemDef.lootValue = 30;
```

# Unknowns
- None.

