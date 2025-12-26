using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "HeroArcheType", menuName = "Data/Hero Archetype")]
/// <summary>
/// Represents an archetype definition for a character in the game.
/// Contains attributes and settings related to the archetype's role and abilities.
/// </summary>
    public class ArchetypeDef : ScriptableObject
    {
        public string ArchetypeId;             // "Vanguard"
        public string DisplayName;             // Lokalisierbarer Name
        public string RoleDescription;         // "Tank, Frontline, Schadensglättung"

        // TODO: SignaturePassiveName / Desc
        public PrimaryAttackArchetype primAttackType;

        public List<HeroSlot> PreferredSlots;  // ["Torso", "Head"]
        public List<HeroAIPrio> DefaultAIPrio; // ["AttackHighestHP", "BuffAllies", "AA"]

        // Stat-Zuordnung
        public HeroAttribs Core;
        public HeroAttribs Secondary1;
        public HeroAttribs Secondary2;
        public HeroAttribs Tertiary;
        public HeroAttribs Edge;

        // Referenz auf GrowthConfig
        public ArchetypeGrowthConfig GrowthConfig;

        [Header("Signature Passive")]
        public SkillModifierDef SignaturePassive;   // ScriptableObject mit ModifierData

        private void OnValidate()
        {
            if (GrowthConfig?.GrowthPattern?.growthPriority == null)
            {
                DebugManager.Error($"[ArchetypeDef] {name}: GrowthPattern must have exactly 5 entries.","Edtior");
                return;
            }

            int len = GrowthConfig.GrowthPattern.growthPriority.Length;
            if (len != 5)
            {
                DebugManager.Error($"[ArchetypeDef] {name}: GrowthPattern must have exactly 5 entries. Current: {len}", "Edtior");
            }
        }
    }

    [Serializable]
    public class ArchetypeGrowthConfig
    {
        public int CoreTarget = 120;
        public int SecondaryTarget = 100;
        public int TertiaryTarget = 80;
        public int EdgeTarget = 65;
        public LevelGrowthPattern GrowthPattern = new LevelGrowthPattern();
    }

    [Serializable]
/// <summary>
/// Represents a pattern for level growth with a defined priority of roles.
/// </summary>
    public class LevelGrowthPattern
    {
        [Tooltip("Pattern aus genau 5 Rollen, z.B. Core, Sec, Sec, Ter, Edge")]
        public LevelGrowthRole[] growthPriority = new LevelGrowthRole[5]
        {
        LevelGrowthRole.Core,
        LevelGrowthRole.Secondary,
        LevelGrowthRole.Secondary,
        LevelGrowthRole.Tertiary,
        LevelGrowthRole.Edge
        };
    }

    public enum LevelGrowthRole
    {
        Core,
        Secondary,
        Tertiary,
        Edge
    }

    public enum PrimaryAttackArchetype
    { 
        Melee,
        Ranged
    }

}
