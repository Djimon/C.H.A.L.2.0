# CHAL.Systems.Research.ResearchEventBridge

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchEventBridge.cs`._

# Purpose
- Defines the `ResearchEventBridge` class for handling research-related events in the game.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `sealed class ResearchEventBridge`
    - **Public methods:**
      - `ResearchEventBridge(ResearchService service)` - Constructor that initializes the bridge with a `ResearchService`.
      - `void OnWaveCompleted()` - Notifies the service that a wave has been completed.
      - `void OnMapCompleted(MapDifficulty difficulty)` - Notifies the service that a map has been completed with a specified difficulty.
      - `void OnEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)` - Notifies the service that an enemy has been killed, providing enemy tags and rank.

# Key Behavior & Side Effects
- Calls to `OnWaveCompleted`, `OnMapCompleted`, and `OnEnemyKilled` trigger corresponding methods in the `ResearchService`.

# Constraints & Failure Modes
- No explicit guards or null handling noted in the provided code.
- Assumes `ResearchService` is properly initialized and available.

# Example
```csharp
var researchService = new ResearchService();
var eventBridge = new ResearchEventBridge(researchService);
eventBridge.OnWaveCompleted();
```

# Unknowns
- The implementation details of `ResearchService`, `MapDifficulty`, and `EnemyRank` are not provided in this file.

