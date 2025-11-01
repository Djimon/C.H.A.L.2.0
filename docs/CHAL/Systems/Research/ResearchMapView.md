# CHAL.Systems.Research.ResearchMapView

_Automatically generated/updated from `Assets/src/Systems/Research/UI/ResearchMapView.cs`._

```text
1) Purpose
- Unity MonoBehaviour that builds, displays, and interacts with a visual map of research nodes based on a ResearchTreeDef.
- Handles layout calculation (lanes, centers, per-stage node placement), node/edge instantiation, and basic panning/zooming.
- Integrates with a HUD (ResearchHUD) and a ResearchService to reflect and drive progression.

2) Public API
- Namespace/module
  - CHAL.Systems.Research

- Types
  - public sealed class ResearchMapView : MonoBehaviour
    - Public fields/properties
      - public RectTransform viewport
        - Viewport for the map UI
      - public RectTransform content
        - Content container that holds map elements
      - public Transform edgeContainer
        - Parent for edge graphics
      - public Transform nodeContainer
        - Parent for node widgets
      - public ResearchUIThemeDef theme
        - Theme definition (colors, dimensions, etc.)
      - public ResearchTreeDef treeDef
        - Tree definition used to build the map
      - public ResearchService service
        - Service providing node/status data
      - public ResearchNodeWidget nodePrefab
        - Prefab used to instantiate nodes
      - public int nodeSpacingX = 60
        - Horizontal spacing between nodes in a stage
      - public int lanePaddingX = 80
        - Padding between lanes
      - public bool enablePan = true
        - Enable mouse drag panning
      - public bool enableZoom = true
        - Enable mouse wheel zoom
      - public ResearchHUD hud
        - HUD instance for details/summary
      - public ResearchUIThemeDef Theme => theme
        - Public accessor for the theme
      - public ResearchService serviceRef => service
        - Public accessor for the service
      - public void initHUD()
        - Initialize HUD with the current service/theme; marks service as ready when done
      - public void BuildMap()
        - Build (recreate) the entire map from treeDef
      - public void SetZoomIndex(int newIndex, Vector2 screenPivot)
        - Change zoom level; keeps the screenPivot under the same world point
      - public void OnNodeClicked(string nodeId)
        - Handle node selection visuals and optional HUD details
      - public void CenterOnActiveOrFirst()
        - Center map on the active node or the first node if none active

    - Private/internal methods (non-public surface)
      - void CreateEdge(Vector2 from, Vector2 to, bool completed)
        - Create and configure an edge between two node positions
      - void RefreshAllStates()
        - Apply state visuals to all node widgets
      - void HandlePan()
        - Pan the map with mouse drag
      - void HandleZoomWheel()
        - Zoom in/out via mouse wheel
      - void Awake()
        - Unity lifecycle hook (empty)
      - void Start()
        - Unity lifecycle hook; wires references and builds map
      - void Update()
        - Unity lifecycle hook; handles HUD init, pan, and zoom
      - void BuildMap() (described above)
      - void CreateEdge(...) (described above)

3) Key Behavior & Side Effects
- Map construction flow (BuildMap)
  - Clears edge/node containers and internal state dictionaries
  - Compiles the tree via ResearchTreeCompiler.Compile(treeDef)
  - Groups nodes by lane and stage; sorts IDs per stage
  - Determines lane widths based on max parallel nodes per stage
  - Computes lane center/start/end X positions with collision resolution
  - Distributes node positions per stage symmetrically around lane centers
  - Instantiates node widgets from nodePrefab into nodeContainer
  - Initializes each node via w.Init(this, id, def.title, null)
  - Stores widgets in a dictionary by node ID
  - Creates edges for all parent-child relationships via CreateEdge
  - Logs build completion and applies initial state visuals via RefreshAllStates
- Interaction flows
  - Pan
    - On mouse down inside viewport (not over HUD): start dragging; pan content by screen delta
    - While dragging: content.anchoredPosition updated by mouse movement
  - Zoom
    - On mouse wheel: adjust zoomIndex within theme-defined steps
    - SetZoomIndex preserves the world point under the cursor by compensating content position after scale
  - Node interactions
    - OnNodeClicked highlights the selected node(s)
    - If the node is available and not completed, shows HUD details (if hud exists) and refreshes states
- Centering
  - CenterOnActiveOrFirst centers content on the active node or the first available node

4) Constraints & Failure Modes
- Start() guards
  - Checks for missing critical references (viewport, content, edge/node containers, nodePrefab, theme, treeDef, service); logs error if any missing
- Null/empty handling
  - zoomSteps defaults to a single step if theme.zoomSteps is null/empty; clamped zoomIndex against zoomSteps
  - Lane/base calculations tolerate null laneBaseX by using 0 lanes
- Runtime state
  - initHUD guards against null service
  - Update() gracefully skips pan/zoom if viewport/content are missing
- Edge/Node instantiation
  - Creates edge objects with Top-Left Anchors to align with edge drawing
  - Ensures edge size is at least 1x1 to avoid culling/masking issues
- Side effects
  - Destroying and recreating edges/nodes each BuildMap call
  - Edge graphic setup relies on theme colors; runtime state depends on ResearchTreeCompiler output

5) Example
- Not applicable (no derivable minimal code example beyond described usage in Unity Inspector and runtime flow)

6) Unknowns
- Behavior/details of ResearchTreeCompiler.Compile and the exact structure of compiled.posById, compiled.parentsById, and compiled.nodesById beyond usage here
- Exact contents/structure of ResearchTreeDef, ResearchUIThemeDef, and how they are authored/edited
- Specifics of ResearchNodeWidget.Init signature beyond parameters used here
- Any additional implicit side effects from external systems (beyond what is visible in this file)

```
