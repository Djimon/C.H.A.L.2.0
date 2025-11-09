# Assets/src/Systems/Research/UI/ResearchEdgeGraphic.cs

_Automatically generated/updated from `Assets/src/Systems/Research/UI/ResearchEdgeGraphic.cs`._

# Purpose
- Defines a UI component that renders an orthogonal line with a small Bezier corner as a mesh.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchEdgeGraphic : MaskableGraphic`
    - Public fields/properties:
      - `public Vector2 start` - Starting point of the line.
      - `public Vector2 end` - Ending point of the line.
      - `[Min(0.5f)] public float thickness` - Thickness of the line.
      - `[Range(0f, 1f)] public float cornerRadius` - Radius of the corner as a fraction of the shorter segment.
      - `public bool useCompletedColor` - Flag to use a specific color when completed.
      - `public Color completedColor` - Color used when `useCompletedColor` is true.
    - Public methods:
      - `protected override void Awake()` - Initializes the component and disables raycast targeting.
      - `protected override void OnPopulateMesh(VertexHelper vh)` - Populates the mesh with the line and corner geometry.

# Key Behavior & Side Effects
- The `Awake` method sets `raycastTarget` to false, preventing UI input blocking.
- The `OnPopulateMesh` method generates the mesh for the line and corner based on the `start`, `end`, `thickness`, `cornerRadius`, and `color` properties.

# Constraints & Failure Modes
- If the alpha component of `color` is less than or equal to 0.001, the mesh will not be populated.
- The `AddQuad` and `AddCorner` methods handle cases where the distance between points is negligible, avoiding unnecessary mesh generation.

# Example
```csharp
var edgeGraphic = gameObject.AddComponent<ResearchEdgeGraphic>();
edgeGraphic.start = new Vector2(0, 0);
edgeGraphic.end = new Vector2(10, 10);
edgeGraphic.thickness = 2f;
edgeGraphic.cornerRadius = 0.25f;
edgeGraphic.useCompletedColor = true;
```

# Unknowns
- None.

