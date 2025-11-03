# CHAL.Data.RuneData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

1) Purpose
- Defines a ScriptableObject ItemDef that acts as a data container for item definitions, including per-type data blocks (RemainData, RuneData, PartData, ModuleData, GearData).
- Provides supporting data types for item configuration: RemainData, RuneColors, RuneData, PartData, ModuleData, GearData.
- Includes editor-time validation to derive type from ID, validate IDs, clamp loot values, and enforce type-safety by clearing non-matching type blocks.

2) Public API
- Namespace/module
  - namespace CHAL.Data
- Types
  - public class ItemDef : ScriptableObject
    - Public fields
      - public string itemId; // Schema: category:item
      - [HideInInspector] public ItemType itemType;
      - [TextArea] public string description;
      - public Sprite icon;
      - public Rarity rarity; // Default: Rarity.Common
      - public int lootValue; // Default: 10
      - public RemainData remainData;
      - public RuneData runeData;
      - public PartData partData;
      - public ModuleData moduleData;
      - public GearData gearData;
    - Note: No public methods are defined; OnValidate is private (Unity event).

  - public static class RuneColors
    - Public static readonly Color runeColorSun;
    - public static readonly Color runeColorVerdant;
    - public static readonly Color runeColorSky;
    - public static readonly Color runeColorIgnis;
    - public static readonly Color runeColorVoid;
    - public static Color Get(RuneColorType type);

  - public class RemainData
    - public string remainType; // Insect, Beast, etc.

  - public class RuneData
    - public string effectType; // e.g. "Armor+", "Lifesteal"
    - public RuneColorType runeColortType;
    - public Color runecolor { get; } // Returns corresponding Color for the given runeColortType.

  - public class PartData
    - public string dnaType; // e.g. "Weapon", "Armor"
    - public List<ItemDef> moduleFuel;

  - public class ModuleData
    - public string effect;
    - public float modulePower;

  - public class GearData
    - public GearType slotType; // Head/Chest/Gloves/Legs/Boots/Amulet 
    - public string[] tags; // e.g. "gear","leather","light"
    - public RuneColorType runeSocketType; // Optional/future: Socket type.

3) Key Behavior & Side Effects
- ItemDef.OnValidate (Editor-time)
  - itemType = ItemTypeUtils.FromId(itemId);
  - If ItemKey.TryParse(itemId, out _) fails, logs a warning about invalid itemId and expected "category:item".
  - If lootValue < 0, clamps lootValue to 0.
  - Calls ClearTypeBlocksExcept(itemType) to enforce type-safety by clearing non-matching type data blocks.
- ClearTypeBlocksExcept(ItemType keep)
  - If keep != ItemType.Remains, sets remainData = null.
  - If keep != ItemType.Rune, sets runeData = null.
  - If keep != ItemType.Part, sets partData = null.
  - If keep != ItemType.Module, sets moduleData = null.
  - If keep != ItemType.Gear, sets gearData = null.
- RuneColors.Get(RuneColorType type)
  - Returns corresponding Color for the given RuneColorType via a switch.
  - Default returns Color.white.

4) Constraints & Failure Modes
- OnValidate relies on external types and utilities (ItemType, ItemTypeUtils, ItemKey) defined elsewhere; behavior depends on their implementations.
- itemType is derived each validation; invalid itemId triggers a warning but does not prevent asset creation.
- Loot value is clamped to non-negative; negative inputs are coerced to 0.
- Type-block clearing means that only the data block matching the derived itemType is kept; all other type-specific data blocks are nulled during OnValidate.
- Unknown or unsupported itemType leads to all per-type data blocks being cleared (since keep != type will clear them all).

5) Example
- Not clearly derivable from this file alone; no minimal usage example is provided.

6) Unknowns
- The exact definitions and expected values of ItemType, RuneColorType, GearType, Rarity, ItemKey, and ItemTypeUtils are not in this file.
- How these ScriptableObjects are instantiated or consumed at runtime beyond CreateAssetMenu is not specified here.
- The behavior if itemId is null or empty is not explicitly documented beyond the parsing check.
- Any runtime validation beyond OnValidate is not shown.

