# Assets/src/Systems/Research/CodexService.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexService.cs`._

# Purpose
- Defines the `CodexService` class for managing research nodes and their progress in a game.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class CodexService`
    - Public fields/properties:
      - `event Action<string, IReadOnlyList<ResearchUnlock>> OnNodeCompleted`
      - `event Action<IReadOnlyList<string>> OnAlwaysUnlockedReady`
    - Public methods:
      - `public void InitFromTree(CodexTreeDef treeDef, CodexState state)`
      - `public bool IsNodeAvailable(string nodeId) -> bool`
      - `public string GetActiveNodeId() -> string`
      - `public bool IsCompleted(string nodeId) -> bool`
      - `public NodeProgress GetNodeProgress(string nodeId) -> NodeProgress`
      - `public CodexNodeDef GetNodeDef(string nodeID) -> CodexNodeDef`
      - `public float GetNodeProgress01(string nodeId) -> float`
      - `public bool SetActive(string nodeId) -> bool`
      - `public void ClearActive()`
      - `public void ApplyWaveCompleted(MapDifficulty difficulty)`
      - `public void ApplyMapCompleted(MapDifficulty difficulty)`
      - `public void ApplyEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)`
      - `public void OnWaveCompleted(int mapId, int waveIndex, MapDifficulty difficulty)`
      - `public void OnMapCompleted(int mapId, MapDifficulty difficultyId)`
      - `public void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> basetags, List<string> bonustags)`
      - `public void OnCraftExecuted(string recipeId)`

# Key Behavior & Side Effects
- Initializes the service with a research tree and state, clearing previous data.
- Tracks the completion of nodes and triggers events when nodes are completed or always unlocked.
- Updates progress based on various actions (waves completed, maps completed, enemies killed).
- Validates node availability and completion based on defined requirements.

# Constraints & Failure Modes
- Methods return false or do nothing if provided IDs are invalid or if nodes are already completed.
- Requires valid `CodexTreeDef` and `CodexState` for initialization.
- Handles null or empty values gracefully in various checks.

# Example
```csharp
var codexService = new CodexService();
codexService.InitFromTree(researchTreeDef, null);
if (codexService.IsNodeAvailable("nodeId"))
{
    codexService.SetActive("nodeId");
}
```

# Unknowns
- The structure and contents of `CodexTreeDef`, `CodexNodeDef`, `NodeProgress`, and `ResearchUnlock` are not defined in this file.
- The behavior of `DebugManager.Log` is not specified.

