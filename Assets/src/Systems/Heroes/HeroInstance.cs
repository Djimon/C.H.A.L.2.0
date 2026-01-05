using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Skill;
using CHAL.Systems.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CHAL.Systems.Hero
{
/// <summary>
/// Represents an instance of a hero with attributes and skills.
/// </summary>
    public class HeroInstance : EffectReceiver, IAttributeHolder
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

        public event Action<HeroInstance> Died;
        private bool _isDead;

        // --- in Map Progression (XP / Orbit / Sockets)
        private HeroProgressData _progressRef;
        private HeroXPConfig _heroXPconfig;
        public int CurrentXP { get; private set; }

        public int TotalXP { get; private set; }

        public int TotalOrbitPointsEarned { get; private set; }

        public int UnspentOrbitPoints { get; private set; }

        public int UnlockedSockets { get; private set; } = 1;


        public HeroInstance(HeroDef def, HeroProgressData progress = null)
        {
            this.heroDef = def;

            if (Archetype == null)
                DebugManager.Error($"No Archetype! for Hero {heroDef.name}");

            if (heroDef.SignaturePassive != null)
            {
                ActiveModifiers.AddGenericModifier(heroDef.SignaturePassive.ToModifierData());
            }

            _heroXPconfig = BalanceManager.Instance.HeroXPConfig;
            // --- Progress übernehmen ---
            _progressRef = progress;

            if (_progressRef != null)
            {
                Level = Mathf.Clamp(_progressRef.Level, 1, 100);
                CurrentXP = Mathf.Max(0, _progressRef.CurrentXP);
                TotalXP = Mathf.Max(0, _progressRef.TotalXP);
                TotalOrbitPointsEarned = Mathf.Max(0, _progressRef.TotalOrbitPoints);
                UnspentOrbitPoints = Mathf.Max(0,_progressRef.UnspentOrbitPoints);
                UnlockedSockets = Mathf.Max(0, _progressRef.UnlockedSockets);
            }
            else
            {
                Level = 1;
                CurrentXP = 0;
                TotalXP = 0;
                TotalOrbitPointsEarned = 0;
                UnspentOrbitPoints = 0;
                UnlockedSockets = 0;
            }

            // Stats auf Basis des Levels rekonstruieren
            InitStatsForCurrentLevel();

            MaxHP = heroDef.BaseHealth;   // später: HP skalierend aus Stats ableiten
            CurrentHP = MaxHP;
        }

        private void InitStatsForCurrentLevel()
        {
            // 1) Basis-Setup wie dein bisheriges InitStats
            InitBaseStatsAtLevel1();

            // 2) Level-1->Level(n) nachziehen, damit Attribute stimmen
            if (Level <= 1) return;

            for (int i = 2; i <= Level; i++)
            {
                InternalLevelUpAttributesOnly();  // Level++ hier nicht setzen!
            }
        }

        // Aus deinem bisherigen InitStats extrahiert:
        private void InitBaseStatsAtLevel1()
        {
                // Startwerte nach GrowthRole
            var startMap = new Dictionary<LevelGrowthRole, int> {
                { LevelGrowthRole.Core, 14 },
                { LevelGrowthRole.Secondary, 11 },
                { LevelGrowthRole.Tertiary, 8 },
                { LevelGrowthRole.Edge, 6 }
            };

            // Zielwerte ...
            var targetMap = new Dictionary<LevelGrowthRole, int> {
                { LevelGrowthRole.Core, Archetype.GrowthConfig.CoreTarget },
                { LevelGrowthRole.Secondary, Archetype.GrowthConfig.SecondaryTarget },
                { LevelGrowthRole.Tertiary, Archetype.GrowthConfig.TertiaryTarget },
                { LevelGrowthRole.Edge, Archetype.GrowthConfig.EdgeTarget }
            };

            HeroAttribs[] slots = {
                Archetype.Core,
                Archetype.Secondary1,
                Archetype.Secondary2,
                Archetype.Tertiary,
                Archetype.Edge
            };

            _accumulator.Clear();
            _goals.Clear();
            attributes.Clear();

            foreach (HeroAttribs s in Enum.GetValues(typeof(HeroAttribs)))
                _accumulator[s] = 0;

            for (int i = 0; i < Archetype.GrowthConfig.GrowthPattern.growthPriority.Length; i++)
            {
                LevelGrowthRole growthPrio = Archetype.GrowthConfig.GrowthPattern.growthPriority[i];
                HeroAttribs stat = slots[i];

                attributes[stat] = startMap[growthPrio];
                _goals[stat] = targetMap[growthPrio] - startMap[growthPrio];
            }

            _totalGrowth = _goals.Values.Sum();
        }


        private void InternalLevelUpAttributesOnly()
        {
            int ptsThisLevel = (Level % 5 == 0) ? 5 : 4;

            foreach (var kv in _goals)
            {
                double share = (double)kv.Value / _totalGrowth;
                _accumulator[kv.Key] += share * ptsThisLevel;
            }

            while (_accumulator.Any(x => x.Value >= 1.0))
            {
                var next = _accumulator
                    .OrderByDescending(x => x.Value)
                    .ThenBy(x => Guid.NewGuid())
                    .First().Key;

                attributes[next] += 1;
                _accumulator[next] -= 1.0;
            }
        }

        /// <summary>
        /// Returns the current value of the requested hero attribute
        /// </summary>
        /// <param name="attribute">The hero attribute to query (STR, DEX, CON, INT, WIL)</param>
        /// <returns></returns>
        public float GetAttributeValue(HeroAttribs attribute)
        {
            // Single source of truth for reading hero attributes.
            // Do not bypass this from gameplay systems.
            return attributes[attribute];
        }


        /// <summary>
        /// Calculates the effective movement speed of the hero.
        /// </summary>
        /// <returns>The effective movement speed as a float.</returns>
        public float GetEffectiveMovementSpeed()
        {
            //TODO: Modfier drauf rechnen
            return heroDef.BaseMovementSpeed;
        }

        protected override void OnDeath()
        {
            if (_isDead) return;                  // idempotent
            _isDead = true;
            CurrentHP = 0;

            DebugManager.Log($"{heroDef.DisplayName} died.", DebugManager.EDebugLevel.Test, "Hero");
            Died?.Invoke(this);            
        }

        private void InitStats()
        {
            // Startwerte nach GrowthRole
            var startMap = new Dictionary<LevelGrowthRole, int> {
                { LevelGrowthRole.Core, 14 },
                { LevelGrowthRole.Secondary, 11 },
                { LevelGrowthRole.Tertiary, 8 },
                { LevelGrowthRole.Edge, 6 }
            };

            // Zielwerte aus Config
            var targetMap = new Dictionary<LevelGrowthRole, int> {
                { LevelGrowthRole.Core, Archetype.GrowthConfig.CoreTarget },
                { LevelGrowthRole.Secondary, Archetype.GrowthConfig.SecondaryTarget },
                { LevelGrowthRole.Tertiary, Archetype.GrowthConfig.TertiaryTarget },
                { LevelGrowthRole.Edge, Archetype.GrowthConfig.EdgeTarget }
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
                LevelGrowthRole growthPrio = Archetype.GrowthConfig.GrowthPattern.growthPriority[i];
                HeroAttribs stat = slots[i];

                attributes[stat] = startMap[growthPrio];
                _goals[stat] = targetMap[growthPrio] - startMap[growthPrio];
            }

            _totalGrowth = _goals.Values.Sum();
        }

        /// <summary>
        /// Calculates the effective base damage of the hero.
        /// </summary>
        /// <returns>The effective base damage as a float.</returns>
        public float GetEffectiveBaseDamage()
        {
            // TODO: ActiveModifiers berücksichtigen (Multiplikatoren/Additive)
            return heroDef.BaseDamage;
        }

        #region Progression (XP / Level / Orbit)

/// <summary>
/// Adds experience points to the hero.
/// Experience points are only added if the amount is positive and the hero has not reached the level cap.
/// </summary>
/// <param name="amount">The amount of experience points to add.</param>
        public void AddXP(int amount)
        {
            if (amount <= 0) return;
            if (_heroXPconfig == null)
            {
                DebugManager.Log(
                    "[HeroInstance] AddXP ohne HeroXPConfig aufgerufen – XP wird ignoriert.", DebugManager.EDebugLevel.Dev, "Hero");
                return;
            }

            if (Level >= _heroXPconfig.LevelCap)
                return;

            CurrentXP += amount;
            TotalXP += amount;

            TryApplyLevelUps();
        }

        private void TryApplyLevelUps()
        {
            if (_heroXPconfig == null)
                return;

            // Maximale Anzahl möglicher LevelUps anhand des Caps begrenzen.
            int maxLevelUps = Mathf.Max(0, _heroXPconfig.LevelCap - Level);

            for (int i = 0; i < maxLevelUps; i++)
            {
                int required = _heroXPconfig.GetRequiredXPForLevel(Level);
                if (required <= 0)
                {
                    // Kein weiteres Level konfiguriert oder Cap erreicht.
                    break;
                }

                if (CurrentXP < required)
                {
                    // Nicht genug XP für den nächsten LevelUp.
                    break;
                }

                CurrentXP -= required;
                ApplyLevelUp();
            }
        }

/// <summary>
/// Calculates the required experience points for the next level.
/// </summary>
/// <param name="level">The current level of the hero.</param>
/// <returns>The experience points needed to reach the next level.</returns>
        public int GetRequiredXPForNextLevel(int level)
        {
            return _heroXPconfig.GetRequiredXPForLevel(Level);
        }


        [ContextMenu("Debug/LevelUP")]
/// <summary>
/// Forces the hero to level up immediately.
/// </summary>
        public void Debug_ForceLevelUp()
        {
            ApplyLevelUp();
        }


        private void ApplyLevelUp()
        {

            Level++;
            DebugManager.Log($"Hero: {heroDef.HeroId} is now level {Level}.", DebugManager.EDebugLevel.Test, "Hero");

            // Wie viele Attributpunkte gibt dieses Level?
            int ptsThisLevel = (Level % 5 == 0) ? 5 : 4;

            // Punkte anteilig nach Accu-Shares auf die 5 Attribute aufladen
            foreach (var kv in _goals)
            {
                // Akkumulator wird auf Basis des Zielwertes in Abhängigkeit des aktuellen Levels geladen
                double share = (double)kv.Value / _totalGrowth;
                _accumulator[kv.Key] += share * ptsThisLevel;
            }

            // Punkte vergeben
            while (_accumulator.Any(x => x.Value >= 1.0))
            {
                // Welcher Stat ist "dran"? -> höchster Akkumulator -> Prio-Reihenfolge
                var next = _accumulator
                    .OrderByDescending(x => x.Value)
                    .ThenBy(x => Guid.NewGuid()) // Random-Tiebreak
                    .First().Key;

                attributes[next] += 1;
                _accumulator[next] -= 1.0;
            }

            // Orbit-Punkte vergeben (MVP: 1 Punkt pro Level)
            GrantOrbitPointsForLevel();

            // Sockets ggf. freischalten (MVP: mindestens 1, Details später über Config)
            UpdateSocketUnlocksForLevel();

            string msg = $"Neue Attribute: STR={attributes[HeroAttribs.STR]} |" +
                         $"DEX={attributes[HeroAttribs.DEX]} | " +
                         $"CON={attributes[HeroAttribs.CON]} | " +
                         $"INT={attributes[HeroAttribs.INT]} | " +
                         $"WIL={attributes[HeroAttribs.WIL]}";

            DebugManager.Log(msg, DebugManager.EDebugLevel.Test, "Hero");
        }

        private void GrantOrbitPointsForLevel()
        {
            // MVP: 1 Orbitpunkt pro Level. Später: Konfiguration über ArchetypeGrowthConfig o.ä.
            //TODO: Config Orbitpunkte pro level, MVP = 2
            TotalOrbitPointsEarned+=2;
            UnspentOrbitPoints+=2;
        }

        private void UpdateSocketUnlocksForLevel()
        {
            // TODO: Socket-Unlock-Stufen aus einer Config ziehen.
            // MVP: Stelle nur sicher, dass immer mindestens 1 Socket aktiv ist.
        }

        #endregion

        #region Persistenz-Brücke


/// <summary>
/// Applies the progress data to the hero, initializing values if the data is null.
/// </summary>
/// <param name="progress">The hero's progress data to apply.</param>
        public void ApplyProgressData(HeroProgressData progress)
        {
            if (progress == null)
            {
                // Default: frischer Hero auf Level 1
                Level = 1;
                CurrentXP = 0;
                TotalXP = 0;
                TotalOrbitPointsEarned = 0;
                UnspentOrbitPoints = 0;
                UnlockedSockets = 0;
                return;
            }

            Level = Math.Max(1, progress.Level);
            CurrentXP = Math.Max(0, progress.CurrentXP);
            TotalXP = Math.Max(0, progress.TotalXP);
            TotalOrbitPointsEarned = Math.Max(0, progress.TotalOrbitPoints);
            UnspentOrbitPoints = Math.Max(0, progress.UnspentOrbitPoints);
            UnlockedSockets = Math.Max(1, progress.UnlockedSockets);

            // Attribut-Verteilung basierend auf Level neu aufbauen:
            // 1) Stats zurück auf Startwerte
            InitStats();

            // 2) Wachstumslogik für Level 2..Level n anwenden
            int targetLevel = Level;
            Level = 1;
            for (int i = 2; i <= targetLevel; i++)
            {
                ApplyLevelUp();
            }
        }

/// <summary>
/// Fills the progress data for the specified hero.
/// </summary>
/// <param name="target">The HeroProgressData object to fill.</param>
        public void FillProgressData(HeroProgressData target)
        {
            if (target == null) return;

            target.HeroId = heroDef != null ? heroDef.HeroId : target.HeroId;
            target.Level = Level;
            target.CurrentXP = CurrentXP;
            target.TotalXP = TotalXP;
            target.TotalOrbitPoints = TotalOrbitPointsEarned;
            target.UnspentOrbitPoints = UnspentOrbitPoints;
            target.UnlockedSockets = UnlockedSockets;
        }

/// <summary>
/// Gets the base damage value for the hero.
/// </summary>
/// <returns>The base damage as a float.</returns>
        public override float GetBaseDamage()
        {
            return heroDef.BaseDamage;
        }

        #endregion

    }
}
