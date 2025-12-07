# Assets/src/Editor/ImplicitGearConfigEditor.cs

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
- Displays a custom inspector for `ImplicitGearTypeConfig`.
- Allows copying templates to the clipboard and applying pasted grid data.
- Uses Unity's Undo system to record changes when applying weights.
- Updates the asset and marks it dirty when changes are made.
- Handles copying blank templates and templates from the asset to the clipboard.

# Constraints & Failure Modes
- Handles empty or null inputs gracefully.
- Throws exceptions for invalid data formats or missing headers during parsing.
- Ensures all gear types exist in the asset before applying changes.

# Example
```csharp
// Example usage in Unity Editor
[CustomEditor(typeof(ImplicitGearTypeConfig))]
public class ExampleEditor : ImplicitGearTypeConfigEditor
{
    // Custom editor logic can be added here
}
```

# Unknowns
- None.
