# CHAL.Systems.Research.NodeProgressEntry

_Automatically generated/updated from `Assets/src/Data/DTO/ResearchSnapShot.cs`._

```csharp
1) Purpose
- Serializable data containers for research snapshot data.
- NodeProgressSave: stores per-node progress metrics (waves, maps by difficulty, kills, and counts).
- ResearchSnapshot: aggregates version, active node, completed nodes, and per-node progress entries.

2) Public API
- Namespace: CHAL.Systems.Research

- public sealed class NodeProgressSave
  - Public fields
    - public int waves
    - public int mapsTotal
    - public List<MapRequirement> mapsByDifficulty = new List<MapRequirement>(); // (difficulty, count)
    - public int killsGeneralWeighted
    - public List<KillTagCount> killsByTagWeighted = new List<KillTagCount>(); // (tag, count)
    - public int eliteCount
    - public int bossCount
    - public int championCount

- public sealed class ResearchSnapshot
  - Public fields
    - public int version = 1
    - public string activeNodeId
    - public List<string> completedNodeIds = new List<string>()
    - public List<NodeProgressEntry> perNodeProgress = new List<NodeProgressEntry>(); // key-value als Liste (JSON-freundlich)

  - public struct NodeProgressEntry
    - Public fields
      - public string nodeId
      - public NodeProgressSave progress

3) Key Behavior & Side Effects
- No methods; pure data containers.
- Default initializers ensure lists are non-null on construction:
  - mapsByDifficulty, killsByTagWeighted, completedNodeIds, perNodeProgress.
- perNodeProgress is intended as a JSON-friendly key-value list of per-node progress.
- NodeProgressEntry is Serializable and used to pair a nodeId with its NodeProgressSave.

4) Constraints & Failure Modes
- activeNodeId may be null if unset (string, no explicit non-null constraint).
- MapRequirement and KillTagCount types are defined elsewhere; their behavior is not defined in this file.
- Unity serialization behavior assumed; no custom constructors provided.

5) Example
- Minimal instantiation of a snapshot with a single node progress entry:

```csharp
var snapshot = new CHAL.Systems.Research.ResearchSnapshot
{
  activeNodeId = "nodeA",
  completedNodeIds = new List<string>(),
  perNodeProgress = new List<CHAL.Systems.Research.ResearchSnapshot.NodeProgressEntry>
  {
    new CHAL.Systems.Research.ResearchSnapshot.NodeProgressEntry
    {
      nodeId = "nodeA",
      progress = new CHAL.Systems.Research.NodeProgressSave()
    }
  }
};
```

6) Unknowns
- Definitions of MapRequirement and KillTagCount (types used but not defined in this file).
- Any additional serialization semantics beyond Unity’s default for these types.
- Relationships or invariants between completedNodeIds and perNodeProgress entries are not specified here.
```
