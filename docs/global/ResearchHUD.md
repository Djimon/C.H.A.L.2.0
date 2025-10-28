# global.ResearchHUD

_Automatically generated/updated from `Assets/src/UI/ResearchHUD.cs`._

# ResearchHUD.cs

## Purpose
- Defines the `ResearchHUD` class for managing the research user interface in a Unity application.
- Provides methods to initialize the HUD, refresh active research details, and display/hide additional information.

## Public API
- Namespace: None
- Types:
  - **public sealed class ResearchHUD : MonoBehaviour**
    - Public fields/properties:
      - `ResearchService Service { get; private set; }` - Reference to the research service.
      - `ResearchUIThemeDef Theme { get; private set; }` - Reference to the UI theme definition.
    - Public methods:
      - `void Init(ResearchService service, ResearchUIThemeDef theme)` - Initializes the HUD with the provided service and theme.
      - `void RefreshActive()` - Updates the HUD with the currently active research node details.
      - `void ShowDetails(string nodeId)` - Displays details for the specified research node.
      - `void HideDetails()` - Hides the details panel.
      - `bool IsPointerOverUI(Vector2 screenPos)` - Checks if the pointer is over the UI elements.

## Key Behavior & Side Effects
- `Awake()`: Initializes UI elements and hides details on startup.
- `Init()`: Sets the service and theme, then refreshes the active research display.
- `RefreshActive()`: Updates the active research node display; logs a warning if the node is not found.
- `ShowDetails()`: Displays details for a specific research node; enables/disables the run button based on node availability.
- `HideDetails()`: Hides the details panel and clears the selected node ID.
- `OnRunClicked()`: Sets the selected node as active and refreshes the HUD if successful.
- `IsPointerOverUI()`: Determines if the pointer is over the active box or detail panel.

## Constraints & Failure Modes
- `RefreshActive()`: Returns early if `Service` is null or if the active node ID is empty.
- `ShowDetails()`: Hides details if `Service` is null or the node ID is invalid.
- `OnRunClicked()`: Does nothing if `Service` is null or the selected node ID is empty.

## Example
```csharp
ResearchHUD researchHUD = gameObject.AddComponent<ResearchHUD>();
researchHUD.Init(researchServiceInstance, researchUIThemeDefInstance);
```

## Unknowns
- The exact structure and properties of `ResearchService`, `ResearchUIThemeDef`, and `ResearchNodeDef` are not defined in this file.
- The behavior of `DebugManager.Log` is not specified.

