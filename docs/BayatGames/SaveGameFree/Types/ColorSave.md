# BayatGames.SaveGameFree.Types.ColorSave

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/Types/ColorSave.cs`._

# Purpose
- Defines a structure for representing RGBA color values.

# Public API
- Namespace: `BayatGames.SaveGameFree.Types`
- Types
  - `struct ColorSave`
    - Public fields/properties:
      - `float r`: Red component of the color.
      - `float g`: Green component of the color.
      - `float b`: Blue component of the color.
      - `float a`: Alpha component of the color.
    - Public methods:
      - `ColorSave(Color color)`: Constructor that initializes a ColorSave instance from a Unity Color.
      - `static implicit operator ColorSave(Color color)`: Converts a Color instance to a ColorSave instance.
      - `static implicit operator Color(ColorSave color)`: Converts a ColorSave instance to a Color instance.

# Key Behavior & Side Effects
- Implicit conversions allow seamless use of Color and ColorSave types interchangeably.

# Constraints & Failure Modes
- No explicit guards or error handling present.
- Assumes valid Color values are provided during conversions. 

# Example
```csharp
Color unityColor = new Color(1f, 0f, 0f, 1f); // Red color
ColorSave colorSave = unityColor; // Implicit conversion to ColorSave
Color convertedBack = colorSave; // Implicit conversion back to Color
```

# Unknowns
- No unknowns present in the file.
