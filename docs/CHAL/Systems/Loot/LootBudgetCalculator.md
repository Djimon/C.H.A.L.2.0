# CHAL.Systems.Loot.LootBudgetCalculator

_Automatically generated/updated from `Assets/src/Systems/Loot/LootBudgetCalculator.cs`._

```text
1) Purpose
- Defines a static utility LootBudgetCalculator in CHAL.Systems.Loot.
- Exposes CalculateBudget(...) to compute an int loot budget for a wave using BalanceManager.Config.
- Applies level-based scaling, map-difficulty multiplier, and a random variance, returning a non-negative integer.

2) Public API
- Namespace/module
  - CHAL.Systems.Loot

- Types
  - public static class LootBudgetCalculator
    - Public methods
      - public static int CalculateBudget(
          int spawns=0,int normals=0, int magics=0, int elites=0, int bosses=0, int champions=0, int level=1, MapDifficulty difficulty=MapDifficulty.Stable)

        - Returns: non-negative budget integer for the given wave parameters.
        - Notes: Uses BalanceManager.Instance.Config for constants; applies raw budget, level scaling, map-difficulty multiplier, and variance.

3) Key Behavior & Side Effects
- Workflow of CalculateBudget:
  - Reads BalanceManager.Instance.Config as cfg.
  - Computes B_raw = spawns * cfg.enemies.budgetPoints.spawn
                   + normals * cfg.enemies.budgetPoints.normal
                   + magics * cfg.enemies.budgetPoints.magic
                   + elites * cfg.enemies.budgetPoints.elite
                   + bosses * cfg.enemies.budgetPoints.boss
                   + champions * cfg.enemies.budgetPoints.champion
  - Computes levelFactor = 1f + cfg.loot.budget.levelFactor * (level - 1)
  - Computes B_scaled = B_raw * levelFactor * getMultiplierFromMapDifficulty(difficulty)
  - Computes variance = Random.Range(-cfg.loot.budget.budgetVariance, cfg.loot.budget.budgetVariance)
  - Computes B_var = B_scaled * (1f + variance)
  - Returns Mathf.Max(0, Mathf.RoundToInt(B_var))
- Helper behavior:
  - private static float getMultiplierFromMapDifficulty(MapDifficulty difficulty)
    - Returns: 1f for Stable, 2f for Strained, 4f for Volatile, 10f for Chaos, default 1f

4) Constraints & Failure Modes
- Depends on BalanceManager.Instance.Config; no null checks in this file (NullReferenceException if BalanceManager or Config is not initialized).
- Uses UnityEngine.Random for variance; variance bounds are [-cfg.loot.budget.budgetVariance, cfg.loot.budget.budgetVariance].
- Final result is clamped to be non-negative via Mathf.Max(0, ...).
- Default parameter values:
  - spawns, normals, magics, elites, bosses, champions default to 0
  - level defaults to 1
  - difficulty defaults to MapDifficulty.Stable

5) Example
```csharp
// Example: calculate budget for a Volatile wave with some composition
int budget = LootBudgetCalculator.CalculateBudget(
    spawns: 10,
    normals: 20,
    level: 5,
    difficulty: MapDifficulty.Volatile);
```

6) Unknowns
- Definitions of BalanceManager, BalanceManager.Config, and the exact structure of cfg.enemies.budgetPoints and cfg.loot.budget are not present in this file.
- The MapDifficulty enum values beyond those used (Stable, Strained, Volatile, Chaos) are not defined here.
- Any side effects beyond budget calculation (e.g., triggering other systems) are not shown in this file.
