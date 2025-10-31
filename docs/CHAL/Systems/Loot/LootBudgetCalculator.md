# CHAL.Systems.Loot.LootBudgetCalculator

_Automatically generated/updated from `Assets/src/Systems/Loot/LootBudgetCalculator.cs`._

# Purpose
- Defines a static class for calculating loot budgets for enemy waves.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - `public static class LootBudgetCalculator`
    - Public methods
      - `public static int CalculateBudget(int spawns=0, int normals=0, int magics=0, int elites=0, int bosses=0, int champions=0, int level=1, MapDifficulty difficulty=MapDifficulty.Stable)`: Calculates the loot budget based on enemy types, level, and map difficulty; returns an integer budget value.
      - `private static float getMultiplierFromMapDifficulty(MapDifficulty difficulty)`: Returns a multiplier based on the map difficulty.

# Key Behavior & Side Effects
- Calculates a raw budget based on enemy counts and configuration values.
- Scales the budget based on the level and map difficulty.
- Applies a variance to the scaled budget using a random value.
- Ensures the final budget is non-negative by using `Mathf.Max`.

# Constraints & Failure Modes
- Uses configuration values from `BalanceManager.Instance.Config`.
- Assumes valid input for enemy counts and level.
- Random variance can lead to fluctuating budget values.

# Example
```csharp
int budget = LootBudgetCalculator.CalculateBudget(spawns: 5, normals: 3, level: 2, difficulty: MapDifficulty.Strained);
```

# Unknowns
- The structure and contents of `BalanceManager.Instance.Config` and its properties.
- The definition of `MapDifficulty` and its possible values.

