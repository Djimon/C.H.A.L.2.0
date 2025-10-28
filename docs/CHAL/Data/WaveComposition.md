# CHAL.Data.WaveComposition

_Automatically generated/updated from `Assets/src/Data/Structs/WaveComposition.cs`._

# Purpose
- Defines the `WaveComposition` struct for managing wave data in a game.
- Provides properties to calculate totals of different enemy ranks.
- Includes a method to clone the `WaveComposition` instance.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public struct WaveComposition`
    - Public fields/properties:
      - `int Level` - Represents the level of the wave.
      - `MapDifficulty Difficulty` - Represents the difficulty of the wave.
      - `List<EnemyStruct> Monsters` - List of enemies in the wave.
      - `int TotalSpawns` - Total count of spawn-ranked monsters.
      - `int TotalNormals` - Total count of normal-ranked monsters.
      - `int TotalMagics` - Total count of magic-ranked monsters.
      - `int TotalElites` - Total count of elite-ranked monsters.
      - `int TotalBosses` - Total count of boss-ranked monsters.
      - `int TotalChampions` - Total count of champion-ranked monsters.
    - Public methods:
      - `WaveComposition Clone()` - Creates a deep copy of the `WaveComposition` instance.

# Key Behavior & Side Effects
- The `Clone` method creates a new instance of `WaveComposition` with a deep copy of the `Monsters` list, ensuring that modifications to the cloned instance do not affect the original.

# Constraints & Failure Modes
- The `Monsters` list can be null; properties that calculate totals handle this with null checks.
- Uses LINQ for summation, which may have performance implications with large lists.

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

int totalSpawns = wave.TotalSpawns; // 5
var clonedWave = wave.Clone();
```

# Unknowns
- The definitions of `MapDifficulty`, `EnemyStruct`, and `EnemyRank` are not provided in this file.

