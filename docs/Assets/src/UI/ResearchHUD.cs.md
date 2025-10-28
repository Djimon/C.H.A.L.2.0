# Assets/src/UI/ResearchHUD.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `ResearchHUD` class for managing the research user interface in a Unity application.
- Provides methods for initializing the HUD, refreshing active research details, and displaying/hiding detailed information about research nodes.

## Public API
- Namespace: None
- Types
  - `public sealed class ResearchHUD : MonoBehaviour`
    - Public fields/properties:
      - `public ResearchService Service { get; private set; }` - Reference to the research service.
      - `public ResearchUIThemeDef Theme { get; private set; }` - Reference to the UI theme definition.
    - Public methods:
      - `public void Init(ResearchService service, ResearchUIThemeDef theme)` - Initializes the HUD with the given service and theme.
      - `public void RefreshActive()` - Updates the HUD with the currently active research node details.
      - `public void ShowDetails(string nodeId)` - Displays details for the specified research node.
      - `public void HideDetails()` - Hides the details panel.
      - `public bool IsPointerOverUI(Vector2 screenPos)` - Checks if the pointer is over the UI elements.

## Key Behavior & Side Effects
- `Awake()`: Initializes UI elements and hides details on startup.
- `Init()`: Sets the service and theme, then refreshes the active research display.
- `RefreshActive()`: Updates the active research node display; logs a warning if the node is not found.
- `ShowDetails()`: Displays details for a specific node; enables/disables the run button based on node availability.
- `HideDetails()`: Hides the details panel and clears the selected node ID.
- `OnRunClicked()`: Sets the selected node as active and refreshes the HUD if successful.
- `IsPointerOverUI()`: Determines if the pointer is over the active box or detail panel.

## Constraints & Failure Modes
- Methods like `RefreshActive()` and `ShowDetails()` guard against null or empty service/node IDs.
- UI elements are only manipulated if they are not null.
- The run button is disabled if the node is not available or already completed.

## Example
```csharp
ResearchHUD researchHUD = gameObject.AddComponent<ResearchHUD>();
researchHUD.Init(researchServiceInstance, researchUIThemeDefInstance);
```

## Unknowns
- The specific implementations of `ResearchService`, `ResearchNodeDef`, and `ResearchUIThemeDef` are not defined in this file.
- The behavior of `DebugManager.Log` and its impact on the application is not detailed.
```
