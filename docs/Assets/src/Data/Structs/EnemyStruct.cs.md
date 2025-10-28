# Assets/src/Data/Structs/EnemyStruct.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `EnemyStruct` structure for representing enemy data in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `struct EnemyStruct`
    - Public fields/properties:
      - `string EnemyId` - Reference to the enemy definition (optional).
      - `int Count` - Number of times the enemy is spawned.
      - `List<string> bonusTags` - Tags associated with the enemy (e.g., {"insect", "swarm"}).
      - `EnemyRank Rank` - Rank of the enemy (e.g., Elite, Boss).

# Key Behavior & Side Effects
- No explicit methods or behaviors defined; primarily a data structure.

# Constraints & Failure Modes
- No explicit guards or error handling present.
- Assumes `bonusTags` can be null or empty, but no handling is defined.

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
- No information on how `EnemyRank` is defined or used.
- No constructor is provided for initializing `EnemyStruct` with parameters.
```
