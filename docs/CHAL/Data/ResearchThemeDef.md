# CHAL.Data.ResearchThemeDef

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchThemeDef.cs`._

1) Purpose
- Defines a Unity ScriptableObject asset ResearchUIThemeDef that stores UI theme settings for the Research UI.
- Exposes serialized fields for map visuals, node visuals, edge visuals, highlighting, zoom, and layout hooks.
- Provides editor-time validation to sanitize values and supply sensible defaults.

2) Public API
- Namespace/module
  - CHAL.Data

- Types
  - public sealed class ResearchUIThemeDef : ScriptableObject
    - Public fields (serialized UI theme data)
      - public Sprite mapBackground
        - Role: Background image for the map area.
      - public Color mapBackgroundTint = Color.white
        - Role: Tint applied under the map background (if the sprite is transparent).
      - public Sprite nodeBackground
        - Role: Background image for nodes.
      - public Sprite nodeIconDefault
        - Role: Default icon for nodes.
      - public Color nodeForegroundColor = Color.white
        - Role: Color for node text/icon in normal state.
      - public Color nodeDisabledColor = new Color(1f, 1f, 1f, 0.35f)
        - Role: Color for inactive/locked nodes.
      - public Color nodeCompletedColor = new Color(0.7f, 1f, 0.7f, 1f)
        - Role: Color for completed nodes.
      - public Color edgeColor = Color.white
        - Role: Base color for edges/links.
      - [Min(0.5f)] public float edgeThickness = 2f
        - Role: Thickness of edges; minimum is 0.5.
      - public Color edgeCompletedColor = new Color(0.6f, 1f, 0.6f, 1f)
        - Role: Color for edges of completed/fulfilled parent-links.
      - public Color highlightColor = new Color(1f, .85f, .2f, 1f)
        - Role: Color used for selection/active highlighting.
      - [Range(0f, 2f)] public float highlightIntensity = 1.0f
        - Role: Intensity of the highlight effect.
      - [Range(0f, 2f)] public float activeGlow = 0.6f
        - Role: Optional glow multiplier for active nodes.
      - [Tooltip("Diskrete Zoomstufen (Scale-Faktoren).")] public float[] zoomSteps = new float[] { 0.75f, 1.0f, 1.25f, 1.5f }
        - Role: Discrete zoom scale factors.
      - [Tooltip("Default-Index in zoomSteps beim ffnen.")] public int defaultZoomIndex = 1
        - Role: Default index into zoomSteps when opening.
      - [Min(1)] public int nodeWidth = 240
        - Role: Node width in layout units.
      - [Min(1)] public int nodeHeight = 120
        - Role: Node height in layout units.
      - [Min(1)] public int stageStepY = 180
        - Role: Vertical step between stages.
      - public int topMarginY = 120
        - Role: Top margin for layout.
      - [Tooltip("X-Basis pro Lane (muss zur Lane-Anzahl des Trees passen).")]
        public System.Collections.Generic.List<int> laneBaseX = new System.Collections.Generic.List<int> { 300, 700, 1100, 1500 }
        - Role: X-basis values per lane; defines horizontal spacing.

    - Private/Unity callbacks
      - private void OnValidate()
        - Role: Editor-time validation to sanitize values and provide defaults.
        - Side effects:
          - If zoomSteps is null or empty:
            - Replaces with { 1f }.
            - Logs a warning via DebugManager.Log.
          - Clamps any zoomSteps values to a minimum of 0.1f.
          - Ensures defaultZoomIndex is within [0, zoomSteps.Length-1].
          - Enforces edgeThickness >= 0.5f.
          - Ensures nodeWidth, nodeHeight, stageStepY are >= 1.
          - If laneBaseX is null or empty:
            - Replaces with { 300, 700, 1100, 1500 }.
            - Logs a warning via DebugManager.Log.

3) Key Behavior & Side Effects
- Editor-time validation (OnValidate)
  - Normalizes zoomSteps values, enforcing a sensible minimum.
  - Validates and clamps defaultZoomIndex to a valid range.
  - Enforces minimum visual/layout values (edge thickness, node size, stage step).
  - Provides fallback defaults for zoom steps and lane layout if missing.
  - Logs warnings when defaults are applied (via DebugManager.Log) in Dev/debug builds.

4) Constraints & Failure Modes
- Guards and defaults are enforced in OnValidate:
  - zoomSteps: null/empty -> [1.0]
  - zoomSteps values: < 0.1f -> set to 0.1f
  - defaultZoomIndex: out of range -> clamped to valid index
  - edgeThickness: < 0.5f -> set to 0.5f
  - nodeWidth/nodeHeight/stageStepY: < 1 -> set to 1
  - laneBaseX: null or empty -> reset to default list and log warning
- Runtime dependencies not defined in this file:
  - DebugManager.Log and DebugManager.EDebugLevel.Dev are referenced but not defined here.
  - Asset creation surface relies on Unity's CreateAssetMenu attribute.
- Threading/async: none evident; OnValidate runs in the editor.
- Serialization: all public fields are serialized; asset data is stored in ScriptableObject.

5) Example
- Editor usage
  - Create a new theme asset via the Unity Editor: right-click in the Project window → Create → Research/UI Theme (as defined by CreateAssetMenu).
  - Configure the fields in the Inspector (map/backgrounds, colors, zoom steps, lane bases, etc.).
  - If any required collections are missing or invalid, OnValidate will attempt to restore sensible defaults and may log warnings.

6) Unknowns
- Runtime consumption: how the theme data is used by the rest of the UI code is not present in this file.
- Debug logging implementation: behavior/details of DebugManager.Log and EDebugLevel.Dev are not defined here.
- Any additional constraints or interactions imposed by other parts of the project (e.g., how laneBaseX affects layout in the tree) are not shown in this file.
- Whether any assets referenced by mapBackground, nodeBackground, etc., are expected to exist or be assigned elsewhere is not specified here.

