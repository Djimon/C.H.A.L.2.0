# CHAL.Systems.Research.ResearchService

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchService.cs`._

# Purpose
- Defines the `ResearchService` class for managing research nodes and their progress within a research tree.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchService`
    - Public fields/properties:
      - `event Action<string, IReadOnlyList<ResearchUnlock>> OnNodeCompleted`: Triggered when a research node is completed.
      - `event Action<IReadOnlyList<string>> OnAlwaysUnlockedReady`: Triggered when always-unlocked IDs are ready.
    - Public methods:
      - `public void InitFromTree(ResearchTreeDef treeDef, ResearchState state)`: Initializes the service with a research tree and state.
      - `public bool IsNodeAvailable(string nodeId)`: Checks if a node is available.
      - `public string GetActiveNodeId()`: Retrieves the ID of the currently active node.
      - `public bool IsCompleted(string nodeId)`: Checks if a node is completed.
      - `public NodeProgress GetNodeProgress(string nodeId)`: Retrieves the progress of a node.
      - `public ResearchNodeDef GetNodeDef(string nodeID)`: Retrieves the definition of a node.
      - `public float GetNodeProgress01(string nodeId)`: Calculates the progress of a node as a float.
      - `public bool SetActive(string nodeId)`: Sets the active research node.
      - `public void ClearActive()`: Clears the currently active research node.
      - `public void ApplyWaveCompleted()`: Marks the completion of the wave process.
      - `public void ApplyMapCompleted(MapDifficulty difficulty)`: Applies the completion of a map.
      - `public void ApplyEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)`: Applies the effects of an enemy being killed.

# Key Behavior & Side Effects
- Initializes the research service with a tree definition and state, compiling nodes and ensuring progress entries.
- Activates a node if it is available and not completed, logging the action.
- Completes nodes based on various requirements, triggering events upon completion.

# Constraints & Failure Modes
- Methods return false if inputs are invalid (e.g., null or empty node IDs).
- Node availability checks depend on the completion status of parent nodes.
- Progress is ensured for each node upon initialization.

# Example
```csharp
var researchService = new ResearchService();
researchService.InitFromTree(researchTreeDef, null);
if (researchService.IsNodeAvailable("nodeId"))
{
    researchService.SetActive("nodeId");
}
```

# Unknowns
- The structure and content of `ResearchTreeDef`, `ResearchState`, `NodeProgress`, and `ResearchNodeDef` are not defined in this file.

