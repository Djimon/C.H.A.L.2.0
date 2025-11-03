# CHAL.Data.CurrencySettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

1) Purpose
- Defines a Unity ScriptableObject (GameBalanceConfig) to hold game balance data for loot, waves, enemies, skills, and economy.
- Provides a set of nested serializable structs to configure detailed balance aspects (budget, scaling, ranges, tag pools, etc.).
- Exposes a Unity editor CreateAssetMenu entry to create a GameBalanceConfig asset.

```

```text
2) Public API
- Namespace/module
  - CHAL.Data

- Types

  - public class GameBalanceConfig : ScriptableObject
    - Public fields
      - LootSettings loot
      - EnemySettings enemies
      - SkillRanges skillRanges
      - bool AllowFriendlyFire = false
      - EconomySettings economy
    - Public methods
      - float GetRangeValue(SkillRange range)

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
    - (private) int GetMultiplier(EnemyRank rank)

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
    - (private) RankScaling GetScaling(EnemyRank rank)

  - public struct EnemySettings
    - public EnemyBudget budgetPoints
    - public EnemyScaling scaling
    - public RankScaling rankScaling
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
- GetRangeValue(SkillRange range): returns the configured numeric value for the specified SkillRange, mapping:
  - SkillRange.Self -> skillRanges.selfRange
  - SkillRange.Melee -> skillRanges.meleeRange
  - SkillRange.Reach -> skillRanges.reachRange
  - SkillRange.MidDistance -> skillRanges.midDistanceRange
  - SkillRange.FarDistance -> skillRanges.farDistanceRange
  - default -> skillRanges.meleeRange
- CreateAssetMenu attribute enables editor asset creation:
  - fileName = "GameBalanceConfig"
  - menuName = "Config/GameBalanceConfig"
- No runtime logic beyond data storage; nested GetMultiplier/GetScaling are private and not part of the public API.

```

```text
4) Constraints & Failure Modes
- Editor-imposed value ranges:
  - LootFloorSettings and LootTrimSettings fields use [Range(0, 1)] for percent-like values.
  - XpSettings.levelCurveFactor uses [Range(1,10)].
- Nullability considerations:
  - Public List<string> magicTagPool may be null if not initialized.
- No explicit threading/async behavior; this is a data container.
- No runtime validation beyond editor attributes; runtime errors may arise if assets are misconfigured or fields are left null.

```

```text
5) Example
- Access a range value from a loaded balance config asset:
```csharp
// Assuming you have a reference to a loaded GameBalanceConfig asset
GameBalanceConfig config = /* load asset from resources/asset bundle or assignment */;
float selfRange = config.GetRangeValue(SkillRange.Self);
// use selfRange as needed
```

```

```text
6) Unknowns
- Definitions and enum values for:
  - EnemyRank (used by several structs)
  - SkillRange (used by GetRangeValue and SkillRanges)
- Default initialization values for all fields in this file (Unity can serialize defaults, but code does not specify defaults).
- Any runtime validation or semantics beyond data layout (e.g., how these values interact in gameplay).
- How this asset is created/assigned in the project workflow (beyond the CreateAssetMenu attribute).

