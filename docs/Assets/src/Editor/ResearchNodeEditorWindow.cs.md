# Assets/src/Editor/ResearchNodeEditorWindow.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a custom editor window for editing `ResearchNodeDef` objects in the Unity Editor.

## Public API
- Namespace: None
- Types
  - `public sealed class ResearchNodeEditorWindow` 
    - Public methods:
      - `static void ShowFor(ResearchNodeDef node)` 
        - Opens the editor window for the specified `ResearchNodeDef`. 
        - Side effect: Logs the opening of the window.

## Key Behavior & Side Effects
- `OnEnable`: Initializes the cached inspector for the `_node` if it is not null.
- `OnDisable`: Destroys the cached inspector if it exists.
- `OnGUI`: 
  - Displays a message if no `ResearchNodeDef` is selected.
  - Provides buttons to ping or select the `ResearchNodeDef`.
  - Renders the default inspector or a fallback UI for the `ResearchNodeDef`.
  - Marks the `ResearchNodeDef` as dirty if changes are made.

## Constraints & Failure Modes
- Guards against null `ResearchNodeDef` in `ShowFor` and `OnGUI`.
- Handles the case where the cached inspector does not match the current `_node`.
- Uses `DestroyImmediate` to clean up the cached inspector.

## Example
```csharp
ResearchNodeEditorWindow.ShowFor(myResearchNodeDef);
```

## Unknowns
- The implementation details of `ResearchNodeDef` and its properties.
```
