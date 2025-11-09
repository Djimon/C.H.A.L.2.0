# Assets/src/Editor/ResearchTreeeEditor.cs

_Automatically generated/updated from `Assets/src/Editor/ResearchTreeeEditor.cs`._

# Purpose
- Defines a custom editor for the `ResearchTreeDef` class in Unity, allowing for visual configuration of research lanes and stages.

# Public API
- Namespace: `CHAL.Editor`
- Types
  - `ResearchTreeDefEditor` [extends `Editor`]
    - Public methods
      - `OnInspectorGUI()`: Draws the custom inspector GUI for the component.

# Key Behavior & Side Effects
- OnEnable: Initializes the editor and builds lists for stages and nodes.
- OnInspectorGUI: Renders the inspector UI, allowing users to modify research lanes, stages, and nodes.
- ValidateAlwaysUnlockedIds: Validates the IDs of always unlocked nodes against existing node IDs.
- RunCompile: Compiles the research tree and logs the results or errors.

# Constraints & Failure Modes
- Handles null or empty properties for `researchTreeLanes` and `stages`.
- Uses `Undo.RecordObject` for actions that modify the serialized properties, allowing for undo functionality.
- GUI operations are performed in the Unity Editor context and may not function outside of it.

# Example
```csharp
// To use the custom editor, simply select a ResearchTreeDef asset in the Unity Editor.
```

# Unknowns
- The exact structure and properties of `ResearchTreeDef`, `ResearchLane`, and `ResearchNodeDef` cannot be determined from this file.

