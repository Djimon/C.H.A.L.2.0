# CHAL.Data.PartData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

```text
1) Purpose
- Defines a Unity ScriptableObject ItemDef to describe game items and their type-specific data blocks.
- Provides serializable data containers for remains, runes, parts, modules, and gear configuration.
- Includes a helper for mapping RuneColorType to Unity Color values (RuneColors).

2) Public API
- Namespace/module
  - CHAL.Data

- Types
  - public class ItemDef : ScriptableObject
    - public string itemId
    - [HideInInspector] public ItemType itemType
    - public string description
    - public Sprite icon
    - public Rarity rarity
    - public int lootValue
    - public RemainData remainData
    - public RuneData runeData
    - public PartData partData
    - public ModuleData moduleData
    - public GearData gearData
    - public void OnValidate()
    - private void ClearTypeBlocksExcept(ItemType keep)

  - public static class RuneColors
    - public static readonly Color runeColorSun
    - public static readonly Color runeColorVerdant
    - public static readonly Color runeColorSky
    - public static readonly Color runeColorIgnis
    - public static readonly Color runeColorVoid
    - public static Color Get(RuneColorType type)

  - [System.Serializable] public class RemainData
    - public string remainType

  - [System.Serializable] public class RuneData
    - public string effectType
    - public RuneColorType runeColortType
    - public Color runecolor { get; }

  - [System.Serializable] public class PartData
    - public string dnaType
    - public List<ItemDef> moduleFuel

  - [System.Serializable] public class ModuleData
    - public string effect
    - public float modulePower

  - [System.Serializable] public class GearData
    - public GearType slotType
    - public string[] tags
    - public RuneColorType runeSocketType

3) Key Behavior & Side Effects
- OnValidate()
  - itemType = ItemTypeUtils.FromId(itemId)
  - Validates itemId via ItemKey.TryParse; logs a German warning if invalid
  - Clamps lootValue to non-negative
  - Enforces type-safety by calling ClearTypeBlocksExcept(itemType)
- ClearTypeBlocksExcept(keep)
  - If keep != ItemType.Remains -> remainData = null
  - If keep != ItemType.Rune -> runeData = null
  - If keep != ItemType.Part -> partData = null
  - If keep != ItemType.Module -> moduleData = null
  - If keep != ItemType.Gear -> gearData = null
- RuneColors.Get(type) returns mapped Color for known RuneColorType values, otherwise Color.white
- runecolor property in RuneData derives color via RuneColors.Get(runeColortType)

4) Constraints & Failure Modes
- OnValidate relies on external types: ItemType, ItemTypeUtils, ItemKey, RuneColorType, GearType, Rarity, etc. (definitions not in this file).
- itemId is parsed to determine itemType; invalid formats log a warning but do not throw.
- lootValue is clamped to >= 0; negative input becomes 0.
- Data blocks (remainData, runeData, partData, moduleData, gearData) are mutually exclusive by design via ClearTypeBlocksExcept.
- itemType is [HideInInspector], so it’s not editable directly in the inspector.
- Serialization depends on Unity’s System.Serializable attributes and Unity types (Sprite, Color, List<T>).

5) Example
- Not explicitly derivable from the file (no usage snippet provided). Omitted.

6) Unknowns
- Definitions and exact behavior of ItemType, ItemKey, ItemTypeUtils, RuneColorType, GearType, Rarity, and how ItemDef is consumed elsewhere.
- Any runtime implications beyond OnValidate-time validation (e.g., how item data blocks are used by gameplay systems).
- Expected itemId formats beyond the documented schema (category:item) and potential extension rules.
- Whether additional editor tooling relies on these types or the specific colors in RuneColors.
```
