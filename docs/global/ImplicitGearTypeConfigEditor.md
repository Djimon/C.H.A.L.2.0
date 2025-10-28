# global.ImplicitGearTypeConfigEditor

_Automatically generated/updated from `Assets/src/Editor/ImplicitGearConfigEditor.cs`._

# Purpose
- Defines a custom editor for `ImplicitGearTypeConfig` in Unity's Editor.
- Provides functionality to copy templates, parse input grids, and apply weights to gear types.

# Public API
- Namespace/module: `UnityEditor`
- Types
  - `ImplicitGearTypeConfigEditor` [extends `UnityEditor.Editor`]
    - Public methods:
      - `OnInspectorGUI()`: Draws the custom inspector GUI.
      - `CopyBlankTemplateToClipboard(ImplicitGearTypeConfig asset)`: Copies a blank template to clipboard.
      - `CopyFromAssetToClipboard(ImplicitGearTypeConfig asset)`: Copies a template from the asset to clipboard.
      - `ApplyFromGrid(ImplicitGearTypeConfig asset, string text)`: Parses and applies weights from a TSV grid.

# Key Behavior & Side Effects
- `OnInspectorGUI()`: Renders the inspector UI, handles button clicks for copying templates and applying weights.
- `CopyBlankTemplateToClipboard()`: Copies a template with zero values if IDs are found; otherwise, shows a status message.
- `CopyFromAssetToClipboard()`: Copies a template based on existing weights in the asset.
- `ApplyFromGrid()`: Parses a TSV input, updates the asset's pools with weights, and marks the asset as dirty for saving.

# Constraints & Failure Modes
- Handles empty/null inputs gracefully, providing status messages for errors.
- Uses `Undo.RecordObject` to allow undoing changes in the editor.
- Requires `ImplicitGearTypeConfig` to have a non-null `Pools` property.

# Example
```csharp
// Example usage in Unity Editor
[CustomEditor(typeof(ImplicitGearTypeConfig))]
public class ImplicitGearTypeConfigEditor : UnityEditor.Editor
{
    // Custom editor implementation...
}
```

# Unknowns
- Specific details about the `ImplicitGearTypeConfig` and `GearType` classes are not defined in this file.
- The behavior of `ImplicitWeight` and `GearTypePool` is not detailed in this file.

