# CHAL.Data.EnemyAIType

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
- No explicit behaviors or side effects defined in this file.

# Constraints & Failure Modes
- No specific constraints or failure modes identified.

# Example
```csharp
EnemyRank rank = EnemyRank.Boss;
EnemyAIType aiType = EnemyAIType.AttackLowestHP;
```

# Unknowns
- No unknowns identified.
