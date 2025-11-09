# Assets/src/Systems/_test/DebugResearchBootstrap.cs

_Automatically generated/updated from `Assets/src/Systems/_test/DebugResearchBootstrap.cs`._

# Purpose
- Defines the `ResearchBootstrap` class, which initializes and manages research-related services and state in a Unity environment.

# Public API
- Namespace/module: None specified.
- Types
  - public sealed class ResearchBootstrap : MonoBehaviour
    - Public fields/properties:
      - ResearchService Service { get; private set; }
      - ResearchUnlockRegistry Registry { get; private set; }
      - ResearchEventBridge Bridge { get; private set; }
      - ResearchState State { get; private set; }
      - ResearchTreeDef TreeDef => treeDef
      - ResearchMapView mapView
    - Public methods:
      - bool SetActiveResearch(string nodeId)
      - void WaveCompleted()
      - void MapCompleted(MapDifficulty difficulty)
      - void EnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)

# Key Behavior & Side Effects
- Initializes `ResearchState`, `ResearchService`, `ResearchUnlockRegistry`, and `ResearchEventBridge` in the `Awake` method.
- Logs errors if `treeDef` is null or if `nodeDefs` is empty.
- Initializes the `ResearchService` with the `treeDef` and `State`.
- Rebuilds the `Registry` from `nodeDefs` and the completed node IDs from `State`.
- Subscribes to `Service.OnNodeCompleted` to apply node unlocks to the `Registry`.
- Calls `mapView.initHUD()` in the `Start` method.
- Provides context menu options for debugging: completing waves, completing maps, and killing enemies, which only function in play mode.

# Constraints & Failure Modes
- Guards against null or empty `treeDef` and `nodeDefs` with logging.
- Debug methods check if the application is in play mode before executing.

# Example
```csharp
var researchBootstrap = new ResearchBootstrap();
researchBootstrap.SetActiveResearch("nodeId123");
researchBootstrap.WaveCompleted();
researchBootstrap.MapCompleted(MapDifficulty.Stable);
researchBootstrap.EnemyKilled(new List<string> { "insectoid" }, EnemyRank.Normal);
```

# Unknowns
- No external dependencies or specific behaviors of `ResearchService`, `ResearchUnlockRegistry`, `ResearchEventBridge`, or `ResearchState` can be determined from this file.

