# CHAL.Data.EnemySettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

1) Purpose
- Defines GameBalanceConfig as a Unity ScriptableObject for configuring game balance.
- Declares a set of serializable nested data structures (loot, waves, enemies, skills, economy) to be edited in the Unity Inspector.
- Exposes public fields to configure loot, enemies, skill ranges, economy, and a few helper methods for range and multiplier lookups.

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
    - public int GetMultiplier(EnemyRank rank)
      - Returns corresponding multiplier for the given rank via a switch:
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
      - Returns corresponding RankScaling for the given rank via a switch:
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

- Public fields (in GameBalanceConfig)
  - [Header("Loot Settings")] public LootSettings loot
  - [Header("Waves")] (no direct field at this header level in this file; shown here for grouping context)
  - [Header("Enemy Settings")] public EnemySettings enemies
  - [Header("Skill Settings")] public SkillRanges skillRanges
  - public bool AllowFriendlyFire = false
  - [Header("Economy Settings")] public EconomySettings economy

Note: EnemyRank, SkillRange types are referenced but defined elsewhere.

3) Key Behavior & Side Effects
- GetRangeValue(SkillRange range) is a pure accessor:
  - Returns the corresponding value from skillRanges based on the provided SkillRange.
  - No state mutations.
- LootRankMultipliers.GetMultiplier(EnemyRank rank)
  - Maps EnemyRank to the configured rank multiplier.
  - Returns a numeric multiplier or 1 as default when rank is unrecognized.
- GetScaling(EnemyRank rank) in EnemyRankSettings
  - Returns the RankScaling for the given rank; default to normal if unrecognized.
- Asset-based configuration: this file is designed to be used as a Unity ScriptableObject asset (CreateAssetMenu attribute), not as runtime logic.

4) Constraints & Failure Modes
- Range attributes constrain values for inspector editing:
  - [Range(0, 1)] on several float fields (e.g., rare, epic, etc.).
  - [Range(1,10)] on levelCurveFactor.
- No runtime validation provided in this file; invalid values can exist if edited outside the Unity Inspector.
- Some fields rely on external enums (EnemyRank, SkillRange) defined elsewhere; behavior depends on those enum definitions.

5) Example
- Minimal usage example:
```csharp
using CHAL.Data;

public class ExampleUsage
{
    public void Demo(GameBalanceConfig config, SkillRange range)
    {
        // Get a range value from the configured skill ranges
        float value = config.GetRangeValue(range);

        // Get a multiplier for a specific enemy rank
        int mult = config.loot.rankMultipliers.GetMultiplier(EnemyRank.Normal);
    }
}
```

6) Unknowns
- Definitions and exact values of EnemyRank and SkillRange enums (and their full set of members).
- How this asset is created/loaded at runtime beyond the CreateAssetMenu attribute.
- Any runtime logic that consumes these config values outside this file.
- Validation or overrides that may occur when assets are edited in the Unity Editor (beyond Range attributes).
- Any additional behavior tied to how “waves” or “economy” interact during gameplay beyond the provided data structures.

