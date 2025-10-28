# CHAL.Systems.Research.NodeProgressEntry

_Automatically generated/updated from `Assets/src/Data/DTO/ResearchSnapShot.cs`._

# Purpose
- Defines data transfer objects (DTOs) for research snapshots in a game system.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - **[Serializable] class** `NodeProgressSave`
    - Public fields/properties:
      - `int waves`
      - `int mapsTotal`
      - `List<MapRequirement> mapsByDifficulty` (difficulty, count)
      - `int killsGeneralWeighted`
      - `List<KillTagCount> killsByTagWeighted` (tag, count)
      - `int eliteCount`
      - `int bossCount`
      - `int championCount`
  
  - **[Serializable] class** `ResearchSnapshot`
    - Public fields/properties:
      - `int version` (default is 1)
      - `string activeNodeId`
      - `List<string> completedNodeIds`
      - `List<NodeProgressEntry> perNodeProgress` (key-value list for JSON compatibility)
  
    - **[Serializable] struct** `NodeProgressEntry`
      - Public fields/properties:
        - `string nodeId`
        - `NodeProgressSave progress`

# Key Behavior & Side Effects
- No explicit behavior or side effects defined in this file.

# Constraints & Failure Modes
- No guards, null/empty handling, threading/async notes, or performance hints evident in this file.

# Example
```csharp
var snapshot = new ResearchSnapshot
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
                mapsTotal = 10,
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
- No unknowns identified from this file.

