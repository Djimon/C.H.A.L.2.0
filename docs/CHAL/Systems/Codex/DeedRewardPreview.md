# Assets/src/Systems/Research/CodexService.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexService.cs`._

# Purpose
- Defines the `CodexService` class for managing codex deeds, their progress, and rewards in the game.

# Public API
- Namespace: `CHAL.Systems.Codex`
- Types
  - public sealed class `CodexService`
    - Public fields/properties:
      - `OnNodeCompleted`: Event triggered when a node is completed.
      - `OnAlwaysUnlockedReady`: Event triggered when always unlocked nodes are ready.
      - `OnCodexChanged`: Event triggered when the codex changes.
      - `OnDeedClaimed`: Event triggered when a deed is claimed.
    - Public methods:
      - `void InitFromDef(CodexDef treeDef, CodexState state)`: Initializes the service with a codex definition and state.
      - `void EnsureFocusSlotCount(int requiredCount)`: Ensures the number of active focus slots meets the required count.
      - `int GetDeedCouponsPreview(string deedId)`: Returns the coupon preview for a deed.
      - `bool TryGetDeedCouponsPreview(string deedId, out int coupons)`: Attempts to get the coupon preview for a deed.
      - `float GetDeedDifficultyScorePreview(string deedId)`: Returns the difficulty score preview for a deed.
      - `int GetFocusSlotCount()`: Returns the count of active focus slots.
      - `string GetActiveDeedId(int slotIndex)`: Returns the active deed ID for a specified slot index.
      - `bool TrySetActiveFocus(int slotIndex, string deedId, out string reason)`: Attempts to set the active focus for a specified slot index.
      - `bool TryClaim(string deedId, out string reason)`: Attempts to claim a deed by its ID.
      - `void PayRewards(string deedId)`: Pays rewards for a claimed deed.
      - `bool TryUnlockNextFocusSlot(out string reason)`: Attempts to unlock the next focus slot.
      - `bool IsClaimed(string deedId)`: Checks if a deed is claimed.
      - `bool IsClaimable(string deedId)`: Checks if a deed is claimable.
      - `bool IsSlotLocked(int slotIndex)`: Checks if a specified slot is locked.
      - `bool IsNodeAvailable(string nodeId)`: Checks if a node is available based on its ID.
      - `DeedProgress GetNodeProgress(string deedId)`: Returns the progress of a node.
      - `CodexDeedDef GetNodeDef(string deedId)`: Returns the definition of a node.
      - `void OnWaveCompleted(int waveIndex, int waveCount, MapDifficulty difficulty)`: Invoked when a wave is completed.
      - `void OnMapCompleted(int mapId, MapDifficulty difficulty)`: Invoked when a map is completed.
      - `void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> tagsWeighted, List<string> tagsRaw)`: Invoked when an enemy is killed.
      - `void OnCraftExecuted(CraftType type, string recipe, int tier)`: Invoked when a craft is executed.
      - `IReadOnlyList<ChapterVM> GetChaptersVM()`: Returns a list of chapter view models.
      - `ChapterVM GetChapterVM(string chapterId)`: Returns a chapter view model by chapter ID.

# Key Behavior & Side Effects
- Initializes the codex service with a definition and state, compiling nodes and setting up rewards.
- Handles focus slots, ensuring at least one is active and managing their locked state.
- Updates progress for deeds based on game events such as waves completed, maps completed, enemies killed, and crafts executed.
- Raises events to notify subscribers of changes in the codex state.

# Constraints & Failure Modes
- Methods that accept IDs (e.g., `deedId`, `nodeId`) check for null or whitespace and return failure reasons if invalid.
- Focus slots must be managed carefully to avoid invalid access or state inconsistencies.
- Events are invoked only if there are subscribers.

# Example
```csharp
var codexService = new CodexService();
codexService.InitFromDef(codexDef, null);
if (codexService.TrySetActiveFocus(0, "deedId123", out var reason))
{
    // Focus set successfully
}
else
{
    // Handle failure: reason contains the failure message
}
```

# Unknowns
- The exact structure and content of `CodexDef`, `CodexState`, `DeedProgress`, and other related types are not defined in this file.

