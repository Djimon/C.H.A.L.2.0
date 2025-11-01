# CHAL.Data.LootTrimSettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

```text
1) Purpose
- Defines CHAL.Data.GameBalanceConfig as a Unity ScriptableObject asset for game balance configuration.
- Groups configuration for Loot, Waves/Enemies, Skills, and Economy into serializable structs.
- Exposes public fields for inspector-based tuning and runtime retrieval helpers (GetRangeValue, multipliers, scalings).
```

```text
2) Public API
- Namespace/module
  - CHAL.Data

- Types
  - public class GameBalanceConfig : ScriptableObject
    - public LootSettings loot
    - public EnemySettings enemies
    - public SkillRanges skillRanges
    - public bool AllowFriendlyFire
    - public EconomySettings economy
    - public float GetRangeValue(SkillRange range)
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
      - returns corresponding multiplier based on rank or 1 for default
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
```

```text
3) Key Behavior & Side Effects
- GetRangeValue(SkillRange range)
  - Returns the corresponding value from skillRanges based on the provided range:
    - Self -> skillRanges.selfRange
    - Melee -> skillRanges.meleeRange
    - Reach -> skillRanges.reachRange
    - MidDistance -> skillRanges.midDistanceRange
    - FarDistance -> skillRanges.farDistanceRange
  - Falls back to skillRanges.meleeRange for unknown ranges.
- LootRankMultipliers.GetMultiplier(EnemyRank rank)
  - Returns the configured multiplier for the given rank:
    - Spawn/Normal/Magic/Elite/Boss/Champion map to the respective fields
    - Unknown ranks default to 1
- EnemyRankSettings.GetScaling(EnemyRank rank)
  - Returns the RankScaling configured for the given rank:
    - Spawn/Normal/Magic/Elite/Boss/Champion map to the respective fields
    - Unknown ranks fallback to normal scaling
- Asset creation
  - This class is a ScriptableObject with CreateAssetMenu, enabling creation of a GameBalanceConfig asset via Unity UI.
```

```text
4) Constraints & Failure Modes
- Inspector constraints
  - LootFloorSettings.rare/epic/legendary/specials and LootTrimSettings fields are annotated with [Range(0,1)].
  - XpSettings.levelCurveFactor is annotated with [Range(1,10)].
- Potential nulls
  - List<string> magicTagPool in EnemySettings may be null if not initialized in the asset.
- External dependencies
  - EnemyRank, SkillRange enums are referenced but definitions are not present in this file.
  - The exact usage semantics of these values depend on other parts of the project.
```

```text
5) Example
- Minimal usage example (C#)
```csharp
using CHAL.Data;
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    public GameBalanceConfig config;

    void Start()
    {
        // Retrieve a range value for the "Self" range
        float self = config.GetRangeValue(SkillRange.Self);
        Debug.Log("Self range: " + self);
    }
}
```
```

```text
6) Unknowns
- Definitions and members of EnemyRank, SkillRange, and EnemyRank/SkillRange enums are not included in this file.
- How values in this config are consumed at runtime (specific gameplay logic) is not defined here.
- Any additional behavior or validation performed outside Unity’s inspector is not present in this file.
```
