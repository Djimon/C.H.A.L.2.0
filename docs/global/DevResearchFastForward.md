# global.DevResearchFastForward

_Automatically generated/updated from `Assets/src/Systems/Research/DevResearchFastForward.cs`._

# DevResearchFastForward.cs

## Purpose
- Provides a development tool for fast-forwarding research progress in the game.
- Allows completion of research nodes based on various conditions during play mode.

## Public API
- Namespace: None specified.

- Types
  - `public sealed class DevResearchFastForward : MonoBehaviour`
    - Public fields/properties:
      - `public ResearchMapView mapView;`
      - `public ResearchTreeDef treeDef;`
      - `public ResearchEventBridge bridge;`
      - `public bool completeAllOnPlay;`
      - `public int completeUpToStage;`
      - `public List<string> extraNodeIds;`
      - `public MapDifficulty fallbackDifficulty;`
      - `public EnemyRank fallbackKillRank;`
      - `public List<string> fallbackKillTags;`
      - `public bool rebuildMapAfterApply;`
    - Public methods:
      - `private void Start();`
      - `private IEnumerator WaitAndMaybeApply();`
      - `private bool TryResolve();`
      - `private void ApplyCheats();`
      - `private void Ctx_CompleteAll();`
      - `private void Ctx_CompleteUpToStage();`
      - `private void Ctx_CompleteExtra();`
      - `private void SaveCheatedResearchProgress();`
      - `private void Post(int ops);`
      - `private int CompleteAll();`
      - `private int CompleteUpToStage(int stage);`
      - `private int CompleteIds(List<string> ids);`
      - `private int CompleteNode(string nodeId);`

## Key Behavior & Side Effects
- On `Start`, initiates a coroutine to resolve dependencies and potentially apply cheats.
- Attempts to resolve `mapView`, `treeDef`, and `bridge` references.
- Applies cheats based on conditions set in public fields.
- Rebuilds the research map if specified after applying cheats.
- Logs operations performed during cheat application.

## Constraints & Failure Modes
- If `mapView`, `treeDef`, or `bridge` cannot be resolved, the operation is aborted.
- No operations are performed if `completeAllOnPlay`, `completeUpToStage`, and `extraNodeIds` are not set.
- Handles null or empty lists for `extraNodeIds` gracefully.
- Assumes that the `ResearchService` and `ResearchEventBridge` are correctly initialized before use.

## Example
```csharp
// Example usage in Unity
DevResearchFastForward fastForward = gameObject.AddComponent<DevResearchFastForward>();
fastForward.completeAllOnPlay = true;
fastForward.Start();
```

## Unknowns
- Specific behavior of `ResearchService`, `ResearchMapView`, and `ResearchEventBridge` cannot be determined from this file.
- The exact structure of `ResearchTreeDef` and its requirements is not defined in this file.

