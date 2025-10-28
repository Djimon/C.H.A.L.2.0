# BayatGames.SaveGameFree.Types.ColorSave

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/ColorSave.cs`._

# Purpose
- Defines a structure for representing RGBA color values.

# Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - `struct ColorSave`
    - Public fields/properties:
      - `float r`: Red component.
      - `float g`: Green component.
      - `float b`: Blue component.
      - `float a`: Alpha component.
    - Public methods:
      - `ColorSave(Color color)`: Constructor that initializes `ColorSave` from a `Color`.
      - `static implicit operator ColorSave(Color color)`: Converts a `Color` to `ColorSave`.
      - `static implicit operator Color(ColorSave color)`: Converts a `ColorSave` to `Color`.

# Key Behavior & Side Effects
- Implicit conversions allow seamless use between `Color` and `ColorSave`.

# Constraints & Failure Modes
- No explicit guards or error handling present.
- No threading or performance considerations noted.

# Example
```csharp
Color color = new Color(1f, 0f, 0f, 1f); // Red color
ColorSave colorSave = color; // Implicit conversion to ColorSave
Color convertedBack = colorSave; // Implicit conversion back to Color
```

# Unknowns
- None.
