# CHAL.Systems.Research.MapRequirement

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchRequirement.cs`._

# Purpose
- Defines the `ResearchRequirement` class and related types for managing research requirements in the game.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - **public class ResearchRequirement**
    - Public fields/properties:
      - `int waves`: Minimum number of waves required.
      - `int maps`: Minimum number of maps required.
      - `List<MapRequirement> mapRequirements`: List of specific map requirements.
      - `int killsGeneral`: Minimum number of general kills required.
      - `List<KillTagCount> killsByTag`: List of kill counts by enemy tag.
      - `int eliteCount`: Minimum number of elite enemies required.
      - `int bossCount`: Minimum number of bosses required.
      - `int championCount`: Minimum number of champions required.
    - Public methods:
      - `void ValidateSoft(Action<string> warn, string ctx)`: Validates the requirement fields and invokes warnings for invalid values.
      - `bool IsEmpty()`: Checks if the requirement is empty (all counts are zero or not applicable).
  
  - **public sealed class KillTagCount**
    - Public fields/properties:
      - `string enemyTag`: Tag of the enemy.
      - `int count`: Number of kills required for the specified enemy tag.

  - **public struct MapRequirement**
    - Public fields/properties:
      - `MapDifficulty difficulty`: Difficulty level of the map.
      - `int amount`: Amount of maps required at the specified difficulty.

# Key Behavior & Side Effects
- `ValidateSoft` checks for negative values in the requirement fields and null entries in `killsByTag`, issuing warnings as necessary.
- `IsEmpty` determines if the `ResearchRequirement` has any active requirements.

# Constraints & Failure Modes
- Negative values for `waves`, `maps`, `killsGeneral`, `eliteCount`, `bossCount`, and `championCount` are not allowed.
- `killsByTag` can contain null entries, which are handled with warnings.
- The method `IsEmpty` returns true only if all counts are zero or not applicable.

# Example
```csharp
var researchRequirement = new ResearchRequirement
{
    waves = 5,
    maps = 3,
    killsGeneral = 10,
    eliteCount = 2,
    bossCount = 1,
    championCount = 0,
    killsByTag = new List<KillTagCount>
    {
        new KillTagCount { enemyTag = "Goblin", count = 5 },
        new KillTagCount { enemyTag = "Orc", count = 3 }
    }
};
```

# Unknowns
- None.

