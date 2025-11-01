# CHAL.Data.PlayerProfile

_Automatically generated/updated from `Assets/src/Core/PlayerProfile.cs`._

```csharp
// (documentation in this file; code blocks shown only for examples)
```

1) Purpose
- Defines the serializable PlayerProfile data container in CHAL.Data that tracks player meta, customization, progress, currencies, inventories, unlocked heroes, map progress, and research state.
- Provides methods to manipulate XP, currencies, hero unlocks, map progress, and to snapshot/restore inventory and research state.
- Implements IWallet and interacts with external systems (e.g., BalanceManager, GameManager, SaveSystem) to compute levels, persist data, and manage starter hero unlocks.

2) Public API
- Namespace: CHAL.Data
- Types
  - public class PlayerProfile : IWallet
    - Public fields
      - string profileId: identifier for the profile
      - DateTime LastSaveTime: last autosave/debug time
      - string playerName: display name
      - Color[] playerColors: customization colors
      - int XP: total experience
      - int Level: computed level (may be derived from XP)
      - int XPInCurrentLevel: XP within the current level
      - int XPToNextLevel: XP required for next level
      - int missingXP: XP remaining to reach next level
      - float levelProgress: 0..1 UI progress toward next level
      - List<string> UnlockedHeroes: IDs of unlocked heroes
      - Dictionary<string, int> Currencies: currency balances (e.g., "gold", "dna")
      - Dictionary<int, int> MapProgress: map progress by map number; value = highest difficulty succeeded
      - [NonSerialized] List<Inventory> Inventories: live inventories (not serialized by Unity)
      - List<InventorySnapshot> InventorySave: persisted snapshot of inventories for save/load
      - [NonSerialized] ResearchState ResearchRuntime: runtime research state (not serialized)
    - Public methods
      - void InitializePlayer(string name, Color[] colors)
        - Sets name/colors, assigns profileId, ensures starter hero, initializes inventories, saves profile.
      - void InitInventories()
        - Creates starter inventories: remains, part, rune, module, gear.
      - int GetXP(): returns XP
      - void AddXP(int amount)
        - Increments XP and recalculates level
      - int GetCurrency(string currencyId)
        - Returns current balance for currencyId or 0 if missing
      - void AddCurrency(string currencyId, int amount)
        - Increases currency by amount (no-op for <=0); initializes key if needed
      - bool SpendCurrency(string currencyId, int amount)
        - Deducts currency if possible; returns success
      - bool CanSpend(string currencyId, int amount)
        - Returns true if amount > 0 and current balance >= amount
      - void Refund(string currencyId, int amount)
        - Adds specified amount back to currency
      - IReadOnlyList<string> GetUnlockedHeroes(): read-only view of unlocked heroes
      - bool IsHeroUnlocked(string heroId)
        - Checks if heroId is unlocked
      - bool UnlockHero(string heroId)
        - Unlocks a new hero; returns true if added
      - bool LockHero(string heroId)
        - Removes hero from unlocked list; returns true if removed
      - bool EnsureStarterHeroUnlocked(string starterHeroId)
        - Ensures starter hero is unlocked; returns true if added
      - void SetMapProgress(int map, MapDifficulty difficulty)
        - Sets progress for a map to the given difficulty
      - int GetMapProgress(int map)
        - Returns highest difficulty reached for map; 0 if none
      - private void RecalculateLevel()
        - Recomputes Level, XPInCurrentLevel, XPToNextLevel, missingXP, levelProgress; logs debug
      - void PrepareInventorySnapshot()
        - Builds InventorySave from live Inventories (non-serialized) for save
      - void RestoreInventoriesFromSnapshot()
        - Restores live Inventories from InventorySave snapshot
      - ResearchSnapshot BuildResearchSnapshotFrom(ResearchState state)
        - Converts a ResearchState into a ResearchSnapshot
      - void RestoreResearchInto(ResearchState state, ResearchSnapshot snap)
        - Applies a ResearchSnapshot back into a ResearchState
  - [Serializable] public struct InventorySnapshot
    - string id: inventory identifier (e.g., "remains", "part", "rune", "module", "gear")
    - Dictionary<string, int> items: itemId -> count snapshot

3) Key Behavior & Side Effects
- InitializePlayer
  - Sets profileId, assigns starter hero via EnsureStarterHeroUnlocked, calls InitInventories, then saves via SaveSystem.Save(this).
- InitInventories
  - Adds five inventories with IDs: "remains", "part", "rune", "module", "gear".
- XP / Level
  - AddXP updates XP and calls RecalculateLevel.
  - RecalculateLevel uses BalanceManager.GetXpForLevel and updates Level, XPInCurrentLevel, XPToNextLevel, missingXP, levelProgress, and logs debug messages.
- Currency
  - GetCurrency returns 0 if missing; AddCurrency initializes missing key and increments; SpendCurrency validates amount and sufficiency, then deducts; Refund delegates to AddCurrency.
- Hero management
  - EnsureStarterHeroUnlocked ensures starter hero exists in UnlockedHeroes; UnlockHero/LockHero modify UnlockedHeroes accordingly.
- Map progress
  - SetMapProgress stores the integer value of the MapDifficulty enum; GetMapProgress returns stored value or 0 if not present.
- Inventory snapshot
  - PrepareInventorySnapshot builds InventorySave from Inventories by converting each inventory to a dictionary and persisting with its id.
  - RestoreInventoriesFromSnapshot rehydrates live Inventories from InventorySave; ensures Inventories exist if empty.
- Research snapshot
  - BuildResearchSnapshotFrom serializes relevant fields from a ResearchState into a ResearchSnapshot.
  - RestoreResearchInto deserializes a ResearchSnapshot back into a ResearchState.
- Non-serialized fields
  - Inventories and ResearchRuntime are not serialized by Unity; their lifecycle must be managed across save/load.
- Logging
  - RecalculateLevel logs debug messages about XP/Level progress via DebugManager.

4) Constraints & Failure Modes
- Guards and validation
  - AddCurrency ignores non-positive amounts; SpendCurrency/CanSpend require amount > 0; GetCurrency returns 0 when currencyId is missing.
  - EnsureStarterHeroUnlocked and UnlockHero guard against null/empty IDs; avoiding duplicates.
- Serialization
  - Inventories and ResearchRuntime are marked [NonSerialized]; they must be re-created or re-initialized after load.
  - InventorySave may be null; PrepareInventorySnapshot ensures it's initialized before use.
- Inventory syncing
  - RestoreInventoriesFromSnapshot relies on matching inventory IDs to existing live Inventories; if an ID is missing, the corresponding snapshot item is skipped.
- External dependencies
  - RecalculateLevel depends on BalanceManager.GetXpForLevel; Build/Restore of ResearchSnapshot/State depends on external types (ResearchState, NodeProgress, MapDifficulty, etc.); behavior is bound to those implementations.
- Null/empty checks
  - Starter hero id, hero ids, and map/difficulty inputs are checked for null/empty values in several methods.

5) Example
- Minimal usage example
```csharp
using CHAL.Data;
using UnityEngine;

public class ExampleUsage
{
    public void CreateProfile()
    {
        var profile = new PlayerProfile();
        profile.InitializePlayer("Alice", new Color[] { Color.red, Color.blue });
        // profile is now initialized, starter hero unlocked, inventories created, and saved
    }
}
```

6) Unknowns
- Exact definitions and members of:
  - IWallet interface (beyond its existence)
  - BalanceManager.GetXpForLevel behavior and unit/edge-case handling
  - DebugManager and its logging semantics
  - GameManager and the specifics of starterHero
  - Inventory class API (ToDictionary, FromDictionary, invID, etc.)
  - InventorySnapshot type in other parts of the codebase (if any)
  - ResearchState, ResearchSnapshot, NodeProgress, NodeProgressEntry, NodeProgressSave, MapDifficulty, and related serialization logic
  - SaveSystem.Save behavior and when LastSaveTime is updated
- Any additional side effects of BuildResearchSnapshotFrom and RestoreResearchInto beyond data copying
- Any runtime constraints for Unity serialization beyond [NonSerialized] attr in this file
