# CHAL.Systems.Wave.WaveLootContext

_Automatically generated/updated from `Assets/src/Systems/Waves/WaveLootContext.cs`._

# Purpose
- Defines the `WaveLootContext` class for managing loot budget and drops in a wave system.

# Public API
- Namespace: `CHAL.Systems.Wave`
- Types
  - public class `WaveLootContext`
    - Public fields/properties:
      - `Wave`: The composition of the wave.
      - `TotalBudget`: The total budget allocated for loot.
      - `SpentBudget`: The amount of budget spent on loot.
      - `RemainingBudget`: The budget remaining after spending.
      - `Drops`: A list of loot result entries.
    - Public methods:
      - `WaveLootContext(WaveComposition wave)`: Constructor that initializes the wave and calculates the total budget.

# Key Behavior & Side Effects
- The constructor calculates the total budget based on various parameters from the `WaveComposition` object.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes `LootBudgetCalculator.CalculateBudget` handles its own error conditions.

# Example
```csharp
var waveComposition = new WaveComposition(/* parameters */);
var waveLootContext = new WaveLootContext(waveComposition);
```

# Unknowns
- Details of `WaveComposition` and `LootBudgetCalculator` are not provided in this file.

