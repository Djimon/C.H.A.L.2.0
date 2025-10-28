# Assets/src/Systems/Loot/LootBudgetModulator.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a static class for calculating loot drop chance modifiers based on budget overflow.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - `public static class LootBudgetModulator`
    - Public methods
      - `public static float GetModifier(int U, int v_i, int B, Rarity rarity);`
        - Calculates the drop chance modifier based on budget overflow.

# Key Behavior & Side Effects
- Returns a modifier of 1.0 if the sum of `U` and `v_i` is less than or equal to `B`.
- Calculates overflow and applies an exponential decay based on the `beta` value from configuration.
- Applies a floor value based on the rarity type, ensuring the modifier does not fall below this floor.

# Constraints & Failure Modes
- Assumes `BalanceManager.Instance.Config.loot.budget` and `BalanceManager.Instance.Config.loot.floors` are properly initialized.
- Handles different rarity types with a default case returning 0.0 for common items.

# Example
```csharp
float modifier = LootBudgetModulator.GetModifier(currentUsage, itemValue, budget, itemRarity);
```

# Unknowns
- The structure and initialization of `BalanceManager.Instance.Config.loot`.
- The definition of the `Rarity` enum.
```
