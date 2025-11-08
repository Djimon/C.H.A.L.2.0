# CHAL.Data.RemainData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

# Purpose
- Defines item definitions for the game, including properties and attributes.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `ItemDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string itemId`: Unique identifier for the item.
      - `ItemType itemType`: Type of the item, set on validation.
      - `string description`: Description of the item.
      - `Sprite icon`: Icon representing the item.
      - `Rarity rarity`: Rarity level of the item, default is `Common`.
      - `int lootValue`: Value for soft cap/budget, default is 10.
      - `RemainData remainData`: Data specific to remains type items.
      - `RuneData runeData`: Data specific to rune type items.
      - `PartData partData`: Data specific to part type items.
      - `ModuleData moduleData`: Data specific to module type items.
      - `GearData gearData`: Data specific to gear type items.
    - Public methods:
      - `void OnValidate()`: Validates item properties and ensures type safety.
  - public class `RemainData`
    - Public fields/properties:
      - `string remainType`: Type of remain (e.g., Insect, Beast).
  - public class `RuneData`
    - Public fields/properties:
      - `string effectType`: Type of effect (e.g., "Armor+", "Lifesteal").
      - `RuneColorType runeColortType`: Type of rune color.
      - `Color runecolor`: Gets the color associated with the rune color type.
  - public class `PartData`
    - Public fields/properties:
      - `string dnaType`: Type of DNA (e.g., "Weapon", "Armor").
      - `List<ItemDef> moduleFuel`: List of item definitions used as fuel for modules.
  - public class `ModuleData`
    - Public fields/properties:
      - `string effect`: Effect of the module.
      - `float modulePower`: Power of the module.
  - public class `GearData`
    - Public fields/properties:
      - `GearType slotType`: Type of gear slot (e.g., Head, Chest).
      - `string[] tags`: Tags associated with the gear.
      - `RuneColorType runeSocketType`: Type of rune socket.

  - public static class `RuneColors`
    - Public methods:
      - `static Color Get(RuneColorType type)`: Gets the color associated with the specified rune color type.

# Key Behavior & Side Effects
- `OnValidate()` method is called when the object is modified in the editor:
  - Sets `itemType` based on `itemId`.
  - Validates `itemId` format and logs a warning if invalid.
  - Ensures `lootValue` is non-negative.
  - Clears type-specific data based on the `itemType`.

# Constraints & Failure Modes
- `itemId` must follow the format 'category:item' to be valid.
- `lootValue` cannot be negative; it defaults to 0 if set below 0.
- Type-specific data fields are cleared based on the `itemType`.

# Example
```csharp
ItemDef item = ScriptableObject.CreateInstance<ItemDef>();
item.itemId = "remains:gland";
item.description = "A gland from a creature.";
item.icon = someSpriteReference;
item.rarity = Rarity.Rare;
item.lootValue = 30;
```

# Unknowns
- None.

