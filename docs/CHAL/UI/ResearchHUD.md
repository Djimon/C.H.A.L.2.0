# CHAL.UI.ResearchHUD

_Automatically generated/updated from `Assets/src/UI/ResearchHUD.cs`._

# Purpose
- Defines the `ResearchHUD` class for managing the research user interface in the game.

# Public API
- Namespace: `CHAL.UI`
- Types
  - public sealed class `ResearchHUD` : MonoBehaviour
    - Public fields/properties:
      - `ResearchService Service`: The research service to interact with.
      - `ResearchUIThemeDef Theme`: The theme applied to the UI.
    - Public methods:
      - `void Init(ResearchService service, ResearchUIThemeDef theme)`: Initializes the research service with the specified theme.
      - `void RefreshActive()`: Refreshes the active state of the service and updates UI elements.
      - `void ShowDetails(string nodeId)`: Displays details of a node identified by the given node ID.
      - `void HideDetails()`: Hides the detail panel.
      - `bool IsPointerOverUI(Vector2 screenPos)`: Checks if the pointer is currently over a UI element based on screen position.

# Key Behavior & Side Effects
- `Awake()`: Initializes UI elements and hides details on startup.
- `Init()`: Sets the `Service` and `Theme`, then refreshes the active state.
- `RefreshActive()`: Updates UI elements based on the current active node; logs a warning if the active node is not found.
- `ShowDetails()`: Displays details for a specified node; enables/disables the run button based on node availability.
- `HideDetails()`: Hides the detail panel and resets the selected node ID.
- `OnRunClicked()`: Sets the active node and refreshes the UI if successful.
- `IsPointerOverUI()`: Determines if the pointer is over the detail panel or active box.

# Constraints & Failure Modes
- `Service` must be initialized before calling `RefreshActive()` or `ShowDetails()`.
- Handles null or empty node IDs gracefully by hiding details.
- UI updates are contingent on the state of the `Service` and the validity of node IDs.

# Example
```csharp
ResearchHUD researchHUD = new ResearchHUD();
researchHUD.Init(researchServiceInstance, researchUIThemeDefInstance);
researchHUD.RefreshActive();
researchHUD.ShowDetails("nodeIdExample");
```

# Unknowns
- The exact structure of `ResearchService`, `ResearchNodeDef`, and `ResearchUIThemeDef` cannot be determined from this file.
- The behavior of `DebugManager.Log` is not defined in this file.

