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
        public HeroAttribs Core;
        public HeroAttribs Secondary1;
        public HeroAttribs Secondary2;
        public HeroAttribs Tertiary;
        public HeroAttribs Edge;

        // Referenz auf GrowthConfig
        public ArchetypeGrowthConfig GrowthConfig;

        [Header("Signature Passive")]
        public ModifierDef SignaturePassive;   // ScriptableObject mit ModifierData

        private void OnValidate()
        {
            if (GrowthConfig?.GrowthPattern?.growthPriority == null)
            {
                DebugManager.Error($"[ArchetypeDef] {name}: GrowthPattern muss genau 5 Einträge haben.","Edtior");
                return;
            }

            int len = GrowthConfig.GrowthPattern.growthPriority.Length;
            if (len != 5)
            {
                DebugManager.Error($"[ArchetypeDef] {name}: GrowthPattern muss genau 5 Einträge haben. Aktuelle: {len}", "Edtior");
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
        public GrowthPattern GrowthPattern = new GrowthPattern();
    }

    [Serializable]
    public class GrowthPattern
    {
        [Tooltip("Pattern aus genau 5 Rollen, z.B. Core, Sec, Sec, Ter, Edge")]
        public GrowthRole[] growthPriority = new GrowthRole[5]
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