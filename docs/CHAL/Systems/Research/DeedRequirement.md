# Assets/src/Data/Defs/DeedRequirement.cs

_Automatically generated/updated from `Assets/src/Data/Defs/DeedRequirement.cs`._

# Purpose
- Defines the `DeedRequirement` class, which represents the requirements for a research task, including waves, maps, and kill counts.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - **public class DeedRequirement**
    - Public fields/properties:
      - `int waves`: Number of waves required (minimum 0).
      - `int maps`: Number of maps required (minimum 0).
      - `List<MapRequirement> mapRequirements`: List of map requirements.
      - `int killsGeneral`: General kill count required (minimum 0).
      - `List<KillTagCount> killsByTag`: List of kill counts by enemy tag.
      - `int eliteCount`: Count of elite enemies required (minimum 0).
      - `int bossCount`: Count of bosses required (minimum 0).
      - `int championCount`: Count of champions required (minimum 0).
    - Public methods:
      - `void ValidateSoft(Action<string> warn, string ctx)`: Validates soft requirements and triggers warnings for invalid conditions.
      - `bool IsEmpty()`: Checks if the current instance is empty (returns true if no active waves, maps, or kills).

  - **public sealed class KillTagCount**
    - Public fields/properties:
      - `string enemyTag`: Tag of the enemy.
      - `int count`: Count of kills required for the enemy tag.

  - **public struct MapRequirement**
    - Public fields/properties:
      - `MapDifficulty difficulty`: Difficulty of the map.
      - `int amount`: Amount of maps required.

# Key Behavior & Side Effects
- `ValidateSoft` method checks for negative values in various fields and triggers warnings if any invalid conditions are found.
- `IsEmpty` method determines if the `DeedRequirement` instance has no active requirements.

# Constraints & Failure Modes
- Negative values for `waves`, `maps`, `killsGeneral`, `eliteCount`, `bossCount`, and `championCount` are not allowed.
- The `killsByTag` list can contain null entries, which will trigger warnings during validation.
- The `killsByTag` entries must have a non-empty `enemyTag` and a non-negative `count`.

# Example
```csharp
DeedRequirement requirement = new DeedRequirement
{
    waves = 5,
    maps = 2,
    killsGeneral = 10,
    eliteCount = 1,
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

