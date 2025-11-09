# Assets/src/Data/Defs/ResearchThemeDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchThemeDef.cs`._

# Purpose
- Defines a ScriptableObject for configuring the UI theme of research elements in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public sealed class `ResearchUIThemeDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `Sprite mapBackground`: Background image for the map.
      - `Color mapBackgroundTint`: Tint color for the map background.
      - `Sprite nodeBackground`: Background image for nodes.
      - `Sprite nodeIconDefault`: Default icon for nodes.
      - `Color nodeForegroundColor`: Color for node text/icon in normal state.
      - `Color nodeDisabledColor`: Color for inactive/locked nodes.
      - `Color nodeCompletedColor`: Color for completed nodes.
      - `Color edgeColor`: Color for edges/links.
      - `float edgeThickness`: Thickness of edges/links.
      - `Color edgeCompletedColor`: Color for edges of completed parent links.
      - `Color highlightColor`: Color for highlighting selected/active nodes.
      - `float highlightIntensity`: Intensity of the highlight effect.
      - `float activeGlow`: Optional glow multiplier for active nodes.
      - `float[] zoomSteps`: Array of discrete zoom factors.
      - `int defaultZoomIndex`: Default index for zoomSteps when opening.
      - `int nodeWidth`: Width of nodes.
      - `int nodeHeight`: Height of nodes.
      - `int stageStepY`: Vertical step size for stages.
      - `int topMarginY`: Top margin for layout.
      - `List<int> laneBaseX`: Base X positions for lanes.
    - Public methods:
      - `void OnValidate()`: Validates and adjusts properties when the asset is modified.

# Key Behavior & Side Effects
- The `OnValidate` method ensures that properties are within valid ranges and sets default values if necessary.
- Logs warnings if `zoomSteps` or `laneBaseX` are empty, and sets them to default values.

# Constraints & Failure Modes
- `zoomSteps` must not be empty; defaults to `[1.0]` if it is.
- Each value in `zoomSteps` must be at least `0.1f`.
- `defaultZoomIndex` must be clamped to valid indices of `zoomSteps`.
- `edgeThickness`, `nodeWidth`, `nodeHeight`, and `stageStepY` must be at least `0.5f` or `1` respectively.
- `laneBaseX` must not be empty; defaults to predefined values if it is.

# Example
```csharp
ResearchUIThemeDef theme = ScriptableObject.CreateInstance<ResearchUIThemeDef>();
theme.mapBackground = someSprite;
theme.nodeWidth = 300;
theme.defaultZoomIndex = 0;
```

# Unknowns
- None.
