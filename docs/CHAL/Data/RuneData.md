# CHAL.Data.RuneData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

1) Purpose
- Defines ItemDef as a Unity ScriptableObject asset for item metadata.
- Groups per-item data blocks (RemainData, RuneData, PartData, ModuleData, GearData) used to describe item-type specific data.
- Provides supporting data/config types in this file (RemainData, RuneColors, RuneData, PartData, ModuleData, GearData) and a helper color resolver.

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public class ItemDef : ScriptableObject
    - Public fields
      - string itemId
      - ItemType itemType
      - string description
      - Sprite icon
      - Rarity rarity
      - int lootValue
      - RemainData remainData
      - RuneData runeData
      - PartData partData
      - ModuleData moduleData
      - GearData gearData
    - (No public methods declared)
  - public class RemainData
    - public string remainType
  - public static class RuneColors
    - public static readonly Color runeColorSun
    - public static readonly Color runeColorVerdant
    - public static readonly Color runeColorSky
    - public static readonly Color runeColorIgnis
    - public static readonly Color runeColorVoid
    - public static Color Get(RuneColorType type)
  - public class RuneData
    - public string effectType
    - public RuneColorType runeColortType
    - public Color runecolor (read-only; computed: RuneColors.Get(runeColortType))
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

3) Key Behavior & Side Effects
- OnValidate()
  - itemType = ItemTypeUtils.FromId(itemId)
  - Validates itemId with ItemKey.TryParse; logs a warning if invalid: "[ItemDef] Ungltige itemId '{itemId}' in {name}. Erwartet 'category:item'."
  - Clamps lootValue to non-negative: if (lootValue < 0) lootValue = 0
  - Calls ClearTypeBlocksExcept(itemType) to enforce type-specific data blocks
- ClearTypeBlocksExcept(ItemType keep)
  - If keep != ItemType.Remains, remainData = null
  - If keep != ItemType.Rune, runeData = null
  - If keep != ItemType.Part, partData = null
  - If keep != ItemType.Module, moduleData = null
  - If keep != ItemType.Gear, gearData = null

4) Constraints & Failure Modes
- itemId must parse via ItemKey.TryParse; invalid IDs trigger a warning (no exception thrown)
- lootValue is capped at 0 minimum
- Per-type data blocks are mutually exclusive via ClearTypeBlocksExcept; non-matching blocks are cleared on validation
- Serialized fields rely on Unity inspector attributes:
  - itemId has Tooltip
  - itemType is HiddenInInspector
  - description uses TextArea
  - icon uses Sprite
  - lootValue and other fields are plain public
- No explicit threading/async behavior; all behavior is synchronous in the Unity editor/runtime as part of asset validation

5) Example
- Not explicitly derivable from this file (no runnable usage example provided)

6) Unknowns
- Definitions/semantics of ItemType, ItemTypeUtils, ItemKey, Rarity, GearType, RuneColorType, and how ItemDef is consumed elsewhere
- Exact behavior/validation details of ItemTypeUtils.FromId and ItemKey.TryParse beyond the warning shown
- How remainData/runeData/partData/moduleData/gearData are used at runtime beyond their storage
- Any runtime constraints or serialization details beyond Unity’s standard ScriptableObject behavior
