# BayatGames.SaveGameFree.Types.MeshSave

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/MeshSave.cs`._

1) Purpose
- Defines a serializable mesh container MeshSave in BayatGames.SaveGameFree.Types.
- Stores mesh data: vertices, triangles, uv, normals, colors, colors32 (via serializable/vector save types).
- Provides implicit conversions to/from UnityEngine.Mesh for easy round-tripping.

2) Public API
- Namespace: BayatGames.SaveGameFree.Types

- Type: public class MeshSave [Serializable]
  - Public fields
    - public Vector3Save[] vertices
      - Vertex positions
    - public int[] triangles
      - Triangle indices
    - public Vector2Save[] uv
      - Texture coordinates
    - public Vector3Save[] normals
      - Vertex normals
    - public Color[] colors
      - Vertex colors (Color)
    - public Color32[] colors32
      - Vertex colors (Color32)

  - Constructor
    - public MeshSave ( Mesh mesh )
      - Populates fields from the provided mesh:
        - vertices = mesh.vertices.Cast<Vector3Save>().ToArray()
        - triangles = mesh.triangles
        - uv = mesh.uv.Cast<Vector2Save>().ToArray()
        - normals = mesh.normals.Cast<Vector3Save>().ToArray()
        - colors = mesh.colors.Cast<Color>().ToArray()
        - colors32 = mesh.colors32.Cast<Color32>().ToArray()

  - Implicit operators
    - public static implicit operator MeshSave ( Mesh mesh )
      - Returns new MeshSave(mesh)
    - public static implicit operator Mesh ( MeshSave mesh )
      - Creates a new Mesh and assigns:
        - newMesh.vertices = mesh.vertices.Cast<Vector3>().ToArray()
        - newMesh.triangles = mesh.triangles
        - newMesh.uv = mesh.uv.Cast<Vector2>().ToArray()
        - newMesh.normals = mesh.normals.Cast<Vector3>().ToArray()
        - newMesh.colors = mesh.colors.Cast<Color>().ToArray()
        - newMesh.colors32 = mesh.colors32.Cast<Color32>().ToArray()
      - Returns newMesh

3) Key Behavior & Side Effects
- MeshSave(mesh) reads and stores mesh data into serializable fields using Cast/ToArray conversions.
- MeshSave -> Mesh via implicit operator creates a new Unity Mesh and copies data back with Cast conversions.
- Mesh -> MeshSave via implicit operator relies on the constructor for population.
- All conversions assume the existence of appropriate implicit/explicit conversions between Unity types and Vector*/Color* save types.

4) Constraints & Failure Modes
- No null checks: passing a null Mesh will throw at runtime.
- Casting relies on external definitions of Vector3Save, Vector2Save and their conversions; behavior depends on those types.
- Allocates new arrays during conversions (potential memory allocations).
- No additional mesh properties (tangents, bone weights, etc.) are handled.

5) Example
```csharp
// Example: implicit conversion between Mesh and MeshSave
Mesh sourceMesh = GetComponent<MeshFilter>().sharedMesh;

// Mesh -> MeshSave
MeshSave saved = sourceMesh;

// MeshSave -> Mesh
Mesh restored = saved;
```

6) Unknowns
- Details of Vector3Save and Vector2Save types and their conversion rules.
- How saving framework serializes these fields at runtime.
- Whether additional mesh attributes (tangents, boneWeights, etc.) are supported elsewhere.

