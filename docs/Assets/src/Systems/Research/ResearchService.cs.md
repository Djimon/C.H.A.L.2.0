# Assets/src/Systems/Research/ResearchService.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `ResearchService` class for managing research nodes and their progress in a game.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchService`
    - Public fields/properties:
      - `event Action<string, IReadOnlyList<ResearchUnlock>> OnNodeCompleted`: Triggered when a research node is completed.
    - Public methods:
      - `public void InitFromTree(ResearchTreeDef treeDef, ResearchState state)`: Initializes the service with a research tree and state.
      - `public bool IsNodeAvailable(string nodeId)`: Checks if a research node is available for activation.
      - `public string GetActiveNodeId()`: Returns the ID of the currently active research node.
      - `public bool IsCompleted(string nodeId)`: Checks if a research node is completed.
      - `public NodeProgress GetNodeProgress(string nodeId)`: Retrieves the progress of a specific research node.
      - `public ResearchNodeDef GetNodeDef(string nodeID)`: Gets the definition of a research node by ID.
      - `public float GetNodeProgress01(string nodeId)`: Returns the progress of a node as a float between 0 and 1.
      - `public bool SetActive(string nodeId)`: Sets a research node as active.
      - `public void ClearActive()`: Clears the currently active research node.
      - `public void ApplyWaveCompleted()`: Records the completion of a wave for the active node.
      - `public void ApplyMapCompleted(MapDifficulty difficulty)`: Records the completion of a map for the active node.
      - `public void ApplyEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)`: Records the killing of an enemy for the active node.

# Key Behavior & Side Effects
- Initializes internal state and structures from a `ResearchTreeDef` and `ResearchState`.
- Tracks progress of research nodes and checks completion based on defined requirements.
- Triggers events when nodes are completed, allowing for further actions (e.g., unlocking features).

# Constraints & Failure Modes
- Methods like `SetActive` and `IsNodeAvailable` return false if the node ID is invalid, completed, or unavailable.
- `GetNodeProgress01` returns 0 if the node is not completed and has no requirements.
- Handles null or empty inputs gracefully, returning default values or no action.

# Example
```csharp
var researchService = new ResearchService();
researchService.InitFromTree(researchTreeDef, researchState);
if (researchService.IsNodeAvailable("nodeId"))
{
    researchService.SetActive("nodeId");
}
```

# Unknowns
- The structure and contents of `ResearchTreeDef`, `ResearchState`, `ResearchNodeDef`, and `ResearchUnlock` are not defined in this file.
- The behavior of `DebugManager.Log` is not detailed in this file.
```
