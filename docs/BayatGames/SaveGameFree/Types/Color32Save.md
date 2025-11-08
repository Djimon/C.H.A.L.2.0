# BayatGames.SaveGameFree.Types.Color32Save

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/Color32Save.cs`._

# Purpose
- Defines a structure for representing RGBA colors in a 32-bit format.

# Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - `struct Color32Save`
    - Public fields/properties:
      - `byte r`: Red component of the color.
      - `byte g`: Green component of the color.
      - `byte b`: Blue component of the color.
      - `byte a`: Alpha component of the color.
    - Public methods:
      - `Color32Save(Color32 color)`: Constructor that initializes a `Color32Save` from a `Color32`.
      - `static implicit operator Color32Save(Color32 color)`: Converts a `Color32` instance to a `Color32Save`.
      - `static implicit operator Color32(Color32Save color)`: Converts a `Color32Save` instance to a `Color32`.

# Key Behavior & Side Effects
- Implicit conversions allow seamless conversion between `Color32` and `Color32Save`.

# Constraints & Failure Modes
- None explicitly stated in the code.

# Example
```csharp
Color32 color = new Color32(255, 0, 0, 255); // Red color
Color32Save colorSave = color; // Implicit conversion to Color32Save
Color32 originalColor = colorSave; // Implicit conversion back to Color32
```

# Unknowns
- None.
