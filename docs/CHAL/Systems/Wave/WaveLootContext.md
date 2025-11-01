# CHAL.Systems.Wave.WaveLootContext

_Automatically generated/updated from `Assets/src/Systems/Waves/WaveLootContext.cs`._

Purpose
- Defines a per-wave loot generation context for tracking budget and results.
- Holds references to the current wave, budget state, and generated loot entries.
- Computes the initial total budget from wave parameters via LootBudgetCalculator.

Public API
- Namespace: CHAL.Systems.Wave
- Type: public class WaveLootContext
  - Public properties
    - public WaveComposition Wave { get; }
      - The associated wave configuration
    - public int TotalBudget { get; }
      - Total loot budget allocated for this wave
    - public int SpentBudget { get; set; }
      - Budget already spent; can be modified by external code
    - public int RemainingBudget => TotalBudget - SpentBudget;
      - Budget left for loot in this wave
    - public List<LootResultEntry> Drops { get; } = new List<LootResultEntry>();
      - Loot results collected for this wave
  - Public constructors
    - public WaveLootContext(WaveComposition wave)
      - Assigns Wave
      - Calculates TotalBudget using LootBudgetCalculator.CalculateBudget with wave.TotalSpawns, wave.TotalNormals, wave.TotalMagics, wave.TotalElites, wave.TotalBosses, wave.TotalChampions, wave.Level, wave.Difficulty
      - Initializes Drops (already set inline)

Key Behavior & Side Effects
- Construction-time behavior
  - WaveLootContext(WaveComposition wave) sets Wave = wave.
  - TotalBudget is computed via LootBudgetCalculator.CalculateBudget(...) using multiple wave properties.
  - Drops is initialized to an empty list.
- State tracking
  - SpentBudget starts at 0 (default) and can be incremented by external code.
  - RemainingBudget is derived from TotalBudget and SpentBudget.
  - Drops collects LootResultEntry items over the lifetime of the context.

Constraints & Failure Modes
- Null handling
  - No null check for wave; passing null will cause NullReferenceException when accessing wave members in constructor.
- Mutability and validation
  - SpentBudget has a public setter with no validation; it may exceed TotalBudget unless controlled externally.
- Threading
  - No synchronization; this class is not thread-safe.
- External dependencies
  - Relies on LootBudgetCalculator, WaveComposition, and LootResultEntry whose internal behavior is not shown here.

Unknowns
- Details of WaveComposition (definitions of TotalSpawns, TotalNormals, TotalMagics, TotalElites, TotalBosses, TotalChampions, Level, Difficulty).
- Implementation and formula of LootBudgetCalculator.CalculateBudget.
- Structure and semantics of LootResultEntry and how Drops are consumed elsewhere.
- Any additional side effects from external code interacting with this class.
