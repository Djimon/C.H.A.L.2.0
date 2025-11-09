# Assets/src/Data/Enums/Enemy.cs

_Automatically generated/updated from `Assets/src/Data/Enums/Enemy.cs`._

# Purpose
- Defines enumerations for enemy ranks and AI types in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public enum EnemyRank`
    - Values: `Spawn`, `Normal`, `Magic`, `Elite`, `Boss`, `Champion`
  - `public enum EnemyAIType`
    - Values: `AttackFirst`, `AttackLowestHP`, `AttackHighestHP`, `AttackRandom`

# Key Behavior & Side Effects
- No explicit behavior or side effects defined; purely enumerative.

# Constraints & Failure Modes
- No constraints or failure modes evident in the code.

# Example
```csharp
EnemyRank rank = EnemyRank.Boss;
EnemyAIType aiType = EnemyAIType.AttackLowestHP;
```

# Unknowns
- No unknowns present; all information is derived from the file.
