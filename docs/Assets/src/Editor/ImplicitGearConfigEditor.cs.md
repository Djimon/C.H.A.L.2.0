# Assets/src/Editor/ImplicitGearConfigEditor.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a custom editor for `ImplicitGearTypeConfig` in Unity's Editor.
- Provides functionality to copy templates, parse input grids, and apply implicit weights.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `ImplicitGearTypeConfigEditor` [extends `UnityEditor.Editor`]
    - Public methods:
      - `OnInspectorGUI()`: Draws the custom inspector UI.
      - `CopyBlankTemplateToClipboard(ImplicitGearTypeConfig asset)`: Copies a blank template to the clipboard.
      - `CopyFromAssetToClipboard(ImplicitGearTypeConfig asset)`: Copies a template from the asset to the clipboard.
      - `ApplyFromGrid(ImplicitGearTypeConfig asset, string text)`: Parses and applies weights from a TSV grid.

# Key Behavior & Side Effects
- `OnInspectorGUI()`: Renders the inspector UI, handles button clicks for copying templates and applying weights.
- `CopyBlankTemplateToClipboard()`: Copies a blank template to the clipboard; updates status if no IDs are found.
- `CopyFromAssetToClipboard()`: Copies a populated template from the asset to the clipboard; updates status with the number of rows copied.
- `ApplyFromGrid()`: Parses TSV input, updates the asset's pools with weights, and marks the asset as dirty for saving.

# Constraints & Failure Modes
- Handles empty or null inputs gracefully (e.g., checks for valid IDs and weights).
- Uses `Undo.RecordObject` to allow undoing changes in the editor.
- Catches exceptions during parsing and applying weights, updating the status with error messages.

# Example
```csharp
// Example usage in Unity Editor
[CustomEditor(typeof(ImplicitGearTypeConfig))]
public class ImplicitGearTypeConfigEditor : UnityEditor.Editor
{
    // Custom editor implementation
}
```

# Unknowns
- Specific details about the `ImplicitGearTypeConfig` and `GearType` types cannot be determined from this file.
```
