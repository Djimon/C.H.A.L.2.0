# CHAL.Data.RuneForgeEntry

_Automatically generated/updated from `Assets/src/Data/Config/RuneForgeConfig.cs`._

# Purpose
- Defines data structures for rune forging configuration in a game.
- Provides a ScriptableObject for storing multiple rune forge entries.

# Public API
- Namespace: CHAL.Data
- Types
  - [Serializable] class RuneForgeEntry
    - Public fields:
      - ItemDef remain: Input item for the rune forge.
      - List<RuneChance> runes: Possible runes with their weights.
  - [Serializable] class RuneChance
    - Public fields:
      - ItemDef rune: The rune item.
      - float weight: Weight of the rune, ranging from 0 to 1.
  - [CreateAssetMenu(fileName = "RuneForgeConfig", menuName = "Config/RuneForgeConfig")] class RuneForgeConfig : ScriptableObject
    - Public fields:
      - List<RuneForgeEntry> entries: Collection of rune forge entries.

# Key Behavior & Side Effects
- The RuneForgeConfig can be created as an asset in the Unity editor, allowing designers to configure rune forging parameters.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes valid ItemDef and RuneChance instances are provided.

# Example
```csharp
// Example of creating a RuneForgeConfig asset in Unity
var runeForgeConfig = ScriptableObject.CreateInstance<RuneForgeConfig>();
runeForgeConfig.entries = new List<RuneForgeEntry>();
```

# Unknowns
- Specific behavior of ItemDef and its impact on the rune forging process cannot be determined from this file.

