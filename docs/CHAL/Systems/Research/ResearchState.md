# Assets/src/Systems/Research/ResearchState.cs

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchState.cs`._

# Purpose
- Defines data structures for tracking research progress in a game system.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - **public sealed class NodeProgress**
    - Public fields/properties:
      - `int waves`: Number of waves.
      - `int mapsTotal`: Total number of maps.
      - `Dictionary<MapDifficulty, int> mapsByDifficulty`: Maps count categorized by difficulty.
      - `int killsGeneralWeighted`: Total weighted kills.
      - `Dictionary<string, int> killsByTagWeighted`: Kills count categorized by tags.
      - `int eliteCount`: Count of elite enemies.
      - `int bossCount`: Count of boss enemies.
      - `internal int champCount`: Count of champion enemies.
  - **public sealed class ResearchState**
    - Public fields/properties:
      - `string activeNodeId`: Identifier for the active research node.
      - `HashSet<string> completedNodeIds`: Set of completed node identifiers.
      - `Dictionary<string, NodeProgress> perNodeProgress`: Progress data for each node.

# Key Behavior & Side Effects
- `NodeProgress` tracks various metrics related to waves, maps, kills, and enemy types.
- `ResearchState` maintains the current active node and a record of completed nodes along with their progress.

# Constraints & Failure Modes
- Uses `StringComparer.Ordinal` for case-sensitive string comparisons in collections.
- No explicit null or empty handling is defined; assumptions on input validity are required.

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
