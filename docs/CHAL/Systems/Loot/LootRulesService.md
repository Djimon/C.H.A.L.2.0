# CHAL.Systems.Loot.LootRulesService

_Automatically generated/updated from `Assets/src/Systems/Loot/LootRulesService.cs`._

# Purpose
- Defines the `LootRulesService` class for managing loot rules and secret loot drops in the game.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - public sealed class `LootRulesService`
    - Public methods:
      - `void LoadAll()`: Loads all loot rules from resources.
      - `bool TryGetRule(string tag, out LootRule rule)`: Tries to get a loot rule associated with the specified tag.
      - `MergedLoot GetMergedForTags(IEnumerable<string> tags)`: Merges loot based on the specified tags.
      - `MergedLoot GetMergedForWave(WaveComposition wave)`: Merges loot for a given wave composition of monsters.
      - `List<LootDropDto> GetSecretDrops(IEnumerable<string> monsterTags)`: Retrieves a list of secret loot drops based on the specified monster tags.

# Key Behavior & Side Effects
- `LoadAll()`: Clears existing rules, loads new rules from resources, and logs any errors encountered during loading.
- `TryGetRule()`: Returns whether a rule was found for the specified tag.
- `GetMergedForTags()`: Merges loot from multiple tags, logging warnings for missing rules.
- `GetMergedForWave()`: Merges loot based on the composition of monsters in a wave.
- `GetSecretDrops()`: Retrieves secret loot drops based on matching monster tags.

# Constraints & Failure Modes
- Throws exceptions for invalid or missing data during rule loading and processing (e.g., missing tags, invalid item IDs).
- Requires valid `LootRuleDto` and `SpecialRulesWrapper` structures for successful loading.
- Assumes that `ItemRegistry` is properly initialized and populated.

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
- The structure and contents of `LootRuleDto`, `SpecialRulesWrapper`, and `LootDropDto` are not defined in this file.
- The behavior of `ItemRegistry` and `DebugManager` is not detailed in this file.

