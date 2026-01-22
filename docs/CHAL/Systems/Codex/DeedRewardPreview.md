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
      - `void InitFromDef(CodexDef treeDef, CodexState state)`
      - `void EnsureFocusSlotCount(int requiredCount)`
      - `bool TryGetDeedCouponsPreview(string deedId, out int coupons)`
      - `float GetDeedDifficultyScorePreview(string deedId)`
      - `int GetFocusSlotCount()`
      - `string GetActiveDeedId(int slotIndex)`
      - `bool TrySetActiveFocus(int slotIndex, string deedId, out string reason)`
      - `bool TryClaim(string deedId, out string reason)`
      - `void PayRewards(string deedId)`
      - `bool TryUnlockNextFocusSlot(out string reason)`
      - `bool IsClaimed(string deedId)`
      - `bool IsClaimable(string deedId)`
      - `bool IsSlotLocked(int slotIndex)`
      - `DeedProgress GetNodeProgress(string deedId)`
      - `CodexDeedDef GetNodeDef(string deedId)`
      - `void OnWaveCompleted(int waveIndex, int waveCount, MapDifficulty difficulty)`
      - `void OnMapCompleted(int mapId, MapDifficulty difficulty)`
      - `void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> tagsWeighted, List<string> tagsRaw)`
      - `void OnCraftExecuted(CraftType type, string recipe, int tier)`
      - `IReadOnlyList<ChapterVM> GetChaptersVM()`
      - `ChapterVM GetChapterVM(string chapterId)`

# Key Behavior & Side Effects
- Initializes the codex service with a definition and state, compiling nodes and ensuring progress.
- Handles claiming deeds, updating their progress and triggering events for completion and rewards.
- Manages focus slots, ensuring they meet required counts and updating their locked status based on deed claimability.
- Updates progress based on game events such as waves completed, maps completed, enemies killed, and crafting actions.

# Constraints & Failure Modes
- Methods like `TrySetActiveFocus` and `TryClaim` return false with a reason if the operation fails due to invalid input or state.
- Focus slots must be managed carefully to avoid exceeding maximum limits or attempting to claim already claimed deeds.
- Requires valid `CodexDef` and `CodexState` objects for initialization.

# Example
```csharp
CodexService codexService = new CodexService();
codexService.InitFromDef(codexDef, codexState);
```

# Unknowns
- The exact structure and contents of `CodexDef`, `CodexState`, `DeedProgress`, and other related types are not defined in this file.

