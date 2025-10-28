# Assets/src/Systems/_test/dbg_LootRoller.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a debug class for rolling loot based on enemy waves.

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
- Assumes that `LootRulesService` and `LootRoller_old` can be instantiated without errors.
- No explicit error handling for loot rolling or logging.

# Example
```csharp
var lootRollerDebug = new LootRollerDebug();
lootRollerDebug.Start();
```

# Unknowns
- The behavior of `LootRoller_old`, `LootRulesService`, and `UnluckyProtection` is not defined in this file.
- The structure of the loot returned by `roller.RollLoot(wave)` is not detailed.
```
