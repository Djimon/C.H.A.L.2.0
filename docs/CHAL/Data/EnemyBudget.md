# CHAL.Data.EnemyBudget

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

Section 1 — Purpose
- Defines GameBalanceConfig as a ScriptableObject for game balance data (Unity asset).
- Encapsulates Loot, Waves, Enemies, Skills, and Economy configuration via nested serializable structs.
- Exposes public configuration fields (loot, enemies, skillRanges, AllowFriendlyFire, economy) for use at runtime/editor.

```

```text
Section 2 — Public API
- Namespace/module
  - CHAL.Data

- Types

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
    - mapping:
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
    - mapping:
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
  - [Range(1, 10)] public int levelCurveFactor

- public struct EconomySettings
  - public CurrencySettings currencies
  - public XpSettings xp

- public class GameBalanceConfig
  - public LootSettings loot
  - public EnemySettings enemies
  - public SkillRanges skillRanges
  - public bool AllowFriendlyFire
  - public EconomySettings economy
  - public float GetRangeValue(SkillRange range)
    - returns a float:
      - SkillRange.Self => skillRanges.selfRange
      - SkillRange.Melee => skillRanges.meleeRange
      - SkillRange.Reach => skillRanges.reachRange
      - SkillRange.MidDistance => skillRanges.midDistanceRange
      - SkillRange.FarDistance => skillRanges.farDistanceRange
      - _ => skillRanges.meleeRange

- Public methods (signatures exactly as in code)
  - public float GetRangeValue(SkillRange range)

```

```text
Section 3 — Key Behavior & Side Effects
- Data container: holds configuration values for loot, waves, enemies, skills, and economy.
- GetRangeValue(SkillRange) selects a range from skillRanges based on the provided SkillRange; does not mutate state.
- LootRankMultipliers.GetMultiplier(EnemyRank) maps rank to configured multipliers; unknown ranks default to 1.
- EnemyRankSettings.GetScaling(EnemyRank) maps rank to its RankScaling; unknown ranks default to normal
  (as coded).

```

```text
Section 4 — Constraints & Failure Modes
- Some fields are constrained by Unity attributes:
  - LootFloorSettings and LootTrimSettings values use [Range(0, 1)].
  - XpSettings.levelCurveFactor uses [Range(1, 10)].
- Null/empty handling: not explicit; behavior relies on asset data. No runtime initialization shown.
- External types referenced but not defined here:
  - EnemyRank, SkillRange
- Threading/async: none; this is a data asset with read access patterns only.
- Performance/allocation hints: uses simple value types and a List<string> (magicTagPool) for runtime string tags.

```

```text
Section 5 — Example
- Minimal usage snippet (assuming a loaded GameBalanceConfig asset exists):
```csharp
// Example: obtain a self-range value from the config
GameBalanceConfig config = /* reference to asset */;
float selfRange = config.GetRangeValue(SkillRange.Self);
```

```

```text
Section 6 — Unknowns
- Definitions and exact members of:
  - EnemyRank
  - SkillRange
- How GameBalanceConfig assets are created/loaded at runtime beyond the asset itself.
- The semantic meaning or intended default values for each field (values come from assets, not code).
- Any editor tooling behavior beyond CreateAssetMenu attributes.

