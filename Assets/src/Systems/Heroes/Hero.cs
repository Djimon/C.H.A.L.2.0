using CHAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CHAL.Systems.Hero
{
    public class Hero
    {
        public string heroID;
        public ArchetypeDef Archetype;
        public int Level = 1;

        public Dictionary<HeroStat, int> Stats = new();

        // interne Akkumulatoren
        private Dictionary<HeroStat, double> _accumulator = new();
        private Dictionary<HeroStat, int> _goals = new();
        private int _totalGrowth;

        public Hero(ArchetypeDef def)
        {
            Archetype = def;
            InitStats();
        }

        private void InitStats()
        {
            // Startwerte nach GrowthRole
            var startMap = new Dictionary<GrowthRole, int> {
                { GrowthRole.Core, 14 },
                { GrowthRole.Secondary, 11 },
                { GrowthRole.Tertiary, 8 },
                { GrowthRole.Edge, 6 }
            };

            // Zielwerte aus Config
            var targetMap = new Dictionary<GrowthRole, int> {
                { GrowthRole.Core, Archetype.GrowthConfig.CoreTarget },
                { GrowthRole.Secondary, Archetype.GrowthConfig.SecondaryTarget },
                { GrowthRole.Tertiary, Archetype.GrowthConfig.TertiaryTarget },
                { GrowthRole.Edge, Archetype.GrowthConfig.EdgeTarget }
            };

            // Reihenfolge der Stats laut ArchetypeDef
            HeroStat[] slots = {
                Archetype.Core,
                Archetype.Secondary1,
                Archetype.Secondary2,
                Archetype.Tertiary,
                Archetype.Edge
            };

            // Akkus initialisieren
            foreach (HeroStat s in Enum.GetValues(typeof(HeroStat)))
                _accumulator[s] = 0;

            // Pattern anwenden
            for (int i = 0; i < Archetype.GrowthConfig.GrowthPattern.Roles.Length; i++)
            {
                GrowthRole role = Archetype.GrowthConfig.GrowthPattern.Roles[i];
                HeroStat stat = slots[i];

                Stats[stat] = startMap[role];
                _goals[stat] = targetMap[role] - startMap[role];
            }

            _totalGrowth = _goals.Values.Sum();
        }

        [ContextMenu("Debug/LevelUP")]
        public void LevelUp()
        {
            if (Level >= 100) return;

            //TODO: Crosscheck, if the coresponding trheshold of XP is reached

            Level++;
            DebugManager.Log($"Hero: {heroID} is now level {Level}.", DebugManager.EDebugLevel.Test, "Hero");
            int ptsThisLevel = (Level % 5 == 0) ? 5 : 4;

            // Punkte anteilig aufladen
            foreach (var kv in _goals)
            {
                double share = (double)kv.Value / _totalGrowth;
                _accumulator[kv.Key] += share * ptsThisLevel;
            }

            // Punkte vergeben
            while (_accumulator.Any(x => x.Value >= 1.0))
            {
                var next = _accumulator
                    .OrderByDescending(x => x.Value)
                    .ThenBy(x => Guid.NewGuid()) // Random-Tiebreak
                    .First().Key;

                Stats[next] += 1;
                _accumulator[next] -= 1.0;
            }

            string msg = $"Neue Punkte: STR={Stats[HeroStat.STR]} " +
                 $"DEX={Stats[HeroStat.DEX]} " +
                 $"CON={Stats[HeroStat.CON]} " +
                 $"INT={Stats[HeroStat.INT]} " +
                 $"WIL={Stats[HeroStat.WIL]}";

            DebugManager.Log(msg,DebugManager.EDebugLevel.Test,"Hero");
        }
    }
}
