# CHAL.Systems.Research.NodeProgressEntry

_Automatically generated/updated from `Assets/src/Data/DTO/ResearchSnapShot.cs`._

# Purpose
- Defines data transfer objects (DTOs) for research snapshots in the CHAL system.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - **[Serializable] class** `NodeProgressSave`
    - `int waves`: Number of waves completed.
    - `int mapsTotal`: Total number of maps.
    - `List<MapRequirement> mapsByDifficulty`: List of maps categorized by difficulty.
    - `int killsGeneralWeighted`: Total weighted kills.
    - `List<KillTagCount> killsByTagWeighted`: List of kills categorized by tag.
    - `int eliteCount`: Count of elite enemies defeated.
    - `int bossCount`: Count of bosses defeated.
    - `int championCount`: Count of champions defeated.
  
  - **[Serializable] class** `ResearchSnapshot`
    - `int version`: Version of the snapshot.
    - `string activeNodeId`: ID of the currently active node.
    - `List<string> completedNodeIds`: List of IDs for completed nodes.
    - `List<NodeProgressEntry> perNodeProgress`: List of progress entries for each node.
      - **[Serializable] struct** `NodeProgressEntry`
        - `string nodeId`: ID of the node.
        - `NodeProgressSave progress`: Progress data for the node.

# Key Behavior & Side Effects
- The `ResearchSnapshot` class maintains a versioning system and tracks the active node and completed nodes.
- The `NodeProgressSave` class tracks various metrics related to node progress, including waves, maps, and kills.

# Constraints & Failure Modes
- No explicit guards or null handling are defined in the code.
- Lists are initialized to avoid null references.

# Example
```csharp
var researchSnapshot = new ResearchSnapshot
{
    version = 1,
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
- No information on the behavior or structure of `MapRequirement` and `KillTagCount` classes.
