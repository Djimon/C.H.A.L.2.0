# CHAL.Systems.Research.MapRequirement

_Automatically generated/updated from `Assets/src/Data/Defs/ResearchRequirement.cs`._

```text
1) Purpose
- Defines ResearchRequirement class (Serializable) in CHAL.Systems.Research to represent a set of numeric and collection-based requirements for research progress.
- Defines KillTagCount class (Serializable, sealed) to represent per-tag kill counts used in ResearchRequirement.
- Defines MapRequirement struct (Serializable) to represent per-map requirements by difficulty and amount.

2) Public API
- Namespace/Module
  - CHAL.Systems.Research

- Types

  - Public class ResearchRequirement [Serializable]
    - Fields
      - public int waves; [Min(0)] // required waves (non-negative)
      - public int maps; [Min(0)]  // required maps (non-negative)
      - public List<MapRequirement> mapRequirements = new List<MapRequirement>(); // per-map requirements
      - public int killsGeneral; [Min(0)] // general kill count requirement (non-negative)
      - public List<KillTagCount> killsByTag = new List<KillTagCount>(); // per-tag kill counts
      - public int eliteCount; [Min(0)] // required elite enemies (non-negative)
      - public int bossCount; [Min(0)] // required boss enemies (non-negative)
      - public int championCount; [Min(0)] // required champion enemies (non-negative)
    - Methods
      - public void ValidateSoft(Action<string> warn, string ctx)
        - Emits warnings via warn for: any of waves/maps/killsGeneral/eliteCount/bossCount < 0
          with message: "{ctx}: Negative Anforderungen sind nicht erlaubt."
        - If killsByTag != null, validates each entry:
          - if entry is null: warn "{ctx}: killsByTag[{i}] ist null."
          - if string.IsNullOrWhiteSpace(entry.enemyTag): warn "{ctx}: killsByTag[{i}] hat leeren Tag."
          - if entry.count < 0: warn "{ctx}: killsByTag[{i}] hat negativen Count."
      - public bool IsEmpty()
        - Returns false if any of waves, maps, killsGeneral, eliteCount, bossCount, championCount > 0
        - Otherwise, if killsByTag != null, returns false if any non-null entry has count > 0
        - Returns true if no positive counts exist and no nonzero per-tag counts

  - Public sealed class KillTagCount
    - Fields
      - public string enemyTag; // tag for the enemy
      - public int count;       // required kill count for this tag

  - Public struct MapRequirement
    - Fields
      - public MapDifficulty difficulty; // map difficulty level
      - public int amount;               // required amount at this difficulty

3) Key Behavior & Side Effects
- ValidateSoft uses a provided warn callback to surface validation messages; no exceptions are thrown.
- IsEmpty computes a logical "emptiness" of the requirement based on all counters and per-tag counts.
- mapRequirements and killsByTag default to non-null lists, preventing NullReference in basic usage.

4) Constraints & Failure Modes
- Numeric fields annotated with [Min(0)] imply non-negative requirements; negative values trigger warnings in ValidateSoft.
- KillsByTag entries are validated for nulls, empty enemyTag, and negative counts.
- Null handling: killsByTag may be null internally; IsEmpty handles null gracefully.
- MapDifficulty type is assumed to be defined elsewhere (not in this file).
- No threading or asynchronous behavior inherent to this code.

5) Example
- Not derivable from this file alone (no usage example provided).

6) Unknowns
- Exact definition and values of MapDifficulty.
- How ResearchRequirement integrates with the broader research system (e.g., where ValidateSoft is invoked).
- Serialization implications in Unity runtime for these public fields.

```
