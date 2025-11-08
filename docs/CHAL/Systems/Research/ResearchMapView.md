# CHAL.Systems.Research.ResearchMapView

_Automatically generated/updated from `Assets/src/Systems/Research/UI/ResearchMapView.cs`._

# Purpose
- Defines the `ResearchMapView` class for displaying and interacting with a research tree in a UI.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - public sealed class `ResearchMapView` : `MonoBehaviour`
    - Public fields/properties:
      - `RectTransform viewport`: The viewport for the map display.
      - `RectTransform content`: The content area of the map.
      - `Transform edgeContainer`: Container for edges between nodes.
      - `Transform nodeContainer`: Container for research nodes.
      - `ResearchUIThemeDef theme`: Theme settings for the UI.
      - `ResearchTreeDef treeDef`: Definition of the research tree structure.
      - `ResearchService service`: Service for managing research data.
      - `ResearchHUD hud`: HUD for displaying research information.
      - `int nodeSpacingX`: Horizontal spacing between nodes.
      - `int lanePaddingX`: Padding between lanes.
    - Public methods:
      - `void initHUD()`: Initializes the HUD if the service is available.
      - `void BuildMap()`: Builds the research map from the tree definition.
      - `void SetZoomIndex(int newIndex, Vector2 screenPivot)`: Sets the zoom level based on the specified index and screen pivot.
      - `void OnNodeClicked(string nodeId)`: Handles node click events and updates visual states.
      - `void CenterOnActiveOrFirst()`: Centers the content on the active node or the first node if none is active.

# Key Behavior & Side Effects
- Initializes the HUD in `Update()` if the service is ready.
- Builds the map by compiling the research tree and instantiating nodes and edges.
- Handles panning and zooming interactions based on user input.
- Updates the visual state of nodes when clicked and shows details in the HUD.

# Constraints & Failure Modes
- Logs an error if any required references are missing during `Start()`.
- Handles null checks for `viewport`, `content`, and `service` in various methods.
- Zooming is constrained to the defined zoom steps.

# Example
```csharp
var researchMapView = new ResearchMapView();
researchMapView.BuildMap();
researchMapView.OnNodeClicked("nodeId123");
```

# Unknowns
- The behavior of `ResearchTreeCompiler.Compile(treeDef)` and its output structure is not defined in this file.
- The implementation details of `ResearchNodeWidget` and `ResearchEdgeGraphic` are not provided.

