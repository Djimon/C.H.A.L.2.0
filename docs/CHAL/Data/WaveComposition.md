# Assets/src/Data/Structs/WaveComposition.cs

_Automatically generated/updated from `Assets/src/Data/Structs/WaveComposition.cs`._

# Purpose
- Defines the `WaveComposition` struct for managing wave data in a game, including level, difficulty, and a list of monsters.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public struct WaveComposition`
    - Public fields/properties:
      - `int Level`: The level of the wave.
      - `MapDifficulty Difficulty`: The difficulty of the wave.
      - `List<EnemyStruct> Monsters`: The list of monsters in the wave.
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
- The `Monsters` list can be null, and the properties that aggregate counts handle this with null-coalescing operators.

# Example
```csharp
var wave = new WaveComposition
{
    Level = 1,
    Difficulty = MapDifficulty.Normal,
    Monsters = new List<EnemyStruct>
    {
        new EnemyStruct { EnemyId = 1, Rank = EnemyRank.Spawn, Count = 5 },
        new EnemyStruct { EnemyId = 2, Rank = EnemyRank.Normal, Count = 3 }
    }
};

var clonedWave = wave.Clone();
```

# Unknowns
- The definitions of `MapDifficulty` and `EnemyStruct` are not provided in this file.
