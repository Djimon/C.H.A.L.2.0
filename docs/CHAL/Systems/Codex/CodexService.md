# Assets/src/Systems/Research/CodexService.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexService.cs`._

# Purpose
- Defines the `CodexService` class for managing research nodes and their progress in a codex system.

# Public API
- Namespace: `CHAL.Systems.Codex`
- Types
  - public sealed class `CodexService`
    - Public fields/properties:
      - `OnNodeCompleted`: Event triggered when a node is completed.
      - `OnAlwaysUnlockedReady`: Event triggered when always unlocked nodes are ready.
      - `OnCodexChanged`: Event triggered when the codex changes.
    - Public methods:
      - `void InitFromDef(CodexDef treeDef, CodexState state)`: Initializes the service with a codex definition and state.
      - `int GetFocusSlotCount()`: Returns the count of active focus slots.
      - `string GetActiveDeedId(int slotIndex)`: Returns the deed ID of the active deed in the specified slot.
      - `bool TrySetActiveFocus(int slotIndex, string deedId, out string reason)`: Attempts to set the active focus for a slot; returns success status and reason.
      - `bool TryClaim(string deedId, out string reason)`: Attempts to claim a deed; returns success status and reason.
      - `bool IsClaimed(string deedId)`: Checks if a deed is claimed.
      - `bool IsClaimable(string deedId)`: Checks if a deed can be claimed.
      - `bool IsSlotLocked(int slotIndex)`: Checks if a focus slot is locked.
      - `DeedProgress GetNodeProgress(string deedId)`: Returns the progress of a specified deed.
      - `CodexDeedDef GetNodeDef(string deedId)`: Returns the definition of a specified deed.
      - `void OnWaveCompleted(int waveIndex, int waveCount, MapDifficulty difficulty)`: Updates progress based on wave completion.
      - `void OnMapCompleted(int mapId, MapDifficulty difficulty)`: Updates progress based on map completion.
      - `void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> tagsWeighted, List<string> tagsRaw)`: Updates progress based on an enemy being killed.
      - `IReadOnlyList<ChapterVM> GetChaptersVM()`: Returns a list of chapter view models.
      - `ChapterVM GetChapterVM(string chapterId)`: Returns a chapter view model for a specified chapter ID.

# Key Behavior & Side Effects
- Initializes internal state and structures based on the provided `CodexDef` and `CodexState`.
- Raises events when nodes are completed or when the codex changes.
- Updates progress based on game events such as waves completed, maps completed, and enemies killed.

# Constraints & Failure Modes
- Handles null or empty inputs gracefully, returning appropriate failure reasons.
- Ensures that deeds cannot be activated in multiple slots and that only available deeds can be activated.
- Progress is only updated for deeds that are active and not locked.

# Example
```csharp
var codexService = new CodexService();
codexService.InitFromDef(codexDef, codexState);
if (codexService.TrySetActiveFocus(0, "deedId", out var reason))
{
    // Successfully set active focus
}
```

# Unknowns
- The exact structure and contents of `CodexDef`, `CodexState`, `CodexDeedDef`, and `DeedProgress` are not defined in this file.

