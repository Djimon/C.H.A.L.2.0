# CHAL.Data.EnemyRank

_Automatically generated/updated from `Assets/src/Data/Enums/Enemy.cs`._

1) Purpose
- Defines EnemyRank with members Spawn, Normal, Magic, Elite, Boss, Champion.
- Defines EnemyAIType with members AttackFirst, AttackLowestHP, AttackHighestHP, AttackRandom.
- These enums are declared within the CHAL.Data namespace.

2) Public API
- Namespace/module: CHAL.Data
- Types
  - public enum EnemyRank
    - Values: Spawn, Normal, Magic, Elite, Boss, Champion
  - public enum EnemyAIType
    - Values: AttackFirst, AttackLowestHP, AttackHighestHP, AttackRandom

3) Key Behavior & Side Effects
- None; this file contains only type declarations (enums) with no runtime behavior, side effects, or methods.

4) Constraints & Failure Modes
- None explicit; enums are simple value types with defined members only.
- No nullability, threading, or async considerations visible in this file.

6) Unknowns
- How these enums are consumed in gameplay logic (usage, mapping, or serialization) is not determinable from this file.
- Whether additional values or extended behaviors are defined elsewhere or in future versions.
- Any Unity inspector/serialization implications beyond standard C# enum behavior.
