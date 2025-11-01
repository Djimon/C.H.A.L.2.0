# CHAL.Data.InventorySnapshot

_Automatically generated/updated from `Assets/src/Core/PlayerProfile.cs`._

1) Purpose
- Serializable data model PlayerProfile implementing IWallet that stores player state (name, colors, XP/level, currencies, inventories, unlocked heroes, map progress) and related runtime data (inventory/research snapshots, non-serialized runtime state).
- Provides methods for XP/level progression, currency management, hero management, map progress, and save/load snapshot handling.
- Defines InventorySnapshot for serializing inventory contents.

2) Public API

- Namespace/module
  - CHAL.Data

- Types
  - public class PlayerProfile : IWallet
    - Public fields
      - string profileId; // identifier for the profile (derived from name on init)
      - DateTime LastSaveTime; // autosave/debug timestamp
      - string playerName; // display/name
      - Color[] playerColors; // character customization colors
      - int XP; // total XP
      - int Level; // current level (may be derived from XP)
      - int XPInCurrentLevel; // XP within the current level
      - int XPToNextLevel; // XP required for next level
      - int missingXP; // XP remaining to reach next level
      - float levelProgress; // 0..1 UI progress into current level
      - List<string> UnlockedHeroes = new(); // IDs of unlocked heroes
      - Dictionary<string, int> Currencies = new(); // currency balances (e.g., { "gold": 0 })
      - Dictionary<int, int> MapProgress = new(); // map progress: mapNo -> highest difficulty completed
      - [NonSerialized] public List<Inventory> Inventories = new(); // live inventories (not serialized)
      - public List<InventorySnapshot> InventorySave = new(); // serialized inventory snapshots (save/load)
      - [NonSerialized] public ResearchState ResearchRuntime; // runtime research state (not serialized)

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

  - public struct InventorySnapshot
    - string id; // e.g., "remains", "part", "rune", "module", "gear"
    - Dictionary<string, int> items; // flat map (itemId -> count)

3) Key Behavior & Side Effects

- InitializePlayer
  - Sets name/colors, builds profileId, ensures starter hero unlocked, initializes inventories, adds gold (0), triggers save via SaveSystem.Save(this).

- InitInventories
  - Adds five inventories with IDs: "remains", "part", "rune", "module", "gear".

- XP/Level
  - AddXP(amount) increments XP and triggers RecalculateLevel().
  - RecalculateLevel() computes Level using BalanceManager.GetXpForLevel(level); updates XPInCurrentLevel, XPToNextLevel, missingXP, levelProgress; logs progress.

- Currency
  - GetCurrency returns balance or 0 if missing.
  - AddCurrency increments balance if amount > 0; initializes entry if needed.
  - SpendCurrency validates amount > 0 and sufficient funds; subtracts amount and returns true on success.
  - CanSpend validates amount > 0 and funds availability.
  - Refund adds currency amount via AddCurrency.

- Heroes
  - GetUnlockedHeroes returns a read-only view.
  - IsHeroUnlocked checks for non-empty id and presence in UnlockedHeroes.
  - UnlockHero adds id if not present; returns true on success.
  - LockHero removes id if present; returns true on success.
  - EnsureStarterHeroUnlocked ensures a starter hero is in UnlockedHeroes; returns true if added.

- Map Progress
  - SetMapProgress stores the highest completed difficulty for a map.
  - GetMapProgress returns stored value or 0 if not present.

- Inventory Snapshot
  - PrepareInventorySnapshot builds InventorySave from live Inventories via ToDictionary() and stores into InventorySnapshot list; logs build details.
  - RestoreInventoriesFromSnapshot reconstitutes live Inventories from InventorySave using FromDictionary(); ensures Inventories exist prior to restoration; logs result.

- Research Snapshot
  - BuildResearchSnapshotFrom(state) creates a ResearchSnapshot from a given ResearchState, copying active node, completed nodes, and per-node progress (including maps and kills data).
  - RestoreResearchInto(state, snap) applies a ResearchSnapshot back into a ResearchState, clearing and repopulating progress data.

4) Constraints & Failure Modes

- AddCurrency
  - Ignores non-positive amounts (no change).

- SpendCurrency / CanSpend / Spend checks
  - Require amount > 0; insufficient funds causes failure.

- EnsureStarterHeroUnlocked
  - Returns false if id is null/empty or already unlocked; returns true if newly added.

- Inventory handling
  - PrepareInventorySnapshot initializes InventorySave if null; uses ToDictionary() with a fallback to empty dictionary.
  - RestoreInventoriesFromSnapshot assumes InventorySave non-null; inflates live inventories if needed; may skip unknown snapshot entries.

- Map/XP logic
  - GetMapProgress returns 0 if map not present; SetMapProgress may create a new entry.
  - RecalculateLevel relies on BalanceManager.GetXpForLevel; behavior depends on BalanceManager implementation.

- Research handling
  - BuildResearchSnapshotFrom assumes state may be null; otherwise copies progress; RestoreResearchInto assumes snap may be null; nulls in nested structures could lead to null references if underlying collections are missing.

- Serialization
  - Inventories and ResearchRuntime are marked NonSerialized; not persisted to storage.

- Threading/Concurrency
  - No explicit synchronization; behavior assumes single-threaded usage typical of Unity game state.

5) Example

```csharp
using CHAL.Data;
using UnityEngine;

public class Demo
{
    public void CreateProfile()
    {
        var profile = new PlayerProfile();
        profile.InitializePlayer("Alice", new Color[] { Color.white, Color.blue });
        // Further usage...
    }
}
```

6) Unknowns

- Definitions and behaviors of:
  - IWallet interface
  - Inventory class (methods ToDictionary, FromDictionary) and Inventory management semantics
  - BalanceManager.GetXpForLevel, DebugManager.Log, GameManager.Instance, SaveSystem.Save
  - ResearchState, ResearchSnapshot, NodeProgressEntry, MapDifficulty, and related research data structures
- Thread-safety guarantees or race conditions
- Exact runtime behavior for null/empty collections in some edge cases (e.g., state.perNodeProgress in BuildResearchSnapshotFrom)
- Any higher-level persistence format or file layout used by SaveSystem

Note: This file is a Unity data container with serialization hints and runtime helpers for save/load and state management.
