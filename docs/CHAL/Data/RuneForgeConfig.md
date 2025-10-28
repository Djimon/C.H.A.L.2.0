# CHAL.Data.RuneForgeConfig

_Automatically generated/updated from `Assets/src/Data/Config/RuneForgeConfig.cs`._

# Purpose
- Defines data structures for rune forging configuration in a game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public class RuneForgeEntry`
    - Public fields:
      - `ItemDef remain`: Input item for the rune forging process.
      - `List<RuneChance> runes`: Possible runes with their associated weights.
  - `public class RuneChance`
    - Public fields:
      - `ItemDef rune`: The rune item.
      - `float weight`: The weight of the rune, constrained between 0 and 1.
  - `public class RuneForgeConfig : ScriptableObject`
    - Public fields:
      - `List<RuneForgeEntry> entries`: Collection of rune forge entries.

# Key Behavior & Side Effects
- `RuneForgeConfig` is a ScriptableObject, allowing it to be used as a data/config asset in Unity.

# Constraints & Failure Modes
- `weight` in `RuneChance` must be in the range [0f, 1f].
- No explicit error handling or threading considerations are present.

# Example
```csharp
var config = ScriptableObject.CreateInstance<RuneForgeConfig>();
config.entries = new List<RuneForgeEntry>();
```

# Unknowns
- Specific behavior of `ItemDef` is not defined in this file.

