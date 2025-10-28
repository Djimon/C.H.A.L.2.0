# Assets/src/xTernal/SaveGameFree/Scripts/Types/MeshSave.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `MeshSave` class for creating and modifying meshes from scripts.

## Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - **public class MeshSave**
    - Public fields/properties:
      - `Vector3Save[] vertices` - Stores mesh vertices.
      - `int[] triangles` - Stores mesh triangle indices.
      - `Vector2Save[] uv` - Stores mesh UV coordinates.
      - `Vector3Save[] normals` - Stores mesh normals.
      - `Color[] colors` - Stores mesh vertex colors.
      - `Color32[] colors32` - Stores mesh vertex colors in 32-bit format.
    - Public methods:
      - `MeshSave(Mesh mesh)` - Constructor that initializes `MeshSave` from a `Mesh`.
      - `static implicit operator MeshSave(Mesh mesh)` - Converts a `Mesh` to `MeshSave`.
      - `static implicit operator Mesh(MeshSave mesh)` - Converts a `MeshSave` to `Mesh`.

## Key Behavior & Side Effects
- The constructor initializes `MeshSave` fields by casting mesh data from a `Mesh` object.
- Implicit conversion operators allow seamless conversion between `Mesh` and `MeshSave`.

## Constraints & Failure Modes
- Assumes that the input `Mesh` is valid and contains the expected data types.
- Casting operations may fail if the underlying types do not match.

## Example
```csharp
Mesh mesh = new Mesh();
MeshSave meshSave = new MeshSave(mesh);
Mesh convertedBack = meshSave; // Implicit conversion to Mesh
```

## Unknowns
- The behavior of `Vector3Save` and `Vector2Save` types is not defined in this file.
```
