# CHAL.Systems.Research.ResearchTreeCompiler

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchTreeCompiler.cs`._

```csharp
```
1) Purpose
- Define a data container (ResearchTreeCompiled) for a compiled research tree: node definitions, per-node position (lane/stage), and parent references.
- Provide a compiler (ResearchTreeCompiler) to transform a ResearchTreeDef into a ResearchTreeCompiled by traversing lanes, stages, and node entries.
- Emit diagnostic logs during compilation and gracefully return empty results on null/empty input.

2) Public API
- Namespace/module
  - CHAL.Systems.Research

- Types
  - public sealed class ResearchTreeCompiled
    - Public fields/properties
      - public readonly Dictionary<string, ResearchNodeDef> nodesById
        - Maps node id to the node definition.
      - public readonly Dictionary<string, (int lane, int stage)> posById
        - Maps node id to its lane and stage position.
      - public readonly Dictionary<string, List<string>> parentsById
        - Maps node id to its list of parent node ids.
    - Public constructors
      - public ResearchTreeCompiled(
          Dictionary<string, ResearchNodeDef> nodesById,
          Dictionary<string, (int lane, int stage)> posById,
          Dictionary<string, List<string>> parentsById)

  - public static class ResearchTreeCompiler
    - Public methods
      - public static ResearchTreeCompiled Compile(ResearchTreeDef tree)
        - Builds and returns a ResearchTreeCompiled from a ResearchTreeDef.
    - Private methods
      - private static bool HasCycle(Dictionary<string, List<string>> parentsById)
        - Detects cycles in parent relationships via DFS.

3) Key Behavior & Side Effects
- Initialization
  - Creates empty dictionaries: nodesById, posById, parentsById with ordinal string comparers.
- Null/empty handling
  - If tree == null: logs error and returns empty ResearchTreeCompiled.
  - If tree.researchTreeLanes is null or empty: logs warning and returns empty ResearchTreeCompiled.
- Population loop
  - Iterates lanes and stages; for each entry with a non-null node having a non-empty id:
    - If id already in nodesById: logs error about duplicate Node-ID and skips that entry.
    - Adds node to nodesById, records its position in posById, and builds a parent list in parentsById from entry.parentRefs (skipping null/empty ids).
- Parent validity checks
  - After building, for each node id, retrieves its (lane, stage) and for each parent id:
    - If parent id not found in posById: logs error about missing parent in tree.
    - If parent stage >= child stage: logs error about stage order violation.
- Cycle detection
  - Runs HasCycle(parentsById); if a cycle is detected, logs error about cyclic dependency.
- Return
  - Returns a new ResearchTreeCompiled containing the built dictionaries (even if some checks produced logs).

4) Constraints & Failure Modes
- Guards and null handling
  - Null inputs and empty lane sets are tolerated by returning empty results with logs.
- Data integrity
  - Duplicate node IDs are logged as errors and the duplicate entry is skipped; subsequent entries may still populate other nodes.
  - Parent references are filtered for null/empty ids when building parentsById.
- Ordering/validation
  - Stage-order validation relies on posById; if a parent is missing, an error is logged but compilation continues.
- Cycle detection
  - Uses a DFS-based approach; cycles trigger a logged error.
- Performance/allocations
  - Uses multiple dictionaries with StringComparer.Ordinal; performed in a single pass over the input tree plus a DFS for cycles.

5) Example
- Minimal usage
```csharp
// Assuming 'tree' is a ResearchTreeDef instance available in context
var compiled = ResearchTreeCompiler.Compile(tree);
```

6) Unknowns
- Definitions and structure of ResearchTreeDef and ResearchNodeDef (external to this file).
- Details of DebugManager.Log behavior beyond invocation and categories.
- Any external expectations on the contents/consistency of the input ResearchTreeDef beyond what is explicit here.
