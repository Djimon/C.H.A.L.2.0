# Assets/src/Systems/_test/dbg_LootRules.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `LootRulesDebug` class for debugging loot rules in a game.
- Provides methods to log loot generation and budget calculations based on enemy composition and difficulty.

## Public API
- Namespace/module: None specified.
- Types
  - `public class LootRulesDebug : MonoBehaviour`
    - Public fields/properties:
      - `public string[] enemyTags`: Tags for enemy types.
      - `public int level`: Level of the loot.
      - `public MapDifficulty difficulty`: Difficulty of the map.
      - `public int spawns`: Number of enemy spawns.
      - `public int normals`: Number of normal enemies.
      - `public int magics`: Number of magical enemies.
      - `public int elites`: Number of elite enemies.
      - `public int bosses`: Number of boss enemies.
      - `public int champions`: Number of champion enemies.
      - `public int Budget`: Total budget for loot.
      - `public int U_budget_Used`: Used budget.
      - `public int vi_item_Value`: Value of the item.
    - Public methods:
      - `void Start()`: Initializes loot rules, calculates budgets, and logs results.

## Key Behavior & Side Effects
- Calls `ItemRegistry.Instance.Reload()` to ensure items are loaded.
- Uses `LootRulesService` to load and merge loot rules based on enemy tags.
- Logs merged drops and their properties.
- Calculates and logs the loot budget based on enemy composition and difficulty.
- Manages and logs the effects of "unlucky protection" on loot chances.
- Applies a budget modifier based on used budget and item value.

## Constraints & Failure Modes
- Assumes `ItemRegistry` and `LootRulesService` are properly implemented and available.
- No explicit error handling for potential failures in loading or calculating loot.

## Example
```csharp
void Start()
{
    // Create an instance of LootRulesDebug and call Start to see debug logs.
    var lootRulesDebug = new LootRulesDebug();
    lootRulesDebug.Start();
}
```

## Unknowns
- The implementation details of `LootRulesService`, `LootBudgetCalculator`, and `UnluckyProtection` are not provided.
- The behavior of `MapDifficulty` and its impact on loot generation is not defined in this file.
```
