# Assets/src/Systems/Research/CodexState.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexState.cs`._

# Purpose
- Defines the `NodeProgress` and `CodexState` classes for tracking research progress in a game system.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - **public sealed class NodeProgress**
    - Public fields/properties:
      - `int waves`: Number of waves.
      - `int mapsTotal`: Total number of maps.
      - `Dictionary<MapDifficulty, int> mapsByDifficulty`: Maps categorized by difficulty.
      - `int killsGeneralWeighted`: Total weighted kills.
      - `Dictionary<string, int> killsByTagWeighted`: Weighted kills categorized by tag.
      - `int eliteCount`: Count of elite enemies.
      - `int bossCount`: Count of boss enemies.
      - `internal int champCount`: Count of champion enemies.
  - **public sealed class CodexState**
    - Public fields/properties:
      - `string activeNodeId`: ID of the currently active node.
      - `HashSet<string> completedNodeIds`: Set of completed node IDs.
      - `Dictionary<string, NodeProgress> perNodeProgress`: Progress data for each node.

# Key Behavior & Side Effects
- `NodeProgress` tracks various statistics related to game progress, including waves, maps, kills, and enemy counts.
- `CodexState` maintains the current state of research, including which node is active and which nodes have been completed.

# Constraints & Failure Modes
- Uses `StringComparer.Ordinal` for case-sensitive string comparisons in dictionaries and hash sets.
- `champCount` is marked as internal, limiting its accessibility to the assembly.

# Example
```csharp
var codexState = new CodexState
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
- None.
