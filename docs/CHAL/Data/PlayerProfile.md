# CHAL.Data.PlayerProfile

_Automatically generated/updated from `Assets/src/Core/PlayerProfile.cs`._

# Purpose
- Defines the `PlayerProfile` class representing a player's profile, including customization and progress data.
- Implements the `IWallet` interface for managing in-game currency.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `PlayerProfile` : `IWallet`
    - Public fields/properties:
      - `string profileId`
      - `DateTime LastSaveTime`
      - `string playerName`
      - `Color[] playerColors`
      - `int XP`
      - `int Level`
      - `int XPInCurrentLevel`
      - `int XPToNextLevel`
      - `int missingXP`
      - `float levelProgress`
      - `List<string> UnlockedHeroes`
      - `Dictionary<string, int> Currencies`
      - `Dictionary<int, int> MapProgress`
      - `List<Inventory> Inventories`
      - `List<InventorySnapshot> InventorySave`
      - `ResearchState ResearchRuntime`
    - Public methods:
      - `void InitializePlayer(string name, Color[] colors)`
      - `void InitInventories()`
      - `int GetXP()`
      - `void AddXP(int amount)`
      - `int GetCurrency(string currencyId)`
      - `void AddCurrency(string currencyId, int amount)`
      - `bool SpendCurrency(string currencyId, int amount)`
      - `bool CanSpend(string currencyId, int amount)`
      - `void Refund(string currencyId, int amount)`
      - `IReadOnlyList<string> GetUnlockedHeroes()`
      - `bool IsHeroUnlocked(string heroId)`
      - `bool UnlockHero(string heroId)`
      - `bool LockHero(string heroId)`
      - `bool EnsureStarterHeroUnlocked(string starterHeroId)`
      - `void SetMapProgress(int map, MapDifficulty difficulty)`
      - `int GetMapProgress(int map)`
      - `void PrepareInventorySnapshot()`
      - `void RestoreInventoriesFromSnapshot()`
      - `ResearchSnapshot BuildResearchSnapshotFrom(ResearchState state)`
      - `void RestoreResearchInto(ResearchState state, ResearchSnapshot snap)`

# Key Behavior & Side Effects
- Initializes player data and inventories upon calling `InitializePlayer`.
- Adds experience points and recalculates level when `AddXP` is called.
- Manages currency through methods like `AddCurrency`, `SpendCurrency`, and `Refund`.
- Unlocks and locks heroes with `UnlockHero` and `LockHero`.
- Prepares and restores inventory snapshots for saving/loading.

# Constraints & Failure Modes
- Methods like `AddCurrency`, `SpendCurrency`, and `Refund` ignore non-positive amounts.
- `GetCurrency` returns 0 if the currency ID is not found.
- `EnsureStarterHeroUnlocked` initializes `UnlockedHeroes` if null.
- `RestoreInventoriesFromSnapshot` ensures live inventories exist before restoring.

# Example
```csharp
PlayerProfile playerProfile = new PlayerProfile();
playerProfile.InitializePlayer("Player1", new Color[] { Color.red, Color.blue });
playerProfile.AddXP(100);
int gold = playerProfile.GetCurrency("gold");
```

# Unknowns
- Specific implementation details of `Inventory`, `ResearchState`, and `BalanceManager` are not provided.

