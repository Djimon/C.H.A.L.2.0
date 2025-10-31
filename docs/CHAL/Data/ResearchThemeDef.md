# CHAL.Data.ResearchThemeDef

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchThemeDef.cs`._

# Purpose
- Defines a `ResearchUIThemeDef` ScriptableObject for configuring UI themes related to research.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public sealed class `ResearchUIThemeDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `Sprite mapBackground` - Background image for the map.
      - `Color mapBackgroundTint` - Tint color for the map background.
      - `Sprite nodeBackground` - Background image for nodes.
      - `Sprite nodeIconDefault` - Default icon for nodes.
      - `Color nodeForegroundColor` - Color for node text/icons in normal state.
      - `Color nodeDisabledColor` - Color for inactive/disabled nodes.
      - `Color nodeCompletedColor` - Color for completed nodes.
      - `Color edgeColor` - Color for edges/links.
      - `float edgeThickness` - Thickness of edges/links.
      - `Color edgeCompletedColor` - Color for edges of completed parent links.
      - `Color highlightColor` - Color for highlighting selected/active nodes.
      - `float highlightIntensity` - Intensity of highlight effect.
      - `float activeGlow` - Optional glow multiplier for active nodes.
      - `float[] zoomSteps` - Array of discrete zoom factors.
      - `int defaultZoomIndex` - Default index for zoom steps.
      - `int nodeWidth` - Width of nodes.
      - `int nodeHeight` - Height of nodes.
      - `int stageStepY` - Vertical step size for stages.
      - `int topMarginY` - Top margin for layout.
      - `List<int> laneBaseX` - X-basis values for lanes.

# Key Behavior & Side Effects
- `OnValidate()` method:
  - Ensures `zoomSteps` is not empty; sets to `[1.0]` if it is.
  - Clamps values in `zoomSteps` to a minimum of `0.1f`.
  - Validates `defaultZoomIndex` to ensure it is within bounds.
  - Clamps `edgeThickness`, `nodeWidth`, `nodeHeight`, and `stageStepY` to minimum values.
  - Resets `laneBaseX` to default if it is empty.

# Constraints & Failure Modes
- `zoomSteps`, `laneBaseX` must not be null or empty; defaults are set if they are.
- Values for `edgeThickness`, `nodeWidth`, `nodeHeight`, and `stageStepY` must be at least `1` or `0.5` respectively.

# Example
```csharp
var researchTheme = ScriptableObject.CreateInstance<ResearchUIThemeDef>();
researchTheme.mapBackground = someSprite;
researchTheme.nodeWidth = 300;
```

# Unknowns
- None.

