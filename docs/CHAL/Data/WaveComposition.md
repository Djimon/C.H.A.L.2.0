# CHAL.Data.WaveComposition

_Automatically generated/updated from `Assets/src/Data/Structs/WaveComposition.cs`._

# Purpose
- Defines the `WaveComposition` struct representing a wave of enemies in a game, including their level, difficulty, and types.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public struct `WaveComposition`
    - Public fields/properties:
      - `int Level`: The level of the wave.
      - `MapDifficulty Difficulty`: The difficulty of the wave.
      - `List<EnemyStruct> Monsters`: The list of enemies in the wave.
      - `int TotalSpawns`: Total count of spawn-ranked monsters.
      - `int TotalNormals`: Total count of normal-ranked monsters.
      - `int TotalMagics`: Total count of magic-ranked monsters.
      - `int TotalElites`: Total count of elite-ranked monsters.
      - `int TotalBosses`: Total count of boss-ranked monsters.
      - `int TotalChampions`: Total count of champion-ranked monsters.
    - Public methods:
      - `WaveComposition Clone()`: Creates a copy of the current `WaveComposition` instance.

# Key Behavior & Side Effects
- The `Clone` method creates a deep copy of the `WaveComposition`, including a new list of `EnemyStruct` instances.

# Constraints & Failure Modes
- The `Monsters` list can be null; the properties that calculate totals handle this with null-coalescing operators.

# Example
```csharp
var wave = new WaveComposition
{
    Level = 1,
    Difficulty = MapDifficulty.Normal,
    Monsters = new List<EnemyStruct>
    {
        new EnemyStruct { EnemyId = 1, Rank = EnemyRank.Spawn, Count = 5 },
        new EnemyStruct { EnemyId = 2, Rank = EnemyRank.Normal, Count = 10 }
    }
};

var clonedWave = wave.Clone();
```

# Unknowns
- None.

