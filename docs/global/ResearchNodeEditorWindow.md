# global.ResearchNodeEditorWindow

_Automatically generated/updated from `Assets/src/Editor/ResearchNodeEditorWindow.cs`._

# Purpose
- Defines the `ResearchNodeEditorWindow` class for displaying and editing `ResearchNodeDef` objects in the Unity Editor.

# Public API
- Namespace/module: None
- Types
  - public sealed class `ResearchNodeEditorWindow`
    - Public methods
      - `static void ShowFor(ResearchNodeDef node)` - Opens the editor window for the specified research node.

# Key Behavior & Side Effects
- `ShowFor` method creates and displays the editor window for a given `ResearchNodeDef`, logging the action.
- On enabling, it caches the inspector for the research node.
- On disabling, it destroys the cached inspector.
- The `OnGUI` method handles the rendering of the editor window, including displaying a help box if no node is selected and allowing for property editing.

# Constraints & Failure Modes
- If the provided `ResearchNodeDef` is null, the editor window will not open.
- The editor window will display a message if no research node is selected.
- The cached inspector is recreated if it does not match the current node.

# Example
```csharp
ResearchNodeEditorWindow.ShowFor(myResearchNodeDef);
```

# Unknowns
- None.

