# CHAL.Data.EnemyScaling

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

Purpose
- Defines GameBalanceConfig as a Unity ScriptableObject for game balance data.
- Encapsulates nested serializable structs for Loot, Waves, Enemies, Skills, and Economy.
- Exposes public fields for configuring game balance in the Unity Inspector.

Public API
- Namespace: CHAL.Data
- Types
  - public class GameBalanceConfig : ScriptableObject
    - Attributes: [CreateAssetMenu(fileName = "GameBalanceConfig", menuName = "Config/GameBalanceConfig")]
    - Public fields
      - public LootSettings loot
      - public EnemySettings enemies
      - public SkillRanges skillRanges
      - public bool AllowFriendlyFire
      - public float GetRangeValue(SkillRange range)
          - Returns the configured range value from skillRanges based on the provided SkillRange
    - Notes
      - Uses [Header("Loot Settings")] to group loot config in inspector

  - LootBudgetSettings
    - public float levelFactor
    - public float budgetVariance
    - public float beta

  - LootFloorSettings
    - [Range(0, 1)] public float rare
    - [Range(0, 1)] public float epic
    - [Range(0, 1)] public float legendary
    - [Range(0, 1)] public float specials

  - LootUnluckySettings
    - public float alphaRare
    - public float alphaEpic
    - public float alphaLegendary
    - public float alphaSpecials

  - LootTrimSettings
    - [Range(0, 1)] public float common
    - [Range(0, 1)] public float uncommon
    - [Range(0, 1)] public float rare
    - [Range(0, 1)] public float epic
    - [Range(0, 1)] public float legendary

  - LootRankMultipliers
    - public int spawn
    - public int normal
    - public int magic
    - public int elite
    - public int boss
    - public int champion
    - public int GetMultiplier(EnemyRank rank)
        - Returns corresponding multiplier for rank, or 1 if not matched

  - LootSettings
    - public LootBudgetSettings budget
    - public LootFloorSettings floors
    - public LootUnluckySettings unlucky
    - public LootTrimSettings trim
    - public LootRankMultipliers rankMultipliers

  - EnemyBudget
    - public int spawn
    - public int normal
    - public int magic
    - public int elite
    - public int boss
    - public int champion

  - EnemyScaling
    - public float hpPerLevel
    - public float dmgPerLevel

  - WaveSettings
    - public EnemyBudget budgetPoints
    - public EnemyScaling scaling

  - RankScaling
    - public float hpMultiplier
    - public float dmgMultiplier
    - public float xpMultiplier

  - EnemyRankSettings
    - public RankScaling spawn
    - public RankScaling normal
    - public RankScaling magic
    - public RankScaling elite
    - public RankScaling boss
    - public RankScaling champion
    - public RankScaling GetScaling(EnemyRank rank)
        - Returns scaling for the given rank

  - EnemySettings
    - public EnemyBudget budgetPoints
    - public EnemyScaling scaling
    - public EnemyRankSettings rankScaling
    - public List<string> magicTagPool
    - public int minEliteTags

  - SkillRanges
    - public float selfRange
    - public float meleeRange
    - public float reachRange
    - public float midDistanceRange
    - public float farDistanceRange

  - SkillRanges usage
    - public float GetRangeValue(SkillRange range)
        - Returns the configured value for the given SkillRange
        - Maps: Self, Melee, Reach, MidDistance, FarDistance

  - EconomySettings
    - public CurrencySettings currencies
    - public XpSettings xp

  - CurrencySettings
    - public int baseGoldReward
    - public float goldPerLevel

  - XpSettings
    - public int baseXpReward
    - public float xpPerLevel
    - public int baseLevelUpXp
    - [Range(1,10)] public int levelCurveFactor

Notes
- EnemyRank type is referenced but defined elsewhere; GetScaling uses it in a switch.
- List fields (e.g., magicTagPool) rely on Unity serialization; may be null if not initialized by the inspector.
- Range attributes restrict values in the editor for the corresponding fields.

Key Behavior & Side Effects
- Data-only configuration container; no runtime logic beyond accessors.
- GetMultiplier, GetScaling, and GetRangeValue are read-only helpers returning values from the configured data.
- No state changes occur within this file; it simply defines structure and defaults via serialization.

Constraints & Failure Modes
- Inspector-enforced ranges via [Range(0, 1)] for certain fields.
- Potential null List<string> magicTagPool if not initialized in the inspector.
- Unknowns depend on external definitions (e.g., EnemyRank, SkillRange, and overall usage in game systems).
- Default behaviors rely on inspector-provided values; no hardcoded defaults defined here.

Unknowns
- Exact definitions and values of EnemyRank and SkillRange enums beyond their usage here.
- How these settings are consumed by gameplay systems (balance interpretation, asset references).
- Any runtime validation beyond what Unity serialization provides.
