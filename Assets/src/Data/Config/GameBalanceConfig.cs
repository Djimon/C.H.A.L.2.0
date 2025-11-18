using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{

    [CreateAssetMenu(fileName = "GameBalanceConfig", menuName = "Config/GameBalanceConfig")]
/// <summary>
/// Holds configuration settings for game balance, including loot parameters.
/// </summary>
    public class GameBalanceConfig : ScriptableObject
    {
        // ==========================
        // LOOT
        // ==========================
        [System.Serializable]
        public struct LootBudgetSettings
        {
            public float levelFactor;      // z. B. 0.08
            public float budgetVariance;   // z. B. 0.2 (20 %)
            public float beta;             // Dämpfungsstärke Overflow
        }

        [System.Serializable]
        public struct LootFloorSettings
        {
            [Range(0, 1)] public float rare;       // z. B. 0.3
            [Range(0, 1)] public float epic;       // z. B. 0.2
            [Range(0, 1)] public float legendary;  // z. B. 0.1
            [Range(0, 1)] public float specials;  // z. B. 0
        }

        [System.Serializable]
        public struct LootUnluckySettings
        {
            public float alphaRare;       // z. B. 0.20
            public float alphaEpic;       // z. B. 0.15
            public float alphaLegendary;  // z. B. 0.10
            public float alphaSpecials; //Mythi,Holy,Daemonic, etc. //0
        }

        [System.Serializable]
        public struct LootTrimSettings
        {
            [Range(0, 1)] public float common;     // Wahrscheinlichkeit, dass Common entfernt wird
            [Range(0, 1)] public float uncommon;   // "
            [Range(0, 1)] public float rare;       // "
            [Range(0, 1)] public float epic;       // "
            [Range(0, 1)] public float legendary;  // "
        }

        [System.Serializable]
        public struct LootRankMultipliers
        {
            public int spawn;
            public int normal;
            public int magic;
            public int elite;
            public int boss;
            public int champion;
/// <summary>
/// Gets the multiplier based on the specified enemy rank.
/// </summary>
/// <param name="rank">The rank of the enemy.</param>
/// <returns>The multiplier associated with the enemy rank.</returns>
            public int GetMultiplier(EnemyRank rank)
            {
                return rank switch
                {
                    EnemyRank.Spawn => spawn,
                    EnemyRank.Normal => normal,
                    EnemyRank.Magic => magic,
                    EnemyRank.Elite => elite,
                    EnemyRank.Boss => boss,
                    EnemyRank.Champion => champion,
                    _ => 1
                };
            }
        }

        [System.Serializable]
        public struct LootSettings
        {
            public LootBudgetSettings budget;
            public LootFloorSettings floors;
            public LootUnluckySettings unlucky;
            public LootTrimSettings trim;
            public LootRankMultipliers rankMultipliers;
        }


        [Header("Loot Settings")]
        public LootSettings loot;

        // ==========================
        // Waves
        // ==========================
        [System.Serializable]
        public struct EnemyBudget
        {
            public int spawn;   // z. B. 5
            public int normal;  // z. B. 10
            public int magic;   // 20
            public int elite;   // 30
            public int boss;    // z. B. 50
            public int champion; // 100
        }

        [System.Serializable]
        public struct EnemyScaling
        {
            public float hpPerLevel;     // z. B. 0.1 = +10 % HP pro Level
            public float dmgPerLevel;    // z. B. 0.08 = +8 % DMG pro Level
        }

        [System.Serializable]
        public struct WaveSettings
        {
            public EnemyBudget budgetPoints;
            public EnemyScaling scaling;
        }


        // ==========================
        // ENEMIES
        // ==========================

        [System.Serializable]
        public struct RankScaling
        {
            public float hpMultiplier;
            public float dmgMultiplier;
            public float xpMultiplier;
        }

        [System.Serializable]
        public struct EnemyRankSettings
        {
            public RankScaling spawn;
            public RankScaling normal;
            public RankScaling magic;
            public RankScaling elite;
            public RankScaling boss;
            public RankScaling champion;

/// <summary>
/// Gets the scaling associated with the specified enemy rank.
/// </summary>
/// <param name="rank">The rank of the enemy.</param>
/// <returns>The corresponding RankScaling for the given enemy rank.</returns>
            public RankScaling GetScaling(EnemyRank rank)
            {
                return rank switch
                {
                    EnemyRank.Spawn => spawn,
                    EnemyRank.Normal => normal,
                    EnemyRank.Magic => magic,
                    EnemyRank.Elite => elite,
                    EnemyRank.Boss => boss,
                    EnemyRank.Champion => champion,
                    _ => normal
                };
            }
        }

        [System.Serializable]
        public struct EnemySettings
        {
            public EnemyBudget budgetPoints;
            public EnemyScaling scaling;

            [Header("Rank Scaling")]
            public EnemyRankSettings rankScaling;

            [Header("Magic Tag Pool")]
            public List<string> magicTagPool;   // Globale Magic-Tags (z. B. "caster", "spirit", "mage")
            public int minEliteTags;
        }

        [Header("Enemy Settings")]
        public EnemySettings enemies;

        // ==========================
        // SKILLS
        // ==========================
        [System.Serializable]
        public struct SkillRanges
        {
            public float selfRange;
            public float meleeRange;
            public float reachRange;
            public float midDistanceRange;
            public float farDistanceRange;
        }

        [Header("Skill Settings")]
        public SkillRanges skillRanges;
        public bool AllowFriendlyFire = false;

/// <summary>
/// Gets the range value based on the specified skill range.
/// </summary>
/// <param name="range">The skill range to evaluate.</param>
/// <returns>The corresponding range value as a float.</returns>
        public float GetRangeValue(SkillRange range)
        {
            return range switch
            {
                SkillRange.Self => skillRanges.selfRange,
                SkillRange.Melee => skillRanges.meleeRange,
                SkillRange.Reach => skillRanges.reachRange,
                SkillRange.MidDistance => skillRanges.midDistanceRange,
                SkillRange.FarDistance => skillRanges.farDistanceRange,
                _ => skillRanges.meleeRange
            };
        }


        // ==========================
        // ECONOMY
        // ==========================
        [System.Serializable]
        public struct CurrencySettings
        {
            public int baseGoldReward;    // z. B. 100 pro Kampf
            public float goldPerLevel;    // z. B. +20 % Gold je Level

        }

        [System.Serializable]
        public struct XpSettings
        {
            public int baseXpReward;      // z. B. 50 pro Kampf
            public float xpPerLevel;      // z. B. +15 % XP je Level
            public int baseLevelUpXp;
            [Range(1,10)]
            public int levelCurveFactor;
        }

        [System.Serializable]
        public struct EconomySettings
        {
            public CurrencySettings currencies;
            public XpSettings xp;
        }

        [Header("Economy Settings")]
        public EconomySettings economy;

        // ==========================
        // HERO PROGRESSION
        // ==========================
        [Header("Hero Progression")]
        public HeroXPConfig heroXP;   // zentrale Config für Helden-XP/Levelkurve

    }
}
