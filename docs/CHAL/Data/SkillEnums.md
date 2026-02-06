# Assets/src/Data/Enums/SkillEnums.cs

_Automatically generated/updated from `Assets/src/Data/Enums/SkillEnums.cs`._

# Purpose
- Defines enumerations related to skills in the game, including types, ranges, tags, and modifiers.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public enum SkillType`
    - Values: `Melee`, `Projectile`, `Spell`, `Summon`
  - `public enum SkillRange`
    - Values: `Self`, `Melee`, `Reach`, `MidDistance`, `FarDistance`
  - `public enum SkillTag`
    - Values: `Melee`, `Projectile`, `Spell`, `AoE`, `Buff`, `Debuff`, `DoT`, `Aura`, `Summon`, `Trap`, `Orb`, `Mark`, `Movement`, `Nuke`, `Ground`, `Hazard`, `Fire`, `Cold`, `Poison`, `Arcane`, `Holy`, `Physical`
  - `public enum ModifierTarget`
    - Values: `Damage`, `CritChance`, `CritMultiplier`, `AttackSpeed`, `ProjectileCount`, `ProjectileSpeed`, `PierceChance`, `Range`, `AoERadius`, `Duration`, `DoTMaxStacks`, `DoTDuration`, `TicksPerSecond`, `CastTime`, `Cooldown`, `SummonCount`, `SummonHP`, `SummonDamage`, `AuraRange`, `MovementSpeed`, `LeechFactor`, `HealAmount`, `StackLimit`
  - `public enum ModifierOperation`
    - Values: `Add`, `Mult`, `Replace`
  - `public enum ModifierHook`
    - Values: `None`, `OnCast`, `OnHit`, `OnCrit`, `OnKill`, `OnEnd`

# Key Behavior & Side Effects
- No explicit behavior or side effects defined; purely enumerative.

# Constraints & Failure Modes
- No constraints or failure modes are defined in the file.

# Example
- No examples are provided in the file.

# Unknowns
- No unknowns can be determined from this file.
