# CHAL.Systems.Loot.LootBudgetModulator

_Automatically generated/updated from `Assets/src/Systems/Loot/LootBudgetModulator.cs`._

```txt
Purpose
- Defines a helper to modulate loot drop budget outcomes based on budget overflow.
- Reads configuration from BalanceManager.Instance.Config.loot to compute the modifier.
- Returns a multiplier used to influence drop chances; 1f when within budget, or a floor/exp-based value when overflow occurs.

Public API
- Namespace/module
  - CHAL.Systems.Loot

- Types
  - public static class LootBudgetModulator
    - Public methods
      - public static float GetModifier(int U, int v_i, int B, Rarity rarity)
        - Parameters
          - int U: current budget value (from context)
          - int v_i: additional budget/input value influencing the drop
          - int B: budget threshold
          - Rarity rarity: target drop rarity (used to select floor)
        - Returns
          - float: drop chance modifier
        - Side effects
          - Reads BalanceManager.Instance.Config.loot.budget (beta) and BalanceManager.Instance.Config.loot.floors (per-rarity floors)
          - Performs a floating-point calculation with Mathf.Exp

Key Behavior & Side Effects
- If within budget:
  - U + v_i <= B -> returns 1f (no change to drop chance)
- If overflow occurs:
  - overflow = ((U + v_i) - B) / B
  - expVal = Exp(-beta * overflow) using beta from cfg
  - floor = 
    - floors.rare if rarity == Rare
    - floors.epic if rarity == Epic
    - floors.legendary if rarity == Legendary
    - 0.0f otherwise (Common has no floor)
  - Returns max(floor, expVal)

Constraints & Failure Modes
- Dependencies
  - Requires BalanceManager.Instance and its non-null Config; no null checks are present.
- Division
  - B is used as a divisor to compute overflow; B should be non-zero to avoid division by zero.
- Rarity handling
  - Only Rare/Epic/Legendary map to a floor; any other rarity yields 0.0f floor.
- Performance
  - Uses Mathf.Exp; inexpensive per call but may be invoked frequently in tight loot loops.

Example
```csharp
// Usage example (assuming proper BalanceManager setup in the project)
float modifier = CHAL.Systems.Loot.LootBudgetModulator.GetModifier(
    U: 100,
    v_i: 20,
    B: 150,
    rarity: Rarity.Rare
);
```

Unknowns
- Exact definitions and structure of:
  - BalanceManager, BalanceManager.Config, and the nested loot budget/floors objects.
  - Rarity enum (values beyond Rare/Epic/Legendary usage are not shown).
- Thread-safety and lifecycle semantics of BalanceManager access.
- Any additional side effects or interactions with other loot systems not visible in this file.
```
