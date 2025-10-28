# CHAL.Systems.Loot.LootRulesService

_Automatically generated/updated from `Assets/src/Systems/Loot/LootRulesService.cs`._

# Purpose
- Defines the `LootRulesService` class for managing loot rules in the game.
- Provides methods to load loot rules from resources, merge rules, and retrieve secret drops.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - `public sealed class LootRulesService`
    - Public methods:
      - `public void LoadAll()`
        - Loads all loot rules from resources and initializes the service.
      - `public bool TryGetRule(string tag, out LootRule rule)`
        - Attempts to retrieve a loot rule by its tag.
      - `public MergedLoot GetMergedForTags(IEnumerable<string> tags)`
        - Merges loot rules for the specified tags and returns a `MergedLoot` object.
      - `public MergedLoot GetMergedForWave(WaveComposition wave)`
        - Merges loot rules based on the composition of a wave of monsters.
      - `public List<LootDropDto> GetSecretDrops(IEnumerable<string> monsterTags)`
        - Retrieves secret drops based on monster tags.

# Key Behavior & Side Effects
- `LoadAll()` clears existing rules, loads new rules from JSON files, and logs any errors encountered during loading.
- `GetMergedForTags()` and `GetMergedForWave()` aggregate loot rules, summing drops and ensuring maximum values for min/max drops and rarity guarantees.
- `GetSecretDrops()` checks for matching tags against secret rules and returns corresponding drops.

# Constraints & Failure Modes
- Throws exceptions for missing or invalid data during rule loading (e.g., missing tags, invalid item IDs).
- Handles empty or null drops by throwing exceptions.
- Logs warnings for duplicate tags and missing rules.

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
- The structure of `LootRuleDto`, `LootDropDto`, `MergedLoot`, `WaveComposition`, and `SpecialRulesWrapper` cannot be determined from this file.
- The implementation details of `DebugManager` and `ItemRegistry` are not provided.

