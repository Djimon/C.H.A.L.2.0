# Assets/src/Data/Structs/EnemyStruct.cs

_Automatically generated/updated from `Assets/src/Data/Structs/EnemyStruct.cs`._

# Purpose
- Defines the `EnemyStruct` structure for representing enemy data in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public struct `EnemyStruct`
    - Public fields/properties:
      - `string EnemyId` - Optional reference to monster definition.
      - `int Count` - Number of times the enemy has spawned.
      - `List<string> bonusTags` - Tags associated with the enemy (e.g., {"insect", "swarm"}).
      - `EnemyRank Rank` - Rank of the enemy (e.g., Elite, Boss).

# Key Behavior & Side Effects
- No explicit methods or behaviors defined; primarily a data structure.

# Constraints & Failure Modes
- No explicit guards or null/empty handling noted.
- Performance considerations not evident.

# Example
```csharp
EnemyStruct enemy = new EnemyStruct
{
    EnemyId = "goblin_01",
    Count = 5,
    bonusTags = new List<string> { "insect", "swarm" },
    Rank = EnemyRank.Elite
};
```

# Unknowns
- No information on the `EnemyRank` type or its possible values.
