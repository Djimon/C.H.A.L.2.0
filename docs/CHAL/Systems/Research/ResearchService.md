# CHAL.Systems.Research.ResearchService

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchService.cs`._

```
1) Purpose
- Provide in-memory management of a research tree: compile, index, and track node progress, completion, and unlocks.
- Maintain per-node progress and active node, and expose methods to apply game events that advance progress.
- Emit events on node completion and when always-unlocked IDs are ready for UI/UX integration.

2) Public API
- Namespace/module
  - CHAL.Systems.Research

- Types
  - public sealed class ResearchService
    - Public events
      - public event Action<string, IReadOnlyList<ResearchUnlock>> OnNodeCompleted
        - Invoked when a node finishes; passes node ID and its unlocks.
      - public event Action<IReadOnlyList<string>> OnAlwaysUnlockedReady
        - Invoked after InitFromTree with the Always-Unlocked IDs (if any).
    - Public methods
      - public void InitFromTree(ResearchTreeDef treeDef, ResearchState state)
        - Initializes internal structures from a tree definition; state can be null (new ResearchState created).
      - public bool IsNodeAvailable(string nodeId)
        - Returns true if the node exists, is not completed, and all required parents are completed (if compiled parents exist).
      - public string GetActiveNodeId()
        - Returns the currently active node ID (may be null).
      - public bool IsCompleted(string nodeId)
        - Returns true if the node has been completed.
      - public NodeProgress GetNodeProgress(string nodeId)
        - Returns progress data for the given node (or a new default if none tracked yet).
      - public ResearchNodeDef GetNodeDef(string nodeID)
        - Returns the node definition for the given ID (or null if not found).
      - public float GetNodeProgress01(string nodeId)
        - Returns 0..1 progress ratio for the node, or 0 if not progressable; accounts for waves, maps, difficulties, kills, elites/bosses.
      - public bool SetActive(string nodeId)
        - Sets the active node if valid; returns true on success, false otherwise.
      - public void ClearActive()
        - Clears the active node.
      - public void ApplyWaveCompleted()
        - Applies a completed wave to the currently active node and reconciles completion.
      - public void ApplyMapCompleted(MapDifficulty difficulty)
        - Applies a completed map (with difficulty) to the active node and reconciles completion.
      - public void ApplyEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)
        - Applies a kill event (with optional tags and rank) to the active node; updates counters and weighted tallies; may trigger completion.
  - (No other public types defined in this file)

3) Key Behavior & Side Effects
- InitFromTree
  - Compiles treeDef via ResearchTreeCompiler.Compile(treeDef).
  - Populates _nodesById and _idsByLaneStage; sorts lane/stage slots deterministically.
  - Ensures progress entries exist for all nodes.
  - Stores compiledParents for later IsNodeAvailable checks.
  - Logs initialization and fires OnAlwaysUnlockedReady if there are always-unlocked IDs.
- IsNodeAvailable
  - Node must exist, not be completed, and all compiled parent IDs must be completed (if _compiledParents contains the node).
- Progress handling
  - EnsureProgress creates/returns per-node progress entry in _state.perNodeProgress.
  - GetNodeProgress01 derives a 0..1 progress value from waves, maps, per-difficulty maps, kills, elites, and bosses.
- Activation flow
  - SetActive enforces non-empty ID, existence, not completed, and availability; sets _state.activeNodeId and logs.
  - ClearActive clears the active node and logs.
- Event inputs
  - ApplyWaveCompleted increments waves and triggers TryComplete for the active node.
  - ApplyMapCompleted increments mapsTotal and per-difficulty map counts; triggers TryComplete.
  - ApplyEnemyKilled updates kill counters (general or by tag, weighted by rank) and triggers TryComplete if any tag matched.
- Completion logic
  - TryComplete marks a node completed if MeetsRequirements is true, adds to _state.completedNodeIds, logs, and fires OnNodeCompleted with the node's unlocks.
  - MeetsRequirements evaluates the node’s requirements (waves, maps, mapRequirements, killsGeneral, killsByTag, eliteCount, bossCount) against current progress.
- Notes
  - Progress and completion decisions are driven solely by node definitions (ResearchNodeDef) and per-node progress (NodeProgress) as wired by InitFromTree and Apply* methods.

4) Constraints & Failure Modes
- InitFromTree requires a non-null treeDef; state may be null (a new ResearchState is created).
- GetNodeDef returns null if the ID is unknown.
- GetActiveNodeId may return null if no active node is set.
- IsNodeAvailable relies on _compiledParents; if InitFromTree wasn’t called, availability may degrade to “not available” unless other state dictates it.
- Threading: no explicit synchronization; single-threaded usage assumed.
- Logging side effects occur via DebugManager/Unity logs during state changes.
- Unknown external types (ResearchTreeDef, ResearchState, NodeProgress, ResearchNodeDef, ResearchUnlock, MapDifficulty, EnemyRank, ResearchUnlock) are assumed to be defined elsewhere; their exact structures are not shown here.

5) Example
```csharp
// Minimal usage example (assuming treeDef is provided from elsewhere)
var service = new CHAL.Systems.Research.ResearchService();
service.InitFromTree(treeDef, new CHAL.Data.ResearchState());

service.OnAlwaysUnlockedReady += ids =>
{
    // bridge UI to know which IDs are always unlocked
};

service.OnNodeCompleted += (nodeId, unlocks) =>
{
    // apply unlocks to UI/UX, etc.
};

// Activate a node and simulate progress
service.SetActive("node-01");
service.ApplyWaveCompleted();
```

6) Unknowns
- Exact structures of ResearchNodeDef, ResearchRequirement, ResearchUnlock, ResearchTreeDef, NodeProgress, and ResearchUnlocks.
- Behavior/details of ResearchTreeCompiler.Compile(treeDef) output (schemas of nodesById, posById, and parentsById).
- Semantics of AlwaysUnlocked IDs in treeDef (beyond their derived list and event).
- Any multithreading implications or Unity-specific lifecycle coupling beyond the provided code.
- How NodeProgress is serialized/deserialized and how it interacts with persistence beyond this file.
