# global.DevResearchFastForward

_Automatically generated/updated from `Assets/src/Systems/Research/DevResearchFastForward.cs`._

1) Purpose
- DevResearchFastForward is a MonoBehaviour (Unity editor/development builds) that simulates progress in the research system by emitting research-related events through a bridge/service, for debugging or rapid testing.
- It wires optional references (ResearchMapView, ResearchTreeDef, ResearchEventBridge) and provides play-time and heuristic options to auto-complete nodes or stages, plus extra node IDs.
- After applying cheats, it can rebuild and center the map view if configured.

2) Public API
- Namespace/module: none declared (no explicit namespace in this file)

- Types
  - public sealed class DevResearchFastForward : MonoBehaviour
    - Public fields
      - ResearchMapView mapView; // Wiring (optional)
      - ResearchTreeDef treeDef; // Wiring (optional)
      - ResearchEventBridge bridge; // Wiring (optional)

      - bool completeAllOnPlay = false; // Play-mode: complete all nodes
      - int completeUpToStage = -1; // Play-mode: complete up to this stage (inclusive)
      - List<string> extraNodeIds = new List<string>(); // Play-mode: additional node IDs to complete

      - MapDifficulty fallbackDifficulty = MapDifficulty.Stable; // Heuristic: default map difficulty
      - EnemyRank fallbackKillRank = EnemyRank.Normal; // Heuristic: default kill rank
      - List<string> fallbackKillTags = new List<string>(); // Heuristic: tags to consider for kills

      - bool rebuildMapAfterApply = true; // After applying cheats, rebuild/center the map if possible

    - Private/internal fields
      - ResearchService _service;
      - ResearchTreeDef _tree;

    - Public methods: none
      - (All executable behavior is via private methods and editor context menu hooks)

3) Key Behavior & Side Effects
- Start() starts a coroutine WaitAndMaybeApply().
- WaitAndMaybeApply() behavior:
  - Polls up to 60 frames for TryResolve() to succeed (to allow bootstrap/initialization to complete).
  - If _service or bridge is null after waiting, logs a warning and aborts.
  - If no auto-cheat conditions are set (completeAllOnPlay is false, completeUpToStage < 0, and extraNodeIds is empty/null), aborts (no action).
  - Calls ApplyCheats() to perform requested progress simulations.
  - If rebuildMapAfterApply and mapView is assigned, rebuilds and recenters the map.
- TryResolve() behavior:
  - If mapView is null, tries to locate a ResearchMapView via FindFirstObjectByType<ResearchMapView>().
  - If mapView is found, assigns _service = mapView.serviceRef; if treeDef is null, assigns treeDef = mapView.treeDef.
  - If bridge is null, creates new ResearchEventBridge(_service).
  - Assigns _tree = treeDef and returns true if _service and bridge are non-null.
- ApplyCheats() behavior:
  - Accumulates a count of operations performed.
  - If completeAllOnPlay is true and _tree is non-null, runs CompleteAll().
  - If completeUpToStage >= 0 and _tree is non-null, runs CompleteUpToStage(completeUpToStage).
  - If extraNodeIds is non-empty, runs CompleteIds(extraNodeIds).
  - Logs the number of operations performed.
- Context menu actions (for Play-mode debugging/tools):
  - DEV/Complete ALL Now: Resolve then run CompleteAll() and Post(ops).
  - DEV/Complete Up To Stage Now: Validates completeUpToStage, resolves, runs CompleteUpToStage(completeUpToStage), Post(ops).
  - DEV/Complete Extra IDs Now: Resolves, runs CompleteIds(extraNodeIds), Post(ops).
  - DEV/Save cheated progress: Saves current cheating progress snapshot to persistent storage.
- Post(int ops) behavior:
  - Logs the number of operations applied.
  - If rebuildMapAfterApply and mapView exists, rebuilds and recenters the map.
- CompleteAll() behavior:
  - Compiles the research tree via ResearchTreeCompiler.Compile(_tree).
  - Iterates through all node IDs in the compiled structure and calls CompleteNode(id) for each, summing returned operation counts.
- CompleteUpToStage(int stage) behavior:
  - Compiles the tree; iterates over compiled.posById (nodeId, positionInfo); if position.stage <= stage, calls CompleteNode(id).
- CompleteIds(List<string> ids) behavior:
  - Iterates IDs and calls CompleteNode(id) for each.
- CompleteNode(string nodeId) behavior (core logic):
  - Skips if nodeId is null/empty or already completed.
  - Attempts to activate the node via _service.SetActive(nodeId); if this fails, returns 0.
  - Retrieves node definition and requirements; if missing, returns 0.
  - For each requirement category, emits simulated events through bridge to satisfy requirements:
    - Waves: emit OnWaveCompleted as many times as needed.
    - Maps per Difficulty: for each mapRequirement, emit OnMapCompleted(difficulty) as needed.
    - Maps total: emit OnMapCompleted(fallbackDifficulty) to fulfill remaining maps.
    - Elites/Bosses: emit OnEnemyKilled for Elite/Boss as needed, using fallbackKillTags and corresponding ranks.
    - Kills by Tag: for each tag-count requirement, emit OnEnemyKilled with a tag set (ensuring the tag is present in the tags list) and using fallbackKillRank.
    - Kills general: emit OnEnemyKilled with fallbackKillTags and fallbackKillRank as needed.
  - Returns 1 to count one operation for this node.
- Notes:
  - The service marks the node as completed when its requirements are fulfilled (logic in the service); DevFastForward may leave a node still active if its requirements are slightly unmet, but Next operation will catch up.
  - The exact behavior relies on external systems: ResearchMapView, ResearchTreeCompiler, ResearchEventBridge, and ResearchService.

4) Constraints & Failure Modes
- Build constraints: This file is compiled only in UNITY_EDITOR or DEVELOPMENT_BUILD.
- Dependency assumptions: Requires a ResearchMapView to provide a serviceRef and treeDef, or finds one via FindFirstObjectByType. If not found, resolution fails gracefully with a log.
- Null handling: Many paths guard against nulls and early-return 0 when a node cannot be completed due to missing data.
- Asynchrony: Start() uses a coroutine to wait a few frames for initialization; actions may depend on initialization order.
- Side effects: Emulates gameplay events (waves, maps, kills) via the ResearchEventBridge, thereby advancing node progress and potentially completing nodes.
- Performance: Completeness logic compiles the tree on each Complete* call; repeated calls may be expensive for large trees.
- Safety: SaveCheatedResearchProgress writes a snapshot to persistent storage; usage is optional and controlled via context menu.

5) Example
- Minimal usage scenario:
  - Attach DevResearchFastForward to a GameObject in a development scene.
  - In Inspector, optionally assign:
    - mapView: your ResearchMapView instance
    - treeDef: your ResearchTreeDef (or leave to be inferred via mapView)
    - bridge: (optional) leave null to auto-create
    - completeAllOnPlay: true (to auto-complete all nodes when Play starts)
    - completeUpToStage: -1 (not used if completeAllOnPlay is true)
    - extraNodeIds: [] (empty)
    - fallbackDifficulty: MapDifficulty.Stable
    - fallbackKillRank: EnemyRank.Normal
    - fallbackKillTags: []
    - rebuildMapAfterApply: true
  - Enter Play mode to have the script auto-apply all progress and rebuild/center the map.
  - Alternatively, in Play mode, use the component’s context menu items (DEV/Complete ALL Now, DEV/Complete Up To Stage Now, DEV/Complete Extra IDs Now) to trigger specific cheats during runtime.
  - Optional: use DEV/Save cheated progress to persist the cheated state.

6) Unknowns
- Exact behavior of external types (ResearchMapView, ResearchTreeDef, ResearchEventBridge, ResearchService, ResearchTreeCompiler) is not defined here; their internal APIs determine precise side effects of OnWaveCompleted, OnMapCompleted, OnEnemyKilled, and the node completion state.
- The FindFirstObjectByType helper is not defined in this file; its behavior and performance characteristics are not known from this snippet.
- The exact impact of repeatedly running CompleteAll/CompleteUpToStage/CompleteIds in sequence on already-advanced nodes depends on the underlying ResearchService semantics.
