# Assets/src/Editor/HeroXpConfigEditor.cs

_Automatically generated/updated from `Assets/src/Editor/HeroXpConfigEditor.cs`._

# Purpose
- Defines a custom editor for the `HeroXPConfig` class, allowing for the visualization and editing of experience point curves and wave requirements in the Unity Inspector.

# Public API
- Namespace: `UnityEditor`
- Types
  - `public class HeroXpConfigEditor : Editor`
    - Public methods:
      - `public override void OnInspectorGUI()`
        - Renders the custom inspector GUI for `HeroXPConfig`.

# Key Behavior & Side Effects
- Displays default inspector fields for `HeroXPConfig`.
- Allows editing of the experience curve using a curve field.
- Provides buttons to rebuild the curve from an array and bake the curve to an array.
- Updates the `HeroXPConfig` instance when changes are made, marking it as dirty.
- Displays warnings if `wavesRequiredPerLevel` does not meet the required size.
- Draws cumulative curves for waves and time based on the configuration.

# Constraints & Failure Modes
- Ensures that `wavesRequiredPerLevel` has at least `LevelCap` entries.
- Clamps wave duration values to a minimum of 1 second.
- Handles null or empty arrays for `wavesRequiredPerLevel` and `wavesCurve` by initializing them appropriately.

# Example
```csharp
// To use the custom editor, attach the HeroXPConfig scriptable object to a GameObject in Unity.
// The custom editor will automatically be used in the Inspector.
```

# Unknowns
- None.

