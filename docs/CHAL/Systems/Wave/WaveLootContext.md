# Assets/src/Systems/Map/Waves/WaveLootContext.cs

_Automatically generated/updated from `Assets/src/Systems/Map/Waves/WaveLootContext.cs`._

# Purpose
- Defines the `WaveLootContext` class for managing wave loot, including budget and drop information.

# Public API
- Namespace: `CHAL.Systems.Wave`
- Types
  - public class `WaveLootContext`
    - Public fields/properties:
      - `Wave`: The wave composition associated with this context.
      - `TotalBudget`: The total budget for loot.
      - `SpentBudget`: The amount of budget that has been spent.
      - `RemainingBudget`: The budget remaining after spending.
      - `Drops`: A list of loot result entries.
    - Public methods:
      - `WaveLootContext(WaveComposition wave)`: Constructor that initializes the wave and calculates the total budget.

# Key Behavior & Side Effects
- The constructor calculates the total budget based on various parameters of the `WaveComposition` provided.

# Constraints & Failure Modes
- No explicit guards or error handling are defined in the code.
- The `RemainingBudget` is calculated as the difference between `TotalBudget` and `SpentBudget`.

# Example
```csharp
var waveComposition = new WaveComposition(/* parameters */);
var waveLootContext = new WaveLootContext(waveComposition);
```

# Unknowns
- The implementation details of `WaveComposition` and `LootBudgetCalculator` are not provided in this file.
