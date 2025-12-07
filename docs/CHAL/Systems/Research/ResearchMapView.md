# Assets/src/Systems/Research/UI/ResearchMapView.cs

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
      - `ResearchHUD hud`: HUD for displaying additional information.
      - `int nodeSpacingX`: Horizontal spacing between nodes.
      - `int lanePaddingX`: Padding between lanes.
      - `ResearchNodeWidget nodePrefab`: Prefab for research nodes.
    - Public methods:
      - `void initHUD()`: Initializes the HUD if the service is available.
      - `void BuildMap()`: Builds the research map based on the tree definition.
      - `void SetZoomIndex(int newIndex, Vector2 screenPivot)`: Sets the zoom index and adjusts the content scale.
      - `void OnNodeClicked(string nodeId)`: Handles node click events and updates visual states.
      - `void CenterOnActiveOrFirst()`: Centers the content on the active node or the first widget.

# Key Behavior & Side Effects
- Initializes the HUD in `Update()` if the service is ready.
- Builds the research map in `BuildMap()`, which includes cleaning up existing nodes and edges, compiling the research tree, and instantiating node widgets.
- Handles user interactions for panning and zooming the map.
- Updates the visual state of nodes when clicked and shows details if the node is available.

# Constraints & Failure Modes
- Logs an error if any required references are missing during `Start()`.
- Handles null checks for `viewport`, `content`, and `service` in various methods to prevent null reference exceptions.
- Zooming and panning are only enabled if the respective flags are set to true.

# Example
```csharp
// Example of initializing the ResearchMapView
ResearchMapView researchMapView = GetComponent<ResearchMapView>();
researchMapView.BuildMap();
```

# Unknowns
- The behavior of `ResearchService` and how it interacts with the `ResearchMapView` is not detailed in this file.
- The implementation details of `ResearchNodeWidget` and `ResearchEdgeGraphic` are not provided.
