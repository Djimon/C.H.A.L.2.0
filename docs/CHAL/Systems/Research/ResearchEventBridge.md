# CHAL.Systems.Research.ResearchEventBridge

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchEventBridge.cs`._

1) Purpose
- Defines a sealed class ResearchEventBridge to forward research-related events to a ResearchService.
- Holds a private reference to a ResearchService instance.
- Exposes methods OnWaveCompleted, OnMapCompleted, OnEnemyKilled that delegate to corresponding Apply* methods on the service.

2) Public API
- Namespace: CHAL.Systems.Research

- Types
  - public sealed class ResearchEventBridge
    - Public constructor
      - ResearchEventBridge(ResearchService service)
        - parameter: ResearchService service
    - Public methods
      - void OnWaveCompleted()
      - void OnMapCompleted(MapDifficulty difficulty)
        - parameters: MapDifficulty difficulty
      - void OnEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)
        - parameters: IReadOnlyList<string> enemyTags, EnemyRank rank

3) Key Behavior & Side Effects
- OnWaveCompleted:
  - Calls _service.ApplyWaveCompleted().
- OnMapCompleted:
  - Calls _service.ApplyMapCompleted(difficulty) with the provided difficulty.
- OnEnemyKilled:
  - Calls _service.ApplyEnemyKilled(enemyTags, rank) with the provided tags and rank.
- Constructor:
  - Stores the provided ResearchService instance into _service.

4) Constraints & Failure Modes
- No null-check on the constructor parameter; passing null may cause NullReferenceException when methods are invoked.
- All behavior is a direct delegation to ResearchService; no additional validation here.

5) Example
```csharp
// Example
var bridge = new ResearchEventBridge(service);
bridge.OnWaveCompleted();
```

6) Unknowns
- Implementation details of ResearchService.ApplyWaveCompleted, ApplyMapCompleted, ApplyEnemyKilled.
- Definitions/values for MapDifficulty and EnemyRank (from CHAL.Data).
- Any threading/async semantics not explicit in this file.

