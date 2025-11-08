# CHAL.Data.RuneChance

_Automatically generated/updated from `Assets/src/Data/Config/RuneForgeConfig.cs`._

# Purpose
- Defines data structures for rune forging configuration in the game.
- Provides a ScriptableObject to store multiple rune forge entries.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **[Serializable] class** `RuneForgeEntry`
    - Public fields:
      - `ItemDef remain`: The item that serves as input.
      - `List<RuneChance> runes`: Possible runes with their weightings.
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
- The `weight` field in `RuneChance` must be within the range of 0 to 1.
- The `entries` list in `RuneForgeConfig` can be empty but should contain valid `RuneForgeEntry` objects for proper functionality.

# Example
```csharp
// Example of creating a RuneForgeConfig asset in Unity
var runeForgeConfig = ScriptableObject.CreateInstance<RuneForgeConfig>();
runeForgeConfig.entries = new List<RuneForgeEntry>();
```

# Unknowns
- The implementation details of `ItemDef` are not provided in this file.
