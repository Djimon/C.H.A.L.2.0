# Assets/src/Data/Defs/CodexDef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/CodexDef.cs`._

# Purpose
- Defines the `CodexDef` ScriptableObject for managing codex-related data in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public sealed class CodexDef : ScriptableObject**
    - Public fields/properties:
      - `int nodeWidth`: Width of the node in the UI.
      - `int nodeHeight`: Height of the node in the UI.
      - `int stageStepY`: Vertical step size for stages in the UI.
      - `List<int> laneBaseX`: Base X positions for lanes in the UI.
      - `int topMarginY`: Top margin in the UI.
      - `Sprite defaultGateGlyph`: Default glyph for gates in the UI.
      - `List<string> alwaysUnlockedIds`: IDs of items that are always unlocked.
      - `List<CodexChapter> codexChapters`: List of chapters in the codex.
  
  - **public sealed class CodexChapter**
    - Public fields/properties:
      - `string chapterId`: Identifier for the chapter.
      - `List<CodexChapterGroup> groups`: List of groups within the chapter.

  - **public sealed class CodexChapterGroup**
    - Public fields/properties:
      - `string groupid`: Identifier for the group.
      - `List<DeedSlot> deedSlots`: List of deed slots in the group.
      - `string dependsOnGroupId`: ID of the group this group depends on for visibility.
      - `float visibleAfterCompletion01`: Completion threshold for visibility.

  - **public sealed class DeedSlot**
    - Public fields/properties:
      - `CodexDeedDef deed`: The deed associated with this slot.
      - `string unlockAfterDeedId`: ID of the deed that must be completed to unlock this slot.
      - `float unlockAfterProgress01`: Progress threshold required to unlock this slot.

# Key Behavior & Side Effects
- The `CodexDef` serves as a configuration asset that holds layout constants, initial unlocks, and the structure of the codex including chapters and groups.

# Constraints & Failure Modes
- No explicit guards or threading considerations are present in the code.
- All lists are initialized to avoid null references.

# Example
```csharp
CodexDef codexDef = ScriptableObject.CreateInstance<CodexDef>();
codexDef.nodeWidth = 300;
codexDef.alwaysUnlockedIds.Add("exampleId");
```

# Unknowns
- No information on how `CodexDeedDef` is defined or used.

