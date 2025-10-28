# Assets/src/Systems/Loot/LootRulesService.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `LootRulesService` class for managing loot rules and special rules in the game.

## Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - `public sealed class LootRulesService`
    - Public methods:
      - `public void LoadAll()`
      - `public bool TryGetRule(string tag, out LootRule rule)`
      - `public MergedLoot GetMergedForTags(IEnumerable<string> tags)`
      - `public MergedLoot GetMergedForWave(WaveComposition wave)`
      - `public List<LootDropDto> GetSecretDrops(IEnumerable<string> monsterTags)`

## Key Behavior & Side Effects
- `LoadAll()`: Loads loot rules from resources, clears existing rules, and logs the number of loaded rules. Handles exceptions during loading.
- `ToRule(LootRuleDto dto, string sourceName)`: Converts DTO to `LootRule`, validates fields, and throws exceptions for invalid data.
- `GetMergedForTags(IEnumerable<string> tags)`: Merges loot rules for given tags, concatenates drops, and calculates max drops and rarity guarantees.
- `GetMergedForWave(WaveComposition wave)`: Merges loot rules based on monster wave composition, summing drops and rarity guarantees.
- `GetSecretDrops(IEnumerable<string> monsterTags)`: Retrieves secret drops based on matching monster tags.

## Constraints & Failure Modes
- Throws exceptions for missing or invalid data in `ToRule`.
- Requires valid item IDs and existing items in the `ItemRegistry`.
- Handles empty or null arrays for drops and rarity guarantees.
- Logs warnings for duplicate tags and missing rules.

## Example
```csharp
var lootService = new LootRulesService();
lootService.LoadAll();
if (lootService.TryGetRule("exampleTag", out var rule)) {
    // Use the retrieved rule
}
```

## Unknowns
- The structure and properties of `LootRule`, `LootRuleDto`, `LootDropDto`, `MergedLoot`, and `WaveComposition` are not defined in this file.
- The behavior of `DebugManager` methods is not detailed in this file.
```
