# global.LootRollerDebug

_Automatically generated/updated from `Assets/src/Systems/_test/dbg_LootRoller.cs`._

# Purpose
- This file defines a debugging class for loot rolling functionality in the game.

# Public API
- Namespace: None
- Types
  - public class LootRollerDebug : MonoBehaviour
    - Public methods
      - void Start() 
        - Initializes loot rolling with defined wave composition and logs the results.

# Key Behavior & Side Effects
- Loads all loot rules and initializes the loot roller with unlucky protection on start.
- Defines a wave composition with specific enemies and their attributes.
- Rolls loot based on the defined wave and logs each loot entry.

# Constraints & Failure Modes
- Assumes that the `LootRulesService` and `LootRoller_old` classes are correctly implemented and available.
- No explicit error handling is present for loot rolling or logging.

# Example
```csharp
// Example usage within a Unity scene
void Start()
{
    var lootRollerDebug = new LootRollerDebug();
    lootRollerDebug.Start();
}
```

# Unknowns
- The behavior of `LootRoller_old`, `LootRulesService`, and `UnluckyProtection` is not defined in this file.
- The structure and properties of `DebugManager.DebugLog` are not detailed here.

