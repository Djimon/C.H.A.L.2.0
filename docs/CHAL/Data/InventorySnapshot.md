# CHAL.Data.InventorySnapshot

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

  - `public struct InventorySnapshot`
    - Public fields/properties:
      - `string id`
      - `Dictionary<string, int> items`

# Key Behavior & Side Effects
- `InitializePlayer`: Sets player name and colors, generates profile ID, unlocks starter hero, initializes inventories, and saves the profile.
- `AddXP`: Increases XP and recalculates level.
- `AddCurrency`: Increases currency amount, ensuring non-negative amounts.
- `SpendCurrency`: Decreases currency amount if sufficient funds are available.
- `UnlockHero`: Adds a hero to the unlocked list if not already present.
- `PrepareInventorySnapshot`: Creates a snapshot of current inventories for saving.
- `RestoreInventoriesFromSnapshot`: Restores inventories from a saved snapshot.
- `BuildResearchSnapshotFrom`: Creates a research snapshot from the current research state.
- `RestoreResearchInto`: Restores research state from a snapshot.

# Constraints & Failure Modes
- Methods like `AddCurrency`, `SpendCurrency`, `UnlockHero`, and `LockHero` guard against invalid inputs (e.g., negative amounts, null or empty IDs).
- `GetCurrency` and `GetMapProgress` return 0 if the currency or map progress is not found.
- `EnsureStarterHeroUnlocked` initializes the `UnlockedHeroes` list if null.

# Example
```csharp
PlayerProfile playerProfile = new PlayerProfile();
playerProfile.InitializePlayer("PlayerOne", new Color[] { Color.red, Color.blue });
playerProfile.AddXP(100);
int currentXP = playerProfile.GetXP();
```

# Unknowns
- The implementation details of `GameManager`, `BalanceManager`, `DebugManager`, and `ResearchState` are not provided in this file.
- The behavior of `Inventory` and `InventorySnapshot` classes is not defined in this file.

