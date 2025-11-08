# global.ImplicitGearTypeConfigEditor

_Automatically generated/updated from `Assets/src/Editor/ImplicitGearConfigEditor.cs`._

# Purpose
- Provides a custom editor for the implicit gear type configuration.

# Public API
- Namespace: None
- Types
  - public class ImplicitGearTypeConfigEditor : UnityEditor.Editor
    - Public fields/properties:
      - string pastedGrid: Stores the pasted grid input.
      - string status: Stores the status message.
    - Public methods:
      - void OnInspectorGUI(): Draws the custom inspector GUI for the component.

# Key Behavior & Side Effects
- Displays a custom inspector for `ImplicitGearTypeConfig` with options to paste a grid, copy templates, and apply weights.
- Handles parsing of TSV input and applies weights to the gear type configuration.
- Uses Unity's Undo system to allow reverting changes made to the asset.

# Constraints & Failure Modes
- Requires `ImplicitGearTypeConfig` to be assigned; otherwise, it shows an error message.
- Handles empty or invalid input gracefully, providing user feedback through the status message.
- Ensures all gear types exist in the asset before applying weights.

# Example
```csharp
// Example usage in Unity Editor
[CustomEditor(typeof(ImplicitGearTypeConfig))]
public class ExampleUsage : ImplicitGearTypeConfigEditor
{
    // Custom editor logic can be added here
}
```

# Unknowns
- None.

