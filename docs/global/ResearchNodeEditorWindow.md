# global.ResearchNodeEditorWindow

_Automatically generated/updated from `Assets/src/Editor/ResearchNodeEditorWindow.cs`._

# Purpose
- Defines a custom editor window for editing `ResearchNodeDef` objects in the Unity Editor.

# Public API
- Namespace: None
- Types
  - public sealed class ResearchNodeEditorWindow
    - Public methods
      - static void ShowFor(ResearchNodeDef node)
        - Opens the editor window for the specified `ResearchNodeDef`. Returns early if `node` is null.

# Key Behavior & Side Effects
- `ShowFor` method creates and displays the editor window, setting the title and minimum size.
- `OnEnable` initializes the cached inspector for the node if it is not null.
- `OnDisable` cleans up the cached inspector to prevent memory leaks.
- `OnGUI` handles the rendering of the editor window, including displaying a help box if no node is selected and allowing interaction with the node's properties.

# Constraints & Failure Modes
- The `ShowFor` method will not open the window if the provided `ResearchNodeDef` is null.
- The editor window will display a message and a close button if no node is selected.
- The cached inspector is recreated if it does not match the current node.

# Example
```csharp
ResearchNodeDef myNode = /* obtain a ResearchNodeDef instance */;
ResearchNodeEditorWindow.ShowFor(myNode);
```

# Unknowns
- None.

