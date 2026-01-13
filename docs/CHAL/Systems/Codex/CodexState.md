# Assets/src/Systems/Research/CodexState.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexState.cs`._

# Purpose
- Defines the `DeedProgress` and `CodexState` classes for tracking progress in a codex system.

# Public API
- Namespace: `CHAL.Systems.Codex`
- Types
  - **[Serializable] class DeedProgress**
    - Public fields/properties:
      - `int waves`: Number of waves completed.
      - `int mapsTotal`: Total number of maps.
      - `Dictionary<MapDifficulty, int> mapsByDifficulty`: Maps completed by difficulty.
      - `int killsGeneralWeighted`: Total weighted kills.
      - `Dictionary<string, int> killsByTagWeighted`: Weighted kills by tag.
      - `int eliteCount`: Count of elite enemies defeated.
      - `int bossCount`: Count of bosses defeated.
      - `int champCount`: Count of champions defeated.
  
  - **[Serializable] class CodexState**
    - Public fields/properties:
      - `Dictionary<string, DeedProgressState> deedProgress`: Progress per deed ID.
      - `List<ActiveFocusSlotState> activeFocusSlots`: Active focus slots for UI/gameplay.
      - `Dictionary<string, DeedGateState> gateCache`: Optional cache for deed gate states.

# Key Behavior & Side Effects
- None explicitly defined in the code.

# Constraints & Failure Modes
- Uses `StringComparer.Ordinal` for dictionary key comparisons to ensure case-sensitive behavior.

# Example
```csharp
var codexState = new CodexState();
codexState.deedProgress["deed1"] = new DeedProgressState();
codexState.activeFocusSlots.Add(new ActiveFocusSlotState());
```

# Unknowns
- The definitions and behaviors of `DeedProgressState`, `ActiveFocusSlotState`, and `DeedGateState` are not provided in this file.

