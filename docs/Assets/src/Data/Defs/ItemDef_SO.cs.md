# Assets/src/Data/Defs/ItemDef_SO.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `ItemDef` ScriptableObject for item definitions in the game.
- Provides data structures for item attributes, including rarity, loot value, and specific item data.

## Public API
- **Namespace**: `CHAL.Data`
- **Types**:
  - `public class ItemDef : ScriptableObject`
    - Public fields/properties:
      - `public string itemId` - Unique identifier for the item.
      - `public ItemType itemType` - Type of the item (hidden in inspector).
      - `public string description` - Description of the item.
      - `public Sprite icon` - Icon representing the item.
      - `public Rarity rarity` - Rarity level of the item.
      - `public int lootValue` - Value for loot calculations.
      - `public RemainData remainData` - Data specific to remains.
      - `public RuneData runeData` - Data specific to runes.
      - `public PartData partData` - Data specific to parts.
      - `public ModuleData moduleData` - Data specific to modules.
    - Public methods:
      - `void OnValidate()` - Validates item properties and ensures type safety.

  - `public class RemainData`
    - Public fields/properties:
      - `public string remainType` - Type of remain (e.g., Insect, Beast).

  - `public class RuneData`
    - Public fields/properties:
      - `public string effectType` - Type of effect (e.g., "Armor+", "Lifesteal").
      - `public RuneColorType runeColortType` - Type of rune color.
      - `public Color runecolor` - Gets the color based on `runeColortType`.

  - `public class PartData`
    - Public fields/properties:
      - `public string dnaType` - Type of DNA (e.g., "Weapon", "Armor").
      - `public List<ItemDef> moduleFuel` - List of item definitions used as fuel.

  - `public class ModuleData`
    - Public fields/properties:
      - `public string effect` - Effect of the module.
      - `public float modulePower` - Power of the module.

  - `public static class RuneColors`
    - Public methods:
      - `public static Color Get(RuneColorType type)` - Returns the color associated with the specified rune color type.

## Key Behavior & Side Effects
- `OnValidate()` method checks and sets the `itemType` based on `itemId`.
- Validates `itemId` format and logs a warning if invalid.
- Ensures `lootValue` is non-negative.
- Clears incompatible data fields based on the prefix of `itemId` (e.g., if `itemId` starts with "remains:", clears `runeData`, `partData`, and `moduleData`).

## Constraints & Failure Modes
- `itemId` must follow the format 'category:item' to be valid.
- `lootValue` must be set to a non-negative integer.
- Type safety is enforced by nullifying incompatible data fields based on `itemId` prefixes.

## Example
```csharp
ItemDef newItem = ScriptableObject.CreateInstance<ItemDef>();
newItem.itemId = "remains:gland";
newItem.description = "A gland from a creature.";
newItem.rarity = Rarity.Common;
newItem.lootValue = 10;
```

## Unknowns
- The definitions and implementations of `ItemType`, `Rarity`, and `RuneColorType` are not provided in this file.
```
