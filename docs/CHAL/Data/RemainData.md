# CHAL.Data.RemainData

_Automatically generated/updated from `Assets/src/Data/Defs/ItemDef_SO.cs`._

1) Purpose
- Defines ItemDef as a Unity ScriptableObject that holds item metadata and per-type data blocks.
- Provides serializable data containers: RemainData, RuneData, PartData, ModuleData, GearData.
- Enforces simple editor-time validation and type-safety via OnValidate.

2) Public API
- Namespace: CHAL.Data

- public class ItemDef : ScriptableObject
  - [CreateAssetMenu(fileName = "ItemDef", menuName = "Data/ItemDef")]
  - public string itemId; // Schema: category:item
  - [HideInInspector] public ItemType itemType;
  - [TextArea] public string description;
  - public Sprite icon;
  - public Rarity rarity = Rarity.Common;
  - [Tooltip("Wert fr Softcap/Budget (empf.: Common 10, Rare 30, Epic 50, Legendary 80)")] public int lootValue = 10;
  - [Tooltip("Type Specific Data")]
  - public RemainData remainData;
  - public RuneData runeData;
  - public PartData partData;
  - public ModuleData moduleData;
  - public GearData gearData;

- private void OnValidate() [not public API; editor lifecycle]
  - (see Key Behavior & Side Effects)

- private void ClearTypeBlocksExcept(ItemType keep) [not public API; internal helper]

- public class RemainData : [System.Serializable]
  - public string remainType; // Insect, Beast, etc.

- public static class RuneColors
  - public static readonly Color runeColorSun = new Color(255f / 255f, 215f / 255f, 0 / 255f);
  - public static readonly Color runeColorVerdant = new Color(0 / 255f, 128f / 255f, 0 / 255f);
  - public static readonly Color runeColorSky = new Color(50 / 255f, 50 / 255f, 255 / 255f);
  - public static readonly Color runeColorIgnis = new Color(200 / 255f, 0 / 255f, 0 / 255f);
  - public static readonly Color runeColorVoid = new Color(135 / 255f, 0 / 255f, 120 / 255f);
  - public static Color Get(RuneColorType type) => type switch
    - Sun => runeColorSun
    - Verdant => runeColorVerdant
    - Sky => runeColorSky
    - Ignis => runeColorIgnis
    - Void => runeColorVoid
    - _ => Color.white

- public class RuneData : [System.Serializable]
  - public string effectType; // e.g. "Armor+", "Lifesteal"
  - public RuneColorType runeColortType;
  - public Color runecolor => RuneColors.Get(runeColortType);

- public class PartData : [System.Serializable]
  - public string dnaType; // e.g. "Weapon", "Armor"
  - public List<ItemDef> moduleFuel;

- public class ModuleData : [System.Serializable]
  - public string effect;
  - public float modulePower;

- public class GearData : [System.Serializable]
  - public GearType slotType; // Head/Chest/Gloves/Legs/Boots/Amulet
  - public string[] tags; // e.g. "gear","leather","light"
  - public RuneColorType runeSocketType;

3) Key Behavior & Side Effects
- OnValidate():
  - itemType = ItemTypeUtils.FromId(itemId);
  - If ItemKey.TryParse(itemId, out _) fails -> log warning about invalid itemId (expects "category:item")
  - If lootValue < 0 -> lootValue = 0
  - ClearTypeBlocksExcept(itemType) to enforce type-specific data blocks
- ClearTypeBlocksExcept(keep):
  - remainData = null if keep != ItemType.Remains
  - runeData = null if keep != ItemType.Rune
  - partData = null if keep != ItemType.Part
  - moduleData = null if keep != ItemType.Module
  - gearData = null if keep != ItemType.Gear

4) Constraints & Failure Modes
- itemId must parse via ItemKey.TryParse; invalid formats produce a warning (no exception)
- lootValue cannot be negative; values below 0 are clamped to 0
- Type-specific data blocks are cleared to maintain exclusive type data; possible data loss if misconfigured
- itemType is public but not shown in Inspector (HideInInspector)

5) Example
- (none derivable from file; omitted)

6) Unknowns
- Definitions/behavior of ItemType, ItemTypeUtils, ItemKey, RuneColorType, GearType, Rarity, and how ItemDef is consumed at runtime (outside OnValidate)
- Exact usage and validation semantics of RemainData, RuneData, PartData, ModuleData, GearData beyond their field definitions
- Any multithreading/async considerations related to ScriptableObject usage in this project

