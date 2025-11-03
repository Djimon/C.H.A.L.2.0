
using CHAL.Systems.Items;
using System.Collections.Generic;
using UnityEngine;


namespace CHAL.Data
{

    [CreateAssetMenu(fileName = "ItemDef", menuName = "Data/ItemDef")]
    public class ItemDef : ScriptableObject
    {
        [Tooltip("Schema: category:item, z.B. remains:gland")]
        public string itemId;
        [HideInInspector]
        public ItemType itemType;

        //public string displayName; //abgeleitet von ID über Localizationmanager -> TODO
        [TextArea] public string description;
        public Sprite icon;

        public Rarity rarity = Rarity.Common;

        [Tooltip("Wert für Softcap/Budget (empf.: Common 10, Rare 30, Epic 50, Legendary 80)")]
        public int lootValue = 10;

        [Tooltip("Type Specific Data")]

        public RemainData remainData;
        public RuneData runeData;
        public PartData partData;
        public ModuleData moduleData;
        public GearData gearData;

        void OnValidate()
        {
            itemType = ItemTypeUtils.FromId(itemId);

            // Basisschutz: korrekte ID
            if (!ItemKey.TryParse(itemId, out _))
            {
                Debug.LogWarning($"[ItemDef] Ungültige itemId '{itemId}' in {name}. Erwartet 'category:item'.");
            }
            // Sanity für LootValue
            if (lootValue < 0) lootValue = 0;

            //Erzwungene Type-Safety
            ClearTypeBlocksExcept(itemType);
            
        }

        private void ClearTypeBlocksExcept(ItemType keep)
        {
            if (keep != ItemType.Remains) remainData = null;
            if (keep != ItemType.Rune) runeData = null;
            if (keep != ItemType.Part) partData = null;
            if (keep != ItemType.Module) moduleData = null;
            if (keep != ItemType.Gear) gearData = null;
        }
    }

    [System.Serializable]
    public class RemainData
    {
        public string remainType;  // Insect, Beast, etc.
    }

    public static class RuneColors
    {
        public static readonly Color runeColorSun = new Color(255f /255f, 215f /255f, 0 /255f);
        public static readonly Color runeColorVerdant = new Color(0 /255f, 128f /255f, 0 /255f);
        public static readonly Color runeColorSky = new Color(50 /255f, 50 /255f, 255 /255f);
        public static readonly Color runeColorIgnis = new Color(200 /255f, 0 /255f, 0 /255f);
        public static readonly Color runeColorVoid = new Color(135 /255f, 0 /255f, 120 /255f);

        public static Color Get(RuneColorType type) => type switch
        {
            RuneColorType.Sun => runeColorSun,
            RuneColorType.Verdant => runeColorVerdant,
            RuneColorType.Sky => runeColorSky,
            RuneColorType.Ignis => runeColorIgnis,
            RuneColorType.Void => runeColorVoid,
            _ => Color.white
        };
    }

    [System.Serializable]
    public class RuneData
    {
        public string effectType; // e.g. "Armor+", "Lifesteal"
        public RuneColorType runeColortType;

        public Color runecolor => RuneColors.Get(runeColortType);

    }

    [System.Serializable]
    public class PartData
    {
        public string dnaType; // e.g. "Weapon", "Armor"
        public List<ItemDef> moduleFuel;
    }

    [System.Serializable]
    public class ModuleData
    {
        public string effect;
        public float modulePower;
    }

    [System.Serializable]
    public class GearData
    {
        public GearType slotType;         // Head/Chest/Gloves/Legs/Boots/Amulet …
        public string[] tags;             // z.B. "gear","leather","light"

        // optional/future: Sockeltyp (jetzt auf None lassen, Enum kannst du später ausbauen)
        public RuneColorType runeSocketType;
    }


}
