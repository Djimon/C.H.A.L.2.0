# Assets/src/Systems/Loot/LootRoller_old.cs

_Automatically generated/updated from `Assets/src/Systems/Loot/LootRoller_old.cs`._

# Purpose
- Defines the `LootRoller_old` class for rolling loot based on wave compositions and loot rules.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - `public sealed class LootRoller_old`
    - **Public fields/properties**: None
    - **Public methods**:
      - `public LootRoller_old(LootRulesService rules, UnluckyProtection unlucky)`
      - `public List<LootResultEntry> RollLoot(WaveComposition wave)`: Rolls loot for a complete wave, returning a list of loot result entries.

# Key Behavior & Side Effects
- Calculates a loot budget based on wave composition.
- Rolls for normal drops for each monster instance based on defined rules and random chance.
- Applies post-processing to ensure minimum drops and rarity guarantees.
- Uses a smart trimming mechanism to enforce maximum drops based on configured weights.

# Constraints & Failure Modes
- Handles cases where `bonusTags` may be null or empty.
- Ensures that the number of loot entries does not exceed the maximum allowed.
- Uses random number generation for loot rolls, which may lead to variability in results.

# Example
```csharp
var lootRoller = new LootRoller_old(rulesService, unluckyProtection);
var lootResults = lootRoller.RollLoot(waveComposition);
```

# Unknowns
- The specific implementations of `LootRulesService`, `UnluckyProtection`, and `LootResultEntry` are not defined in this file.
- The behavior of `DebugManager.Log` is not detailed in this file.

