# CHAL.Systems.Research.ResearchState

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchState.cs`._

```text
1) Purpose
- Defines serializable data structures for tracking research progress.
- NodeProgress stores per-node metrics (waves, maps, kills, elites/bosses).
- ResearchState aggregates per-node progress and global progress markers.

2) Public API
- Namespace: CHAL.Systems.Research
- Types
  - public sealed class NodeProgress
    - public int waves
      - (waves progressed so far)
    - public int mapsTotal
      - (total maps in the node)
    - public Dictionary<MapDifficulty, int> mapsByDifficulty
      - initialized to new Dictionary<MapDifficulty, int>()
      - key = (int)MapDifficulty (per comment)
      - tracks maps count by difficulty
    - public int killsGeneralWeighted
      - (general weighted kill count)
    - public Dictionary<string, int> killsByTagWeighted
      - initialized to new Dictionary<string, int>(StringComparer.Ordinal)
      - tracks kills by tag with ordinal string keys
    - public int eliteCount
      - (count of elites)
    - public int bossCount
      - (count of bosses)
    - internal int champCount
      - (champion-count; internal visibility)
  - public sealed class ResearchState
    - public string activeNodeId
      - (currently active node identifier)
    - public HashSet<string> completedNodeIds
      - initialized to new HashSet<string>(StringComparer.Ordinal)
      - holds IDs of completed nodes
    - public Dictionary<string, NodeProgress> perNodeProgress
      - initialized to new Dictionary<string, NodeProgress>(StringComparer.Ordinal)
      - maps node IDs to per-node progress

3) Key Behavior & Side Effects
- Pure data containers; no methods defined.
- All collections are initialized with explicit empty instances:
  - mapsByDifficulty = new Dictionary<MapDifficulty, int>()
  - killsByTagWeighted = new Dictionary<string, int>(StringComparer.Ordinal)
  - completedNodeIds = new HashSet<string>(StringComparer.Ordinal)
  - perNodeProgress = new Dictionary<string, NodeProgress>(StringComparer.Ordinal)
- Serializable marker indicates support for serialization (e.g., Unity/NET).

4) Constraints & Failure Modes
- MapDifficulty keys rely on external type; not defined in this file.
- String-based dictionaries use ordinal string comparison (StringComparer.Ordinal).
- champCount is internal; not accessible outside the assembly.
- No methods to enforce consistency between perNodeProgress and completedNodeIds; users must manage consistency externally.

5) Example
```csharp
using CHAL.Systems.Research;

var state = new ResearchState
{
    activeNodeId = "node_1"
};
state.completedNodeIds.Add("node_1");
state.perNodeProgress["node_1"] = new NodeProgress
{
    waves = 1,
    mapsTotal = 2,
    eliteCount = 1,
    bossCount = 0
};
```

6) Unknowns
- Definition and values of MapDifficulty enum are not present in this file.
- Serialization behavior beyond [Serializable] (e.g., Unity-specific serialization nuances) is not specified.
- Any higher-level behavior or methods that manipulate these structures are not present in this file.
```
