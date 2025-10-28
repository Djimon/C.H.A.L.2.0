# Assets/src/Data/Defs/ResearchRequirement.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `ResearchRequirement` class and related types for managing research criteria in a game context.

## Public API
- Namespace/module: None specified.
- Types
  - **public class ResearchRequirement**
    - Public fields/properties:
      - `int waves`: Number of waves required.
      - `int maps`: Number of maps required.
      - `List<MapRequirement> mapRequirements`: List of specific map requirements.
      - `int killsGeneral`: General kill count required.
      - `List<KillTagCount> killsByTag`: List of kill counts by enemy tag.
      - `int eliteCount`: Count of elite enemies required.
      - `int bossCount`: Count of boss enemies required.
      - `int championCount`: Count of champion enemies required.
    - Public methods:
      - `void ValidateSoft(Action<string> warn, string ctx)`: Validates the requirement fields and invokes warnings for invalid values.
      - `bool IsEmpty()`: Checks if the requirement is empty (returns true if all counts are zero or null).

  - **public sealed class KillTagCount**
    - Public fields/properties:
      - `string enemyTag`: Tag of the enemy.
      - `int count`: Count of kills required for the specified enemy tag.

  - **public struct MapRequirement**
    - Public fields/properties:
      - `MapDifficulty difficulty`: Difficulty level of the map.
      - `int amount`: Amount of maps required at the specified difficulty.

## Key Behavior & Side Effects
- `ValidateSoft` checks for negative values in various fields and null entries in `killsByTag`, invoking warnings as necessary.
- `IsEmpty` determines if the `ResearchRequirement` instance has any non-zero counts, indicating whether it is effectively empty.

## Constraints & Failure Modes
- Negative values for `waves`, `maps`, `killsGeneral`, `eliteCount`, `bossCount`, and `championCount` are not allowed.
- Null entries in `killsByTag` are handled with warnings.
- The method `IsEmpty` checks for both zero counts and null entries in `killsByTag`.

## Example
```csharp
var requirement = new ResearchRequirement
{
    waves = 5,
    maps = 3,
    killsGeneral = 10,
    eliteCount = 2,
    bossCount = 1,
    championCount = 0,
    killsByTag = new List<KillTagCount>
    {
        new KillTagCount { enemyTag = "Orc", count = 5 },
        new KillTagCount { enemyTag = "Goblin", count = 3 }
    }
};

requirement.ValidateSoft(warning => Debug.Log(warning), "Research Requirement");
bool isEmpty = requirement.IsEmpty();
```

## Unknowns
- No external dependencies or specific usage contexts are defined within this file.
```
