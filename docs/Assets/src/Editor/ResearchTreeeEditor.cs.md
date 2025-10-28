# Assets/src/Editor/ResearchTreeeEditor.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a custom editor for `ResearchTreeDef` in Unity's Editor.
- Provides functionality to manage and visualize research lanes and stages.

## Public API
- Namespace: `CHAL.Editor`
- Types
  - `ResearchTreeDefEditor` [extends `Editor`]
    - Public fields/properties: None
    - Public methods:
      - `OnInspectorGUI()`: Draws the custom inspector GUI.
      - `ShowParentPickerMenu(SerializedProperty parentsProp, int laneIndex, int stageIndex)`: Displays a menu to select parent nodes.

## Key Behavior & Side Effects
- On enabling the editor, initializes the research tree and builds lists for stages and nodes.
- Allows adding and removing stages and nodes through a reorderable list interface.
- Syncs tree lanes with visual lanes and validates the research tree structure.
- Compiles the research tree and logs the results or errors.

## Constraints & Failure Modes
- Handles null checks for properties like `researchTreeLanes` and `stages`.
- Uses `Undo.RecordObject` for actions that modify the serialized properties.
- Ensures that the GUI updates correctly after modifications.

## Example
```csharp
// Example usage in Unity Editor
[CustomEditor(typeof(ResearchTreeDef))]
public class ExampleUsage : ResearchTreeDefEditor
{
    // Custom implementation can go here
}
```

## Unknowns
- Specific details about the `ResearchTreeDef` structure and its properties are not defined in this file.
- The behavior of `ResearchTreeCompiler.Compile` is not detailed in this file.
```
