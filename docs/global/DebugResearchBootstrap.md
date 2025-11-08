# global.DebugResearchBootstrap

_Automatically generated/updated from `Assets/src/Systems/_test/DebugResearchBootstrap.cs`._

# Purpose
- Defines the `ResearchBootstrap` class for initializing and managing research-related services and states in a Unity environment.

# Public API
- Namespace/module: None specified.
- Types
  - **sealed class** `ResearchBootstrap` : `MonoBehaviour`
    - **Public fields/properties**
      - `ResearchService Service { get; private set; }` - Manages research operations.
      - `ResearchUnlockRegistry Registry { get; private set; }` - Tracks unlockable research nodes.
      - `ResearchEventBridge Bridge { get; private set; }` - Facilitates event communication.
      - `ResearchState State { get; private set; }` - Represents the current state of research.
      - `ResearchTreeDef TreeDef => treeDef;` - Exposes the research tree definition.
      - `ResearchMapView mapView;` - UI component for displaying the research map.
    - **Public methods**
      - `bool SetActiveResearch(string nodeId)` - Sets the active research node.
      - `void WaveCompleted()` - Notifies that a wave has been completed.
      - `void MapCompleted(MapDifficulty difficulty)` - Notifies that a map has been completed.
      - `void EnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)` - Reports an enemy kill.

# Key Behavior & Side Effects
- **Awake()**
  - Initializes `State`, `Service`, `Registry`, and `Bridge`.
  - Logs errors if `treeDef` is null or if `nodeDefs` is empty.
  - Calls `Service.InitFromTree(treeDef, State)` to initialize the research service.
  - Rebuilds the `Registry` from `nodeDefs` and the completed state.
  - Subscribes to `Service.OnNodeCompleted` to apply node unlocks.
- **Start()**
  - Initializes the HUD in `mapView`.
- **Debug Methods**
  - `Debug_CompleteWave()`, `Debug_CompleteMap()`, and `Debug_KillEnemy()` methods log actions and require play mode to execute.

# Constraints & Failure Modes
- Guards against null or empty `treeDef` and `nodeDefs`.
- Debug methods require the application to be in play mode; otherwise, they log a warning.

# Example
```csharp
// Example of setting an active research node
ResearchBootstrap researchBootstrap = new ResearchBootstrap();
bool isActive = researchBootstrap.SetActiveResearch("nodeId123");
```

# Unknowns
- No external dependencies or context for `ResearchTreeDef`, `ResearchNodeDef`, `ResearchService`, `ResearchUnlockRegistry`, `ResearchEventBridge`, `ResearchState`, `MapDifficulty`, or `EnemyRank`.

