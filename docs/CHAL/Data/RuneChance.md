# CHAL.Data.RuneChance

_Automatically generated/updated from `Assets/src/Data/Config/RuneForgeConfig.cs`._

# Purpose
- Defines configuration data for rune forging in the game.
- Provides a structure for rune entries and their associated weights.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **[Serializable] class** `RuneForgeEntry`
    - Public fields:
      - `ItemDef remain`: Input item for the rune forge.
      - `List<RuneChance> runes`: Possible runes and their weights.
  - **[Serializable] class** `RuneChance`
    - Public fields:
      - `ItemDef rune`: The rune item.
      - `float weight`: Weight of the rune, constrained between 0 and 1.
  - **[CreateAssetMenu] class** `RuneForgeConfig` [extends `ScriptableObject`]
    - Public fields:
      - `List<RuneForgeEntry> entries`: List of rune forge entries.

# Key Behavior & Side Effects
- `RuneForgeConfig` can be created as an asset in the Unity editor, allowing designers to configure rune forging parameters.

# Constraints & Failure Modes
- `weight` in `RuneChance` must be between 0 and 1 due to the `[Range(0f, 1f)]` attribute.
- No explicit null or empty handling is defined in the code.

# Example
```csharp
var runeForgeConfig = ScriptableObject.CreateInstance<RuneForgeConfig>();
runeForgeConfig.entries = new List<RuneForgeEntry>();
```

# Unknowns
- Specific behavior of `ItemDef` is not defined in this file.
- The impact of the `weight` field on rune selection is not detailed.

