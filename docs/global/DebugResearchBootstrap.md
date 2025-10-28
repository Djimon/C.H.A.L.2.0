# global.DebugResearchBootstrap

_Automatically generated/updated from `Assets/src/Systems/_test/DebugResearchBootstrap.cs`._

# Purpose
- Defines the `ResearchBootstrap` class for managing research-related services and state in a Unity game.

# Public API
- Namespace: None
- Types
  - public sealed class `ResearchBootstrap` : `MonoBehaviour`
    - Public fields/properties:
      - `ResearchService Service { get; private set; }` - Manages research operations.
      - `ResearchUnlockRegistry Registry { get; private set; }` - Tracks unlocked research nodes.
      - `ResearchEventBridge Bridge { get; private set; }` - Connects events between services.
      - `ResearchState State { get; private set; }` - Holds the current state of research.
      - `ResearchTreeDef TreeDef => treeDef;` - Exposes the tree definition.
      - `ResearchMapView mapView;` - UI component for displaying research map.
    - Public methods:
      - `bool SetActiveResearch(string nodeId)` - Sets the active research node.
      - `void WaveCompleted()` - Notifies that a wave is completed.
      - `void MapCompleted(MapDifficulty difficulty)` - Notifies that a map is completed.
      - `void EnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)` - Reports an enemy kill.

# Key Behavior & Side Effects
- `Awake()`:
  - Initializes `State`, `Service`, `Registry`, and `Bridge`.
  - Logs errors if `treeDef` or `nodeDefs` are missing.
  - Initializes the `Service` with the research tree.
  - Rebuilds the `Registry` from `nodeDefs` and completed state.
  - Sets up event handling for node completion.
- `Start()`:
  - Initializes the HUD in `mapView`.
- Context menu methods (`Debug_CompleteWave`, `Debug_CompleteMap`, `Debug_KillEnemy`):
  - Require the application to be in play mode; logs warnings otherwise.

# Constraints & Failure Modes
- Guards against null or empty `treeDef` and `nodeDefs`.
- Context menu methods only execute in play mode; otherwise, they log a warning.

# Example
```csharp
var researchBootstrap = new ResearchBootstrap();
researchBootstrap.SetActiveResearch("nodeId123");
researchBootstrap.WaveCompleted();
```

# Unknowns
- No information on the implementation details of `ResearchService`, `ResearchUnlockRegistry`, `ResearchEventBridge`, or `ResearchState`.
- No details on the behavior of `mapView` or how it interacts with the `Service`.

