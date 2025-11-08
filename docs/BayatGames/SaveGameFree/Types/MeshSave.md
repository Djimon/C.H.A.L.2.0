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
      - `MeshSave(Mesh mesh)`: Constructor that initializes a `MeshSave` object from a `Mesh`.
      - `static implicit operator MeshSave(Mesh mesh)`: Converts a `Mesh` object to a `MeshSave` object.
      - `static implicit operator Mesh(MeshSave mesh)`: Converts a `MeshSave` object to a `Mesh` object.

# Key Behavior & Side Effects
- The constructor initializes the `MeshSave` object by casting mesh data into appropriate types.
- Implicit conversion operators allow seamless conversion between `Mesh` and `MeshSave`.

# Constraints & Failure Modes
- Assumes that the input `Mesh` is valid and contains data that can be cast to the specified types.
- No explicit error handling is present for invalid mesh data.

# Example
```csharp
Mesh mesh = new Mesh();
MeshSave meshSave = mesh; // Implicit conversion from Mesh to MeshSave
Mesh convertedBack = meshSave; // Implicit conversion from MeshSave to Mesh
```

# Unknowns
- None.
