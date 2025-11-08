# CHAL.Systems.Loot.LootBudgetModulator

_Automatically generated/updated from `Assets/src/Systems/Loot/LootBudgetModulator.cs`._

1) Purpose
- Defines a static class for calculating loot drop chance modifiers based on budget overflow.

2) Public API
- Namespace: CHAL.Systems.Loot
- Types
  - public static class LootBudgetModulator
    - Public methods
      - static float GetModifier(int U, int v_i, int B, Rarity rarity)
        - Calculates the drop chance modifier based on budget overflow.

3) Key Behavior & Side Effects
- Returns a modifier of 1.0 if the sum of U and v_i is less than or equal to B.
- Calculates overflow and applies an exponential decay based on a beta value.
- Determines a floor value based on the rarity type, ensuring the returned modifier is at least the floor value.

4) Constraints & Failure Modes
- Assumes that BalanceManager.Instance.Config.loot.budget and BalanceManager.Instance.Config.loot.floors are properly initialized.
- Handles different rarity types with a default case for Common, which does not require a floor.

5) Example
```csharp
float modifier = LootBudgetModulator.GetModifier(currentBudget, itemValue, maxBudget, itemRarity);
```

6) Unknowns
- The structure and initialization of BalanceManager.Instance.Config.loot.budget and BalanceManager.Instance.Config.loot.floors are not defined in this file.
