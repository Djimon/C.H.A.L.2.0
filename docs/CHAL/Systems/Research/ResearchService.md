# CHAL.Systems.Research.ResearchService

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchService.cs`._

```text
Purpose
- Defines a ResearchService that tracks progress and unlocks for a research tree.
- Builds in-memory mappings from node IDs to definitions and from lane/stage to node IDs; caches compiled parent relationships for availability checks.
- Emits OnNodeCompleted when a node finishes and unlocks are applied.
```

```text
Public API
- Namespace/module: CHAL.Systems.Research
- Types
  - public sealed class ResearchService
    - Public event
      - public event Action<string, IReadOnlyList<ResearchUnlock>> OnNodeCompleted;
    - Public methods
      - public void InitFromTree(ResearchTreeDef treeDef, ResearchState state)
      - public bool IsNodeAvailable(string nodeId)
      - public string GetActiveNodeId()
      - public bool IsCompleted(string nodeId)
      - public NodeProgress GetNodeProgress(string nodeId)
      - public ResearchNodeDef GetNodeDef(string nodeID)
      - public float GetNodeProgress01(string nodeId)
      - public bool SetActive(string nodeId)
      - public void ClearActive()
      - public void ApplyWaveCompleted()
      - public void ApplyMapCompleted(MapDifficulty difficulty)
      - public void ApplyEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)
```

```text
Key Behavior & Side Effects
- InitFromTree
  - Stores treeDef and state (or creates new ResearchState if null).
  - Clears internal caches: _nodesById, _idsByLaneStage.
  - Compiles the tree via ResearchTreeCompiler.Compile(treeDef).
  - Populates _nodesById from compiled.nodesById.
  - Builds _idsByLaneStage from compiled.posById; sorts IDs per (lane, stage) deterministically.
  - Ensures a progress entry exists for every node via EnsureProgress.
  - Stores compiled parents in _compiledParents; used for availability checks.
  - Logs initialization summary.
- IsNodeAvailable
  - Returns false if nodeId not present or node already completed.
  - If _compiledParents exists, returns false if any parent is not completed in _state.completedNodeIds.
  - Otherwise returns true.
- Progress/state
  - EnsureProgress creates/returns NodeProgress for a node and stores it in _state.perNodeProgress.
  - GetActiveNodeId returns _state.activeNodeId.
  - IsCompleted checks _state.completedNodeIds.
  - GetNodeProgress returns per-node progress or a new NodeProgress if missing.
  - GetNodeDef returns the node definition from _nodesById or null if not found.
  - GetNodeProgress01 computes progress as a ratio of earned vs required, across waves, maps, difficulty-specific maps, kills (general and by tag), elites/bosses, and respects completed state.
- Active/Completion flow
  - SetActive validates input, ensures node exists, not completed, and is available; sets _state.activeNodeId and logs.
  - ClearActive clears _state.activeNodeId.
  - ApplyWaveCompleted/ApplyMapCompleted/ApplyEnemyKilled operate on the active node:
    - EnsureProgress(active node) to get/allocate progress.
    - Increment relevant counters (waves, mapsTotal, maps by difficulty, kills by tag, general weighted, elite/boss/champ counters).
    - For ApplyEnemyKilled, accumulate tag-weighted kills if any matching enemyTag is present; otherwise accumulate general weighted.
    - Call TryComplete(def, p) to potentially finish the node.
- Completion handling
  - TryComplete checks if already completed; if not, and MeetsRequirements(def, p) is true:
    - Adds node to _state.completedNodeIds.
    - Logs completion.
    - Invokes OnNodeCompleted with def.id and def.unlocks.
- Internals
  - _rankWeights maps EnemyRank to weighted values for scoring in kills and progress.
  - MeetsRequirements evaluates whether a given NodeProgress satisfies its ResearchNodeDef.requirements (waves, maps, per-difficulty maps, kills, elites, bosses, etc.).
```

```text
Constraints & Failure Modes
- Input validation
  - SetActive returns false for null/empty/whitespace nodeId, non-existent node, already completed, or unavailable node.
- Defensive checks
  - GetNodeDef returns null if node not found.
  - GetNodeProgress returns a new NodeProgress if no stored progress exists.
  - IsNodeAvailable relies on _compiledParents; if absent, availability relies on completed parents as per code.
- State initialization
  - InitFromTree requires a valid treeDef; state may be replaced with a new ResearchState if null.
- Race/threading
  - No explicit threading guarantees; behavior inferred as single-threaded (state mutations occur through public API calls).
- Logging
  - Uses DebugManager for informational logs; side effects are output/logs, not functional state changes.
```

```text
Example
using CHAL.Systems.Research;

public class ExampleUsage
{
    public void Run(ResearchTreeDef treeDef)
    {
        var service = new ResearchService();
        service.InitFromTree(treeDef, new ResearchState());

        // Subscribe to unlocks
        service.OnNodeCompleted += (nodeId, unlocks) =>
        {
            // Handle unlocks, e.g., update UI
        };

        // Choose and activate a node
        if (service.SetActive("node_01"))
        {
            // Simulate progress
            service.ApplyWaveCompleted();
            service.ApplyMapCompleted(MapDifficulty.Normal);
            service.ApplyEnemyKilled(new[] { "enemy_tag_1" }, EnemyRank.Elite);
        }
    }
}
```

```text
Unknowns
- Details of types outside this file:
  - ResearchTreeDef, ResearchNodeDef, ResearchState, NodeProgress, ResearchUnlock, MapDifficulty, EnemyRank, ResearchUnlocks (used in OnNodeCompleted)
  - ResearchTreeCompiler and its Compile method behavior
  - Structure and fields of ResearchNodeDef.requirements and ResearchNodeDef.unlocks
  - Exact contents/shape of ResearchState (activeNodeId, completedNodeIds, perNodeProgress)
  - Implementation specifics of DebugManager
- API surface beyond this file: how UI or other systems consume OnNodeCompleted or react to unlocks
- Any concurrency guarantees or threading implications beyond implicit single-threaded usage in this file
```
