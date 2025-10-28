# global.ResearchTreeeEditor

_Automatically generated/updated from `Assets/src/Editor/ResearchTreeeEditor.cs`._

# Purpose
- Defines a custom editor for `ResearchTreeDef` in Unity's Editor.
- Provides functionality to manage and visualize research lanes and stages.

# Public API
- Namespace: `CHAL.Editor`
- Types
  - `ResearchTreeDefEditor` [extends `Editor`]
    - Public fields/properties: None
    - Public methods:
      - `OnEnable()`: Initializes the editor and builds lists.
      - `OnInspectorGUI()`: Draws the custom inspector GUI.
      - `ShowParentPickerMenu(SerializedProperty parentsProp, int laneIndex, int stageIndex)`: Displays a menu to select parent nodes.

# Key Behavior & Side Effects
- On enabling the editor, it initializes the `ResearchTreeDef` and builds reorderable lists for stages and nodes.
- The `OnInspectorGUI` method handles the drawing of the inspector, including lane tabs and validation buttons.
- The `RunCompile()` method compiles the research tree and logs the results or errors.

# Constraints & Failure Modes
- Handles null checks for properties like `researchTreeLanes` and `stages`.
- Uses `Undo.RecordObject` for actions that modify the serialized properties to ensure changes can be undone.
- GUI elements are disabled if there are no lanes or stages defined.

# Example
```csharp
// To use the custom editor, simply select a ResearchTreeDef asset in the Unity Editor.
```

# Unknowns
- The exact structure of `ResearchTreeDef`, `ResearchLane`, and `ResearchTreeLane` is not defined in this file.
- The behavior of `ResearchTreeCompiler.Compile()` is not detailed in this file.

