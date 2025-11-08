# CHAL.Systems.Research.MapRequirement

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchRequirement.cs`._

# Purpose
- Defines the `ResearchRequirement` class, which represents the requirements for a research task, including waves, maps, and kill counts.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - **public class** `ResearchRequirement`
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
      - `void ValidateSoft(Action<string> warn, string ctx)`: Validates soft requirements and triggers warnings for invalid conditions.
      - `bool IsEmpty()`: Checks if the current instance is empty (no active waves, maps, or kills).
  - **public sealed class** `KillTagCount`
    - Public fields/properties:
      - `string enemyTag`: Tag of the enemy.
      - `int count`: Count of kills required for the enemy tag.
  - **public struct** `MapRequirement`
    - Public fields/properties:
      - `MapDifficulty difficulty`: Difficulty of the map.
      - `int amount`: Amount of maps required at the specified difficulty.

# Key Behavior & Side Effects
- `ValidateSoft` method checks for negative values in various fields and triggers warnings if any invalid conditions are found.
- `IsEmpty` method determines if the `ResearchRequirement` instance has no active requirements.

# Constraints & Failure Modes
- Negative values for `waves`, `maps`, `killsGeneral`, `eliteCount`, `bossCount`, and `championCount` are not allowed.
- The `killsByTag` list can contain null entries, which will trigger warnings if encountered during validation.
- The `killsByTag` entries must have a non-empty `enemyTag` and a non-negative `count`.

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
        new KillTagCount { enemyTag = "Orc", count = 5 },
        new KillTagCount { enemyTag = "Goblin", count = 3 }
    }
};

researchRequirement.ValidateSoft(warning => Console.WriteLine(warning), "Research Task");
bool isEmpty = researchRequirement.IsEmpty();
```

# Unknowns
- The definition of `MapDifficulty` is not provided in this file.
