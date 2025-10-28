# BayatGames.SaveGameFree.Types.MeshSave

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/MeshSave.cs`._

# Purpose
- Defines the `MeshSave` class for creating and modifying meshes from scripts.

# Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - **public class MeshSave**
    - Public fields/properties:
      - `Vector3Save[] vertices`: Array of vertices.
      - `int[] triangles`: Array of triangle indices.
      - `Vector2Save[] uv`: Array of UV coordinates.
      - `Vector3Save[] normals`: Array of normals.
      - `Color[] colors`: Array of vertex colors.
      - `Color32[] colors32`: Array of vertex colors in 32-bit format.
    - Public methods:
      - `MeshSave(Mesh mesh)`: Constructor that initializes `MeshSave` from a `Mesh`.
      - `static implicit operator MeshSave(Mesh mesh)`: Converts a `Mesh` to `MeshSave`.
      - `static implicit operator Mesh(MeshSave mesh)`: Converts a `MeshSave` to a `Mesh`.

# Key Behavior & Side Effects
- The constructor initializes the `MeshSave` fields by casting the corresponding fields from the provided `Mesh`.
- Implicit conversion operators allow seamless conversion between `Mesh` and `MeshSave`.

# Constraints & Failure Modes
- Assumes that the input `Mesh` is valid and contains the expected data types.
- Casting operations may fail if the types do not match or are incompatible.

# Example
```csharp
Mesh mesh = new Mesh();
MeshSave meshSave = mesh; // Implicit conversion from Mesh to MeshSave
Mesh convertedBack = meshSave; // Implicit conversion from MeshSave to Mesh
```

# Unknowns
- The implementation details of `Vector3Save` and `Vector2Save` are not provided in this file.

