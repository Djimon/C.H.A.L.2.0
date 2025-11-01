# CHAL.Data.EnemyStruct

_Automatically generated/updated from `Assets/src/Data/Structs/EnemyStruct.cs`._

Purpose
- Serializable data container: defines a public struct EnemyStruct.
- Holds per-enemy spawn data: EnemyId, Count, bonusTags, Rank.
- Located in namespace CHAL.Data; accessible publicly.

Public API
- Namespace/module
  - CHAL.Data
- Types
  - public struct EnemyStruct [Serializable]
    - public string EnemyId
      - Optional: reference to Monster-Def
    - public int Count
      - how many times spawned
    - public List<string> bonusTags
      - e.g. {"insect","swarm"}
    - public EnemyRank Rank
      - e.g. Elite, Boss

Key Behavior & Side Effects
- No behavior or methods defined; acts as a data container.
- [Serializable] indicates it can be serialized/deserialized.
- Copying a struct copies its value fields; bonusTags (the List<string>) remains a reference type, so multiple copies share the same list instance if not cloned.
- No constructors defined; fields rely on default initialization when not explicitly set.

Constraints & Failure Modes
- bonusTags may be null when the struct is default-initialized (no constructor initializes it).
- As a value type containing a reference-type field (List<string>), copying the struct copies the reference to the list, not the list contents.
- No validation or normalization is defined; attempted use may rely on external code to enforce non-null/valid values.
- EnemyRank type is not defined in this file; its definition is external.

Example
```csharp
// Minimal usage example (no explicit constructor)
var es = new EnemyStruct
{
    EnemyId = "dragon",
    Count = 2,
    bonusTags = new List<string> { "fire", "flying" },
    Rank = default(EnemyRank)
};
```

Unknowns
- Definition and members of EnemyRank are not present in this file.
- Any runtime expectations for EnemyId being null/empty are not specified.
- Behavior when bonusTags is null (read/write semantics) are not defined beyond normal C# rules.
