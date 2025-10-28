# Assets/src/Systems/Research/ResearchState.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines data structures for tracking research progress in a game system.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class NodeProgress`
    - Public fields/properties:
      - `int waves`: Number of waves.
      - `int mapsTotal`: Total number of maps.
      - `Dictionary<MapDifficulty, int> mapsByDifficulty`: Maps count categorized by difficulty.
      - `int killsGeneralWeighted`: Weighted count of general kills.
      - `Dictionary<string, int> killsByTagWeighted`: Weighted kills count by tag.
      - `int eliteCount`: Count of elite enemies.
      - `int bossCount`: Count of boss enemies.
      - `internal int champCount`: Count of champion enemies.
  - `public sealed class ResearchState`
    - Public fields/properties:
      - `string activeNodeId`: ID of the currently active research node.
      - `HashSet<string> completedNodeIds`: Set of completed node IDs.
      - `Dictionary<string, NodeProgress> perNodeProgress`: Progress data per node.

# Key Behavior & Side Effects
- `NodeProgress` tracks various metrics related to game progress, including waves, maps, kills, and enemy counts.
- `ResearchState` maintains the current state of research, including active and completed nodes.

# Constraints & Failure Modes
- Uses `StringComparer.Ordinal` for case-sensitive string comparisons in collections.
- `champCount` is marked as internal, limiting its accessibility outside the assembly.

# Example
```csharp
var researchState = new ResearchState
{
    activeNodeId = "node1",
    completedNodeIds = new HashSet<string> { "node0" },
    perNodeProgress = new Dictionary<string, NodeProgress>
    {
        { "node1", new NodeProgress { waves = 5, mapsTotal = 3 } }
    }
};
```

# Unknowns
- No information on how `MapDifficulty` is defined or used.
- No details on how these classes interact with other systems or components.
```
