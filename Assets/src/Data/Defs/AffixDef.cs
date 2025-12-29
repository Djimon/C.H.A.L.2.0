using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    /// <summary>
    /// Static designer data for a single affix modifier that can roll on gear.
    /// Affixes have no prefix/suffix distinction in this project.
    /// </summary>
    [CreateAssetMenu(fileName = "AffixDef", menuName = "Data/Affixes/AffixDef")]
    public sealed class AffixDef : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Stable identifier, recommended: lower_snake_case (e.g. life_flat, dmg_fire_pct).")]
        public string AffixId;

        [Header("Categorization")]
        public AffixCategory Category;
        public AffixFamilyBitMask FamilyMembership;

        [Header("Slot Restrictions (optional)")]
        [Tooltip("If empty, this implicit is allowed on all gear types. If set, it can only roll on these GearTypes.")]
        public GearType[] AllowedGearTypes;

        [Header("Effect")]
        [Tooltip("Which stat is modified by this affix (gear/base stats, not combat buffs).")]
        public GearStatTarget Target;

        [Tooltip("How the value applies (flat or percent).")]
        public GearValueKind ValueKind;

        [Header("Roll Ranges by Base Tier")]
        public TieredRollRange Ranges;

        [Header("Optional Weight")]
        [Min(0f)]
        [Tooltip("Default 1.0: lower = lower chance, higher = higher chance (used when family doesn't override).")]
        public float customWeight = 1f;


/// <summary>
/// Determines if the specified gear type is allowed.
/// </summary>
/// <param name="gearType">The gear type to check.</param>
/// <returns>True if the gear type is allowed; otherwise, false.</returns>
        public bool Allows(GearType gearType)
        {
            if (AllowedGearTypes == null || AllowedGearTypes.Length == 0) return true;
            for (int i = 0; i < AllowedGearTypes.Length; i++)
                if (AllowedGearTypes[i] == gearType) return true;
            return false;
        }

        private void OnValidate()
        {
            AffixId = (AffixId ?? string.Empty).Trim();

            if (customWeight < 0f) customWeight = 0f;

            if (!string.IsNullOrEmpty(AffixId) && !IsValidId(AffixId))
                DebugManager.Warning($"[AffixDef] Unusual Id '{AffixId}' in asset '{name}'. Recommended: lower_snake_case (a-z, 0-9, _).", "System");
            

            if (FamilyMembership == AffixFamilyBitMask.None && !string.IsNullOrEmpty(AffixId))          
                DebugManager.Warning($"[AffixDef] '{AffixId}' has no FamilyMembership set.", "System");
            

            if (!string.IsNullOrEmpty(AffixId) && Category == AffixCategory.None)
                DebugManager.Warning($"[AffixDef] '{AffixId}' has Category=None. Limits/dedupe will not behave as intended.", "System");

        }

        /// <summary>
        /// Deterministic enumeration like your ImplicitDef. Used for indexing in AffixRegistry.
        /// </summary>
        public IEnumerable<AffixFamily> EnumerateFamilies()
        {
            if ((FamilyMembership & AffixFamilyBitMask.Core) != 0) yield return AffixFamily.Core;
            if ((FamilyMembership & AffixFamilyBitMask.Defensive) != 0) yield return AffixFamily.Defensive;
            if ((FamilyMembership & AffixFamilyBitMask.Synergy) != 0) yield return AffixFamily.Synergy;
            if ((FamilyMembership & AffixFamilyBitMask.Utility) != 0) yield return AffixFamily.Utility;
        }

        private static bool IsValidId(string id)
        {
            for (int i = 0; i < id.Length; i++)
            {
                var c = id[i];
                var ok = (c >= 'a' && c <= 'z')
                      || (c >= '0' && c <= '9')
                      || (c == '_');
                if (!ok) return false;
            }
            return true;
        }
    }

    [Serializable]
    public enum AffixCategory
    { 
        None = 0,
        Attribute,
        Crit,
        Damage,
        Defense,
        Life,
        Skill, //inxcl. cast speed and cooldown
        Special // rarities, etc.
    }

    [Flags]
    public enum AffixFamilyBitMask
    {
        None = 0,
        Core = 1 << 0, // 0001
        Defensive = 1 << 1, // 0010
        Synergy = 1 << 2, // 0100
        Utility = 1 << 3, // 1000
    }

    [Serializable]
    public enum AffixFamily
    {
        Core = 0,
        Defensive = 1,
        Synergy = 2,
        Utility = 3
    }
}
