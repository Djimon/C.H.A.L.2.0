# CHAL.Data.ModuleData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

```markdown
Oops: The requested code fence tag was not used as this is documentation, not code.
```

Purpose
- Defines ItemDef as a ScriptableObject for item metadata and per-type payload blocks (RemainData, RuneData, PartData, ModuleData, GearData).
- Includes validation and type-safety enforcement in OnValidate; provides Rune color helpers.

Public API
- Namespace: CHAL.Data
- Types
  - public class ItemDef : ScriptableObject
    - public string itemId; [Tooltip] "Schema: category:item, z.B. remains:gland"
    - public ItemType itemType; [HideInInspector]
    - public string description; [TextArea]
    - public Sprite icon
    - public Rarity rarity = Rarity.Common
    - public int lootValue = 10; [Tooltip] "Wert fr Softcap/Budget ..."
    - public RemainData remainData
    - public RuneData runeData
    - public PartData partData
    - public ModuleData moduleData
    - public GearData gearData
    - void OnValidate()
    - private void ClearTypeBlocksExcept(ItemType keep)
  - public class RemainData
    - public string remainType
  - public static class RuneColors
    - public static readonly Color runeColorSun
    - public static readonly Color runeColorVerdant
    - public static readonly Color runeColorSky
    - public static readonly Color runeColorIgnis
    - public static readonly Color runeColorVoid
    - public static Color Get(RuneColorType type) => switch (type) { ... }
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

Key behaviors, flows, and state changes
- OnValidate
  - itemType = ItemTypeUtils.FromId(itemId)
  - If !ItemKey.TryParse(itemId, out _), logs warning:
    [ItemDef] Ungltige itemId '{itemId}' in {name}. Erwartet 'category:item'.
  - If lootValue < 0, set lootValue = 0
  - Calls ClearTypeBlocksExcept(itemType) to enforce type-safety
- ClearTypeBlocksExcept(keep)
  - remainData = null unless keep == ItemType.Remains
  - runeData = null unless keep == ItemType.Rune
  - partData = null unless keep == ItemType.Part
  - moduleData = null unless keep == ItemType.Module
  - gearData = null unless keep == ItemType.Gear

Constraints & Failure Modes
- itemId validation: warnings emitted for invalid IDs; no exception handling shown.
- Loot value sanitized to non-negative.
- Per-type data blocks are cleared to enforce a single active type payload; others become null when the type changes.

Unknowns
- Definitions and behavior of ItemType, ItemTypeUtils, ItemKey, Rarity, GearType, RuneColorType, and related enums/classes are not defined in this file.
- How ItemDef is consumed at runtime (beyond OnValidate behavior) is not shown.
- Any serialization or runtime validation beyond OnValidate is not present in this file.
