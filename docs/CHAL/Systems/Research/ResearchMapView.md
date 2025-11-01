# CHAL.Systems.Research.ResearchMapView

_Automatically generated/updated from `Assets/src/Systems/Research/UI/ResearchMapView.cs`._

```text
1) Purpose
- Unity MonoBehaviour that renders a research map with lanes and edges.
- Builds the map layout and visual nodes/edges from a ResearchTreeDef and ResearchService, styled by a ResearchUIThemeDef.
- Supports pan and zoom, node selection, and basic HUD integration via a public OnNodeClicked and CenterOnActiveOrFirst.

2) Public API
- Namespace: CHAL.Systems.Research

- Type
  - public sealed class ResearchMapView : MonoBehaviour

- Public fields
  - public RectTransform viewport
  - public RectTransform content
  - public Transform edgeContainer
  - public Transform nodeContainer
  - public ResearchUIThemeDef theme
  - public ResearchTreeDef treeDef
  - public ResearchService service
  - public ResearchNodeWidget nodePrefab
  - public int nodeSpacingX = 60
  - public int lanePaddingX = 80
  - public bool enablePan = true
  - public bool enableZoom = true
  - public ResearchHUD hud

- Public methods
  - void Awake()
  - void Start()
  - public void initHUD()
  - void Update()
  - public void BuildMap()
  - void CreateEdge(Vector2 from, Vector2 to, bool completed)
  - void RefreshAllStates()
  - void HandlePan()
  - void HandleZoomWheel()
  - public void SetZoomIndex(int newIndex, Vector2 screenPivot)
  - public ResearchUIThemeDef Theme => theme
  - public ResearchService serviceRef => service
  - public void OnNodeClicked(string nodeId)
  - public void CenterOnActiveOrFirst()

3) Key Behavior & Side Effects
- BuildMap
  - Destroys existing edge and node GameObjects under edgeContainer/nodeContainer.
  - Clears internal maps (widgets, nodePositions).
  - Compiles the tree via ResearchTreeCompiler.Compile(treeDef).
  - Groups nodes by lane/stage, sorts IDs within each stage, and computes lane widths based on parallelism per stage.
  - Computes lane centers/start/end X positions with collision resolution (laneBaseX and lanePaddingX).
  - Assigns node positions (x, y) per stage, using a symmetric distribution per stage.
  - Instantiates node prefabs, configures RectTransform (top-left anchored, centered pivot, node size), and calls Init on each ResearchNodeWidget.
  - Draws edges by instantiating Edge objects and configuring a ResearchEdgeGraphic with theme colors, completion state, and thickness.
  - Logs build completion and refreshes node states.
- CreateEdge
  - Computes a local bounding box from two node positions.
  - Creates a new Edge GameObject with RectTransform and ResearchEdgeGraphic.
  - Anchors to top-left, sizes to bounding box, and sets graphic start/end relative to the edge rect.
  - Applies theme colors and completed-state coloring.
- Update
  - If HUD is not ready, calls initHUD().
  - Handles pan (if enabled) and zoom (if enabled) each frame.
- HandlePan
  - Starts panning on left-click within viewport, unless HUD UI is under the pointer.
  - Updates content.anchoredPosition by mouse delta for 1:1 panning.
  - Stops dragging on mouse up.
- HandleZoomWheel
  - Reads mouse scroll delta; if significant, adjusts zoom via SetZoomIndex.
- SetZoomIndex
  - Clamps the new index to valid range.
  - Keeps the world point under the mouse cursor stationary by recomputing local coordinates before/after zoom and adjusting content.anchoredPosition accordingly.
  - Applies the new scale to content.localScale and logs the zoom level.
- OnNodeClicked
  - Visually marks the clicked node as selected across all widgets.
  - If the node is available and not completed, shows HUD details and refreshes all node states.
- CenterOnActiveOrFirst
  - Uses service.GetActiveNodeId(); if none, falls back to first widget key.
  - If an id and position are known, centers the content so the node is centered under the viewport.

4) Constraints & Failure Modes
- Start() validates critical references (viewport, content, edgeContainer, nodeContainer, nodePrefab, theme, treeDef, service) and logs an error if missing.
- Zoom steps default: if theme.zoomSteps is null/empty, use {1f}.
- Zoom index is clamped to [0, zoomSteps.Length - 1].
- BuildMap relies on non-null theme/treeDef/service; missing values are guarded by Start().
- CreateEdge guards against missing positions when drawing edges.
- BuildMap re-instantiates all nodes/edges; existing ones are destroyed first to avoid duplicates.
- Public API assumes external dependencies (ResearchTreeCompiler, ResearchNodeWidget, ResearchEdgeGraphic, etc.) provide expected behavior; internal logic uses their interfaces as shown.

5) Example
- Minimal usage in code:
```csharp
// Example: simulate a node click on startup
using CHAL.Systems.Research;

public class ExampleUsage : MonoBehaviour
{
    void Start()
    {
        var mapView = FindObjectOfType<ResearchMapView>();
        if (mapView != null)
        {
            mapView.OnNodeClicked("node42");
        }
    }
}
```

6) Unknowns
- Details of external types and behaviors (ResearchTreeCompiler, ResearchNodeWidget, ResearchEdgeGraphic, ResearchUIThemeDef, ResearchTreeDef, ResearchService, ResearchHUD) are not defined here.
- Exact visuals/colors beyond what theme provides, and how theme maps to edge/node visuals.
- Threading/async behavior beyond Unity’s main thread assumptions.
- Any runtime ordering guarantees beyond what is explicit in Start/BuildMap calls.
- Whether additional editor tooling or serialization attributes apply beyond public fields.
