# CHAL.Systems.Research.ResearchEdgeGraphic

_Automatically generated/updated from `Assets/src/Systems/Research/UI/ResearchEdgeGraphic.cs`._

# Purpose
- Defines a UI component that renders an orthogonal line with a small Bezier corner as a mesh.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchEdgeGraphic : MaskableGraphic`
    - Public fields/properties:
      - `Vector2 start`: Starting point of the line.
      - `Vector2 end`: Ending point of the line.
      - `float thickness`: Thickness of the line (minimum 0.5).
      - `float cornerRadius`: Radius of the corner (0 to 1, relative to the shorter segment).
      - `bool useCompletedColor`: Flag to use a completed color.
      - `Color completedColor`: Color used when `useCompletedColor` is true.
    - Public methods:
      - `protected override void Awake()`: Initializes the graphic and disables raycast targeting.
      - `protected override void OnPopulateMesh(VertexHelper vh)`: Populates the mesh with the line segments and corner.

# Key Behavior & Side Effects
- The `Awake` method sets `raycastTarget` to false, preventing UI input blocking.
- The `OnPopulateMesh` method generates the mesh based on the `start`, `end`, `thickness`, `cornerRadius`, and color properties.

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

