# CHAL.Data.ItemDef

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

1) Purpose
- Define ItemDef as a ScriptableObject that holds item metadata and optional type-specific data blocks.
- Provide editor-time validation and automatic type-safety enforcement via OnValidate.
- Expose a CreateAssetMenu entry for easily creating ItemDef assets.

2) Public API
- Namespace/module: CHAL.Data
- Types
  - public class ItemDef : ScriptableObject
    - Public fields/properties:
      - public string itemId; // "category:item" schema
      - public ItemType itemType; // [HideInInspector]
      - public string description; // TextArea
      - public Sprite icon;
      - public Rarity rarity; // default is Rarity.Common
      - public int lootValue; // default is 10
      - public RemainData remainData;
      - public RuneData runeData;
      - public PartData partData;
      - public ModuleData moduleData;
      - public GearData gearData;
    - Public methods:
      - void OnValidate()
      - private void ClearTypeBlocksExcept(ItemType keep)
  - public class RemainData
    - public string remainType; // Insect, Beast, etc.
  - public static class RuneColors
    - public static readonly Color runeColorSun
    - public static readonly Color runeColorVerdant
    - public static readonly Color runeColorSky
    - public static readonly Color runeColorIgnis
    - public static readonly Color runeColorVoid
    - public static Color Get(RuneColorType type)
  - public class RuneData
    - public string effectType; // e.g. "Armor+", "Lifesteal"
    - public RuneColorType runeColortType;
    - public Color runecolor => RuneColors.Get(runeColortType);
  - public class PartData
    - public string dnaType; // e.g. "Weapon", "Armor"
    - public List<ItemDef> moduleFuel;
  - public class ModuleData
    - public string effect;
    - public float modulePower;
  - public class GearData
    - public GearType slotType; // Head/Chest/Gloves/Legs/Boots/Amulet 
    - public string[] tags; // e.g. "gear","leather","light"
    - public RuneColorType runeSocketType; // optional/future

3) Key Behavior & Side Effects
- OnValidate:
  - itemType = ItemTypeUtils.FromId(itemId);
  - If itemId is invalid per ItemKey.TryParse, logs a warning.
  - Loot value clamped to minimum 0.
  - Enforces type-safety by clearing non-matching type blocks via ClearTypeBlocksExcept.
- ClearTypeBlocksExcept(keep):
  - If keep != ItemType.Remains, sets remainData = null.
  - If keep != ItemType.Rune, sets runeData = null.
  - If keep != ItemType.Part, sets partData = null.
  - If keep != ItemType.Module, sets moduleData = null.
  - If keep != ItemType.Gear, sets gearData = null.

4) Constraints & Failure Modes
- itemType is maintained automatically in editor; not serialized in inspector.
- Invalid itemId triggers a warning; asset creation not blocked.
- lootValue cannot be negative (clamped to 0).
- Type-specific data blocks are nulled if not matching the current itemType, potentially losing data when changing type.

5) Example
- Not deducible from this file; no inline usage example provided.

6) Unknowns
- Definitions and behavior of ItemType, ItemTypeUtils, ItemKey, RuneColorType, GearType, Rarity, and other dependent types.
- Exact runtime behavior beyond editor-time validation (e.g., how assets are consumed at runtime).
- Any additional serialization behavior or editor tooling beyond what is shown.

