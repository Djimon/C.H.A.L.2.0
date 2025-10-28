# Assets/src/Systems/Loot/LootBudgetCalculator.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a static class for calculating loot budgets based on various parameters.

## Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - `public static class LootBudgetCalculator`
    - Public methods
      - `public static int CalculateBudget(int spawns=0, int normals=0, int magics=0, int elites=0, int bosses=0, int champions=0, int level=1, MapDifficulty difficulty=MapDifficulty.Stable)`: Calculates the loot budget for a wave based on enemy types, level, and map difficulty. Returns a non-negative integer.

      - `private static float getMultiplierFromMapDifficulty(MapDifficulty difficulty)`: Returns a multiplier based on the provided map difficulty.

## Key Behavior & Side Effects
- Calculates a raw budget based on the number of different enemy types and their respective budget points.
- Scales the budget based on the level and map difficulty.
- Applies a random variance to the scaled budget before returning the final value.

## Constraints & Failure Modes
- Returns a maximum of 0 if the calculated budget is negative.
- Assumes `BalanceManager.Instance.Config` is properly initialized and accessible.

## Example
```csharp
int budget = LootBudgetCalculator.CalculateBudget(spawns: 5, normals: 3, level: 2, difficulty: MapDifficulty.Strained);
```

## Unknowns
- The structure and initialization of `BalanceManager.Instance.Config` and its properties.
```
