# CHAL.Data.InventorySnapshot

_Automatically generated/updated from `Assets/src/Core/PlayerProfile.cs`._

```text
Purpose
- Defines a serializable PlayerProfile class implementing IWallet to represent a player's data (identity, progression, currencies, inventories, and related state) in CHAL.
- Encapsulates methods for initializing and mutating player state (XP, currencies, heroes, map progress, inventories, and research state).
- Provides a nested InventorySnapshot struct for snapshotting inventories for save/load flows.
```

```text
Public API
- Namespace/Module: CHAL.Data

- Type: public class PlayerProfile : IWallet
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
    - void PrepareInventorySnapshot()
    - void RestoreInventoriesFromSnapshot()
    - ResearchSnapshot BuildResearchSnapshotFrom(ResearchState state)
    - void RestoreResearchInto(ResearchState state, ResearchSnapshot snap)

- Type: public struct InventorySnapshot
  - public string id
  - public Dictionary<string, int> items

```

```text
Key Behavior & Side Effects
- InitializePlayer(string name, Color[] colors)
  - Sets playerName, playerColors; profileId = "p_" + name
  - Determines starterHeroId from GameManager.Instance.starterHero?.HeroId or "TestHero"
  - Calls EnsureStarterHeroUnlocked(starterId) and InitInventories()
  - Triggers SaveSystem.Save(this)

- InitInventories()
  - Adds inventories with IDs: "remains", "part", "rune", "module", "gear"

- XP & Leveling
  - AddXP(int amount): increments XP, calls RecalculateLevel()
  - RecalculateLevel(): computes Level, XPInCurrentLevel, XPToNextLevel, missingXP, levelProgress using BalanceManager.GetXpForLevel; logs via DebugManager

- Currency
  - GetCurrency(string): returns amount or 0 if missing
  - AddCurrency(string, int): ignores non-positive amounts; initializes missing key; increments
  - SpendCurrency(string, int): validates amount > 0 and CanSpend; deducts amount on success
  - CanSpend(string, int): checks amount > 0 and current balance via GetCurrency
  - Refund(string, int): adds amount back (guarding non-positive)

- Heroes
  - GetUnlockedHeroes(): exposes UnlockedHeroes as IReadOnlyList
  - IsHeroUnlocked(string): checks non-empty and presence in UnlockedHeroes
  - UnlockHero(string): adds heroId if valid and not already unlocked
  - LockHero(string): removes heroId if present
  - EnsureStarterHeroUnlocked(string): ensures starter hero is in UnlockedHeroes; returns true if added

- Map Progress
  - SetMapProgress(int map, MapDifficulty difficulty): stores difficulty as int in MapProgress
  - GetMapProgress(int map): returns stored value or 0 if absent

- Inventory Snapshot
  - PrepareInventorySnapshot(): builds InventorySave from Inventories by converting each to a dictionary
  - RestoreInventoriesFromSnapshot(): applies InventorySave back to live Inventories (initializes Inventories if needed)

- Research
  - BuildResearchSnapshotFrom(ResearchState state): converts runtime ResearchState into a ResearchSnapshot
  - RestoreResearchInto(ResearchState state, ResearchSnapshot snap): restores state from snapshot

- Additional behavior
  - Null/empty guards in several methods (e.g., empty or null strings, null maps)
  - NonSerialized fields are excluded from Unity serialization (Inventories, ResearchRuntime)
```

```text
Constraints & Failure Modes
- Guarded inputs
  - AddCurrency, SpendCurrency, CanSpend, and Refund guard non-positive amounts
  - EnsureStarterHeroUnlocked guards null/empty starterHeroId and initializes list if needed
  - GetMapProgress returns 0 if map not present
- Inventory snapshot integrity
  - PrepareInventorySnapshot uses Inventory.ToDictionary(); handles null invs gracefully
  - RestoreInventoriesFromSnapshot matches inventories by invID; skips when missing
- Research snapshot
  - BuildResearchSnapshotFrom returns empty snapshot if state is null
  - RestoreResearchInto exits early if state is null
- Serialization/Runtime
  - Inventories and ResearchRuntime are [NonSerialized], so not persisted by Unity serialization
- Dependencies (external to file)
  - IWallet, Inventory, InventorySnapshot (type), BalanceManager, DebugManager, SaveSystem, GameManager, ResearchState, ResearchSnapshot, MapDifficulty, and Unity types (Color) are defined elsewhere
```

```text
Example
// Minimal usage example (assuming appropriate using directives)
using CHAL.Data;
using UnityEngine;

public class ExampleUsage
{
    public void CreateProfile()
    {
        var profile = new PlayerProfile();
        profile.InitializePlayer("Alice", new Color[] { Color.red, Color.blue });
        // Further interactions...
    }
}
```

```text
Unknowns
- Definitions and behavior of:
  - IWallet interface
  - Inventory class and its ToDictionary / FromDictionary implementations
  - InventorySnapshot structure details beyond this file
  - BalanceManager.GetXpForLevel, DebugManager.Log, SaveSystem.Save
  - GameManager.Instance.starterHero and HeroId
  - MapDifficulty enum
  - ResearchState and ResearchSnapshot types
  - Inventory.GetAllItems, Inventory.invID, and related inventory APIs
- Any runtime guarantees beyond what is explicit in this file (threading, async behavior, or persistence guarantees)
```
