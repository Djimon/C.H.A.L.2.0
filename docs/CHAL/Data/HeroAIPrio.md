# CHAL.Data.HeroAIPrio

_Automatically generated/updated from `Assets/src/Data/Enums/AIPrio.cs`._

# Purpose
- Defines an enumeration for AI priority behaviors in a game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public enum HeroAIPrio`
    - `RandomAttack`: Attack randomly.
    - `AttackHighestHP`: Target the enemy with the highest health points.
    - `AttackLowestHP`: Target the enemy with the lowest health points.
    - `AttackNearest`: Target the nearest enemy.
    - `FocusFirstInRange`: Focus on the first enemy within range.
    - `BuffAllies`: Provide buffs to allies.
    - `HealAllies`: Heal allied characters.
    - `DebuffTarget`: Apply debuffs to the target.
    - `MaintainMinions`: Ensure minions are maintained.
    - `SpreadDoTs`: Spread damage over time effects.
    - `CCFirstThreat`: Crowd control the first threat encountered.

# Key Behavior & Side Effects
- No explicit flows, state changes, or error handling present.

# Constraints & Failure Modes
- No guards, null/empty handling, or threading/async notes present.

# Example
- No example derivable from the file.

# Unknowns
- No unknowns present.
