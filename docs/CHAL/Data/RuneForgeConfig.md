# Assets/src/Data/Config/RuneForgeConfig.cs

_Automatically generated/updated from `Assets/src/Data/Config/RuneForgeConfig.cs`._

1) Purpose
- Defines data structures for rune forging configuration in the game.
- Provides a ScriptableObject to hold multiple rune forge entries.

2) Public API
- Namespace/module: CHAL.Data
- Types
  - [Serializable] class RuneForgeEntry
    - Public fields:
      - ItemDef remain: Input item for the rune forge.
      - List<RuneChance> runes: Possible runes with their weightings.
  - [Serializable] class RuneChance
    - Public fields:
      - ItemDef rune: The rune item.
      - float weight: The weight of the rune, constrained between 0 and 1.
  - [CreateAssetMenu(fileName = "RuneForgeConfig", menuName = "Config/RuneForgeConfig")] class RuneForgeConfig : ScriptableObject
    - Public fields:
      - List<RuneForgeEntry> entries: Collection of rune forge entries.

3) Key Behavior & Side Effects
- None explicitly defined in the code.

4) Constraints & Failure Modes
- The weight of each RuneChance must be in the range of 0 to 1.

5) Example
```csharp
// Example of creating a RuneForgeConfig asset
var runeForgeConfig = ScriptableObject.CreateInstance<RuneForgeConfig>();
runeForgeConfig.entries = new List<RuneForgeEntry>();
```

6) Unknowns
- None.
