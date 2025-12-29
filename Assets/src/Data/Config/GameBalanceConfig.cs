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
            public EnemyScaling scalingIncrfeasedPercent;
        }

        public WaveSettings waves;


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
        // GEAR
        // ==========================
        [System.Serializable]
        public struct GearTierSlotCaps
        {
            [Min(0)] public int maxImplicits;
            [Min(0)] public int maxAffixes;
        }

        [System.Serializable]
        public struct GearSlotCapsByTier
        {
            public GearTierSlotCaps tier1;
            public GearTierSlotCaps tier2;
/// <summary>
/// Represents the capabilities of the third gear tier slot.
/// </summary>
            public GearTierSlotCaps tier3;

/// <summary>
/// Gets the slot capabilities for the specified gear tier.
/// </summary>
/// <param name="tier">The gear base tier to get capabilities for.</param>
/// <returns>The gear tier slot capabilities.</returns>
/// <summary>
/// Gets the slot capabilities for the specified gear tier.
/// </summary>
/// <param name="tier">The gear base tier to get capabilities for.</param>
/// <returns>The gear tier slot capabilities.</returns>
            public GearTierSlotCaps GetCaps(GearBaseTier tier)
            {
                return tier switch
                {
                    GearBaseTier.T1 => tier1,
                    GearBaseTier.T2 => tier2,
                    GearBaseTier.T3 => tier3,
                    _ => tier1
                };
            }

            public int GetMaxImplicits(GearBaseTier tier) => GetCaps(tier).maxImplicits;
            public int GetMaxAffixes(GearBaseTier tier) => GetCaps(tier).maxAffixes;
        }

        [System.Serializable]
        public struct GearRoleWeights
        {
            public int defense;
            public int offense;
            public int utility;
        }

        [System.Serializable]
        public struct GearTypeRoleWeights
        {
            public GearType gearType;
            public GearRoleWeights signature; // Slot1
            public GearRoleWeights normal;    // Slot2/3
        }

        [System.Serializable]
/// <summary>
/// Represents a collection of weights for different slot pools.
/// </summary>
        public struct SlotPoolWeights
        {
            [Range(0f, 1f)] public float main;
            [Range(0f, 1f)] public float neutral;
            [Range(0f, 1f)] public float pool2;
            [Range(0f, 1f)] public float pool3;

            public void Normalize()
            {
                var sum = main + neutral + pool2 + pool3;
                if (sum <= 0.0001f) { main = 1f; neutral = pool2 = pool3 = 0f; return; }
                main /= sum; neutral /= sum; pool2 /= sum; pool3 /= sum;
            }
        }

        [System.Serializable]
/// <summary>
/// Represents a collection of weights for different affix families.
/// </summary>
        public struct AffixFamilyWeights
        {
            [Range(0f, 1f)] public float core;
            [Range(0f, 1f)] public float defensive;
            [Range(0f, 1f)] public float synergy;
            [Range(0f, 1f)] public float utility;

/// <summary>
/// Normalizes the values of core, defensive, synergy, and utility.
/// </summary>
            public void Normalize()
            {
                var sum = core + defensive + synergy + utility;
                if (sum <= 0.0001f) { core = 1f; defensive = synergy = utility = 0f; return; }
                core /= sum; defensive /= sum; synergy /= sum; utility /= sum;
            }

            public float Get(AffixFamily family)
            {
                return family switch
                {
                    AffixFamily.Core => core,
                    AffixFamily.Defensive => defensive,
                    AffixFamily.Synergy => synergy,
                    AffixFamily.Utility => utility,
                    _ => core
                };
            }
        }

        [System.Serializable]
        public struct GearTypeAffixFamilyWeights
        {
            public GearType gearType;
            public AffixFamilyWeights weights;
        }

        [System.Serializable]
        public struct AffixCategoryCap
        {
            public AffixCategory category;

            [Min(0)]
            [Tooltip("Max number of affixes with this Category allowed on a single item. 0 = not allowed.")]
            public int maxOnItem;
        }

        [System.Serializable]
        public struct GearAffixCategoryCaps
        {
            [Tooltip("Global caps (applies to all gear types unless overridden).")]
/// <summary>
/// Holds the global caps for affix categories.
/// </summary>
            public List<AffixCategoryCap> globalCaps;

            //[Tooltip("Optional per-gear-type overrides. If a category is present here, it overrides the global cap for that gear type.")]
            //public List<GearTypeAffixCategoryCaps> overridesByGearType;

            public int GetCap(GearType gearType, AffixCategory category, int fallbackIfMissing = 99)
            {
                // 1) overrides
                //if (overridesByGearType != null)
                //{
                //    for (int i = 0; i < overridesByGearType.Count; i++)
                //    {
                //        var o = overridesByGearType[i];
                //        if (o.gearType != gearType) continue;

                //        var cap = o.GetCap(category, int.MinValue);
                //        if (cap != int.MinValue) return cap;
                //        break;
                //    }
                //}

                // 2) global
                if (globalCaps != null)
                {
                    for (int i = 0; i < globalCaps.Count; i++)
                        if (globalCaps[i].category == category)
                            return globalCaps[i].maxOnItem;
                }

                return fallbackIfMissing;
            }
        }

        [System.Serializable]
        public struct GearTypeAffixCategoryCaps
        {
            public GearType gearType;
/// <summary>
/// A list of affix category caps.
/// </summary>
            public List<AffixCategoryCap> caps;

            public int GetCap(AffixCategory category, int fallbackIfMissing)
            {
                if (caps == null) return fallbackIfMissing;
                for (int i = 0; i < caps.Count; i++)
                    if (caps[i].category == category)
                        return caps[i].maxOnItem;
                return fallbackIfMissing;
            }
        }

        [System.Serializable]
        public struct GearAffixRules
        {
            [Header("Duplicate Rules")]
            [Tooltip("If false, the same AffixId cannot appear twice on one item.")]
            public bool allowDuplicateAffixIdPerItem;

            [Header("Category Caps")]
            public GearAffixCategoryCaps categoryCaps;
        }

        [System.Serializable]
        public struct GearSettings
        {
            [Header("Slot Caps per Base Tier")]
            public GearSlotCapsByTier slotCapsByTier;

            [Header("Implicit Roll Settings")]
            public SlotPoolWeights slot1PoolWeights;
            public SlotPoolWeights slot2PoolWeights;
            public SlotPoolWeights slot3PoolWeights;
            public List<GearTypeRoleWeights> roleWeightsByGearType;

            [Header("Affix Roll Settings")]
            [Tooltip("Used for non-family-selected rolls (e.g. drops). If the player explicitly chooses a family in crafting, this is ignored.")]
            public AffixFamilyWeights defaultAffixFamilyWeights;

            [Tooltip("Optional gear-type-specific affix family weights.")]
            public List<GearTypeAffixFamilyWeights> affixFamilyWeightsByGearType;

            [Header("Affix Rules")]
/// <summary>
/// Holds the rules for gear affixes.
/// </summary>
            public GearAffixRules affixRules;

            public float GetAffixFamilyWeight(GearType gearType, AffixFamily family)
            {
                if (affixFamilyWeightsByGearType != null)
                {
                    for (int i = 0; i < affixFamilyWeightsByGearType.Count; i++)
                    {
                        var e = affixFamilyWeightsByGearType[i];
                        if (e.gearType == gearType)
                            return e.weights.Get(family);
                    }
                }
                return defaultAffixFamilyWeights.Get(family);
            }
        }

        [Header("Gear Settings")]
        public GearSettings gear;




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
                SkillRange.MeleeRange => skillRanges.meleeRange,
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
