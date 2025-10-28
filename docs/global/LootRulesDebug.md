# global.LootRulesDebug

_Automatically generated/updated from `Assets/src/Systems/_test/dbg_LootRules.cs`._

# Purpose
- Defines the `LootRulesDebug` class for debugging loot rules in a game.
- Provides methods to calculate loot budgets and manage loot drop probabilities.

# Public API
- Namespace/module: None specified.
- Types
  - public class LootRulesDebug : MonoBehaviour
    - Public fields/properties:
      - string[] enemyTags: Tags for enemy types.
      - int level: Level of the loot.
      - MapDifficulty difficulty: Difficulty level of the map.
      - int spawns: Number of enemy spawns.
      - int normals: Number of normal enemies.
      - int magics: Number of magical enemies.
      - int elites: Number of elite enemies.
      - int bosses: Number of boss enemies.
      - int champions: Number of champion enemies.
      - int Budget: Total loot budget.
      - int U_budget_Used: Used portion of the budget.
      - int vi_item_Value: Value of the item.
    - Public methods:
      - void Start(): Initializes loot rules, calculates budgets, and logs results.

# Key Behavior & Side Effects
- Calls `ItemRegistry.Instance.Reload()` to ensure item registry is loaded.
- Uses `LootRulesService` to load and merge loot rules based on enemy tags.
- Logs merged drops and their properties.
- Calculates and logs the loot budget based on enemy composition and difficulty.
- Manages and logs the effects of "unlucky protection" on loot drops.
- Calculates and logs the budget modifier based on used budget and item value.

# Constraints & Failure Modes
- Assumes `ItemRegistry` and `LootRulesService` are properly implemented and available.
- No explicit error handling for potential failures in service calls or calculations.

# Example
```csharp
void Start()
{
    var lootRulesDebug = new LootRulesDebug();
    lootRulesDebug.Start();
}
```

# Unknowns
- The implementation details of `LootRulesService`, `LootBudgetCalculator`, and `UnluckyProtection`.
- The structure of the `MapDifficulty` and `Rarity` enums.
- The behavior of `Debug.Log` in the context of the game engine.

