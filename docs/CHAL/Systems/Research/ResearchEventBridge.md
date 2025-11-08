# CHAL.Systems.Research.ResearchEventBridge

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchEventBridge.cs`._

# Purpose
- Defines the `ResearchEventBridge` class for handling events related to research progress in the game.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchEventBridge`
    - **Public fields/properties**: None
    - **Public methods**:
      - `public ResearchEventBridge(ResearchService service)` - Constructor that initializes the bridge with a `ResearchService`.
      - `public void OnWaveCompleted()` - Calls the service to apply wave completion.
      - `public void OnMapCompleted(MapDifficulty difficulty)` - Calls the service to apply map completion with the specified difficulty.
      - `public void OnEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)` - Calls the service to apply enemy killed event with associated tags and rank.

# Key Behavior & Side Effects
- `OnWaveCompleted`: Triggers the application of wave completion in the `ResearchService`.
- `OnMapCompleted`: Triggers the application of map completion in the `ResearchService` with the provided difficulty.
- `OnEnemyKilled`: Triggers the application of enemy killed event in the `ResearchService` with the provided enemy tags and rank.

# Constraints & Failure Modes
- Assumes that the `ResearchService` instance passed to the constructor is valid and properly initialized.
- No explicit error handling is present in the methods; failures in the `ResearchService` methods are not managed here.

# Example
```csharp
var researchService = new ResearchService();
var eventBridge = new ResearchEventBridge(researchService);
eventBridge.OnWaveCompleted();
eventBridge.OnMapCompleted(MapDifficulty.Hard);
eventBridge.OnEnemyKilled(new List<string> { "Goblin", "Orc" }, EnemyRank.Elite);
```

# Unknowns
- The implementation details of `ResearchService`, `MapDifficulty`, and `EnemyRank` are not provided in this file.
