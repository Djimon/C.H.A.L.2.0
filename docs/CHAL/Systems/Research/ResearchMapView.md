# CHAL.Systems.Research.ResearchMapView

_Automatically generated/updated from `Assets/src/Systems/Research/UI/ResearchMapView.cs`._

# Purpose
- Defines the `ResearchMapView` class for visualizing a research tree in a UI.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchMapView : MonoBehaviour`
    - Public fields/properties:
      - `public RectTransform viewport;`
      - `public RectTransform content;`
      - `public Transform edgeContainer;`
      - `public Transform nodeContainer;`
      - `public ResearchUIThemeDef theme;`
      - `public ResearchTreeDef treeDef;`
      - `public ResearchService service;`
      - `public ResearchHUD hud;`
      - `public ResearchNodeWidget nodePrefab;`
      - `public int nodeSpacingX;`
      - `public int lanePaddingX;`
    - Public methods:
      - `public void initHUD();` 
        - Initializes the HUD if the service is available.
      - `public void BuildMap();`
        - Constructs the research map based on the tree definition.
      - `public void OnNodeClicked(string nodeId);`
        - Visualizes selection and shows details if the node is available.
      - `public void CenterOnActiveOrFirst();`
        - Centers the view on the active node or the first node if none is active.
      - `public ResearchUIThemeDef Theme => theme;`
      - `public ResearchService serviceRef => service;`
      - `public void SetZoomIndex(int newIndex, Vector2 screenPivot);`
        - Sets the zoom level based on the index and adjusts the content position.

# Key Behavior & Side Effects
- Initializes HUD in `Update` if the service is ready.
- Builds the research map in `BuildMap`, which includes:
  - Clearing existing nodes and edges.
  - Compiling the research tree.
  - Calculating node positions and instantiating node widgets.
  - Drawing edges between nodes.
- Handles user interactions for panning and zooming in `HandlePan` and `HandleZoomWheel`.

# Constraints & Failure Modes
- Checks for missing references in `Start` and logs an error if any are missing.
- Panning only occurs if the mouse is over the viewport and not over the HUD.
- Zooming is limited to the defined zoom steps.

# Example
```csharp
// Example of initializing the ResearchMapView
ResearchMapView researchMapView = new ResearchMapView();
researchMapView.BuildMap();
```

# Unknowns
- The exact implementation details of `ResearchTreeCompiler`, `ResearchNodeWidget`, and `ResearchEdgeGraphic` are not provided.
- The behavior of `ResearchService` methods like `IsNodeAvailable` and `GetActiveNodeId` is not defined in this file.

