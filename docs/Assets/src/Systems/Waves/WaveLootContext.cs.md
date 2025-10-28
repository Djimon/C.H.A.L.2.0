# Assets/src/Systems/Waves/WaveLootContext.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `WaveLootContext` class for managing loot budget and drops in a wave system.

## Public API
- Namespace: `CHAL.Systems.Wave`
- Types
  - `public class WaveLootContext`
    - Public fields/properties:
      - `WaveComposition Wave` - Represents the wave composition.
      - `int TotalBudget` - Total budget for loot.
      - `int SpentBudget` - Amount of budget spent.
      - `int RemainingBudget` - Calculated remaining budget.
      - `List<LootResultEntry> Drops` - List of loot result entries.
    - Public methods:
      - `WaveLootContext(WaveComposition wave)` - Constructor that initializes the wave and calculates the total budget.

## Key Behavior & Side Effects
- The constructor calculates the total budget based on various parameters of the `WaveComposition` object.

## Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes valid `WaveComposition` is provided to the constructor.

## Example
```csharp
var waveComposition = new WaveComposition(/* parameters */);
var waveLootContext = new WaveLootContext(waveComposition);
```

## Unknowns
- Details of `WaveComposition` and `LootBudgetCalculator` are not defined in this file.
- The structure of `LootResultEntry` is not provided.
```
