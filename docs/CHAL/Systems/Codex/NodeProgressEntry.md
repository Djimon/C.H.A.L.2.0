# Assets/src/Data/DTO/ResearchSnapShot.cs

_Automatically generated/updated from `Assets/src/Data/DTO/ResearchSnapShot.cs`._

# Purpose
- Defines data transfer objects (DTOs) for saving research progress in the Codex system.

# Public API
- Namespace: `CHAL.Systems.Codex`
- Types
  - `public sealed class NodeProgressSave`
    - `public int waves` - Number of waves completed.
    - `public int mapsTotal` - Total number of maps.
    - `public List<MapRequirement> mapsByDifficulty` - List of maps categorized by difficulty (difficulty, count).
    - `public int killsGeneralWeighted` - Total weighted kills.
    - `public List<KillTagCount> killsByTagWeighted` - List of kills categorized by tag (tag, count).
    - `public int eliteCount` - Count of elite enemies defeated.
    - `public int bossCount` - Count of bosses defeated.
    - `public int championCount` - Count of champions defeated.
  
  - `public sealed class ResearchSnapshot`
    - `public int version` - Version of the snapshot, default is 1.
    - `public string activeNodeId` - ID of the currently active node.
    - `public List<string> completedNodeIds` - List of IDs of completed nodes.
    - `public List<NodeProgressEntry> perNodeProgress` - List of progress entries for each node.

    - `public struct NodeProgressEntry`
      - `public string nodeId` - ID of the node.
      - `public NodeProgressSave progress` - Progress data associated with the node.

# Key Behavior & Side Effects
- The `ResearchSnapshot` class holds the overall state of research progress, including active and completed nodes.
- The `NodeProgressSave` class tracks detailed progress metrics for each node.

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
- The definitions of `MapRequirement` and `KillTagCount` are not provided in this file.
