// File: Assets/src/CHAL/Data/Implicits/ImplicitDef.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    /// <summary>
    /// Data definition for a single implicit modifier that can roll on gear.
    /// This is static content (designer data), not runtime roll state.
    /// </summary>
    [CreateAssetMenu(fileName = "ImplicitDef", menuName = "Data/Implicits/ImplicitDef")]
    public sealed class ImplicitDef : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Stable identifier, recommended: lower_snake_case (e.g. armor_pct).")]
        public string Id;

        [Header("Categorization")]
        public ImplicitPool Pool;
        public ImplicitRole Role;

        [Header("Slot Restrictions (optional)")]
        [Tooltip("If empty, this implicit is allowed on all gear types. If set, it can only roll on these GearTypes.")]
        public GearType[] AllowedGearTypes;

        [Header("Effect")]
        [Tooltip("Which stat is modified by this implicit (gear/base stats, not combat buffs).")]
        public GearStatTarget Target;

        [Tooltip("How the value applies (flat or percent).")]
        public GearValueKind ValueKind;

        [Header("Roll Ranges by Base Tier")]
        public TieredRollRange Ranges;

        [Header("Optional Weight")]
        [Min(0f)]
        [Tooltip("Default 1.0: lower = lower chance, higher = higher chance.")]
        public float customWeight = 1f;

        private void OnValidate()
        {
            Id = (Id ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(Id) && !IsValidId(Id))
            {
                DebugManager.Warning(
                    $"[ImplicitDef] Unusual Id '{Id}' in asset '{name}'. Recommended format: lower_snake_case (a-z, 0-9, _).",
                    "System");
            }

            if (customWeight < 0f) customWeight = 0f;

            // Normalize/validate ranges
            Ranges = Ranges.Normalize();

            // Deduplicate AllowedGearTypes
            if (AllowedGearTypes != null && AllowedGearTypes.Length > 1)
            {
                var set = new HashSet<GearType>();
                var list = new List<GearType>(AllowedGearTypes.Length);
                for (int i = 0; i < AllowedGearTypes.Length; i++)
                {
                    var gt = AllowedGearTypes[i];
                    if (set.Add(gt)) list.Add(gt);
                }
                AllowedGearTypes = list.ToArray();
            }
        }

        public bool Allows(GearType gearType)
        {
            if (AllowedGearTypes == null || AllowedGearTypes.Length == 0) return true;
            for (int i = 0; i < AllowedGearTypes.Length; i++)
                if (AllowedGearTypes[i] == gearType) return true;
            return false;
        }

        private static bool IsValidId(string id)
        {
            // erlaubt: a-z, 0-9, _
            for (int i = 0; i < id.Length; i++)
            {
                char c = id[i];
                bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                if (!ok) return false;
            }
            return true;
        }
    }

    [Serializable]
    public enum ImplicitPool
    {
        Melee = 0,
        Ranged = 1,
        Caster = 2,
        Neutral = 3
    }

    [Serializable]
    public enum ImplicitRole
    {
        Defense = 0,
        Offense = 1,
        Utility = 2
    }

    [Serializable]
    public enum GearValueKind
    {
        Flat = 0,
        Percent = 1
    }


    /// <summary>
    /// Targets for static gear modifiers (persisted on item instances).
    /// Keep this separate from combat-time modifier targets to avoid mixing concerns.
    /// </summary>
    [Serializable]
    public enum GearStatTarget
    {
        // Defensive
        Armor = 0,
        Barrier = 1,
        DodgeChance = 2,
        ElementResist = 3,

        // Vital
        MaxLife = 10,

        // Utility
        MovementSpeed = 20,
        ItemRarity = 21,

        // Offense (basic)
        Damage = 30,
        PhysDamage = 31,
        ElementDamage = 32,
        Thorns = 33,

        CritChacne = 36,
        CritDmgBonus = 37,

        // Attributes (optional, if you want implicits like +STR)
        STR = 40,
        DEX = 41,
        CON = 42,
        INT = 43,
        WIL = 44
    }

    [Serializable]
    public struct RollRange
    {
        public float Min;
        public float Max;

        public RollRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public RollRange Normalize()
        {
            if (Min > Max)
            {
                var tmp = Min;
                Min = Max;
                Max = tmp;
            }
            return this;
        }
    }

    /// <summary>
    /// Roll ranges per gear base tier (T1/T2/T3).
    /// </summary>
    [Serializable]
    public struct TieredRollRange
    {
        public RollRange Tier1;
        public RollRange Tier2;
        public RollRange Tier3;

        public TieredRollRange Normalize()
        {
            Tier1 = Tier1.Normalize();
            Tier2 = Tier2.Normalize();
            Tier3 = Tier3.Normalize();
            return this;
        }
    }
}
