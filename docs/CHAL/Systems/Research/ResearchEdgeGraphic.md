# CHAL.Systems.Research.ResearchEdgeGraphic

_Automatically generated/updated from `Assets/src/Systems/Research/UI/ResearchEdgeGraphic.cs`._

# Purpose
- Defines a UI component for rendering an orthogonal line with a small Bezier corner as a mesh.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchEdgeGraphic : MaskableGraphic`
    - Public fields/properties:
      - `public Vector2 start` - Starting point of the line.
      - `public Vector2 end` - Ending point of the line.
      - `public float thickness` - Thickness of the line (minimum 0.5).
      - `public float cornerRadius` - Radius of the corner (0 to 1).
      - `public bool useCompletedColor` - Flag to use a completed color.
      - `public Color completedColor` - Color used when `useCompletedColor` is true.
    - Public methods:
      - `protected override void Awake()` - Initializes the component and disables raycast targeting.
      - `protected override void OnPopulateMesh(VertexHelper vh)` - Populates the mesh with the line segments and corner.

# Key Behavior & Side Effects
- The `Awake` method sets `raycastTarget` to false, preventing UI input blocking.
- The `OnPopulateMesh` method generates the mesh for the line and corner based on the `start`, `end`, `thickness`, `cornerRadius`, and color properties.

# Constraints & Failure Modes
- The mesh will not be populated if the alpha component of the color is less than or equal to 0.001.
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

