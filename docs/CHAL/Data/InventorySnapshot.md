# CHAL.Data.InventorySnapshot

_Automatically generated/updated from `Assets/src/Core/PlayerProfile.cs`._

```text
1) Purpose
- Serializable data model for a player's profile in CHAL.Data, including XP/level, currencies, hero unlocks, inventories, map progress, and research state.
- Helpers to snapshot and restore inventories (InventorySnapshot) and to serialize/deserialize research progress.
- High-level hooks for saving, initializing starter data, and integrating with other subsystems (GameManager, BalanceManager, DebugManager).

2) Public API

- Namespace/module
  - CHAL.Data

- Types
  - public class PlayerProfile : IWallet
    - Public fields
      - public string profileId; // Unique identifier for the profile (constructed during initialization)
      - public DateTime LastSaveTime; // Last autosave/debug timestamp
      - public string playerName; // Display name chosen by the player
      - public Color[] playerColors; // Character customization colors
      - public int XP; // Total experience points
      - public int Level; // Current level (may be derived from XP)
      - public int XPInCurrentLevel; // XP accumulated within the current level
      - public int XPToNextLevel; // XP required for the next level
      - public int missingXP; // XP still needed to reach next level
      - public float levelProgress; // 0..1 progress within the current level
      - public List<string> UnlockedHeroes = new(); // IDs of unlocked heroes
      - public Dictionary<string, int> Currencies = new(); // CurrencyStore: currencyId -> amount
      - public Dictionary<int, int> MapProgress = new(); // MapProgress: mapNo -> highest difficulty achieved
      - [NonSerialized] public List<Inventory> Inventories = new(); // Live inventories (not serialized by Unity)
      - public List<InventorySnapshot> InventorySave = new(); // Persisted snapshot of inventories for save/load
      - [NonSerialized] public ResearchState ResearchRuntime; // Runtime research state (not serialized)

    - Public methods
      - public void InitializePlayer(string name, Color[] colors)
        - Initialize basic fields, derive profileId, ensure starter hero, initialize inventories, and trigger a save.
      - public void InitInventories()
        - Populate Inventories with: "remains", "part", "rune", "module", "gear".
      - public int GetXP()
        - Returns XP.
      - public void AddXP(int amount)
        - Adds amount to XP and calls RecalculateLevel().
      - public int GetCurrency(string currencyId)
        - Returns amount for currencyId or 0 if missing.
      - public void AddCurrency(string currencyId, int amount)
        - Adds amount to a currency; creates entry if missing; ignores non-positive amounts.
      - public bool SpendCurrency(string currencyId, int amount)
        - Deducts amount if possible; returns success; uses CanSpend.
      - public bool CanSpend(string currencyId, int amount)
        - Returns true if amount > 0 and currency balance >= amount.
      - public void Refund(string currencyId, int amount)
        - Increases currency by amount (positive only).
      - public IReadOnlyList<string> GetUnlockedHeroes()
        - Returns a read-only view of UnlockedHeroes.
      - public bool IsHeroUnlocked(string heroId)
        - True if heroId is non-empty and present in UnlockedHeroes.
      - public bool UnlockHero(string heroId)
        - Adds heroId to UnlockedHeroes if valid and not already unlocked; returns success.
      - public bool LockHero(string heroId)
        - Removes heroId from UnlockedHeroes if present; returns success.
      - public bool EnsureStarterHeroUnlocked(string starterHeroId)
        - Ensures starterHeroId is in UnlockedHeroes; returns true if added; false otherwise.
      - public void SetMapProgress(int map, MapDifficulty difficulty)
        - Stores difficulty (cast to int) for given map in MapProgress.
      - public int GetMapProgress(int map)
        - Returns highest difficulty for map or 0 if not present.
      - private void RecalculateLevel()
        - Recomputes Level, XPInCurrentLevel, XPToNextLevel, missingXP, levelProgress based on XP using BalanceManager.GetXpForLevel.
      - public void PrepareInventorySnapshot()
        - Builds InventorySave by converting each non-null Inventory to a dictionary snapshot (id + items).
      - public void RestoreInventoriesFromSnapshot()
        - Restores live Inventories from InventorySave; initializes inventories if needed.
      - public ResearchSnapshot BuildResearchSnapshotFrom(ResearchState state)
        - Creates a serializable ResearchSnapshot from a runtime ResearchState.
      - public void RestoreResearchInto(ResearchState state, ResearchSnapshot snap)
        - Applies data from a ResearchSnapshot back into a ResearchState.

  - public struct InventorySnapshot
    - public string id; // Inventory identifier (e.g., "remains", "part", "rune", "module", "gear")
    - public Dictionary<string, int> items; // ItemId -> count

3) Key Behavior & Side Effects

- InitializePlayer
  - Sets playerName and colors.
  - Sets profileId to "p_" + name.
  - Determines starterHeroId from GameManager.Instance.starterHero or "TestHero".
  - Calls EnsureStarterHeroUnlocked(starterId).
  - Calls InitInventories().
  - Invokes SaveSystem.Save(this).

- InitInventories
  - Adds five Inventory instances with ids: "remains", "part", "rune", "module", "gear".

- XP/Level
  - AddXP increments XP and triggers RecalculateLevel() to update Level, XPInCurrentLevel, XPToNextLevel, missingXP, levelProgress.

- Currency
  - GetCurrency returns 0 when missing.
  - AddCurrency creates currency entry if needed and increments amount (only for positive amounts).
  - SpendCurrency validates amount > 0 and CanSpend; reduces balance on success.
  - Refund delegates to AddCurrency.

- Hero management
  - UnlockHero/LockHero modify UnlockedHeroes with basic guards (non-empty, non-duplicate) and return success flags.
  - EnsureStarterHeroUnlocked makes sure the starter hero is present.

- Map progress
  - SetMapProgress stores the numeric difficulty for a map.
  - GetMapProgress returns 0 if no progress stored.

- Inventory snapshot
  - PrepareInventorySnapshot serializes live inventories to InventorySave via ToDictionary on each inventory.
  - RestoreInventoriesFromSnapshot rebuilds live inventories from InventorySave via FromDictionary, initializing inventories if needed.

- Research snapshot
  - BuildResearchSnapshotFrom copies relevant runtime ResearchState into a serializable ResearchSnapshot, including node progress per node and progress details.
  - RestoreResearchInto overwrites a ResearchState from a ResearchSnapshot, clearing existing progress and re-populating from the snapshot.

4) Constraints & Failure Modes

- Null/empty guards
  - EnsureStarterHeroUnlocked returns false if starterHeroId is null/empty.
  - UnlockHero/LockHero guard against null/empty heroId and duplicates/removals accordingly.
  - SetMapProgress uses direct dictionary assignment; missing maps will be created.
  - GetMapProgress returns 0 if map not present.
  - BuildResearchSnapshotFrom returns a default/empty snapshot if input state is null.
  - RestoreInventoriesFromSnapshot safely handles null InventorySave, initializes inventories if needed.

- Serialization behavior
  - Inventories is marked [NonSerialized], meaning Unity's serializer will skip it; InventorySave exists to persist snapshot data.
  - ResearchRuntime is [NonSerialized], so runtime state is not saved as part of Unity serialization.

- Currency handling
  - AddCurrency ignores non-positive amounts.
  - SpendCurrency and CanSpend guard against non-positive amounts and missing currencies.

- External dependencies (not defined in this file)
  - BalanceManager.GetXpForLevel, DebugManager.Log, GameManager.Instance, SaveSystem.Save, Inventory.ToDictionary/FromDictionary, Inventory.invID, and the research snapshot types (ResearchSnapshot, NodeProgressEntry, NodeProgressSave, MapRequirement, KillTagCount) come from other parts of the project.

5) Example

- Minimal usage
```csharp
// Example: create and initialize a new profile
var profile = new CHAL.Data.PlayerProfile();
profile.InitializePlayer("Alice", new UnityEngine.Color[] { UnityEngine.Color.Red, UnityEngine.Color.Blue });
```

6) Unknowns

- Details of IWallet interface implementation and any required interface members not shown here.
- Exact behavior and structure of:
  - Inventory, InventorySnapshot, Inventory.ToDictionary/FromDictionary
  - GameManager, BalanceManager, DebugManager, SaveSystem
  - ResearchState, ResearchSnapshot, NodeProgressEntry, NodeProgressSave, MapRequirement, KillTagCount
- Any further persistence format or save file layout beyond SaveSystem.Save(this).
- Thread-safety guarantees and potential race conditions around concurrent saves/loads.
- Any additional side effects triggered by external systems during saves or state restoration.

```
