# CHAL.Data.RemainData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

Purpose
- Defines ItemDef as a ScriptableObject asset and related serializable data containers for item configuration.
- Provides editor-time validation and automatic data-block management (derive type from ID, validate ID format, clamp lootValue, clear non-matching type data).
- Includes a RuneColors helper and per-type data blocks: RemainData, RuneData, PartData, ModuleData, GearData.

Public API
- Namespace: CHAL.Data
- public class ItemDef : ScriptableObject
  - public string itemId
  - public ItemType itemType
  - public string description
  - public Sprite icon
  - public Rarity rarity
  - public int lootValue
  - public RemainData remainData
  - public RuneData runeData
  - public PartData partData
  - public ModuleData moduleData
  - public GearData gearData
  - private void OnValidate()
  - private void ClearTypeBlocksExcept(ItemType keep)

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
    - public Color runecolor => RuneColors.Get(runeColortType)

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

Key Behavior & Side Effects
- OnValidate (Unity editor callback)
  - itemType = ItemTypeUtils.FromId(itemId)
  - If ItemKey.TryParse(itemId, out _) fails -> log warning about invalid itemId
  - lootValue clamped to 0 if negative
  - ClearTypeBlocksExcept(itemType) to keep only the relevant data block for the item type
- ClearTypeBlocksExcept(keep)
  - remainData = null unless keep == ItemType.Remains
  - runeData = null unless keep == ItemType.Rune
  - partData = null unless keep == ItemType.Part
  - moduleData = null unless keep == ItemType.Module
  - gearData = null unless keep == ItemType.Gear

- RuneColors.Get(type)
  - Maps RuneColorType to a specific Color constant

Constraints & Failure Modes
- OnValidate runs in the Unity editor (not necessarily in builds)
- Invalid itemId triggers a warning; does not throw
- lootValue is clamped to non-negative; negative values become 0
- Data-blocks are cleared selectively based on itemType, potentially leading to null references if other code assumes non-null data

Unknowns
- Definitions/locations of ItemType, ItemTypeUtils, ItemKey, Rarity, GearType, RuneColorType, and ItemKey.TryParse behavior
- How ItemDef assets are created/consumed at runtime beyond this file
- Any external localization or display behavior implied by itemId or description
