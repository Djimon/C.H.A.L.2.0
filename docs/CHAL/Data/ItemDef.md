# CHAL.Data.ItemDef

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

Purpose
- Defines ItemDef as a Unity ScriptableObject asset (with CreateAssetMenu).
- Provides data containers for item-type-specific data: RemainData, RuneData, PartData, ModuleData, GearData.
- Includes RuneColors helper (color mapping for rune types).

Public API

- Namespace/module
  - CHAL.Data

- Types

  - public class ItemDef : ScriptableObject
    - public string itemId
      - Tooltip: "Schema: category:item, z.B. remains:gland"
    - [HideInInspector] public ItemType itemType
    - [TextArea] public string description
    - public Sprite icon
    - public Rarity rarity = Rarity.Common
    - public int lootValue = 10
      - Tooltip: "Wert fr Softcap/Budget (empf.: Common 10, Rare 30, Epic 50, Legendary 80)"
    - public RemainData remainData
    - public RuneData runeData
    - public PartData partData
    - public ModuleData moduleData
    - public GearData gearData
    - void OnValidate()
      - Sets itemType = ItemTypeUtils.FromId(itemId)
      - If ItemKey.TryParse(itemId, out _) fails, logs a warning about invalid itemId
      - Clamps lootValue to >= 0
      - Calls ClearTypeBlocksExcept(itemType) to enforce type-safety
    - private void ClearTypeBlocksExcept(ItemType keep)
      - If keep != ItemType.Remains, sets remainData = null
      - If keep != ItemType.Rune, sets runeData = null
      - If keep != ItemType.Part, sets partData = null
      - If keep != ItemType.Module, sets moduleData = null
      - If keep != ItemType.Gear, sets gearData = null

  - public class RemainData
    - public string remainType
      - // e.g. "Insect", "Beast", etc.

  - public static class RuneColors
    - public static readonly Color runeColorSun
    - public static readonly Color runeColorVerdant
    - public static readonly Color runeColorSky
    - public static readonly Color runeColorIgnis
    - public static readonly Color runeColorVoid
    - public static Color Get(RuneColorType type) => switch (type) { ... }
      - Returns a color for: Sun, Verdant, Sky, Ignis, Void; default Color.white

  - public class RuneData
    - public string effectType
      - e.g. "Armor+", "Lifesteal"
    - public RuneColorType runeColortType
    - public Color runecolor => RuneColors.Get(runeColortType)

  - public class PartData
    - public string dnaType
      - e.g. "Weapon", "Armor"
    - public List<ItemDef> moduleFuel

  - public class ModuleData
    - public string effect
    - public float modulePower

  - public class GearData
    - public GearType slotType
      - e.g. Head/Chest/Gloves/Legs/Boots/Amulet
    - public string[] tags
      - e.g. "gear", "leather", "light"
    - public RuneColorType runeSocketType

Key Behavior & Side Effects
- OnValidate (editor-time):
  - Derives itemType from itemId via ItemTypeUtils.FromId
  - Validates itemId with ItemKey.TryParse; logs a warning if invalid
  - Ensures lootValue is non-negative
  - Enforces type-specific data blocks via ClearTypeBlocksExcept
- ClearTypeBlocksExcept(ItemType keep):
  - Keeps only the data block corresponding to the given item type
  - Sets all other type data blocks to null

Constraints & Failure Modes
- itemId is validated against an external parser (ItemKey.TryParse); invalid IDs trigger a warning but do not crash
- lootValue is clamped to 0 if negative
- Type-block data are mutually exclusive: only the data block for the current itemType is kept; others are cleared
- Public fields may be null if not defined in a given asset

Example
- Not derivable from the file beyond standard ScriptableObject usage; no explicit example included.

Unknowns
- Exact definitions/behaviors of ItemType, ItemTypeUtils.FromId, and ItemKey.TryParse (not defined in this file)
- Semantics of how remaining, rune, part, module, or gear data interact at runtime beyond OnValidate enforcement
- Any runtime logic that consumes these data blocks (not present in this file)

Notes
- Asset creation: ItemDef assets can be created via Unity editor under Data/ItemDef
- Editor-only: OnValidate runs in the editor; data sanitization/logging is editor-time
- Colors: RuneColors defines five colors mapped to RuneColorType values (Sun, Verdant, Sky, Ignis, Void)
