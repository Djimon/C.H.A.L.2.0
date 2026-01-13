# Assets/src/Systems/Research/CodexViewModel.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexViewModel.cs`._

# Purpose
- Defines data structures for a codex system, including chapters, groups, and deeds.

# Public API
- Namespace: `CHAL.Systems.Codex`
- Types
  - `ChapterVM`
    - Public fields:
      - `string chapterId`: Identifier for the chapter.
      - `List<GroupVM> groups`: Collection of groups within the chapter.
  - `GroupVM`
    - Public fields:
      - `string groupId`: Identifier for the group.
      - `GroupGateState gate`: State of the group gate.
      - `List<DeedVM> deeds`: Collection of deeds within the group.
  - `DeedVM`
    - Public fields:
      - `string deedId`: Identifier for the deed.
      - `string title`: Title of the deed.
      - `DeedGateState gate`: State of the deed gate.
      - `float progress01`: Progress of the deed (0.0 to 1.0).
      - `bool completed`: Indicates if the deed is completed.
      - `bool claimed`: Indicates if the deed has been claimed.
      - `bool isActive`: Indicates if the deed is currently active.
      - `int activeSlotIndex`: Index of the active slot (-1 if not active).
      - `bool isSlotLocked`: Indicates if the slot is locked (derived from "claimable").

# Key Behavior & Side Effects
- No explicit behavior or side effects are defined in this file.

# Constraints & Failure Modes
- No specific guards, null/empty handling, or threading/async notes are present in this file.

# Example
```csharp
var chapter = new ChapterVM
{
    chapterId = "chapter1",
    groups = new List<GroupVM>
    {
        new GroupVM
        {
            groupId = "group1",
            gate = new GroupGateState(),
            deeds = new List<DeedVM>
            {
                new DeedVM
                {
                    deedId = "deed1",
                    title = "First Deed",
                    gate = new DeedGateState(),
                    progress01 = 0.5f,
                    completed = false,
                    claimed = false,
                    isActive = true,
                    activeSlotIndex = 0,
                    isSlotLocked = false
                }
            }
        }
    }
};
```

# Unknowns
- The definitions and behaviors of `GroupGateState` and `DeedGateState` are not provided in this file.
