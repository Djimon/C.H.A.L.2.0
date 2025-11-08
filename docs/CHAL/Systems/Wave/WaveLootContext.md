# CHAL.Systems.Wave.WaveLootContext

_Automatically generated/updated from `Assets/src/Systems/Waves/WaveLootContext.cs`._

# Purpose
- Defines the `WaveLootContext` class for managing wave loot, including budget and drop information.

# Public API
- Namespace: `CHAL.Systems.Wave`
- Types
  - public class `WaveLootContext`
    - Public fields/properties:
      - `Wave`: The composition of the wave.
      - `TotalBudget`: The total budget for loot.
      - `SpentBudget`: The amount of budget spent.
      - `RemainingBudget`: The budget remaining after spending.
      - `Drops`: A list of loot result entries.
    - Public methods:
      - `WaveLootContext(WaveComposition wave)`: Constructor that initializes the wave and calculates the total budget.

# Key Behavior & Side Effects
- The constructor calculates the total budget based on various parameters from the `WaveComposition` object.

# Constraints & Failure Modes
- No explicit guards or null handling are present in the code.
- Assumes valid `WaveComposition` input for budget calculation.

# Example
```csharp
var waveComposition = new WaveComposition(...); // Initialize with appropriate parameters
var waveLootContext = new WaveLootContext(waveComposition);
```

# Unknowns
- Details about the `WaveComposition` class and `LootBudgetCalculator` are not provided in this file.
