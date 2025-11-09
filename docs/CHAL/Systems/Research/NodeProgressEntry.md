# Assets/src/Data/DTO/ResearchSnapShot.cs

_Automatically generated/updated from `Assets/src/Data/DTO/ResearchSnapShot.cs`._

# Purpose
- Defines data transfer objects (DTOs) for research snapshots and node progress in the CHAL system.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class NodeProgressSave`
    - Public fields/properties:
      - `int waves`
      - `int mapsTotal`
      - `List<MapRequirement> mapsByDifficulty` (difficulty, count)
      - `int killsGeneralWeighted`
      - `List<KillTagCount> killsByTagWeighted` (tag, count)
      - `int eliteCount`
      - `int bossCount`
      - `int championCount`
  - `public sealed class ResearchSnapshot`
    - Public fields/properties:
      - `int version` (default is 1)
      - `string activeNodeId`
      - `List<string> completedNodeIds`
      - `List<NodeProgressEntry> perNodeProgress` (key-value list for JSON compatibility)
    - Public struct:
      - `public struct NodeProgressEntry`
        - Public fields/properties:
          - `string nodeId`
          - `NodeProgressSave progress`

# Key Behavior & Side Effects
- The `ResearchSnapshot` class maintains a version number, an active node ID, and lists of completed node IDs and per-node progress entries.

# Constraints & Failure Modes
- No explicit guards or null handling are defined in the code.
- Lists are initialized to avoid null references.

# Example
```csharp
var researchSnapshot = new ResearchSnapshot
{
    activeNodeId = "node_1",
    completedNodeIds = new List<string> { "node_0" },
    perNodeProgress = new List<ResearchSnapshot.NodeProgressEntry>
    {
        new ResearchSnapshot.NodeProgressEntry
        {
            nodeId = "node_1",
            progress = new NodeProgressSave
            {
                waves = 5,
                mapsTotal = 3,
                killsGeneralWeighted = 100,
                eliteCount = 2,
                bossCount = 1,
                championCount = 3
            }
        }
    }
};
```

# Unknowns
- The definitions and structures of `MapRequirement` and `KillTagCount` are not provided in this file.
