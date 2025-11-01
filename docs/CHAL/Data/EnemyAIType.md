# CHAL.Data.EnemyAIType

_Automatically generated/updated from `Assets/src/Data/Enums/Enemy.cs`._

```csharp
1) Purpose
- Defines enemy-related enums in the CHAL.Data namespace.
- Provides EnemyRank and EnemyAIType for categorizing enemies and selecting AI behavior.

2) Public API
- Namespace/module: CHAL.Data
- Types
  - public enum EnemyRank
    - Spawn
    - Normal
    - Magic
    - Elite
    - Boss
    - Champion
  - public enum EnemyAIType
    - AttackFirst
    - AttackLowestHP
    - AttackHighestHP
    - AttackRandom

3) Key Behavior & Side Effects
- No runtime behavior or side effects defined; these are simple data enums.

4) Constraints & Failure Modes
- No explicit constraints or guards.
- Underlying type and value assignments are not specified in the file (defaults apply if used in C#).

5) Example
```csharp
// Example usage
EnemyRank rank = EnemyRank.Normal;
EnemyAIType ai = EnemyAIType.AttackFirst;
```

6) Unknowns
- How these enums are used elsewhere in the project.
- Serialization/deserialization behavior for these enums.
- Any domain-specific semantics beyond the defined members.
```
