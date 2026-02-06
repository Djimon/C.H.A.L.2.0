# Assets/src/Systems/Research/CodexSnapshot.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexSnapshot.cs`._

# Purpose
- Defines data structures for saving progress in a codex system, including deed progress and active focus slots.

# Public API
- Namespace: `CHAL.Systems.Codex`
- Types
  - **[Serializable] class** `DeedProgressSave`
    - Public fields:
      - `int waves`
      - `int mapsTotal`
      - `List<MapCountEntry> mapsByDifficulty` (difficulty, count)
      - `int killsGeneralWeighted`
      - `List<TagCountEntry> killsByTagWeighted` (tag, count)
      - `int eliteCount`
      - `int bossCount`
      - `int champCount`
  - **[Serializable] class** `CodexSnapshot`
    - Public fields:
      - `int version` (default is 2)
      - `List<DeedProgressEntry> deedProgress`
      - `List<FocusSlotEntry> activeFocusSlots`
  - **[Serializable] struct** `DeedProgressEntry`
    - Public fields:
      - `string deedId`
      - `float progress01`
      - `bool completed`
      - `bool claimed`
      - `DeedProgressSave counters`
  - **[Serializable] struct** `FocusSlotEntry`
    - Public fields:
      - `int slotIndex`
      - `string deedId`

# Key Behavior & Side Effects
- The `CodexSnapshot` class holds persistent data for tracking progress across different deeds and focus slots.

# Constraints & Failure Modes
- No explicit guards or null handling are defined in the code.
- Performance implications are not evident from the provided code.

# Example
```csharp
CodexSnapshot snapshot = new CodexSnapshot();
snapshot.deedProgress.Add(new CodexSnapshot.DeedProgressEntry {
    deedId = "exampleDeed",
    progress01 = 0.5f,
    completed = false,
    claimed = false,
    counters = new DeedProgressSave {
        waves = 10,
        mapsTotal = 5,
        killsGeneralWeighted = 100,
        eliteCount = 2,
        bossCount = 1,
        champCount = 3
    }
});
```

# Unknowns
- No information on how these classes are utilized within the broader application context.

