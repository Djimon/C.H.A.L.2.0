# CHAL.Data.SkillRanges

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

1) Purpose
- Define a ScriptableObject-based game balance configuration (GameBalanceConfig) with structured, serializable settings for loot, waves, enemies, skills, and economy.
- Provide public, nested data structures to configure budgets, scaling, ranges, multipliers, and tag pools used by gameplay systems.
- Offer small runtime helpers for mapping enums to config values (range lookup, rank-based scaling, loot multipliers).

```

```text
2) Public API
- Namespace/module
  - CHAL.Data

- Types
  - public class GameBalanceConfig : ScriptableObject
    - Public fields
      - [Header("Loot Settings")] public LootSettings loot
      - [Header("Enemy Settings")] public EnemySettings enemies
      - [Header("Skill Settings")] public SkillRanges skillRanges
      - public bool AllowFriendlyFire = false
      - public EconomySettings economy

    - Public methods
      - public float GetRangeValue(SkillRange range)
        - Returns corresponding value from skillRanges:
          - SkillRange.Self => skillRanges.selfRange
          - SkillRange.Melee => skillRanges.meleeRange
          - SkillRange.Reach => skillRanges.reachRange
          - SkillRange.MidDistance => skillRanges.midDistanceRange
          - SkillRange.FarDistance => skillRanges.farDistanceRange
          - default => skillRanges.meleeRange

  - public struct LootBudgetSettings
    - public float levelFactor
    - public float budgetVariance
    - public float beta

  - public struct LootFloorSettings
    - [Range(0, 1)] public float rare
    - [Range(0, 1)] public float epic
    - [Range(0, 1)] public float legendary
    - [Range(0, 1)] public float specials

  - public struct LootUnluckySettings
    - public float alphaRare
    - public float alphaEpic
    - public float alphaLegendary
    - public float alphaSpecials

  - public struct LootTrimSettings
    - [Range(0, 1)] public float common
    - [Range(0, 1)] public float uncommon
    - [Range(0, 1)] public float rare
    - [Range(0, 1)] public float epic
    - [Range(0, 1)] public float legendary

  - public struct LootRankMultipliers
    - public int spawn
    - public int normal
    - public int magic
    - public int elite
    - public int boss
    - public int champion
    - public int GetMultiplier(EnemyRank rank)
      - switch (rank)
        - EnemyRank.Spawn => spawn
        - EnemyRank.Normal => normal
        - EnemyRank.Magic => magic
        - EnemyRank.Elite => elite
        - EnemyRank.Boss => boss
        - EnemyRank.Champion => champion
        - _ => 1

  - public struct LootSettings
    - public LootBudgetSettings budget
    - public LootFloorSettings floors
    - public LootUnluckySettings unlucky
    - public LootTrimSettings trim
    - public LootRankMultipliers rankMultipliers

  - public struct EnemyBudget
    - public int spawn
    - public int normal
    - public int magic
    - public int elite
    - public int boss
    - public int champion

  - public struct EnemyScaling
    - public float hpPerLevel
    - public float dmgPerLevel

  - public struct WaveSettings
    - public EnemyBudget budgetPoints
    - public EnemyScaling scaling

  - public struct RankScaling
    - public float hpMultiplier
    - public float dmgMultiplier
    - public float xpMultiplier

  - public struct EnemyRankSettings
    - public RankScaling spawn
    - public RankScaling normal
    - public RankScaling magic
    - public RankScaling elite
    - public RankScaling boss
    - public RankScaling champion
    - public RankScaling GetScaling(EnemyRank rank)
      - switch (rank)
        - EnemyRank.Spawn => spawn
        - EnemyRank.Normal => normal
        - EnemyRank.Magic => magic
        - EnemyRank.Elite => elite
        - EnemyRank.Boss => boss
        - EnemyRank.Champion => champion
        - _ => normal

  - public struct EnemySettings
    - public EnemyBudget budgetPoints
    - public EnemyScaling scaling
    - public EnemyRankSettings rankScaling
    - public List<string> magicTagPool
    - public int minEliteTags

  - public struct SkillRanges
    - public float selfRange
    - public float meleeRange
    - public float reachRange
    - public float midDistanceRange
    - public float farDistanceRange

  - public struct CurrencySettings
    - public int baseGoldReward
    - public float goldPerLevel

  - public struct XpSettings
    - public int baseXpReward
    - public float xpPerLevel
    - public int baseLevelUpXp
    - [Range(1,10)] public int levelCurveFactor

  - public struct EconomySettings
    - public CurrencySettings currencies
    - public XpSettings xp

```

```text
3) Key Behavior & Side Effects
- GetRangeValue(SkillRange): maps a SkillRange enum to a concrete numeric range from skillRanges; returns a default of meleeRange if the range is unrecognized.
- LootRankMultipliers.GetMultiplier(EnemyRank): maps an EnemyRank to the configured spawn/normal/magic/elite/boss/champion multipliers; falls back to 1 if rank is unrecognized.
- EnemyRankSettings.GetScaling(EnemyRank): returns the RankScaling corresponding to the given rank; defaults to normal if unrecognized.
- Asset-based configuration: This class is a ScriptableObject intended to be created via Unity's CreateAssetMenu and serialized in the editor.
- Editor-only annotations: [Header], [Range], and [System.Serializable] influence inspector presentation and validation.

```

```text
4) Constraints & Failure Modes
- Nullability: List<string> magicTagPool may be null if not assigned; methods do not guard against null lists.
- Editor-only constraints: [Range] attributes affect editor UI but are not runtime guarantees.
- Default fallbacks: GetRangeValue, GetMultiplier, and GetScaling provide default branches if inputs are not matched.
- No runtime initialization: All fields rely on serialization; no constructors initialize nested structs automatically.
- Enum surface: Requires EnemyRank and SkillRange definitions elsewhere; not defined in this file.

```

```text
5) Example
- Minimal usage (Unity C#):
```csharp
// Load the config asset (assumes a GameBalanceConfig asset named "GameBalanceConfig" exists)
var cfg = Resources.Load<GameBalanceConfig>("GameBalanceConfig");

// Query a range value for Melee
float meleeRange = cfg.GetRangeValue(SkillRange.Melee);

// Get the XP multiplier for Boss rank
float bossXpMult = cfg.enemies.rankScaling.GetScaling(EnemyRank.Boss).xpMultiplier;

// Access base gold reward
int baseGold = cfg.economy.currencies.baseGoldReward;
```

```

```text
6) Unknowns
- Definitions of EnemyRank and SkillRange (enumerations) are not in this file.
- Exact runtime usage of these settings in gameplay systems (e.g., how budgets and scaling translate to waves or loot drops) is not specified here.
- Semantics of some fields (e.g., LootBudgetSettings.beta) are not defined beyond simple comments in the code.
- Default asset values and how missing assets are handled at runtime are not described.

