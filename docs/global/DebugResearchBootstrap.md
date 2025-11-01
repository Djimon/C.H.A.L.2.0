# global.DebugResearchBootstrap

_Automatically generated/updated from `Assets/src/Systems/_test/DebugResearchBootstrap.cs`._

1) Purpose
- Defines a debugging bootstrap MonoBehaviour (ResearchBootstrap) for initializing and wiring the research system at runtime.
- Creates and wires ResearchService, ResearchUnlockRegistry, and ResearchEventBridge; loads/initializes state from provided defs.
- Exposes runtime references (Service, Registry, Bridge, State) and inspector-facing references (treeDef, nodeDefs, mapView) for debugging and UI.

2) Public API
- Namespace/module: none

- Types
  - public sealed class ResearchBootstrap : MonoBehaviour
    - Public fields/properties
      - public ResearchService Service { get; private set; } — Runtime service instance
      - public ResearchUnlockRegistry Registry { get; private set; } — Runtime registry instance
      - [SerializeField] public ResearchEventBridge Bridge { get; private set; } — Bridge to propagate events
      - public ResearchState State { get; private set; } — Runtime state
      - public ResearchMapView mapView; — Debug UI view (assigned by Inspector or code)
      - public ResearchTreeDef TreeDef => treeDef; — Public alias to inspector field
    - Public methods
      - public bool SetActiveResearch(string nodeId) => Service.SetActive(nodeId);
      - public void WaveCompleted() => Bridge.OnWaveCompleted();
      - public void MapCompleted(MapDifficulty difficulty) => Bridge.OnMapCompleted(difficulty);
      - public void EnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank) => Bridge.OnEnemyKilled(enemyTags, rank);

3) Key Behavior & Side Effects
- Awake flow
  - State = new ResearchState();
  - Service = new ResearchService();
  - Registry = new ResearchUnlockRegistry();
  - Bridge = new ResearchEventBridge(Service);
  - Logs errors/warnings if treeDef or nodeDefs are missing/unassigned
  - Service.InitFromTree(treeDef, State);
  - Registry.RebuildFrom(nodeDefs, State.completedNodeIds);
  - Service.OnNodeCompleted += (nodeId, unlocks) => Registry.ApplyNodeUnlocks(nodeId, unlocks);
  - Debug log indicating readiness and node count
  - mapView.service = Service; // wires runtime service to UI
- Start flow
  - mapView.initHUD();
- Debug context menu actions (only usable in Play mode)
  - Debug_CompleteWave: calls WaveCompleted() and logs
  - Debug_CompleteMap: calls MapCompleted(debugMapDifficulty) and logs
  - Debug_KillEnemy: calls EnemyKilled(debugEnemyTags, debugEnemyRank) and logs
- Public API forwards
  - SetActiveResearch bubbles to Service.SetActive
  - WaveCompleted/MapCompleted/EnemyKilled bubble to Bridge handlers

4) Constraints & Failure Modes
- Guards/inspections in Awake
  - If treeDef is null: logs error (does not crash immediately)
  - If nodeDefs is null or empty: logs warning
- Potential null-reference risks
  - mapView is assumed non-null when assigning mapView.service and when Start calls mapView.initHUD()
  - Service, Registry, Bridge are created before use; rely on treeDef and nodeDefs for meaningful initialization
- Debug actions gated to Play mode (Application.isPlaying) to avoid in-editor side effects
- Behavior depends on external types (DebugManager, CHAL.* types); not defined in this file

5) Example
- Not provided (not clearly derivable from this single file without external context)

6) Unknowns
- Exact behavior/details of DebugManager.Log, and Debug levels
- Internal implementations of ResearchService, ResearchUnlockRegistry, ResearchEventBridge, and ResearchState (beyond their method calls here)
- Effects of Registry.RebuildFrom with the provided nodeDefs and State.completedNodeIds
- Any additional serialization behavior for the [SerializeField] Bridge property on a public property
- Any lifecycle interactions beyond Awake/Start (e.g., OnDestroy cleanup) not shown in this file
