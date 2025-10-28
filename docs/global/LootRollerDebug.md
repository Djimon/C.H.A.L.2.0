# global.LootRollerDebug

_Automatically generated/updated from `Assets/src/Systems/_test/dbg_LootRoller.cs`._

# Purpose
- Defines a debug class for rolling loot based on enemy waves in a game.

# Public API
- Namespace: None
- Types
  - public class LootRollerDebug : MonoBehaviour
    - Public methods:
      - void Start() 
        - Initializes loot rolling based on defined enemy waves and logs the results.

# Key Behavior & Side Effects
- Loads loot rules and initializes a loot roller on Start.
- Defines a wave of enemies with specific counts and tags.
- Rolls loot based on the defined wave and logs each loot entry.

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
- The implementation details of `LootRulesService`, `LootRoller_old`, and their interactions.
- The structure of the loot entries returned by `roller.RollLoot(wave)`.

