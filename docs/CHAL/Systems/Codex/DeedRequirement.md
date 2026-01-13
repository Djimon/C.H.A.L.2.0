# Assets/src/Data/Defs/DeedRequirement.cs

_Automatically generated/updated from `Assets/src/Data/Defs/DeedRequirement.cs`._

# Purpose
- Defines the `DeedRequirement` class, which represents the requirements for a research task, including waves, maps, and kill counts.

# Public API
- Namespace: `CHAL.Systems.Codex`
- Types
  - **public class DeedRequirement**
    - Public fields/properties:
      - `int waves`: Number of waves required.
      - `int maps`: Number of maps required.
      - `List<MapRequirement> mapRequirements`: List of map requirements.
      - `int killsGeneral`: General kill count required.
      - `List<KillTagCount> killsByTag`: List of kill counts by enemy tag.
      - `int eliteCount`: Count of elite enemies required.
      - `int bossCount`: Count of boss enemies required.
      - `int championCount`: Count of champion enemies required.
    - Public methods:
      - `void ValidateSoft(Action<string> warn, string ctx)`: Validates soft requirements and triggers warnings for invalid conditions.
      - `bool IsEmpty()`: Checks if the current instance is empty (no active waves, maps, or kills).
  - **public sealed class KillTagCount**
    - Public fields/properties:
      - `string enemyTag`: Tag of the enemy.
      - `int count`: Count of kills required for the enemy tag.
  - **public struct MapRequirement**
    - Public fields/properties:
      - `MapDifficulty difficulty`: Difficulty of the map.
      - `int amount`: Amount of maps required at the specified difficulty.

# Key Behavior & Side Effects
- `ValidateSoft` method checks for negative values in `waves`, `maps`, `killsGeneral`, `eliteCount`, `bossCount`, and triggers warnings if any are found.
- It also checks the `killsByTag` list for null entries, empty tags, and negative counts, triggering warnings as necessary.
- `IsEmpty` method determines if the instance has no active requirements based on the counts of waves, maps, kills, and entries in `killsByTag`.

# Constraints & Failure Modes
- Negative values for `waves`, `maps`, `killsGeneral`, `eliteCount`, `bossCount`, and `championCount` are not allowed.
- The `killsByTag` list can contain null entries, which are handled with warnings.
- The `killsByTag` entries must have non-empty tags and non-negative counts.

# Example
```csharp
DeedRequirement requirement = new DeedRequirement();
requirement.waves = 5;
requirement.maps = 3;
requirement.killsGeneral = 10;
requirement.ValidateSoft(warning => Debug.Log(warning), "Requirement Check");
bool isEmpty = requirement.IsEmpty();
```

# Unknowns
- The definition of `MapDifficulty` is not provided in this file.
