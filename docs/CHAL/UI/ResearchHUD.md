# CHAL.UI.ResearchHUD

_Automatically generated/updated from `Assets/src/UI/ResearchHUD.cs`._

1) Purpose
- Unity MonoBehaviour that binds to a UIDocument UI to display current active research and its details.
- Exposes API to initialize with a ResearchService and UI theme, refresh the active node, and show/hide node details.
- Provides formatting helpers for unlocks and requirements via a public helper class.

2) Public API
- Namespace/module: CHAL.UI

- Types
  - public sealed class ResearchHUD : MonoBehaviour
    - Public properties
      - public ResearchService Service { get; private set; }
      - public ResearchUIThemeDef Theme { get; private set; }
    - Public methods
      - public void Init(ResearchService service, ResearchUIThemeDef theme)
      - public void RefreshActive()
      - public void ShowDetails(string nodeId)
      - public void HideDetails()
      - public bool IsPointerOverUI(Vector2 screenPos)

  - public static class ResearchUIFormat
    - Public static methods
      - public static string FormatUnlocks(ResearchNodeDef def)
      - public static string FormatRequirements(ResearchNodeDef def)

3) Key Behavior & Side Effects
- Awake lifecycle (Unity)
  - Binds to UIDocument, queries UI elements, and calls HideDetails() to start hidden.
- Init(ResearchService, ResearchUIThemeDef)
  - Stores references and triggers RefreshActive().
- RefreshActive()
  - If Service is null, no-op.
  - Gets active node id; if empty, shows "Keine aktive Forschung" and "0%".
  - If active id exists but def not found, logs a warning and exits.
  - Updates _activeName to node title or id; updates _activePercent to percentage (rounded).
  - If Theme and nodeIconDefault exist, applies as background image to _activeIcon.
- ShowDetails(string nodeId)
  - Stores _selectedNodeId; guards against null Service or empty nodeId.
  - Fetches node def; hides when missing.
  - Enables _detailPanel (if present) and populates:
    - _detailTitle, _detailFlavor, _detailUnlocks, _detailCosts
  - Configures _runButton:
    - Enables if node is available, not completed, and not currently active.
    - Safely unsubscribes and re-subscribes to OnRunClicked to avoid duplicates.
  - Makes detail panel visible (DisplayStyle.Flex) if present.
- HideDetails()
  - Hides the detail panel (DisplayStyle.None) and clears _selectedNodeId.
- OnRunClicked()
  - If Service or _selectedNodeId invalid, no-op.
  - Calls Service.SetActive(_selectedNodeId); on success, RefreshActive() and ShowDetails(_selectedNodeId) to refresh state.
- IsPointerOverUI(Vector2 screenPos)
  - Converts screen coordinates to panel coordinates.
  - If detail panel is visible and contains the point, returns true.
  - If active box contains the point, returns true.
  - Otherwise, returns false.
- Helper formatting
  - FormatUnlocks and FormatRequirements are used to build detail strings for UI, based on the node definition.

4) Constraints & Failure Modes
- Guards nulls/empties in public paths (Service, nodeId, def) to avoid exceptions.
- Safe UI access with null checks on UI elements before acting.
- Run button event wiring: unsubscribes before subscribing to avoid multiple handlers.
- String-building for details uses StringBuilder; returns "—" when there is no data.
- Theme/icon usage guarded by Theme and nodeIconDefault presence.

5) Example
```csharp
// Example usage (in some setup script or another component)
var hud = GetComponent<CHAL.UI.ResearchHUD>();
hud.Init(serviceInstance, themeDef);
hud.ShowDetails("node_fighters");
```

6) Unknowns
- Definitions of ResearchService, ResearchUIThemeDef, and ResearchNodeDef are not provided in this file.
- Exact UI layout and GUIDs beyond the queried element names (root, activeBox, activeIcon, detailPanel, detailTitle, detailFlavor, detailUnlocks, detailCosts, runButton).
- Behavior of DebugManager.Log path and how logs are reported in the editor/runtime.
- Any side effects of ResearchService.GetNodeDef/IsNodeAvailable/SetActive beyond what is used here.
