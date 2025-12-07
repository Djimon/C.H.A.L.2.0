# Assets/src/Systems/Research/DevResearchFastForward.cs

_Automatically generated/updated from `Assets/src/Systems/Research/DevResearchFastForward.cs`._

# Purpose
- Defines a development tool for fast-forwarding research progress in the game.

# Public API
- Namespace: None
- Types
  - public sealed class DevResearchFastForward : MonoBehaviour
    - Public fields/properties:
      - ResearchMapView mapView: Reference to the research map view.
      - ResearchTreeDef treeDef: Definition of the research tree.
      - bool completeAllOnPlay: If true, completes all research on play.
      - int completeUpToStage: Completes research up to a specified stage.
      - List<string> extraNodeIds: Additional node IDs to complete.
      - MapDifficulty fallbackDifficulty: Default difficulty for map requirements.
      - EnemyRank fallbackKillRank: Default rank for enemy kills.
      - List<string> fallbackKillTags: Tags for fallback enemy kills.
      - bool rebuildMapAfterApply: If true, rebuilds the map after applying changes.
    - Public methods:
      - void Ctx_CompleteAll(): Completes all research nodes immediately.
      - void Ctx_CompleteUpToStage(): Completes research up to the specified stage immediately.
      - void Ctx_CompleteExtra(): Completes additional specified research nodes immediately.
      - void SaveCheatedResearchProgress(): Saves the current research progress.

# Key Behavior & Side Effects
- On Start, attempts to resolve dependencies and apply cheats if conditions are met.
- Applies cheats based on the configuration (complete all, up to stage, or extra IDs).
- Rebuilds the research map after applying changes if specified.
- Logs warnings if the required services or bridges are not found.

# Constraints & Failure Modes
- If `mapView`, `treeDef`, or `bridge` are not resolved, the operation is aborted.
- If no actions are specified (complete all, complete up to stage, or extra IDs), no operations are performed.
- Handles null or empty lists for extra node IDs gracefully.

# Example
```csharp
DevResearchFastForward fastForward = new DevResearchFastForward();
fastForward.completeAllOnPlay = true;
fastForward.Ctx_CompleteAll();
```

# Unknowns
- None.
