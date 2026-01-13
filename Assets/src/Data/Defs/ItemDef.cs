
using CHAL.Systems.Items;
using System.Collections.Generic;
using UnityEngine;


namespace CHAL.Data
{

    [CreateAssetMenu(fileName = "ItemDef", menuName = "Data/ItemDef")]
/// <summary>
/// Represents an item definition in the game, including its properties and attributes.
/// </summary>
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
        public CoreData coreData;

        void OnValidate()
        {
            itemType = ItemTypeUtils.FromId(itemId);

            // Basisschutz: korrekte ID
            if (!ItemKey.TryParse(itemId, out _))
            {
                DebugManager.Warning($"Invalid itemId '{itemId}' in {name}. Expected 'category:item'.");
            }
            // Sanity für LootValue
            if (lootValue < 0) lootValue = 0;

            //Erzwungene Type-Safety
            ClearTypeBlocksExcept(itemType);

            if (itemType == ItemType.Module && moduleData != null)
            {
                ValidateAndSyncModuleData();
            }

        }


        private void ValidateAndSyncModuleData()
        {
            // If a SkillDef is assigned -> enforce skillId from it
            if (moduleData.skillDef != null)
            {
                var id = moduleData.skillDef.SkillId;

                if (string.IsNullOrWhiteSpace(id))
                {
                    DebugManager.Warning($"[ItemDef] Module '{itemId}' has SkillDef '{moduleData.skillDef.name}' with empty SkillId.", "Validation");
                    return;
                }

                if (!string.Equals(moduleData.skillId, id, System.StringComparison.Ordinal))
                {
                    moduleData.skillId = id;
                }
            }
            else
            {
                // No SkillDef assigned, but skillId exists -> keep it, just warn so it doesn't silently drift.
                if (string.IsNullOrWhiteSpace(moduleData.skillId))
                {
                    DebugManager.Warning($"[ItemDef] Module '{itemId}' has no skillDef and no skillId. This module cannot resolve a skill.", "Validation");
                }
            }
        }

        private void ClearTypeBlocksExcept(ItemType keep)
        {
            if (keep != ItemType.Remains) remainData = null;
            if (keep != ItemType.Rune) runeData = null;
            if (keep != ItemType.Part) partData = null;
            if (keep != ItemType.Module) moduleData = null;
            if (keep != ItemType.Gear) gearData = null;
            if (keep != ItemType.Core) coreData = null;
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

/// <summary>
/// Gets the color associated with the specified rune color type.
/// </summary>
/// <param name="type">The type of rune color.</param>
/// <returns>The corresponding color for the rune color type.</returns>
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
        [Tooltip("Designer-Reference: which Skill this module represents.")]
        public SkillModuleDef skillDef;
        public string skillId;
    }

    [System.Serializable]
    public class CoreData
    {
        public CoreType coreType;
    }

    [System.Serializable]
    public class GearData
    {
        public GearType slotType;         // Head/Chest/Gloves/Legs/Boots/Amulet …
        public ArmorClass armorClass;

        public string[] tags;

    }


}
