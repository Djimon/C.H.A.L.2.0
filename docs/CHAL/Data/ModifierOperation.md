# CHAL.Data.ModifierOperation

_Automatically generated/updated from `Assets/src/Data/Enums/SkillEnums.cs`._

# Purpose
- Defines various enumerations related to skills in the game.

# Public API
- Namespace: `CHAL.Data`
- Types:
  - `public enum SkillType`
    - Values: `Melee`, `Projectile`, `Spell`, `Summon`
  - `public enum SkillRange`
    - Values: `Self`, `Melee`, `Reach`, `MidDistance`, `FarDistance`
  - `public enum SkillTag`
    - Values: `Melee`, `Projectile`, `Spell`, `AoE`, `Buff`, `Debuff`, `DoT`, `Aura`, `Summon`, `Fire`, `Cold`, `Poison`, `Arcane`, `Holy`, `Physical`
  - `public enum ModifierTarget`
    - Values: `Damage`, `CritChance`, `CritMultiplier`, `AttackSpeed`, `ProjectileCount`, `ProjectileSpeed`, `PierceChance`, `Range`, `AoERadius`, `Duration`, `BuffDuration`, `DebuffDuration`, `DoTMaxStacks`, `DotDuration`, `CastTime`, `Cooldown`, `SummonCount`, `SummonHP`, `SummonDamage`, `AuraRange`, `MovementSpeed`, `Resist`, `Armor`, `MaxHP`, `LeechFactor`
  - `public enum ModifierOperation`
    - Values: `Add`, `Mult`, `Replace`
  - `public enum ModifierHook`
    - Values: `None`, `OnCast`, `OnHit`, `OnCrit`, `OnKill`

# Key Behavior & Side Effects
- No explicit behavior or side effects defined in the enumerations.

# Constraints & Failure Modes
- No constraints or failure modes are defined in the file.

# Example
```csharp
SkillType skill = SkillType.Spell;
SkillRange range = SkillRange.MidDistance;
```

# Unknowns
- No unknowns can be determined from this file.

