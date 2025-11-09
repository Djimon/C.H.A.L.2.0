# Assets/src/Editor/ResearchNodeEditorWindow.cs

_Automatically generated/updated from `Assets/src/Editor/ResearchNodeEditorWindow.cs`._

# Purpose
- Defines the `ResearchNodeEditorWindow` for editing `ResearchNodeDef` instances in the Unity Editor.

# Public API
- Namespace: None
- Types
  - public sealed class `ResearchNodeEditorWindow`
    - Public methods
      - `static void ShowFor(ResearchNodeDef node)`
        - Displays the editor window for the specified research node.

# Key Behavior & Side Effects
- Opens a new editor window for a `ResearchNodeDef` when `ShowFor` is called.
- On enable, creates a cached inspector for the research node.
- On disable, destroys the cached inspector if it exists.
- On GUI render:
  - Displays a message if no research node is selected.
  - Allows pinging and selecting the research node.
  - Renders the default inspector or a minimal fallback if the cached inspector is not available.

# Constraints & Failure Modes
- If the provided `ResearchNodeDef` is null, the editor window will not open.
- The editor window requires a valid `ResearchNodeDef` to function properly; otherwise, it shows an info message and provides a close button.

# Example
```csharp
ResearchNodeDef myNode = /* obtain a ResearchNodeDef instance */;
ResearchNodeEditorWindow.ShowFor(myNode);
```

# Unknowns
- None.

