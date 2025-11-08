# CHAL.Data.EnemyStruct

_Automatically generated/updated from `Assets/src/Data/Structs/EnemyStruct.cs`._

# Purpose
- Defines the `EnemyStruct` structure for representing enemy data in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public struct `EnemyStruct`
    - Public fields/properties:
      - `string EnemyId` - Reference to the enemy definition (optional).
      - `int Count` - Number of times the enemy has spawned.
      - `List<string> bonusTags` - Tags associated with the enemy (e.g., {"insect", "swarm"}).
      - `EnemyRank Rank` - Rank of the enemy (e.g., Elite, Boss).

# Key Behavior & Side Effects
- None specified.

# Constraints & Failure Modes
- None specified.

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
- None.

