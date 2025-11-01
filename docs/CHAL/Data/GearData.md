# CHAL.Data.GearData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

1) Purpose
- Defines CHAL.Data.ItemDef as a Unity ScriptableObject for item definitions (CreateAssetMenued asset).
- Groups type-specific data into RemainData, RuneData, PartData, ModuleData, GearData; exposes item metadata (ID, description, icon, rarity, loot value).
- Provides supporting types and color helpers (RemainData, RuneColors, RuneData, PartData, ModuleData, GearData).

2) Public API
- Namespace/module: CHAL.Data

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
  - public Color runecolor

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
- OnValidate (Unity editor hook):
  - itemType = ItemTypeUtils.FromId(itemId)
  - If ItemKey.TryParse(itemId, out _) fails, logs warning about invalid itemId with expected format "category:item"
  - lootValue clamped to >= 0
  - ClearTypeBlocksExcept(itemType) invoked to enforce type-safety
- ClearTypeBlocksExcept(ItemType keep):
  - remainData = null if keep != ItemType.Remains
  - runeData = null if keep != ItemType.Rune
  - partData = null if keep != ItemType.Part
  - moduleData = null if keep != ItemType.Module
  - gearData = null if keep != ItemType.Gear

- RuneColors.Get maps RuneColorType to specific Colors via a switch.

4) Constraints & Failure Modes
- itemId must follow the schema "category:item"; violations trigger a warning but not an exception.
- lootValue cannot be negative (clamped to 0).
- Data blocks (remainData/runeData/partData/moduleData/gearData) are automatically nulled unless their type matches the itemType, enforcing a basic form of type-safety.
- Public surface relies on external types: ItemType, ItemKey, ItemTypeUtils, Rarity, RuneColorType, GearType, etc., whose definitions are not in this file.
- OnValidate runs in the editor context; runtime behavior depends on how item definitions are consumed elsewhere.

5) Example
- Unknown from this file (asset creation and usage are editor/runtime external to this snippet).

6) Unknowns
- Exact definitions and semantics of ItemType, ItemKey, ItemTypeUtils, GearType, RuneColorType, and Rarity beyond their usage here.
- How remainData/runeData/partData/moduleData/gearData are consumed at runtime.
- Any additional validation rules outside what OnValidate enforces.
