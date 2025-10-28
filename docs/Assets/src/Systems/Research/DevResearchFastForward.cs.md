# Assets/src/Systems/Research/DevResearchFastForward.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a MonoBehaviour for fast-forwarding research progress in a Unity game during development.

# Public API
- Namespace: None
- Types
  - public sealed class DevResearchFastForward : MonoBehaviour
    - Public fields/properties:
      - ResearchMapView mapView: Reference to the research map view.
      - ResearchTreeDef treeDef: Definition of the research tree.
      - ResearchEventBridge bridge: Bridge for research events.
      - bool completeAllOnPlay: Complete all research on play.
      - int completeUpToStage: Complete research up to a specific stage.
      - List<string> extraNodeIds: Additional node IDs to complete.
      - MapDifficulty fallbackDifficulty: Fallback difficulty for map requirements.
      - EnemyRank fallbackKillRank: Fallback rank for enemy kills.
      - List<string> fallbackKillTags: Tags for fallback enemy kills.
      - bool rebuildMapAfterApply: Rebuild the map after applying changes.
    - Public methods:
      - void Ctx_CompleteAll(): Completes all research nodes.
      - void Ctx_CompleteUpToStage(): Completes research up to a specified stage.
      - void Ctx_CompleteExtra(): Completes additional specified research nodes.
      - void SaveCheatedResearchProgress(): Saves the current research progress.

# Key Behavior & Side Effects
- On Start, attempts to resolve dependencies and apply cheats after a brief wait.
- If conditions are met, applies research completion cheats and optionally rebuilds the map.
- Provides context menu options for completing all nodes, up to a stage, or specific nodes.

# Constraints & Failure Modes
- Requires valid references for mapView, service, and bridge to function correctly.
- If no actions are specified (completeAllOnPlay is false, completeUpToStage < 0, and extraNodeIds is empty), no operations are performed.
- Logs warnings if required components are not found or if invalid parameters are provided.

# Example
```csharp
// Example usage in Unity Editor context menu
DevResearchFastForward fastForward = new DevResearchFastForward();
fastForward.Ctx_CompleteAll(); // Completes all research nodes
```

# Unknowns
- Specific implementation details of ResearchMapView, ResearchTreeDef, ResearchEventBridge, and ResearchService cannot be determined from this file.
```
