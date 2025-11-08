# CHAL.Data.EconomySettings

_Automatically generated/updated from `Assets/src/Data/Config/GameBalanceConfig.cs`._

# Purpose
- Defines a configuration asset for game balance, including loot parameters, enemy settings, skill ranges, and economy settings.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class** `GameBalanceConfig` [extends `ScriptableObject`]
    - **public LootSettings** `loot` - Configuration for loot parameters.
    - **public EnemySettings** `enemies` - Configuration for enemy parameters.
    - **public SkillRanges** `skillRanges` - Configuration for skill range settings.
    - **public bool** `AllowFriendlyFire` - Indicates if friendly fire is allowed.
    - **public EconomySettings** `economy` - Configuration for economic parameters.
    - **public float** `GetRangeValue(SkillRange range)` - Returns the range value based on the specified skill range.
    - **public int** `GetMultiplier(EnemyRank rank)` - Returns the multiplier based on the specified enemy rank.
    - **public RankScaling** `GetScaling(EnemyRank rank)` - Returns the scaling associated with the specified enemy rank.

# Key Behavior & Side Effects
- The `GetRangeValue` method retrieves the corresponding range value based on the provided `SkillRange`.
- The `GetMultiplier` method retrieves the loot multiplier based on the provided `EnemyRank`.
- The `GetScaling` method retrieves the rank scaling based on the provided `EnemyRank`.

# Constraints & Failure Modes
- The `GetRangeValue`, `GetMultiplier`, and `GetScaling` methods default to specific values if an unrecognized enum value is provided.
- The `LootFloorSettings`, `LootTrimSettings`, and `XpSettings` use `[Range(0, 1)]` or `[Range(1,10)]` attributes to enforce value constraints in the Unity editor.

# Example
```csharp
GameBalanceConfig config = ScriptableObject.CreateInstance<GameBalanceConfig>();
float meleeRange = config.GetRangeValue(SkillRange.Melee);
int multiplier = config.loot.rankMultipliers.GetMultiplier(EnemyRank.Normal);
```

# Unknowns
- The definitions and values of `EnemyRank` and `SkillRange` enums are not provided in this file.

