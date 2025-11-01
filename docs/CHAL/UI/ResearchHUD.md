# CHAL.UI.ResearchHUD

_Automatically generated/updated from `Assets/src/UI/ResearchHUD.cs`._

```text
1) Purpose
- Unity UI HUD component for research: binds to UI elements, shows active research and a detail panel.
- Public API for initialization and UI updates: Init(service, theme), RefreshActive(), ShowDetails(nodeId), HideDetails(), IsPointerOverUI(screenPos).
- Formatting helpers: ResearchUIFormat.FormatUnlocks and ResearchUIFormat.FormatRequirements.

```

```text
2) Public API
- Namespace/module
  - CHAL.UI

- Types
  - public sealed class ResearchHUD : MonoBehaviour
    - Public properties
      - public ResearchService Service { get; private set; }
        - External refs assigned (see Init)
      - public ResearchUIThemeDef Theme { get; private set; }
        - External refs assigned (see Init)
    - Public methods
      - public void Init(ResearchService service, ResearchUIThemeDef theme)
      - public void RefreshActive()
      - public void ShowDetails(string nodeId)
      - public void HideDetails()
      - public bool IsPointerOverUI(Vector2 screenPos)
    - (Private/internal members and methods exist but are not part of the public API surface)

  - public static class ResearchUIFormat
    - public static string FormatUnlocks(ResearchNodeDef def)
    - public static string FormatRequirements(ResearchNodeDef def)

- Unity lifecycle (MonoBehaviour)
  - Awake(): Bind UI elements from UIDocument and hide details initially.

```

```text
3) Key Behavior & Side Effects
- Awake
  - Binds UIDocument and queries key VisualElements/Labels/Buttons.
  - Calls HideDetails() to ensure detail panel starts hidden.
- Init(ResearchService, ResearchUIThemeDef)
  - Stores references to external service and theme.
  - Calls RefreshActive() to reflect current active node.
- RefreshActive()
  - If Service is null, no-op.
  - Retrieves active node ID; if empty, shows:
    - activeName = "Keine aktive Forschung"
    - activePercent = "0%"
  - If an active node exists:
    - Retrieves node definition; if missing, logs a developer warning and returns.
    - Sets activeName to def.title or the node ID.
    - Computes progress (0..1) via GetNodeProgress01(id) and formats as percentage.
    - Optionally applies Theme.nodeIconDefault as the active icon background image if available.
- ShowDetails(string nodeId)
  - Stores _selectedNodeId; returns early with HideDetails() if Service is null or nodeId invalid.
  - Retrieves node def; returns early with HideDetails() if null.
  - Enables detailPanel, fills detail fields:
    - detailTitle from def.title or nodeId
    - detailFlavor from def.desc
    - detailUnlocks via ResearchUIFormat.FormatUnlocks(def)
    - detailCosts via ResearchUIFormat.FormatRequirements(def)
  - Configures run button (if present):
    - canRun = Service.IsNodeAvailable(nodeId) && !Service.IsCompleted(nodeId) && active node is not this node
    - Sets button enabled state; safely rebinds OnRunClicked (unsubscribes then subscribes)
  - Ensures detail panel is visible (DisplayStyle.Flex)
- HideDetails()
  - Hides detail panel (DisplayStyle.None) and clears _selectedNodeId.
- OnRunClicked()
  - If Service is null or no selected node, no-op.
  - If Service.SetActive(_selectedNodeId) succeeds:
    - RefreshActive()
    - ShowDetails(_selectedNodeId) to refresh (button state, etc.)
- IsPointerOverUI(Vector2 screenPos)
  - Converts screen coordinates to panel coordinates.
  - If detail panel is visible and contains the position, returns true.
  - If active box contains the position, returns true.
  - Otherwise, returns false.

```

```text
4) Constraints & Failure Modes
- Null guards
  - RefreshActive exits early if Service is null.
  - ShowDetails exits/hides if nodeId is null/empty or if def is null.
  - Many UI elements (detailPanel, _runButton, etc.) are checked for null before use.
- Logging
  - If the active node definition cannot be found, logs a warning via DebugManager.Log.
- UI state handling
  - _detailPanel.display toggled between Flex and None; relies on DisplayStyle.Flex for visible state.
  - _runButton enabled state derived from multiple service checks; safely (un)binds click handler.
- Theme/asset assumptions
  - Optional: if Theme.nodeIconDefault exists, its texture is used for activeIcon background.
- Performance
  - Formatting relies on ResearchUIFormat methods which build strings only when details are shown.
- Threading/async
  - All UI access occurs on the main thread; no explicit async behavior present.

```

```text
5) Example
- Not derivable from this file in a minimal example form without involving ResearchService/Def types; no standalone runnable example is provided.

```

```text
6) Unknowns
- Definitions of ResearchService, ResearchNodeDef, and ResearchUIThemeDef (structure, fields, behavior) are not shown here.
- Exact contents and types of def.title, def.desc, def.unlocks, and def.requirements are assumed from usage.
- Details of DebugManager.Log, and how the theme texture is provided, are not specified beyond usage.
- Any broader game flow interactions (e.g., persisting active node across scenes) are not present in this file.

```
