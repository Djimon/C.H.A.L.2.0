# CHAL.Data.PartData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

```text
1) Purpose
- Defines ItemDef as a Unity ScriptableObject (data asset) for item definitions, with per-item data blocks.
- Provides data containers: RemainData, RuneData, PartData, ModuleData, GearData, plus a helper RuneColors for color mapping.
- Exposes item metadata (id, description, icon, rarity, loot value) and per-type data blocks; enforces type-safety via OnValidate.

2) Public API
- Namespace: CHAL.Data

- Types
  - public class ItemDef : ScriptableObject
    - Public fields
      - string itemId
      - ItemType itemType (HideInInspector)
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
- ItemDef.OnValidate()
  - itemType = ItemTypeUtils.FromId(itemId)
  - If ItemKey.TryParse(itemId, out _) fails, logs a warning about invalid itemId
  - Clamps lootValue to non-negative (lootValue < 0 => lootValue = 0)
  - Calls ClearTypeBlocksExcept(itemType) to enforce type-safety by nulling non-matching data blocks

- ClearTypeBlocksExcept(ItemType keep)
  - remainData = null if keep != ItemType.Remains
  - runeData = null if keep != ItemType.Rune
  - partData = null if keep != ItemType.Part
  - moduleData = null if keep != ItemType.Module
  - gearData = null if keep != ItemType.Gear

4) Constraints & Failure Modes
- itemId must parse via ItemKey.TryParse; invalid IDs trigger a warning but do not throw
- lootValue cannot be negative; negative values are reset to 0
- Data blocks (remainData, runeData, partData, moduleData, gearData) are automatically cleared unless matching the current itemType
- OnValidate runs in the editor; runtime behavior depends on how/when assets are loaded
- References to external types (ItemType, ItemKey, ItemTypeUtils, Rarity, GearType, RuneColorType) are not defined in this file; their definitions are outside scope

5) Example
- Not derivable from this file alone; no concrete usage example provided

6) Unknowns
- Definitions and behavior of ItemType, ItemKey, ItemTypeUtils, Rarity, GearType, RuneColorType
- How ItemDef assets are created/used at runtime beyond OnValidate behavior
- Any serialization specifics beyond standard Unity ScriptableObject serialization
```
