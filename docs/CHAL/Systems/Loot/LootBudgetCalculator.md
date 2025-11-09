# Assets/src/Systems/Loot/LootBudgetCalculator.cs

_Automatically generated/updated from `Assets/src/Systems/Loot/LootBudgetCalculator.cs`._

# Purpose
- Defines a static class `LootBudgetCalculator` for calculating loot budgets based on various parameters.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - **public static class** `LootBudgetCalculator`
    - **public static int** `CalculateBudget(int spawns=0, int normals=0, int magics=0, int elites=0, int bosses=0, int champions=0, int level=1, MapDifficulty difficulty=MapDifficulty.Stable)`
      - Calculates the loot budget based on enemy types, level, and map difficulty.
      - Returns the calculated budget as a non-negative integer.
    - **private static float** `getMultiplierFromMapDifficulty(MapDifficulty difficulty)`
      - Returns a multiplier based on the provided map difficulty.

# Key Behavior & Side Effects
- Calculates a raw budget based on enemy counts and configuration values.
- Scales the budget based on the level and map difficulty.
- Applies a random variance to the scaled budget.
- Ensures the returned budget is non-negative.

# Constraints & Failure Modes
- Uses `Mathf.Max` to ensure the budget is not less than zero.
- Relies on `BalanceManager.Instance.Config` for configuration values; assumes this instance is properly initialized.

# Example
```csharp
int budget = LootBudgetCalculator.CalculateBudget(spawns: 5, normals: 3, level: 2, difficulty: MapDifficulty.Strained);
```

# Unknowns
- The structure and initialization of `BalanceManager.Instance.Config` and its properties.
