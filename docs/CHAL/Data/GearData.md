# CHAL.Data.GearData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

1) Purpose
- Define a Unity ScriptableObject item definition (ItemDef) with item metadata and per-type data blocks.
- Provide serializable data containers for item subtypes: RemainData, RuneData, PartData, ModuleData, GearData.
- Provide a small utility (RuneColors) to map RuneColorType to UnityEngine.Color.

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public class ItemDef : ScriptableObject
    - public string itemId
      - Tooltip: "Schema: category:item, z.B. remains:gland"
    - [HideInInspector] public ItemType itemType
      - Set by OnValidateFrom itemId
    - [TextArea] public string description
    - public Sprite icon
    - public Rarity rarity
      - Default: Rarity.Common
    - public int lootValue
      - Tooltip: "Wert fr Softcap/Budget (empf.: Common 10, Rare 30, Epic 50, Legendary 80)"
      - Default: 10
    - [Tooltip] public RemainData remainData
    - public RuneData runeData
    - public PartData partData
    - public ModuleData moduleData
    - public GearData gearData
  - public class RemainData
    - public string remainType
  - public static class RuneColors
    - public static readonly Color runeColorSun
    - public static readonly Color runeColorVerdant
    - public static readonly Color runeColorSky
    - public static readonly Color runeColorIgnis
    - public static readonly Color runeColorVoid
    - public static Color Get(RuneColorType type) => returns corresponding color via switch
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

3) Key Behavior & Side Effects
- OnValidate (editor-time):
  - itemType = ItemTypeUtils.FromId(itemId)
  - If ItemKey.TryParse(itemId, out _) fails, logs a warning about invalid itemId format
  - lootValue is clamped to non-negative (>= 0)
  - ClearTypeBlocksExcept(itemType) is called to enforce type-safety
- ClearTypeBlocksExcept(keep):
  - remainData = null if keep != ItemType.Remains
  - runeData = null if keep != ItemType.Rune
  - partData = null if keep != ItemType.Part
  - moduleData = null if keep != ItemType.Module
  - gearData = null if keep != ItemType.Gear

4) Constraints & Failure Modes
- itemType is public but hidden in the inspector; it is derived from itemId during validation.
- Data blocks (remainData, runeData, partData, moduleData, gearData) can be nulled depending on itemType to enforce type-safety.
- lootValue has a guard to prevent negative values.
- OnValidate runs in the editor; runtime behavior not defined in this file.
- Dependencies on external types (ItemType, ItemTypeUtils, ItemKey, RuneColorType, GearType, Rarity) are not defined within this file.

5) Example
- Not derivable from this file alone.

6) Unknowns
- Definitions and current values of:
  - ItemType, ItemTypeUtils, ItemKey
  - RuneColorType, GearType
  - Rarity
- How ItemDef is instantiated and consumed elsewhere in the project beyond this file.
- Any runtime impact of OnValidate beyond editor-time validation.
