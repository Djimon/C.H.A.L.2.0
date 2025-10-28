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
      - `Color32Save(Color32 color)`: Constructor that initializes the struct from a `Color32`.
      - `static implicit operator Color32Save(Color32 color)`: Converts a `Color32` to `Color32Save`.
      - `static implicit operator Color32(Color32Save color)`: Converts a `Color32Save` to `Color32`.

# Key Behavior & Side Effects
- Implicit conversions allow seamless use between `Color32` and `Color32Save`.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- No threading or performance considerations evident.

# Example
```csharp
Color32 color = new Color32(255, 0, 0, 255); // Red color
Color32Save colorSave = color; // Implicit conversion to Color32Save
Color32 originalColor = colorSave; // Implicit conversion back to Color32
```

# Unknowns
- No unknowns identified from the file.

