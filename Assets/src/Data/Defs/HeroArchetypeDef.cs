using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "HeroArcheType", menuName = "Data/Hero Archetype")]
    public class ArchetypeDef : ScriptableObject
    {
        public string ArchetypeId;             // "Vanguard"
        public string DisplayName;             // Lokalisierbarer Name
        public string RoleDescription;         // "Tank, Frontline, Schadensglättung"

        // TODO: SignaturePassiveName / Desc

        public List<HeroSlot> PreferredSlots;  // ["Torso", "Head"]
        public List<HeroAIPrio> DefaultAIPrio; // ["AttackHighestHP", "BuffAllies", "AA"]

        // Stat-Zuordnung
        public HeroStat Core;
        public HeroStat Secondary1;
        public HeroStat Secondary2;
        public HeroStat Tertiary;
        public HeroStat Edge;

        // Referenz auf GrowthConfig
        public ArchetypeGrowthConfig GrowthConfig;
    }

    [Serializable]
    public class ArchetypeGrowthConfig
    {
        public int CoreTarget = 120;
        public int SecondaryTarget = 100;
        public int TertiaryTarget = 80;
        public int EdgeTarget = 65;
        public GrowthPattern GrowthPattern = new GrowthPattern();
    }

    [Serializable]
    public class GrowthPattern
    {
        [Tooltip("Pattern aus genau 5 Rollen, z.B. Core, Sec, Sec, Ter, Edge")]
        public GrowthRole[] Roles = new GrowthRole[5]
        {
        GrowthRole.Core,
        GrowthRole.Secondary,
        GrowthRole.Secondary,
        GrowthRole.Tertiary,
        GrowthRole.Edge
        };
    }

    public enum GrowthRole
    {
        Core,
        Secondary,
        Tertiary,
        Edge
    }

}