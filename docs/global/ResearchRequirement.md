# global.ResearchRequirement

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchRequirement.cs`._

# Purpose
- Defines the `ResearchRequirement` class and related types for managing research criteria in a game context.

# Public API
- Namespace: None specified

- Types
  - **public class ResearchRequirement**
    - Public fields/properties:
      - `int waves`: Number of waves required.
      - `int maps`: Number of maps required.
      - `List<MapRequirement> mapRequirements`: List of specific map requirements.
      - `int killsGeneral`: Number of general kills required.
      - `List<KillTagCount> killsByTag`: List of kill counts by enemy tag.
      - `int eliteCount`: Number of elite enemies required.
      - `int bossCount`: Number of bosses required.
      - `int championCount`: Number of champions required.
    - Public methods:
      - `void ValidateSoft(Action<string> warn, string ctx)`: Validates the requirement fields and invokes warnings for invalid values.
      - `bool IsEmpty()`: Checks if the requirement is empty (no criteria met).

  - **public sealed class KillTagCount**
    - Public fields/properties:
      - `string enemyTag`: Tag of the enemy.
      - `int count`: Number of kills required for the enemy tag.

  - **public struct MapRequirement**
    - Public fields/properties:
      - `MapDifficulty difficulty`: Difficulty level of the map.
      - `int amount`: Amount of maps required at the specified difficulty.

# Key Behavior & Side Effects
- `ValidateSoft` method checks for negative values in various fields and null entries in `killsByTag`, issuing warnings as necessary.
- `IsEmpty` method determines if the `ResearchRequirement` has any active criteria.

# Constraints & Failure Modes
- Negative values for `waves`, `maps`, `killsGeneral`, `eliteCount`, `bossCount`, and `championCount` are not allowed.
- `killsByTag` can contain null entries, which are handled with warnings.
- The method `IsEmpty` checks for non-zero counts in the fields and lists.

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

researchRequirement.ValidateSoft(warning => Debug.Log(warning), "Research Check");
bool isEmpty = researchRequirement.IsEmpty();
```

# Unknowns
- No information on the `MapDifficulty` type and its possible values.
- No context on how `ResearchRequirement` is utilized within the broader application or game logic.

