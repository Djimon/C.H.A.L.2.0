# Assets/src/Systems/Skills/HitContext.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/HitContext.cs`._

# Purpose
- Defines the `HitContext` and `HitResult` structs for handling skill hit mechanics in a game.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `readonly struct HitContext`
    - `public readonly SkillInstance Skill` - The skill being used.
    - `public readonly EffectReceiver Attacker` - The entity initiating the attack.
    - `public readonly EffectReceiver Defender` - The entity receiving the attack.
    - `public readonly IReadOnlyList<SkillTag> Tags` - Tags associated with the skill.
    - `public readonly bool IsAttack` - Indicates if the skill is an attack.
    - `public readonly bool IsSpell` - Indicates if the skill is a spell.
    - `public readonly bool IsProjectile` - Indicates if the skill is a projectile.
    - `public readonly bool IsAoE` - Indicates if the skill is an area of effect.
    - `HitContext(SkillInstance skill, EffectReceiver attacker, EffectReceiver defender)` - Constructor initializing the context.
  
  - `readonly struct HitResult`
    - `public readonly HitContext Context` - The context of the hit.
    - `public readonly bool IsHit` - Indicates if the hit was successful.
    - `public readonly bool IsCrit` - Indicates if the hit was a critical hit.
    - `public readonly float HitChance` - The chance of hitting.
    - `public readonly float CritChance` - The chance of a critical hit.
    - `public readonly float CritMultiplier` - The multiplier for critical damage.
    - `HitResult(HitContext context, bool isHit, bool isCrit, float hitChance, float critChance, float critMultiplier)` - Constructor initializing the hit result.
    - `static HitResult CreateDefault(SkillInstance skill, EffectReceiver attacker, EffectReceiver defender)` - Creates a default hit result.

# Key Behavior & Side Effects
- `HitContext` constructor initializes tags and determines skill type properties (IsAttack, IsSpell, IsProjectile, IsAoE) based on the provided `SkillInstance`.
- `HitResult.CreateDefault` method creates a default hit result with a guaranteed hit and no critical hit.

# Constraints & Failure Modes
- If `Tags` is null, it is initialized to an empty array.
- The skill type defaults to `SkillType.Melee` if not specified.

# Example
```csharp
var skillInstance = new SkillInstance(); // Assume this is initialized
var attacker = new EffectReceiver(); // Assume this is initialized
var defender = new EffectReceiver(); // Assume this is initialized

var hitContext = new HitContext(skillInstance, attacker, defender);
var hitResult = HitResult.CreateDefault(skillInstance, attacker, defender);
```

# Unknowns
- The implementation details of `SkillInstance`, `EffectReceiver`, and `SkillTag` are not provided in this file.
