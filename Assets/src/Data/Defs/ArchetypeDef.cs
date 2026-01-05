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

        public PrimaryAttackArchetype primAttackType;

        public List<HeroAIPrio> DefaultAIPrio; // ["AttackHighestHP", "BuffAllies", "AA"]

        // Stat-Zuordnung
        public HeroAttribs Core;
        public HeroAttribs Secondary1;
        public HeroAttribs Secondary2;
        public HeroAttribs Tertiary;
        public HeroAttribs Edge;

        // Referenz auf GrowthConfig
        //public LevelGrowthPattern GrowthPattern = new LevelGrowthPattern();
        //public ArchetypeGrowthConfig GrowthConfig; //TODO-> move to global gamebalance config, not per Archetpye/hero

        private void OnValidate()
        {

        }
    }




    public enum PrimaryAttackArchetype
    { 
        Melee,
        Ranged
    }

}
