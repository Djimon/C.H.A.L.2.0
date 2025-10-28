# Assets/src/Systems/Research/UI/ResearchMapView.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `ResearchMapView` class for displaying and interacting with a research tree UI in Unity.

## Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchMapView : MonoBehaviour`
    - Public fields/properties:
      - `public RectTransform viewport`
      - `public RectTransform content`
      - `public Transform edgeContainer`
      - `public Transform nodeContainer`
      - `public ResearchUIThemeDef theme`
      - `public ResearchTreeDef treeDef`
      - `public ResearchService service`
      - `public ResearchHUD hud`
      - `public ResearchNodeWidget nodePrefab`
      - `public int nodeSpacingX`
      - `public int lanePaddingX`
      - `public ResearchUIThemeDef Theme`
      - `public ResearchService serviceRef`
    - Public methods:
      - `public void initHUD()`
      - `public void BuildMap()`
      - `public void OnNodeClicked(string nodeId)`
      - `public void CenterOnActiveOrFirst()`
      - `public void SetZoomIndex(int newIndex, Vector2 screenPivot)`

## Key Behavior & Side Effects
- Initializes HUD in `Update()` if the service is ready.
- Builds the research map in `BuildMap()`, which includes:
  - Clearing existing nodes and edges.
  - Compiling the research tree and calculating node positions.
  - Instantiating node widgets and drawing edges.
- Handles user interactions for panning and zooming in `HandlePan()` and `HandleZoomWheel()`.

## Constraints & Failure Modes
- Logs an error if any required references are missing in `Start()`.
- Guards against null references in various methods (e.g., `initHUD()`, `Update()`).
- Zoom index is clamped to valid range in `SetZoomIndex()`.

## Example
```csharp
var researchMapView = new ResearchMapView();
researchMapView.BuildMap();
researchMapView.CenterOnActiveOrFirst();
```

## Unknowns
- The exact structure and contents of `ResearchUIThemeDef`, `ResearchTreeDef`, and `ResearchService` cannot be determined from this file.
```
