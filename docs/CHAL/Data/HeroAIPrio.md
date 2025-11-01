# CHAL.Data.HeroAIPrio

_Automatically generated/updated from `Assets/src/Data/Enums/AIPrio.cs`._

1) Purpose
- Defines a public enumeration HeroAIPrio in CHAL.Data.
- Enumerates AI priority options for hero behavior (used by AI decision logic).

2) Public API
- Namespace/module: CHAL.Data
- Types
  - public enum HeroAIPrio
    - RandomAttack
      - Brief role: choose a random attack target/action
    - AttackHighestHP
      - Brief role: target enemy with the highest HP
    - AttackLowestHP
      - Brief role: target enemy with the lowest HP
    - AttackNearest
      - Brief role: target the nearest enemy
    - FocusFirstInRange
      - Brief role: focus the first enemy within range
    - BuffAllies
      - Brief role: apply buffs to allies
    - HealAllies
      - Brief role: heal allies
    - DebuffTarget
      - Brief role: apply debuffs to a target
    - MaintainMinions
      - Brief role: maintain or manage minions
    - SpreadDoTs
      - Brief role: spread damage-over-time effects
    - CCFirstThreat
      - Brief role: crowd-control the first threat

3) Key Behavior & Side Effects
- No runtime logic, methods, or state changes defined here.
- This enum serves as a data surface for AI decision-making elsewhere in the codebase.

4) Constraints & Failure Modes
- No explicit guards or threading/async notes.
- It's a value-type enum; lacks behavioral guarantees within this file.
- Underlying type is the default enum integral type (C# behavior), but not specified here.

5) Example
```csharp
using CHAL.Data;

HeroAIPrio prio = HeroAIPrio.AttackNearest;
```

6) Unknowns
- How each prio maps to concrete AI actions is not defined in this file.
- How values are serialized, persisted, or compared at runtime is not specified here.
- Whether additional prio values exist elsewhere in the project is not determinable from this file alone.
