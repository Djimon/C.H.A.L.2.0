# CHAL.Systems.Loot.UnluckyProtection

_Automatically generated/updated from `Assets/src/Systems/Loot/UnluckyProtection.cs`._

```csharp
Purpose
- Defines UnluckyProtection, which tracks dry-streaks per rarity and computes drop multipliers.
- Provides lifecycle-like methods to reset/increase streaks based on drop/fail events and to obtain the current multiplier.

Public API
- Namespace: CHAL.Systems.Loot
- Type: public class UnluckyProtection

Public constructors
- UnluckyProtection()

Public methods
- void OnDrop(Rarity rarity)
  - Resets the streak for the given rarity to 0 if the rarity is tracked.
  - Logs a debug message: "decreased unlucky protection for [<rarity>] to 0" (level Debug, category "System").
- void OnFail(Rarity rarity)
  - Increments the streak for the given rarity if the rarity is tracked.
  - Logs a debug message: "increased unlucky protection for [<rarity>] to <value>" (level Debug, category "System").
- float GetMultiplier(Rarity rarity)
  - Returns the current drop multiplier for the given rarity based on its streak.
  - Rarity-specific formulae:
    - Rare: 1f + alphaRare * s
    - Epic: 1f + alphaEpic * s
    - Legendary: 1f + alphaLegendary * s
    - Daemonic: 1f + alphaSpecials * s
    - Holy: 1f + alphaSpecials * s
    - Mythic: 1f + alphaSpecials * s
    - Other (e.g., Common): 1f
- string DebugInfo()
  - Returns a compact string with current Rare/Epic/Legendary streaks, e.g., "Rare=0, Epic=0, Legendary=0"

Key behavior & Side Effects
- Streak tracking
  - Maintains a Dictionary<Rarity, int> _streaks, initialized to 0 for all Rarity values.
- Config access
  - alphaRare/alphaEpic/alphaLegendary/alphaSpecials are read-only properties pulling values from BalanceManager.Instance.Config.loot.unlucky.{alpha*}.
- Event handling
  - OnDrop(rarity): if rarity is tracked, resets its streak to 0; logs debug action.
  - OnFail(rarity): if rarity is tracked, increments its streak; logs debug action.
- Multiplier calculation
  - Multiplier depends on rarity and its current streak; uses per-rarity alpha factors.
  - Common (and other non-specified rarities) yield 1f (no dry-streak effect).
- Debug info
  - DebugInfo returns current streaks for Rare/Epic/Legendary for quick logs.

Constraints & Failure Modes
- Guarding
  - OnDrop/OnFail only operate when IsTracked(rarity) is true (Rares, Epics, Legendary, Daemonic, Holy, Mythic). Other rarities are effectively ignored for streak updates.
- Initialization guarantees
  - Streaks dictionary is populated for all Rarity values in the constructor, preventing KeyNotFound errors when reading _streaks[rarity].
- Dependency assumptions
  - alpha values and config are retrieved via BalanceManager.Instance.Config; null or missing config could cause runtime issues (not handled here).
- Threading
  - No explicit threading or synchronization; assumes single-threaded usage or external synchronization.

Unknowns
- Exact contents of the Rarity enum beyond the values used in IsTracked (Rare, Epic, Legendary, Daemonic, Holy, Mythic, and others like Common) are not defined in this file.
- The full structure and possible values of BalanceManager.Instance.Config.loot.unlucky.* are not defined here.
- Behavior for rarities outside IsTracked (e.g., Common) in GetMultiplier is inferred from the switch default; exact intent for all non-tracked rarities is not explicitly documented here.
- Any higher-level system interactions or side effects beyond the included Debug logs are not shown in this file.

Example
- Minimal usage scenario (illustrative only):
```csharp
var luckProtect = new CHAL.Systems.Loot.UnluckyProtection();

// A Rare item did not drop
luckProtect.OnFail(Rarity.Rare);

// A Rare item drops now
luckProtect.OnDrop(Rarity.Rare);

// Get current multiplier for Rare
float mult = luckProtect.GetMultiplier(Rarity.Rare);

// Debug info
string info = luckProtect.DebugInfo();
```
