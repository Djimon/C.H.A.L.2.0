# Assets/src/Systems/Research/CodexStructs.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexStructs.cs`._

# Purpose
- Defines data structures for tracking progress and state related to deeds and groups in a research system.

# Public API
- Namespace: CHAL.Systems.Codex

- Types
  - public struct DeedProgressState
    - Public fields:
      - float progress01: UI-friendly progress representation.
      - bool completed: Indicates if the deed is completed.
      - bool claimed: Indicates if the deed has been claimed.
      - DeedProgress counters: Holds counters related to the deed's progress.
  
  - public struct ActiveFocusSlotState
    - Public fields:
      - string deedId: Identifier for the associated deed.
      - bool locked: Indicates if the slot is locked.

  - public struct DeedGateState
    - Public fields:
      - bool isVisible: Indicates if the gate is visible.
      - bool isAvailable: Indicates if the gate is available.
      - string blockedByDeedId: Identifier of the deed blocking access.
      - float blockedByRequProgress01: Required progress for the blocking deed.
      - string blockedByGroupId: Identifier of the group blocking access.
      - float blockedByRequGroupProgress01: Required progress for the blocking group.

  - public struct GroupGateState
    - Public fields:
      - bool isVisible: Indicates if the group gate is visible.
      - float completion01: Ratio of claimed count to total.
      - float requiredCompletion01: Progress required for visibility.
      - string dependsOnGroupId: Identifier of the group this gate depends on.

# Key Behavior & Side Effects
- No explicit behaviors or side effects are defined in the provided code.

# Constraints & Failure Modes
- No specific guards, null/empty handling, or threading/async notes are present in the provided code.

# Example
```csharp
DeedProgressState progressState = new DeedProgressState
{
    progress01 = 0.5f,
    completed = false,
    claimed = false,
    counters = new DeedProgress() // Assuming DeedProgress is defined elsewhere
};
```

# Unknowns
- The definition and structure of `DeedProgress` are not provided in this file.
