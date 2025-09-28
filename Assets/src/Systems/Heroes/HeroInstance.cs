using CHAL.Data;
using CHAL.Systems.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CHAL.Systems.Hero
{
    public class HeroInstance : EffectReceiver
    {
        public HeroDef heroDef;
        public ArchetypeDef Archetype => heroDef.Archetype;

        public int Level = 1;

        public Dictionary<HeroAttribs, int> attributes = new();

        // interne Akkumulatoren
        private Dictionary<HeroAttribs, double> _accumulator = new();
        private Dictionary<HeroAttribs, int> _goals = new();
        private int _totalGrowth;

        //SKILL
        public List<SkillInstance> Skills;

        public GameObject currentTarget;

        public HeroInstance(HeroDef def)
        {
            this.heroDef = def;

            if (Archetype == null)
                DebugManager.Error($"No Archetype! for Hero {heroDef.name}");

            if (Archetype.SignaturePassive != null)
            {
                ActiveModifiers.AddModifier(Archetype.SignaturePassive.ToModifierData());
            }

            InitStats();

            MaxHP = 100;
            CurrentHP = MaxHP;
        }

        public override void TakeDamage(float amount, DamageType type)
        {
            //ToDO: Armor Resistences, etc
            CurrentHP -= amount;
            if (CurrentHP < 0)
            {
                OnDeath();
            }
        }

        protected override void OnDeath()
        {
            //DIE
            DebugManager.Log($"{heroDef.DisplayName} died.", DebugManager.EDebugLevel.Test, "Hero");
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
            HeroAttribs[] slots = {
                Archetype.Core,
                Archetype.Secondary1,
                Archetype.Secondary2,
                Archetype.Tertiary,
                Archetype.Edge
            };

            // Akkus initialisieren
            foreach (HeroAttribs s in Enum.GetValues(typeof(HeroAttribs)))
                _accumulator[s] = 0;

            // GrowthPattern anwenden
            for (int i = 0; i < Archetype.GrowthConfig.GrowthPattern.growthPriority.Length; i++)
            {
                GrowthRole growthPrio = Archetype.GrowthConfig.GrowthPattern.growthPriority[i];
                HeroAttribs stat = slots[i];

                attributes[stat] = startMap[growthPrio];
                _goals[stat] = targetMap[growthPrio] - startMap[growthPrio];
            }

            _totalGrowth = _goals.Values.Sum();
        }

        [ContextMenu("Debug/LevelUP")]
        public void LevelUp()
        {
            if (Level >= 100) return;

            //TODO: CrossCheck, if the coresponding trheshold of XP is reached
            Level++;
            DebugManager.Log($"Hero: {heroDef.HeroId} is now level {Level}.", DebugManager.EDebugLevel.Test, "Hero");

            //How many AttriPoints are given in this level?
            int ptsThisLevel = (Level % 5 == 0) ? 5 : 4;

            // Punkte anteilig das accu-shares auf die 5 Attribute aufladen
            foreach (var kv in _goals)
            {
                //akkumulator wird auf Basis des Zielwertes in abhängigkeit des aktuellen levels geladen
                double share = (double)kv.Value / _totalGrowth;
                _accumulator[kv.Key] += share * ptsThisLevel;
            }

            // Punkte vergeben
            while (_accumulator.Any(x => x.Value >= 1.0))
            {
                //Welcher stat ist "dran"? -> höchster Akkumulator  -> Prio-riehenfolge
                var next = _accumulator
                    .OrderByDescending(x => x.Value)
                    .ThenBy(x => Guid.NewGuid()) // Random-Tiebreak
                    .First().Key;

                attributes[next] += 1;
                _accumulator[next] -= 1.0;
            }

            string msg = $"Neue Attribute: STR={attributes[HeroAttribs.STR]} |" +
                 $"DEX={attributes[HeroAttribs.DEX]} | " +
                 $"CON={attributes[HeroAttribs.CON]} | " +
                 $"INT={attributes[HeroAttribs.INT]} | " +
                 $"WIL={attributes[HeroAttribs.WIL]}";

            DebugManager.Log(msg,DebugManager.EDebugLevel.Test,"Hero");
        }

    }
}
