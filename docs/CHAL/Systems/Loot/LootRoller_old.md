# CHAL.Systems.Loot.LootRoller_old

_Automatically generated/updated from `Assets/src/Systems/Loot/LootRoller_old.cs`._

# Purpose
- Defines the `LootRoller_old` class for rolling loot based on wave composition and rules.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - `public sealed class LootRoller_old`
    - **Public Fields/Properties**: None
    - **Public Methods**:
      - `public LootRoller_old(LootRulesService rules, UnluckyProtection unlucky)`
      - `public List<LootResultEntry> RollLoot(WaveComposition wave)`: Rolls loot for a complete wave, returning a list of loot result entries.

# Key Behavior & Side Effects
- Calculates loot budget based on wave composition.
- Rolls for normal drops for each monster instance based on bonus tags and loot rules.
- Applies secret rules for additional drops.
- Post-processes loot to ensure minimum and maximum drops, as well as rarity guarantees.

# Constraints & Failure Modes
- Handles cases where `bonusTags` may be null or empty.
- Uses random number generation for loot rolls, which may lead to variability in outcomes.
- Ensures that the total loot does not exceed specified maximum drops.

# Example
```csharp
var lootRoller = new LootRoller_old(rulesService, unluckyProtection);
var lootResults = lootRoller.RollLoot(waveComposition);
```

# Unknowns
- Specific implementations of `LootRulesService`, `UnluckyProtection`, `LootBudgetCalculator`, and other referenced classes and methods.
- The structure of `LootResultEntry`, `WaveComposition`, and `MergedLoot`.

