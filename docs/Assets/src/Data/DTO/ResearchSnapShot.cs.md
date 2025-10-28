# Assets/src/Data/DTO/ResearchSnapShot.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines data transfer objects (DTOs) for research snapshots in the CHAL.Systems.Research namespace.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class NodeProgressSave`
    - Public fields/properties:
      - `public int waves`
      - `public int mapsTotal`
      - `public List<MapRequirement> mapsByDifficulty` (difficulty, count)
      - `public int killsGeneralWeighted`
      - `public List<KillTagCount> killsByTagWeighted` (tag, count)
      - `public int eliteCount`
      - `public int bossCount`
      - `public int championCount`
  - `public sealed class ResearchSnapshot`
    - Public fields/properties:
      - `public int version`
      - `public string activeNodeId`
      - `public List<string> completedNodeIds`
      - `public List<NodeProgressEntry> perNodeProgress`
        - `public struct NodeProgressEntry`
          - Public fields/properties:
            - `public string nodeId`
            - `public NodeProgressSave progress`

# Key Behavior & Side Effects
- None explicitly defined in the file.

# Constraints & Failure Modes
- None explicitly defined in the file.

# Example
```csharp
var snapshot = new ResearchSnapshot {
    activeNodeId = "node_1",
    completedNodeIds = new List<string> { "node_0" },
    perNodeProgress = new List<ResearchSnapshot.NodeProgressEntry> {
        new ResearchSnapshot.NodeProgressEntry {
            nodeId = "node_1",
            progress = new NodeProgressSave {
                waves = 5,
                mapsTotal = 10
            }
        }
    }
};
```

# Unknowns
- No information on the usage or integration of `MapRequirement` and `KillTagCount` types.
```
