# Assets/src/Systems/Research/ResearchEventBridge.cs

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchEventBridge.cs`._

1) Purpose
- Defines the `ResearchEventBridge` class for handling events related to research progress in the game.

2) Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - public sealed class `ResearchEventBridge`
    - Public fields/properties: None
    - Public methods:
      - `ResearchEventBridge(ResearchService service)` - Constructor that initializes the event bridge with a `ResearchService`.
      - `void OnWaveCompleted()` - Calls the service to apply wave completion.
      - `void OnMapCompleted(MapDifficulty difficulty)` - Calls the service to apply map completion with the specified difficulty.
      - `void OnEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)` - Calls the service to apply the event of an enemy being killed.

3) Key Behavior & Side Effects
- `OnWaveCompleted`: Triggers the application of wave completion in the `ResearchService`.
- `OnMapCompleted`: Triggers the application of map completion with a specified difficulty in the `ResearchService`.
- `OnEnemyKilled`: Triggers the application of an enemy kill event in the `ResearchService`.

4) Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes `ResearchService` is properly initialized and available.

5) Example
```csharp
var researchService = new ResearchService();
var eventBridge = new ResearchEventBridge(researchService);
eventBridge.OnWaveCompleted();
eventBridge.OnMapCompleted(MapDifficulty.Hard);
eventBridge.OnEnemyKilled(new List<string> { "Goblin", "Orc" }, EnemyRank.Elite);
```

6) Unknowns
- The implementation details of `ResearchService`, `MapDifficulty`, and `EnemyRank` are not provided in this file.
