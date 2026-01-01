using UnityEngine;

namespace CHAL.Data
{
    public enum MonsterTagCategory
    {
        Unknown = 0,

        // typische Kategorien (V1, kann wachsen)
        Species,     // undead, reptiloid, insectoid...
        Element,     // fire, frost, poison...
        Role,        // caster, ranged, melee...
        Mechanics,     // armored, enraged, cursed...
        Rank,        // normal, elite, boss...
        Biome,      // molten, swamp, desert...
        Misc         // fallback
    }

    [CreateAssetMenu(menuName = "Data/Monster Tag Def", fileName = "tag_")]
    public sealed class MonsterTagDef : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Canonical id, used everywhere (e.g. 'armored', 'undead', 'molten').")]
        public string tagId;

        public MonsterTagCategory category = MonsterTagCategory.Unknown;

        private void OnValidate()
        {
            // minimal canonicalization: trim (lowercase optional - ich lasse es bewusst weg)
            if (!string.IsNullOrWhiteSpace(tagId))
                tagId = tagId.Trim();
        }
    }
}
