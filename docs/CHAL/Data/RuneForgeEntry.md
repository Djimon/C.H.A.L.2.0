# CHAL.Data.RuneForgeEntry

_Automatically generated/updated from `Assets/src/Data/Config/RuneForgeConfig.cs`._

# Purpose
- Defines data structures for rune forging configuration in the game.
- Provides a ScriptableObject for storing multiple rune forge entries.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **[Serializable] class** `RuneForgeEntry`
    - Public fields:
      - `ItemDef remain`: The item that serves as input.
      - `List<RuneChance> runes`: Possible runes with their weights.
  - **[Serializable] class** `RuneChance`
    - Public fields:
      - `ItemDef rune`: The rune item.
      - `float weight`: The weight of the rune, constrained between 0 and 1.
  - **[CreateAssetMenu] class** `RuneForgeConfig` [extends `ScriptableObject`]
    - Public fields:
      - `List<RuneForgeEntry> entries`: List of rune forge entries.

# Key Behavior & Side Effects
- `RuneForgeConfig` can be created as an asset in the Unity editor, allowing designers to configure rune forging entries.

# Constraints & Failure Modes
- The `weight` field in `RuneChance` must be between 0 and 1 due to the `[Range(0f, 1f)]` attribute.
- No explicit null or empty handling is defined in the code.

# Example
```csharp
// Example of creating a RuneForgeConfig asset in Unity
var runeForgeConfig = ScriptableObject.CreateInstance<RuneForgeConfig>();
runeForgeConfig.entries = new List<RuneForgeEntry>();
```

# Unknowns
- The definition of `ItemDef` is not provided in this file.
