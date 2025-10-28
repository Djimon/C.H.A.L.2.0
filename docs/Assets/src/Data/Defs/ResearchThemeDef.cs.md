# Assets/src/Data/Defs/ResearchThemeDef.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `ScriptableObject` for configuring UI themes related to research.

## Public API
- Namespace: `CHAL.Data`
- Types
  - `public sealed class ResearchUIThemeDef : ScriptableObject`
    - Public fields/properties:
      - `public Sprite mapBackground`
      - `public Color mapBackgroundTint`
      - `public Sprite nodeBackground`
      - `public Sprite nodeIconDefault`
      - `public Color nodeForegroundColor`
      - `public Color nodeDisabledColor`
      - `public Color nodeCompletedColor`
      - `public Color edgeColor`
      - `public float edgeThickness`
      - `public Color edgeCompletedColor`
      - `public Color highlightColor`
      - `public float highlightIntensity`
      - `public float activeGlow`
      - `public float[] zoomSteps`
      - `public int defaultZoomIndex`
      - `public int nodeWidth`
      - `public int nodeHeight`
      - `public int stageStepY`
      - `public int topMarginY`
      - `public System.Collections.Generic.List<int> laneBaseX`
    - Public methods:
      - `private void OnValidate()`
        - Validates and adjusts properties when the asset is modified.

## Key Behavior & Side Effects
- `OnValidate` method ensures:
  - `zoomSteps` is initialized to `[1.0]` if empty.
  - Each value in `zoomSteps` is clamped to a minimum of `0.1f`.
  - `defaultZoomIndex` is clamped within valid bounds.
  - `edgeThickness`, `nodeWidth`, `nodeHeight`, and `stageStepY` are clamped to minimum values.
  - `laneBaseX` is initialized to default values if empty.

## Constraints & Failure Modes
- Properties have minimum value constraints (e.g., `edgeThickness` must be at least `0.5f`).
- `zoomSteps` and `laneBaseX` must not be null or empty; defaults are set if they are.

## Example
```csharp
var theme = ScriptableObject.CreateInstance<ResearchUIThemeDef>();
theme.mapBackground = someSprite;
theme.nodeWidth = 300;
```

## Unknowns
- None.
```
