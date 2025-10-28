# Assets/src/Data/Config/RuneForgeConfig.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines data structures for rune forging configuration in a game.
- Provides a ScriptableObject for storing multiple rune forge entries.

## Public API
- Namespace: `CHAL.Data`
- Types
  - `public class RuneForgeEntry`
    - Public fields/properties:
      - `public ItemDef remain` - Input item definition.
      - `public List<RuneChance> runes` - Possible runes with their weights.
  - `public class RuneChance`
    - Public fields/properties:
      - `public ItemDef rune` - Rune item definition.
      - `public float weight` - Weight of the rune, constrained between 0 and 1.
  - `public class RuneForgeConfig : ScriptableObject`
    - Public fields/properties:
      - `public List<RuneForgeEntry> entries` - List of rune forge entries.

## Key Behavior & Side Effects
- `RuneForgeConfig` can be created as an asset via the Unity Editor with the menu option "Config/RuneForgeConfig".

## Constraints & Failure Modes
- `weight` in `RuneChance` must be in the range [0, 1].
- No explicit error handling or threading considerations are present.

## Example
```csharp
var config = ScriptableObject.CreateInstance<RuneForgeConfig>();
config.entries = new List<RuneForgeEntry>();
```

## Unknowns
- The definitions and behaviors of `ItemDef` and how it interacts with the rune forging system are not provided in this file.
```
