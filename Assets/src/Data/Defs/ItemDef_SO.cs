
using System.Collections.Generic;
using UnityEngine;


namespace CHAL.Data
{

    [CreateAssetMenu(fileName = "ItemDef", menuName = "Data/ItemDef")]
    public class ItemDef : ScriptableObject
    {
        [Tooltip("Schema: category:item, z.B. remains:gland")]
        public string itemId;

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

        void OnValidate()
        {
            // Basisschutz: korrekte ID
            if (!ItemKey.TryParse(itemId, out _))
            {
                Debug.LogWarning($"[ItemDef] Ungültige itemId '{itemId}' in {name}. Erwartet 'category:item'.");
            }
            // Sanity für LootValue
            if (lootValue < 0) lootValue = 0;

            //Erzwungene Type-Safety
            if (itemId.StartsWith("remains:"))
            {
                runeData = null;
                partData = null;
                moduleData = null;
            }
            else if (itemId.StartsWith("rune:"))
            {
                remainData = null;
                partData = null;
                moduleData = null;
            }
            else if (itemId.StartsWith("part:"))
            {
                remainData = null;
                runeData = null;
                moduleData = null;
            }
            else if (itemId.StartsWith("module:"))
            {
                remainData = null;
                runeData = null;
                partData = null;
            }
        }

    }

    [System.Serializable]
    public class RemainData
    {
        public string remainType;  // Insect, Beast, etc.
    }

    public static class RuneColors
    {
        public static readonly Color runeColorSun = new Color(255 /255, 215 /255, 0 /255);
        public static readonly Color runeColorVerdant = new Color(0 /255, 128 /255, 0 /255);
        public static readonly Color runeColorSky = new Color(50 /255, 50 /255, 255 /255);
        public static readonly Color runeColorIgnis = new Color(200 /255, 0 /255, 0 /255);
        public static readonly Color runeColorVoid = new Color(135 /255, 0 /255, 120 /255);

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
}
