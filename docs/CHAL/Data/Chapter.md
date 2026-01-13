# Assets/src/Data/Defs/CodexTreeDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/CodexTreeDef.cs`._

# Purpose
- Defines the `CodexTreeDef` ScriptableObject for managing research tree data in the game.
- Provides methods to retrieve chapter names and colors based on lane indices.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public sealed class** `CodexTreeDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `List<Chapter> chapters`: List of chapters with names and colors.
      - `int nodeWidth`: Width of nodes in the UI (minimum 1).
      - `int nodeHeight`: Height of nodes in the UI (minimum 1).
      - `int stageStepY`: Vertical step between stages in the UI (minimum 1).
      - `List<int> laneBaseX`: Base X positions for lanes.
      - `int topMarginY`: Top margin for the UI.
      - `Sprite defaultGateGlyph`: Default glyph for gates in the UI.
      - `List<string> alwaysUnlockedIds`: List of IDs that are always unlocked.
      - `List<CodexChapter> codexChapters`: List of chapters in the actual research tree.
    - Public methods:
      - `string GetChapterName(int lane)`: Returns the name of the specified lane or "unknown lane" if out of range.
      - `Color GetChapterColor(int lane)`: Returns the color of the specified lane or black if out of range.
  - **[Serializable] public struct** `Chapter`
    - Public fields/properties:
      - `string chapterName`: Name of the chapter.
      - `Color chapterColor`: Color associated with the chapter.
  - **[Serializable] public sealed class** `CodexChapter`
    - Public fields/properties:
      - `string chapterName`: Name of the chapter.
      - `Color chapterColor`: Color associated with the chapter.
      - `List<CodexStageGroup> stages`: List of stage groups in the chapter.
  - **[Serializable] public sealed class** `CodexStageGroup`
    - Public fields/properties:
      - `string groupName`: Name of the stage group.
      - `List<CodexDeedRef> deeds`: List of deeds in the stage group.
  - **[Serializable] public sealed class** `CodexDeedRef`
    - Public fields/properties:
      - `CodexDeedDef node`: Reference to the deed node.
      - `List<CodexDeedDef> parentRefs`: List of parent deed references.

# Key Behavior & Side Effects
- `GetChapterName(int lane)`: Validates the lane index and returns the corresponding chapter name or a default message.
- `GetChapterColor(int lane)`: Validates the lane index and returns the corresponding chapter color or a default color (black).

# Constraints & Failure Modes
- Methods `GetChapterName` and `GetChapterColor` handle out-of-range indices by returning default values.
- Fields `nodeWidth`, `nodeHeight`, and `stageStepY` are constrained to a minimum value of 1.

# Example
```csharp
CodexTreeDef codexTree = ScriptableObject.CreateInstance<CodexTreeDef>();
string chapterName = codexTree.GetChapterName(0);
Color chapterColor = codexTree.GetChapterColor(0);
```

# Unknowns
- The behavior and structure of `CodexDeedDef` are not defined in this file.
