# CHAL.Systems.Research.ResearchRequirement

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchRequirement.cs`._

```text
1) Purpose
- Defines data structures for research requirements in CHAL.Systems.Research.
- Exposes ResearchRequirement (with thresholds and per-tag kill counts) and supporting types KillTagCount and MapRequirement.
- Provides simple validation and emptiness-check utilities for research requirements.
```

```text
2) Public API
- Namespace/module
  - CHAL.Systems.Research

- Types
  - public class ResearchRequirement
    - Public fields
      - [Min(0)] public int waves
      - [Min(0)] public int maps
      - public List<MapRequirement> mapRequirements = new List<MapRequirement>()
      - [Min(0)] public int killsGeneral
      - public List<KillTagCount> killsByTag = new List<KillTagCount>()
      - [Min(0)] public int eliteCount
      - [Min(0)] public int bossCount
      - [Min(0)] public int championCount
    - Public methods
      - public void ValidateSoft(Action<string> warn, string ctx)
        - Emits warnings through warn for negative numeric fields and for issues within killsByTag entries
        - Rules:
          - Warn if waves, maps, killsGeneral, eliteCount, bossCount, or championCount are negative
          - If killsByTag is non-null, iterate and:
            - Warn if element is null
            - Warn if enemyTag is null/empty/whitespace
            - Warn if count is negative
      - public bool IsEmpty()
        - Returns true if all numeric thresholds are not positive (<= 0) and no killsByTag entry has count > 0
        - Otherwise returns false

  - public sealed class KillTagCount
    - Public fields
      - public string enemyTag
      - public int count

  - public struct MapRequirement
    - Public fields
      - public MapDifficulty difficulty
      - public int amount
```

```text
3) Key Behavior & Side Effects
- ValidateSoft
  - Performs defensive validation without throwing exceptions.
  - Uses the provided warn delegate to report:
    - Negative values for waves, maps, killsGeneral, eliteCount, bossCount, championCount
    - For each non-null entry in killsByTag:
      - Null entries generate a warning
      - Empty or whitespace enemyTag generate a warning
      - Negative count generates a warning
- IsEmpty
  - Checks whether any of the numeric thresholds are > 0; if so, returns false.
  - If numeric thresholds are not positive, iterates killsByTag (if not null) and returns false if any non-null entry has count > 0.
  - Otherwise returns true.
- MapRequirements list is initialized to an empty list by default.
```

```text
4) Constraints & Failure Modes
- Field constraints
  - [Min(0)] attributes indicate non-negative expectations for: waves, maps, killsGeneral, eliteCount, bossCount, championCount.
- Null handling
  - killsByTag may be null; ValidateSoft guards against null entries and reports issues for null elements.
- Dependency on external types
  - MapDifficulty is referenced but not defined in this file.
- Side effects
  - ValidateSoft emits warnings via the provided callback; does not throw.
```

```text
5) Example
```csharp
using CHAL.Systems.Research;
using System;

public class Example
{
    public void Run()
    {
        var r = new ResearchRequirement();
        r.waves = 1;
        r.maps = 0;
        r.mapRequirements.Add(new MapRequirement { difficulty = default(MapDifficulty), amount = 1 });
        r.killsGeneral = 5;
        r.killsByTag.Add(new KillTagCount { enemyTag = "Zombie", count = 3 });

        bool empty = r.IsEmpty(); // false

        r.ValidateSoft(Console.WriteLine, "Example");
    }
}
```
Note: MapDifficulty is defined elsewhere in the project; usage with default(MapDifficulty) demonstrates structure without assuming specific values.
```

```text
6) Unknowns
- Definition and possible values of MapDifficulty (external to this file).
- How ResearchRequirement is consumed by other systems (e.g., interpretation of thresholds) beyond this file.
- Any runtime behavior tied to Unity’s serialization or inspector beyond the included Min attributes.
```
