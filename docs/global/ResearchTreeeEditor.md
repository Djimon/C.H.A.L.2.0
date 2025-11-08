# global.ResearchTreeeEditor

_Automatically generated/updated from `Assets/src/Editor/ResearchTreeeEditor.cs`._

# Purpose
- Defines a custom editor for the `ResearchTreeDef` class in Unity, allowing for visual configuration of research lanes and stages.

# Public API
- Namespace: `CHAL.Editor`
- Types
  - `ResearchTreeDefEditor` [extends `Editor`]
    - Public fields/properties: None
    - Public methods:
      - `OnEnable()`: Initializes the editor and builds stage and node lists.
      - `OnInspectorGUI()`: Draws the custom inspector GUI for the component.

# Key Behavior & Side Effects
- On enabling the editor, it initializes the `ResearchTreeDef` target and builds lists for stages and nodes.
- The inspector GUI allows users to add, remove, and modify stages and nodes within the research tree.
- Validation of "Always Unlocked IDs" is performed when the corresponding button is clicked, checking for overlaps with node unlocks.
- Compiling the research tree logs details about lanes, stages, nodes, and parent links.

# Constraints & Failure Modes
- If `researchTreeLanes` is null, no stages or nodes can be displayed or modified.
- The editor relies on serialized properties; if they are not properly set up, it may lead to unexpected behavior.
- The `CreateNewNodeAsset` method requires a valid directory path; if the path is invalid, it will create a new directory.

# Example
```csharp
// Example of using the ResearchTreeDefEditor in Unity
[CustomEditor(typeof(ResearchTreeDef))]
public class ExampleUsage : ResearchTreeDefEditor
{
    // Custom implementation can go here
}
```

# Unknowns
- The exact structure and properties of `ResearchTreeDef`, `ResearchLane`, and `ResearchNodeDef` are not defined in this file.
- The behavior of `ResearchTreeCompiler.Compile` is not detailed in this file.

