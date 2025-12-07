# Assets/src/Systems/Loot/LootRulesService.cs

_Automatically generated/updated from `Assets/src/Systems/Loot/LootRulesService.cs`._

# Purpose
- Defines the `LootRulesService` class for managing loot rules and secret loot drops in the game.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - public sealed class `LootRulesService`
    - Public methods:
      - `void LoadAll()`
      - `bool TryGetRule(string tag, out LootRule rule)`
      - `MergedLoot GetMergedForTags(IEnumerable<string> tags)`
      - `MergedLoot GetMergedForWave(WaveComposition wave)`
      - `List<LootDropDto> GetSecretDrops(IEnumerable<string> monsterTags)`

# Key Behavior & Side Effects
- `LoadAll()`: Loads loot rules from resources, clears existing rules, and logs any duplicates or errors. Also loads secret rules.
- `TryGetRule(string tag, out LootRule rule)`: Attempts to retrieve a loot rule by tag.
- `GetMergedForTags(IEnumerable<string> tags)`: Merges loot rules based on provided tags, logging warnings for missing rules.
- `GetMergedForWave(WaveComposition wave)`: Merges loot based on the composition of monsters in a wave.
- `GetSecretDrops(IEnumerable<string> monsterTags)`: Retrieves secret loot drops based on monster tags.

# Constraints & Failure Modes
- Throws exceptions for invalid data in loot rules (e.g., missing tags, invalid item IDs, invalid chances).
- Requires valid item definitions in the `ItemRegistry`.
- Handles empty or null inputs gracefully in some methods.

# Example
```csharp
var lootService = new LootRulesService();
lootService.LoadAll();
if (lootService.TryGetRule("exampleTag", out var rule))
{
    // Use the retrieved rule
}
```

# Unknowns
- The structure and contents of `LootRuleDto`, `LootDropDto`, `SpecialRulesWrapper`, and `WaveComposition` are not defined in this file.
- The behavior of `DebugManager` is not detailed in this file.
