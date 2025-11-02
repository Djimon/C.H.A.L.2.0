# CHAL.Data.PlayerProfile

_Automatically generated/updated from `Assets/src/Core/PlayerProfile.cs`._

1) Purpose
- Serializable data model for a player's profile, including meta, customization, progress, currencies, inventories, and research state.
- Provides methods to manipulate XP, currencies, unlocks, map progress, and to snapshot/restore inventories and research state for save/load.
- Exposes InventorySnapshot type for persisting per-inventory item counts.

2) Public API
- Namespace/module
  - CHAL.Data

- Types
  - public class PlayerProfile : IWallet
    - Public fields
      - string profileId;                                    // identifier for the profile
      - DateTime LastSaveTime;                               // last save timestamp
      - string playerName;                                     // player display name
      - Color[] playerColors;                                   // character colors
      - int XP;                                                // total XP
      - int Level;                                             // current level (may be derived from XP)
      - int XPInCurrentLevel;                                   // XP within current level
      - int XPToNextLevel;                                      // XP required for next level
      - int missingXP;                                          // XP needed to reach next level
      - float levelProgress;                                    // UI progress 0..1 for current level
      - List<string> UnlockedHeroes = new();                   // IDs of unlocked heroes
      - Dictionary<string, int> Currencies = new();             // currency balances (e.g., "gold" -> 123)
      - Dictionary<int, int> MapProgress = new();               // map progress: first = mapNo, second = highest difficulty succeeded
      - [NonSerialized] public List<Inventory> Inventories = new(); // live inventories (not serialized)
      - public List<InventorySnapshot> InventorySave = new();     // persistent inventory snapshots for save/load
      - [NonSerialized] public ResearchState ResearchRuntime;     // in-memory research state (not serialized)

    - Public methods
      - public void InitializePlayer(string name, Color[] colors)
      - public void InitInventories()
      - public int GetXP()
      - public void AddXP(int amount)
      - public int GetCurrency(string currencyId)
      - public void AddCurrency(string currencyId, int amount)
      - public bool SpendCurrency(string currencyId, int amount)
      - public bool CanSpend(string currencyId, int amount)
      - public void Refund(string currencyId, int amount)
      - public IReadOnlyList<string> GetUnlockedHeroes()
      - public bool IsHeroUnlocked(string heroId)
      - public bool UnlockHero(string heroId)
      - public bool LockHero(string heroId)
      - public bool EnsureStarterHeroUnlocked(string starterHeroId)
      - public void SetMapProgress(int map, MapDifficulty difficulty)
      - public int GetMapProgress(int map)
      - private void RecalculateLevel()                                    // internal, updates Level and XP-related fields
      - public void PrepareInventorySnapshot()
      - public void RestoreInventoriesFromSnapshot()
      - public ResearchSnapshot BuildResearchSnapshotFrom(ResearchState state)
      - public void RestoreResearchInto(ResearchState state, ResearchSnapshot snap)

  - public struct InventorySnapshot
    - public string id;                                           // inventory ID (e.g., "remains", "part", ...)
    - public Dictionary<string, int> items;                       // flat map of itemId -> count

Notes
- MapDifficulty is an enum type used in SetMapProgress (parameter type appears in code).
- Inventory and Research-related types come from other parts of the project (Inventory, InventorySnapshot, ResearchState, etc.).

3) Key Behavior & Side Effects
- InitializePlayer
  - Sets profileId = "p_" + name
  - Determines starterHeroId from GameManager.Instance.starterHero or "TestHero"
  - Ensures starter hero is unlocked
  - Initializes inventories
  - Adds 0 gold currency
  - Triggers a save via SaveSystem.Save(this)

- XP and level
  - AddXP(amount) updates XP and calls RecalculateLevel
  - RecalculateLevel computes Level from XP using BalanceManager.GetXpForLevel, updates XPInCurrentLevel, XPToNextLevel, missingXP, levelProgress, and logs progress

- Currencies
  - GetCurrency returns balance or 0 if missing
  - AddCurrency ignores non-positive amounts; initializes currency key to 0 if absent; then increments
  - SpendCurrency validates amount > 0 and affordability via CanSpend, then subtracts
  - CanSpend checks positive amount and sufficiency in GetCurrency
  - Refund adds currency amount (positive) back via AddCurrency

- Heroes
  - GetUnlockedHeroes returns a read-only view of UnlockedHeroes
  - IsHeroUnlocked checks for non-empty heroId and membership
  - UnlockHero adds heroId if valid and not already unlocked
  - LockHero removes heroId if present

- Starter hero
  - EnsureStarterHeroUnlocked ensures starterHeroId exists in UnlockedHeroes; returns true if added

- Map progress
  - SetMapProgress stores the highest difficulty achieved for a given map
  - GetMapProgress returns stored progress or 0 if missing

- Inventories and snapshots
  - PrepareInventorySnapshot builds InventorySave by converting each live Inventory to a dictionary via ToDictionary and storing per-inventory id
  - RestoreInventoriesFromSnapshot rehydrates live Inventories from InventorySave by matching invID and applying FromDictionary
  - InventorySave is used for persistence; Inventories is the in-memory live representation

- Research
  - BuildResearchSnapshotFrom creates a ResearchSnapshot from a given ResearchState
  - RestoreResearchInto applies data from a ResearchSnapshot back into a ResearchState

- Misc
  - NonSerialized fields are excluded from Unity serialization (Inventories and ResearchRuntime)

4) Constraints & Failure Modes
- Guard clauses
  - AddCurrency, SpendCurrency, CanSpend, and EnsureStarterHeroUnlocked guard against null/empty strings and non-positive amounts
  - RestoreInventoriesFromSnapshot safely handles null InventorySave and missing inventory mappings
  - PrepareInventorySnapshot creates InventorySave if null and clears existing entries
  - BuildResearchSnapshotFrom and RestoreResearchInto gracefully handle null state/snap
- Serialization
  - Inventories and ResearchRuntime are non-serialized; they exist only in memory
  - InventorySave and other public fields participate in save/load
- Initialization assumptions
  - InitializePlayer relies on GameManager.Instance.starterHero and saves immediately
  - RecalculateLevel depends on BalanceManager.GetXpForLevel and DebugManager; behavior contingent on those systems
- Partial restoration
  - Restoring inventories and research snapshot may skip missing entries or null data without throwing
- Threading
  - No explicit thread-safety guarantees; likely single-threaded use within Unity main thread

5) Example
```csharp
// Example usage
using CHAL.Data;
using UnityEngine;

var profile = new PlayerProfile();
profile.InitializePlayer("Alice", new Color[] { Color.red, Color.blue });
```

6) Unknowns
- Definitions and members of IWallet
- Implementations of BalanceManager.GetXpForLevel, DebugManager.Log, and SaveSystem.Save
- Details of Inventory, Inventory.ToDictionary, Inventory.FromDictionary, and Inventory.invID
- Exact definitions of MapDifficulty enum and its values
- ResearchState, ResearchSnapshot, NodeProgress, NodeProgressEntry, and related progress structures
- CHAL.GameManager, CHAL.Systems.Inventory, CHAL.Systems.Research, and related integration points
- Any additional serialization behavior or Unity-specific persistence not visible in this file
