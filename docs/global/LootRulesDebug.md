# Assets/src/Systems/_test/dbg_LootRules.cs

_Automatically generated/updated from `Assets/src/Systems/_test/dbg_LootRules.cs`._

# Purpose
- Manages loot rules and enemy composition for debugging purposes.

# Public API
- Namespace: None
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
      - int Budget: Total budget for loot.
      - int U_budget_Used: Budget already used.
      - int vi_item_Value: Value of the item.
    - Public methods:
      - void Start(): Initializes loot rules, calculates budget, and manages unlucky protection.

# Key Behavior & Side Effects
- Reloads item registry and loads loot rules on Start.
- Merges loot drops based on enemy tags and logs the results.
- Calculates loot budget based on enemy composition and logs the budget.
- Manages and logs the effects of unlucky protection on loot chances.
- Applies a soft cap modifier based on budget usage and item value.

# Constraints & Failure Modes
- Assumes that the ItemRegistry is properly initialized before calling Reload.
- No explicit error handling for potential failures in loading loot rules or calculating budgets.

# Example
```csharp
void Start()
{
    var lootRulesDebug = new LootRulesDebug();
    lootRulesDebug.Start();
}
```

# Unknowns
- None.

