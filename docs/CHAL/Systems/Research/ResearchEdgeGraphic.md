# CHAL.Systems.Research.ResearchEdgeGraphic

_Automatically generated/updated from `Assets/src/Systems/Research/UI/ResearchEdgeGraphic.cs`._

Purpose
- Defines a Unity UI graphic (MaskableGraphic) that renders an orthogonal edge with a small Bezier-like corner as a mesh.
- Exposes public fields to configure start/end points, thickness, and corner appearance; supports an optional “completed” color.
- Ensures the edge does not block UI input (raycastTarget is disabled).

Public API
- Namespace: CHAL.Systems.Research
- Type
  - public sealed class ResearchEdgeGraphic : MaskableGraphic
    - Public fields
      - public Vector2 start; // edge start point (local UI space)
      - public Vector2 end; // edge end point (local UI space)
      - [Min(0.5f)] public float thickness = 2f; // edge thickness
      - [Range(0f, 1f)] public float cornerRadius = 0.25f; // fraction of shorter segment used for the corner
      - public bool useCompletedColor; // choose completedColor when true
      - public Color completedColor = new Color(0.6f, 1f, 0.6f, 1f); // color when useCompletedColor is true
    - Protected overrides
      - protected override void Awake()
        - Sets raycastTarget = false
      - protected override void OnPopulateMesh(VertexHelper vh)
        - Builds the edge mesh for an orthogonal path with a corner
    - Private static helpers
      - private static void AddQuad(VertexHelper vh, Vector2 a, Vector2 b, float thick, Color c)
        - Adds a quad along segment [a, b] with given thickness
      - private static void AddCorner(VertexHelper vh, Vector2 from, Vector2 to, float thick, Color c)
        - Approximates a 90-degree corner using multiple short quads

Key behavior & side effects
- Mesh construction
  - Defines path p0 = start, p1 = (end.x, start.y), p2 = end (orthogonal route)
  - Chooses color: useCompletedColor ? completedColor : color
  - Corner radius cr = min(hLen, vLen) * cornerRadius, where hLen = |p1.x - p0.x|, vLen = |p2.y - p1.y|
  - Shortens segment 1 by cr (a1 to b1) and segment 2 by cr (a2 to b2)
  - Draws two quads for the shortened segments
  - Adds a small corner approximation via AddCorner(vh, b1, a2, thickness, col)
- Color handling
  - If color.a <= 0.001f, mesh is not populated
  - If useCompletedColor is true, completedColor is used for all drawing
- Input behavior
  - raycastTarget is explicitly disabled in Awake (edges do not block UI input)

Constraints & Failure Modes
- Guard: if color.a <= 0.001f, no mesh is produced
- Degenerate segments are skipped
  - AddQuad returns early if segment length squared < 0.0001f
  - AddCorner returns early if length < 0.001f
- Public fields are not clamped beyond [Min] / [Range] attributes
- No threading; Unity main-thread code only
- Behavior depends on OnPopulateMesh invocation by Unity (not continuous auto-refresh described)

Example
- Minimal usage (Inspector or code)
```csharp
using CHAL.Systems.Research;

public class ExampleUsage : MonoBehaviour
{
    void Start()
    {
        var edge = gameObject.AddComponent<ResearchEdgeGraphic>();
        edge.start = new Vector2(100f, 50f);
        edge.end = new Vector2(300f, 150f);
        edge.thickness = 3f;
        edge.cornerRadius = 0.25f;
        edge.useCompletedColor = true;
        edge.completedColor = new Color(0.6f, 1f, 0.6f, 1f);
    }
}
```

Unknowns
- Unused using: CHAL.Core (DebugManager) is present but not used in the file.
- Interaction details with specific Canvas/RectTransform scaling or DPI are not described.
- Performance characteristics (vertex count, redraw frequency) are not documented beyond implementation.
- External behavior if start/end are animated or changed frequently is not specified.

Code references (from this file)
- Public surface:
  - Start/end/thickness/cornerRadius/useCompletedColor/completedColor
- Methods:
  - Awake()
  - OnPopulateMesh(VertexHelper vh)
- Helpers:
  - AddQuad(VertexHelper vh, Vector2 a, Vector2 b, float thick, Color c)
  - AddCorner(VertexHelper vh, Vector2 from, Vector2 to, float thick, Color c)

```csharp
// Example: only if you want to illustrate a self-contained usage snippet
```
