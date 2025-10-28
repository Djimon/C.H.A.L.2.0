# Assets/src/Systems/_test/DebugResearchBootstrap.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `ResearchBootstrap` class, which initializes and manages research-related services and state in a Unity game.

## Public API
- Namespace: None
- Types
  - `public sealed class ResearchBootstrap : MonoBehaviour`
    - Public fields/properties:
      - `ResearchService Service { get; private set; }` - Manages research operations.
      - `ResearchUnlockRegistry Registry { get; private set; }` - Tracks unlocked research nodes.
      - `ResearchEventBridge Bridge { get; private set; }` - Facilitates event communication.
      - `ResearchState State { get; private set; }` - Holds the current state of research.
      - `ResearchTreeDef TreeDef => treeDef;` - Exposes the research tree definition.
      - `ResearchMapView mapView;` - UI component for displaying research map.
    - Public methods:
      - `bool SetActiveResearch(string nodeId)` - Sets the active research node.
      - `void WaveCompleted()` - Notifies that a wave has been completed.
      - `void MapCompleted(MapDifficulty difficulty)` - Notifies that a map has been completed.
      - `void EnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)` - Reports an enemy kill.

## Key Behavior & Side Effects
- `Awake()`:
  - Initializes `State`, `Service`, `Registry`, and `Bridge`.
  - Logs errors if `treeDef` is null or `nodeDefs` is empty.
  - Initializes the `Service` with the research tree and state.
  - Rebuilds the `Registry` from node definitions and completed state.
  - Sets up event handling for node completion.
- `Start()`:
  - Initializes the HUD in `mapView`.
- Context menu methods (`Debug_CompleteWave`, `Debug_CompleteMap`, `Debug_KillEnemy`):
  - Require the application to be in play mode; otherwise, log a warning.

## Constraints & Failure Modes
- Guards against null or empty `treeDef` and `nodeDefs` during initialization.
- Context menu methods only execute in play mode; otherwise, they log a warning.

## Example
```csharp
var researchBootstrap = new ResearchBootstrap();
researchBootstrap.SetActiveResearch("nodeId123");
researchBootstrap.WaveCompleted();
```

## Unknowns
- The specific implementations of `ResearchService`, `ResearchUnlockRegistry`, `ResearchEventBridge`, and `ResearchState` cannot be determined from this file.
```
