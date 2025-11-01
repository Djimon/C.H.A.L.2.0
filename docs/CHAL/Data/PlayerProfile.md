# CHAL.Data.PlayerProfile

_Automatically generated/updated from `Assets/src/Core/PlayerProfile.cs`._

1) Purpose
- Serializable data model for a player profile in CHAL, implementing IWallet; aggregates customization, progress, currencies, map progress, inventories, and research state.
- Provides initialization, state management, and snapshot/restore utilities for inventories and research data.
- Exposes public API to modify XP, currency, hero unlocks, map progress, and to convert between in-memory state and saved snapshots.

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public class PlayerProfile : IWallet
    - Public fields
      - string profileId
      - DateTime LastSaveTime
      - string playerName
      - Color[] playerColors
      - int XP
      - int Level
      - int XPInCurrentLevel
      - int XPToNextLevel
      - int missingXP
      - float levelProgress
      - List<string> UnlockedHeroes = new()
      - Dictionary<string, int> Currencies = new()
      - Dictionary<int, int> MapProgress = new()
      - [NonSerialized] public List<Inventory> Inventories = new()
      - public List<InventorySnapshot> InventorySave = new()
      - [NonSerialized] public ResearchState ResearchRuntime
    - Public methods
      - void InitializePlayer(string name, Color[] colors)
      - void InitInventories()
      - int GetXP()
      - void AddXP(int amount)
      - int GetCurrency(string currencyId)
      - void AddCurrency(string currencyId, int amount)
      - bool SpendCurrency(string currencyId, int amount)
      - bool CanSpend(string currencyId, int amount)
      - void Refund(string currencyId, int amount)
      - IReadOnlyList<string> GetUnlockedHeroes()
      - bool IsHeroUnlocked(string heroId)
      - bool UnlockHero(string heroId)
      - bool LockHero(string heroId)
      - bool EnsureStarterHeroUnlocked(string starterHeroId)
      - void SetMapProgress(int map, MapDifficulty difficulty)
      - int GetMapProgress(int map)
      - private void RecalculateLevel()
      - void PrepareInventorySnapshot()
      - void RestoreInventoriesFromSnapshot()
      - ResearchSnapshot BuildResearchSnapshotFrom(ResearchState state)
      - void RestoreResearchInto(ResearchState state, ResearchSnapshot snap)
- Nested/related types
  - public struct InventorySnapshot
    - public string id
    - public Dictionary<string, int> items

3) Key Behavior & Side Effects
- XP/Level
  - AddXP(int amount) increases XP and triggers RecalculateLevel() to update Level, XPInCurrentLevel, XPToNextLevel, missingXP, and levelProgress using BalanceManager.GetXpForLevel(level).
- Initialization
  - InitializePlayer(name, colors) sets name/colors, derives profileId as "p_" + name, ensures starter hero via EnsureStarterHeroUnlocked, initializes inventories, and saves via SaveSystem.Save(this).
- Inventories
  - InitInventories() populates Inventories with five inventories: "remains", "part", "rune", "module", "gear".
  - PrepareInventorySnapshot() builds InventorySave from live Inventories by converting each to a dictionary representation (inv.ToDictionary()).
  - RestoreInventoriesFromSnapshot() restores live Inventories from InventorySave by matching IDs and applying FromDictionary(snap.items).
- Currencies
  - GetCurrency(string) returns current amount or 0 if missing.
  - AddCurrency(string, int) increases currency if amount > 0; initializes missing keys to 0.
  - SpendCurrency(string, int) checks amount > 0 and CanSpend, then deducts from Currencies.
  - CanSpend(string, int) requires amount > 0 and sufficient balance.
  - Refund(string, int) adds the amount back via AddCurrency.
- Heroes
  - GetUnlockedHeroes() exposes a read-only view of UnlockedHeroes.
  - IsHeroUnlocked(string) checks presence in UnlockedHeroes.
  - UnlockHero(string) adds heroId if valid and not already present.
  - LockHero(string) removes heroId if present.
  - EnsureStarterHeroUnlocked(string) ensures a starter hero is in UnlockedHeroes (returns false if invalid or already unlocked); creates list if null.
- Map progress
  - SetMapProgress(int map, MapDifficulty difficulty) stores the numeric value of difficulty in MapProgress[map].
  - GetMapProgress(int map) returns stored progress or 0 if absent.
- Research
  - BuildResearchSnapshotFrom(ResearchState state) creates a ResearchSnapshot from a non-null state, copying active/null fields and per-node progress.
  - RestoreResearchInto(ResearchState state, ResearchSnapshot snap) applies snapshot data back into a given ResearchState, clearing existing per-node progress and repopulating from the snapshot.
- Snapshots
  - InventorySave is used for persistence of inventories (non-serialized live inventories vs persisted snapshot).
  - ResearchRuntime is non-serialized; Snapshot-related operations convert between runtime state and serializable snapshots.

4) Constraints & Failure Modes
- Guard clauses
  - AddCurrency ignores non-positive amounts.
  - SpendCurrency and CanSpend reject non-positive amounts.
  - EnsureStarterHeroUnlocked returns false on null/empty input; ensures non-null UnlockedHeroes.
  - PrepareInventorySnapshot assumes Inventories exists; iterates non-null inventories.
  - RestoreInventoriesFromSnapshot gracefully handles null InventorySave or missing inventories (skips with a warning in logs).
- State assumptions
  - MapProgress is initialized to a new dictionary; Set/Get rely on this existing container.
  - Inventories is non-serialized; InventorySave is the serialized snapshot backing store.
  - ResearchRuntime is non-serialized; BuildResearchSnapshotFrom/RestoreResearchInto operate on provided state/snapshot.
- Threading/async
  - Public methods do not appear to be thread-safe; external callers should synchronize access if needed (not explicit in file).
- Performance
  - RecalculateLevel recalculates from scratch; XP-based level computation uses BalanceManager.GetXpForLevel in a loop.
- Null handling
  - Some methods defensively check for null inputs (e.g., BuildResearchSnapshotFrom, RestoreResearchInto). Others assume non-null (e.g., MapProgress, Inventories).

5) Example
- Not derivable from this file in a concise, deterministic snippet without external context; omitted.

6) Unknowns
- Definitions and behavior of external types/interfaces: IWallet, Inventory, InventorySnapshot, InventoryToDictionary/FromDictionary, BalanceManager, DebugManager, GameManager, SaveSystem, MapDifficulty, ResearchState, ResearchSnapshot, NodeProgress, NodeProgressEntry, MapRequirement, KillTagCount, and related serialization behavior.
- Exact gameplay implications of certain methods (e.g., how XP thresholds map to levels via BalanceManager) are not defined here.
- Any side effects of SaveSystem.Save(this) (e.g., asynchronous operations, serialization format) are not shown in this file.
