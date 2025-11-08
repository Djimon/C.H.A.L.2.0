# global.DevResearchFastForward

_Automatically generated/updated from `Assets/src/Systems/Research/DevResearchFastForward.cs`._

# Purpose
- Defines a development tool for fast-forwarding research progress in a Unity game.

# Public API
- Namespace: None
- Types
  - public sealed class DevResearchFastForward : MonoBehaviour
    - Public fields/properties:
      - ResearchMapView mapView: Reference to the research map view.
      - ResearchTreeDef treeDef: Definition of the research tree.
      - ResearchEventBridge bridge: Bridge for research events.
      - bool completeAllOnPlay: If true, completes all research on play.
      - int completeUpToStage: Completes research up to a specified stage.
      - List<string> extraNodeIds: Additional node IDs to complete.
      - MapDifficulty fallbackDifficulty: Fallback difficulty for map requirements.
      - EnemyRank fallbackKillRank: Fallback rank for enemy kills.
      - List<string> fallbackKillTags: Tags for fallback enemy kills.
      - bool rebuildMapAfterApply: If true, rebuilds the map after applying changes.
    - Public methods:
      - void Ctx_CompleteAll(): Completes all research nodes immediately.
      - void Ctx_CompleteUpToStage(): Completes research nodes up to a specified stage.
      - void Ctx_CompleteExtra(): Completes additional research nodes by IDs.
      - void SaveCheatedResearchProgress(): Saves the current research progress.

# Key Behavior & Side Effects
- On Start, attempts to resolve dependencies and apply cheats based on the configuration.
- If conditions are met, it completes research nodes and optionally rebuilds the map.
- Uses a coroutine to wait for dependencies to be available before applying changes.

# Constraints & Failure Modes
- If the required services or bridge are not found, the operation is aborted with a warning.
- If no completion conditions are specified, no actions are taken.
- Handles null or empty lists for extra node IDs gracefully.

# Example
```csharp
DevResearchFastForward fastForward = new DevResearchFastForward();
fastForward.completeAllOnPlay = true;
fastForward.Ctx_CompleteAll();
```

# Unknowns
- None.

