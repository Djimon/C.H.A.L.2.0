# CHAL.Data.PlayerProfile

_Automatically generated/updated from `Assets/src/Core/PlayerProfile.cs`._

# Purpose
- Defines the `PlayerProfile` class representing a player's profile in the game, including customization, progress, currencies, and inventory management.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public class PlayerProfile : IWallet`
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
- Initializes player profile with name and colors, generates a profile ID, and saves the profile.
- Adds experience points and recalculates the player's level and progress.
- Manages currency addition, spending, and refunds with checks for valid amounts.
- Unlocks and locks heroes, ensuring starter heroes are unlocked as needed.
- Prepares and restores inventory snapshots for saving/loading game state.
- Builds and restores research snapshots for tracking research progress.

# Constraints & Failure Modes
- Methods that modify state (e.g., `AddCurrency`, `SpendCurrency`, `UnlockHero`) guard against invalid inputs (e.g., negative amounts, null hero IDs).
- Inventory and research states are only restored if valid snapshots are provided.
- The `InitInventories` method initializes inventories if they are empty.

# Example
```csharp
PlayerProfile playerProfile = new PlayerProfile();
playerProfile.InitializePlayer("PlayerOne", new Color[] { Color.red, Color.blue });
playerProfile.AddXP(100);
bool canSpend = playerProfile.SpendCurrency("gold", 50);
```

# Unknowns
- The implementation details of `GameManager`, `BalanceManager`, `DebugManager`, and `ResearchState` are not provided in this file.
- The structure and behavior of the `Inventory` class and `ResearchSnapshot` are not defined in this file.

