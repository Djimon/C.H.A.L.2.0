# CHAL.Systems.Research.ResearchService

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchService.cs`._

# Purpose
- Defines the `ResearchService` class for managing research nodes and their progress in a game.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchService`
    - Public fields/properties:
      - `event Action<string, IReadOnlyList<ResearchUnlock>> OnNodeCompleted` - Triggered when a research node is completed.
    - Public methods:
      - `public void InitFromTree(ResearchTreeDef treeDef, ResearchState state)`
      - `public bool IsNodeAvailable(string nodeId)`
      - `public string GetActiveNodeId()`
      - `public bool IsCompleted(string nodeId)`
      - `public NodeProgress GetNodeProgress(string nodeId)`
      - `public ResearchNodeDef GetNodeDef(string nodeID)`
      - `public float GetNodeProgress01(string nodeId)`
      - `public bool SetActive(string nodeId)`
      - `public void ClearActive()`
      - `public void ApplyWaveCompleted()`
      - `public void ApplyMapCompleted(MapDifficulty difficulty)`
      - `public void ApplyEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)`

# Key Behavior & Side Effects
- Initializes the research tree and state from a `ResearchTreeDef`.
- Checks if a node is available based on its completion status and parent nodes.
- Tracks progress for each research node and triggers completion events when requirements are met.
- Updates active node and clears it as needed.

# Constraints & Failure Modes
- Methods return false for invalid inputs (e.g., null or whitespace node IDs).
- Handles null or empty requirements gracefully in completion checks.
- Uses `Dictionary` for efficient lookups of nodes and progress.

# Example
```csharp
var researchService = new ResearchService();
researchService.InitFromTree(treeDef, new ResearchState());
if (researchService.IsNodeAvailable("nodeId")) {
    researchService.SetActive("nodeId");
}
```

# Unknowns
- The structure and contents of `ResearchTreeDef`, `ResearchNodeDef`, `ResearchState`, and `NodeProgress` cannot be determined from this file.
- The implementation details of `ResearchTreeCompiler.Compile` are not provided.

