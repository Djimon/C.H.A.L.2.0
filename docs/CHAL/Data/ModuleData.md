# CHAL.Data.ModuleData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

```csharp
// Unity ScriptableObject-based item definition data and related data containers
```

1) Purpose
- Defines a Unity ScriptableObject (ItemDef) that represents item metadata and type-specific data blocks.
- Provides data containers for remains, runes, parts, modules, and gear, plus a helper for rune colors.
- Enforces simple in-editor validation and type-safety by clearing non-relevant data blocks.

2) Public API
- Namespace: CHAL.Data

- Types

  - public class ItemDef : ScriptableObject
    - Public fields
      - public string itemId
      - [HideInInspector] public ItemType itemType
      - [TextArea] public string description
      - public Sprite icon
      - public Rarity rarity
      - public int lootValue
      - public RemainData remainData
      - public RuneData runeData
      - public PartData partData
      - public ModuleData moduleData
      - public GearData gearData
    - Public methods
      - (None; OnValidate is private)

  - public class RemainData
    - public string remainType

  - public static class RuneColors
    - public static readonly Color runeColorSun
    - public static readonly Color runeColorVerdant
    - public static readonly Color runeColorSky
    - public static readonly Color runeColorIgnis
    - public static readonly Color runeColorVoid
    - public static Color Get(RuneColorType type) => Color (switch on type)

  - public class RuneData
    - public string effectType
    - public RuneColorType runeColortType
    - public Color runecolor { get; }

  - public class PartData
    - public string dnaType
    - public List<ItemDef> moduleFuel

  - public class ModuleData
    - public string effect
    - public float modulePower

  - public class GearData
    - public GearType slotType
    - public string[] tags
    - public RuneColorType runeSocketType

Notes:
- itemType is public but hidden in inspector.
- The file references external types: ItemType, ItemKey, ItemTypeUtils, Rarity, RuneColorType, GearType, etc. These are not defined in this file.

3) Key Behavior & Side Effects
- OnValidate (Unity editor callback)
  - itemType = ItemTypeUtils.FromId(itemId)
  - Validates itemId format via ItemKey.TryParse(itemId, out _); logs a warning if invalid
  - Clamps lootValue to a minimum of 0
  - Calls ClearTypeBlocksExcept(itemType) to enforce type-safety
- ClearTypeBlocksExcept(ItemType keep)
  - Clears non-matching type blocks:
    - remainData = null if keep != ItemType.Remains
    - runeData = null if keep != ItemType.Rune
    - partData = null if keep != ItemType.Part
    - moduleData = null if keep != ItemType.Module
    - gearData = null if keep != ItemType.Gear

4) Constraints & Failure Modes
- itemId must follow the expected schema (category:item); non-conforming IDs trigger a warning in OnValidate.
- lootValue is clamped to non-negative; negative values are reset to 0.
- Data blocks are automatically nulled except for the block matching the resolved itemType, via OnValidate -> ClearTypeBlocksExcept.
- OnValidate is editor-only behavior; runtime behavior relies on other runtime code not shown here.
- Surface relies on external types (ItemType, ItemKey, ItemTypeUtils, Rarity, RuneColorType, GearType) that are not defined in this file.

5) Example
- Not derivable from this file alone; no runnable usage snippet is provided here.

6) Unknowns
- Exact definitions and enum values of:
  - ItemType, ItemKey, ItemTypeUtils
  - Rarity
  - RuneColorType
  - GearType
- How these data structures are consumed at runtime beyond what OnValidate enforces.
- Any runtime validation beyond editor-time OnValidate.

```csharp
// End of documentation
```
