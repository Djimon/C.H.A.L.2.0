# CHAL.Systems.Loot.LootRoller_old

_Automatically generated/updated from `Assets/src/Systems/Loot/LootRoller_old.cs`._

# Purpose
- Defines the `LootRoller_old` class for rolling loot based on wave composition and loot rules.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - `public sealed class LootRoller_old`
    - **Constructor**
      - `LootRoller_old(LootRulesService rules, UnluckyProtection unlucky)`
    - **Public Methods**
      - `List<LootResultEntry> RollLoot(WaveComposition wave)`
        - Rolls loot for a complete wave based on the provided `WaveComposition`.
  
# Key Behavior & Side Effects
- Calculates a loot budget based on various parameters of the wave.
- Iterates through monsters in the wave to determine loot drops based on rules and random chance.
- Applies post-processing to ensure minimum and guaranteed drops are met.
- Logs debug information for dropped items and adjustments made during processing.

# Constraints & Failure Modes
- Handles cases where `bonusTags` may be null or empty.
- Uses random number generation for loot chances, which may lead to variability in results.
- Ensures that the number of loot entries does not exceed configured maximum drops.

# Example
```csharp
var lootRoller = new LootRoller_old(rulesService, unluckyProtection);
var lootResults = lootRoller.RollLoot(waveComposition);
```

# Unknowns
- The exact implementation details of `LootRulesService`, `UnluckyProtection`, and other referenced classes and methods.
- The structure of `WaveComposition` and `LootResultEntry`.

