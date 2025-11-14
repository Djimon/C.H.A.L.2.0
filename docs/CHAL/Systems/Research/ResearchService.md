# Assets/src/Systems/Research/ResearchService.cs

_Automatically generated/updated from `Assets/src/Systems/Research/ResearchService.cs`._

# Purpose
- Defines the `ResearchService` class for managing research nodes and their progress in a game.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchService`
    - Public fields/properties:
      - `event Action<string, IReadOnlyList<ResearchUnlock>> OnNodeCompleted`
      - `event Action<IReadOnlyList<string>> OnAlwaysUnlockedReady`
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
      - `public void ApplyWaveCompleted(MapDifficulty difficulty)`
      - `public void ApplyMapCompleted(MapDifficulty difficulty)`
      - `public void ApplyEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)`
      - `public void OnWaveCompleted(int mapId, int waveIndex, MapDifficulty difficulty)`
      - `public void OnMapCompleted(int mapId, MapDifficulty difficultyId)`
      - `public void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> basetags, List<string> bonustags)`
      - `public void OnCraftExecuted(string recipeId)`

# Key Behavior & Side Effects
- Initializes the research service with a tree definition and state, compiling nodes and ensuring progress entries.
- Checks if a node is available based on its completion status and parent nodes.
- Sets the active node and clears it, logging actions to the debug manager.
- Applies progress updates for waves, maps, and enemy kills, triggering completion checks.
- Handles enemy kills and updates progress based on enemy tags and ranks.

# Constraints & Failure Modes
- Handles null or empty node IDs gracefully, returning false for availability checks and active node settings.
- Requires valid `ResearchNodeDef` and `ResearchState` objects for proper functionality.
- Uses `Mathf.Clamp` to ensure progress values remain within valid ranges.

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
- The structure and contents of `ResearchTreeDef`, `ResearchState`, `ResearchNodeDef`, and `NodeProgress` are not defined in this file.
- The implementation details of `ResearchTreeCompiler.Compile` are not provided.
