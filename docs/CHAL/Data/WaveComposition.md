# CHAL.Data.WaveComposition

_Automatically generated/updated from `Assets/src/Data/Structs/WaveComposition.cs`._

```text
1) Purpose
- Defines a serializable struct WaveComposition in CHAL.Data.
- Represents a wave: level, difficulty, and a list of Monster definitions.
- Provides computed totals by monster rank and a Clone method for shallow/deep-copy semantics.

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public struct WaveComposition
    - Public fields/properties
      - public int Level
        - Level of the wave.
      - public MapDifficulty Difficulty
        - Difficulty setting for the wave.
      - public List<EnemyStruct> Monsters
        - Definitions of monsters in the wave.
      - public int TotalSpawns => Monsters?.Where(m => m.Rank == EnemyRank.Spawn).Sum(m => m.Count) ?? 0
        - Total count of Spawn-ranked monsters.
      - public int TotalNormals => Monsters?.Where(m => m.Rank == EnemyRank.Normal).Sum(m => m.Count) ?? 0
        - Total count of Normal-ranked monsters.
      - public int TotalMagics => Monsters?.Where(m => m.Rank == EnemyRank.Magic).Sum(m => m.Count) ?? 0
        - Total count of Magic-ranked monsters.
      - public int TotalElites => Monsters?.Where(m => m.Rank == EnemyRank.Elite).Sum(m => m.Count) ?? 0
        - Total count of Elite-ranked monsters.
      - public int TotalBosses => Monsters?.Where(m => m.Rank == EnemyRank.Boss).Sum(m => m.Count) ?? 0
        - Total count of Boss-ranked monsters.
      - public int TotalChampions => Monsters?.Where(m => m.Rank == EnemyRank.Champion).Sum(m => m.Count) ?? 0
        - Total count of Champion-ranked monsters.
    - Public methods
      - public WaveComposition Clone()
        - Returns a new WaveComposition with:
          - Level and Difficulty copied.
          - Monsters replaced by a new list containing copies of each EnemyStruct:
            - EnemyId, Rank, Count preserved.
            - bonusTags recreated as a new List<string>(m.bonusTags).
        - Note: The cloning uses a per-item copy, effectively deep-copying the inner Monster list structure.

3) Key Behavior & Side Effects
- Computed totals (TotalSpawns, TotalNormals, TotalMagics, TotalElites, TotalBosses, TotalChampions) are evaluated on access via LINQ; null-monster lists yield 0.
- Clone creates a new WaveComposition with:
  - Level = this.Level
  - Difficulty = this.Difficulty
  - Monsters = a new List<EnemyStruct> with per-item copies and a new bonusTags list.
- No other dynamic side effects; no event hooks or external state changes.

4) Constraints & Failure Modes
- Monsters can be null; totals handle null via null-conditional and coalescing to 0.
- Clone assumes this.Monsters is non-null; if Monsters is null, Clone will throw NullReferenceException at Select.
- Clone assumes each m.bonusTags is non-null; if any m.bonusTags is null, new List<string>(m.bonusTags) will throw ArgumentNullException.
- No explicit thread-safety guarantees; this is a plain data struct with no synchronization.

5) Example
```csharp
using CHAL.Systems.Loot.Models;
using CHAL.Data;

var wave = new WaveComposition
{
    Level = 1,
    Difficulty = MapDifficulty.Easy,
    Monsters = new List<EnemyStruct>
    {
        new EnemyStruct { EnemyId = 101, Rank = EnemyRank.Normal, Count = 3, bonusTags = new List<string> { "starter" } }
    }
};

var waveCopy = wave.Clone();
```

6) Unknowns
- Definition and members of MapDifficulty (enum values, semantics).
- Full definition of EnemyStruct (fields beyond EnemyId, Rank, Count, bonusTags; nullability/other properties).
- Exact semantics of EnemyRank enum values and how they map to gameplay.
- Whether bonusTags can be null in typical usage (Clone copies may throw if null).
- Any additional serialization behavior tied to [Serializable] in this project context.
- Any other public surface not visible in this file (extensions, implicit operators, etc.).
```
