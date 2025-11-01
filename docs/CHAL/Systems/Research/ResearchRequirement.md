# CHAL.Systems.Research.ResearchRequirement

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchRequirement.cs`._

1) Purpose
- Defines data structures for describing research requirements in CHAL.Systems.Research.
- Provides validation and emptiness checks for the research requirements data.
- Implements serializable data types used by the Unity editor (fields with Min attributes, lists of sub-entries).

2) Public API
- Namespace/module
  - CHAL.Systems.Research

- Types
  - public class ResearchRequirement : [Serializable]
    - Public fields
      - [Min(0)] public int waves;
      - [Min(0)] public int maps;
      - public List<MapRequirement> mapRequirements = new List<MapRequirement>();
      - [Min(0)] public int killsGeneral;
      - public List<KillTagCount> killsByTag = new List<KillTagCount>();
      - [Min(0)] public int eliteCount;
      - [Min(0)] public int bossCount;
      - [Min(0)] public int championCount;
    - Public methods
      - public void ValidateSoft(Action<string> warn, string ctx)
        - Validates non-negative counters; emits warnings via warn with German messages.
        - If killsByTag is non-null, iterates entries and warns on null entries, empty enemyTag, or negative count.
      - public bool IsEmpty()
        - Returns true if all top-level counters are <= 0 and no killsByTag entry has a positive count; otherwise false.
  - public sealed class KillTagCount
    - Public fields
      - public string enemyTag;
      - public int count;
  - public struct MapRequirement
    - Public fields
      - public MapDifficulty difficulty;
      - public int amount;

3) Key Behavior & Side Effects
- ValidateSoft(warn, ctx)
  - Checks: waves, maps, killsGeneral, eliteCount, bossCount for negative values; warns with $"{ctx}: Negative Anforderungen sind nicht erlaubt."
  - If killsByTag != null, for each entry:
    - If entry is null: warn $"{ctx}: killsByTag[{i}] ist null."
    - If enemyTag is null/empty/whitespace: warn $"{ctx}: killsByTag[{i}] hat leeren Tag."
    - If count < 0: warn $"{ctx}: killsByTag[{i}] hat negativen Count."
- IsEmpty()
  - Returns false if any of waves, maps, killsGeneral, eliteCount, bossCount, championCount > 0.
  - If killsByTag contains any non-null entry with count > 0, returns false.
  - Otherwise returns true.
- Data initialization
  - mapRequirements and killsByTag are initialized to empty lists.
- Attributes
  - [Min(0)] constraints indicate non-negative values enforced by Unity editor on those fields.
  - [Serializable] on types enables Unity serialization.

4) Constraints & Failure Modes
- Guards and null handling
  - ValidateSoft guards against negative numeric fields; uses a null-safe warn delegate.
  - KillsByTag iteration handles null entries gracefully.
- Non-atomic state changes
  - No state mutation occurs in validation or emptiness checks.
- Performance
  - Linear scans over small lists; no heavy async/threading behavior evident.

5) Example
```csharp
var req = new CHAL.Systems.Research.ResearchRequirement { waves = 1 };
bool empty = req.IsEmpty(); // false
req.ValidateSoft(s => System.Console.WriteLine(s), "Req"); // may emit warnings if fields are invalid
```

6) Unknowns
- MapDifficulty type definition and possible values are not defined in this file.
- How ResearchRequirement is consumed elsewhere in the project (no usage shown here).
