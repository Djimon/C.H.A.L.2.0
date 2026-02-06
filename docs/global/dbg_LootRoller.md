# Assets/src/Systems/_test/dbg_LootRoller.cs

_Automatically generated/updated from `Assets/src/Systems/_test/dbg_LootRoller.cs`._

# Purpose
- This file defines a debugging class for loot rolling functionality in the game.

# Public API
- Namespace: None
- Types
  - public class LootRollerDebug : MonoBehaviour
    - Public methods
      - void Start() 
        - Initializes loot rules and rolls loot based on defined enemy waves, logging results to the debug manager.

# Key Behavior & Side Effects
- Loads all loot rules at the start.
- Initializes a `LootRoller_old` instance with the loaded rules and an `UnluckyProtection` instance.
- Defines a wave of enemies with specific IDs, counts, and bonus tags.
- Rolls loot based on the defined wave and logs each loot entry to the debug manager.

# Constraints & Failure Modes
- Assumes that `LootRulesService` and `LootRoller_old` are correctly implemented and available.
- No explicit error handling is present for loot rolling or logging.

# Example
```csharp
void Start()
{
    // Example usage of LootRollerDebug
    var debugLootRoller = new LootRollerDebug();
    debugLootRoller.Start();
}
```

# Unknowns
- The behavior and implementation details of `LootRoller_old`, `LootRulesService`, and `UnluckyProtection` are not defined in this file.

