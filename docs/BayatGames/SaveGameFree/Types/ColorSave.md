# BayatGames.SaveGameFree.Types.ColorSave

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/ColorSave.cs`._

Purpose
- Serializable representation of RGBA color (uses UnityEngine.Color components).
- Provides implicit conversions to/from UnityEngine.Color.
- Exposes RGBA components as public fields.

Public API
- Namespace/module: BayatGames.SaveGameFree.Types
- Types
  - public struct ColorSave
    - Public fields
      - public float r; // red component
      - public float g; // green component
      - public float b; // blue component
      - public float a; // alpha component
    - Public constructors
      - public ColorSave ( Color color )
        - Initializes r, g, b, a from color.r, color.g, color.b, color.a
    - Public implicit operators
      - public static implicit operator ColorSave ( Color color )
      - public static implicit operator Color ( ColorSave color )

Key Behavior & Side Effects
- ColorSave ( Color color )
  - Copies color components into the struct fields.
- public static implicit operator ColorSave ( Color color )
  - Returns a new ColorSave initialized from the given Color.
- public static implicit operator Color ( ColorSave color )
  - Returns a new UnityEngine.Color constructed from the ColorSave fields (r, g, b, a).

Constraints & Failure Modes
- No validation of RGBA ranges; direct field assignment.
- Public fields imply mutability of the struct.
- No explicit threading/async guarantees or synchronization.

Example
```csharp
// Example
Color c = Color.red;
ColorSave cs = c;       // implicit conversion to ColorSave
Color back = cs;          // implicit conversion back to Color
```

Unknowns
- How this type is serialized/deserialized by the SaveGameFree system beyond the [Serializable] attribute.
- Any additional usage patterns within the library beyond this file.
