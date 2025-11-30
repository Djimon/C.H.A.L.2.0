# Assets/src/Systems/Skills/HitResolver.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/HitResolver.cs`._

# Purpose
- Defines the `HitResolver` class for resolving hit outcomes in a skill-based system.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public static class HitResolver`
    - Public methods:
      - `public static HitResult Resolve(EffectReceiver attacker, EffectReceiver defender, SkillInstance skill)`: Resolves the hit outcome based on the attacker, defender, and skill.

# Key Behavior & Side Effects
- The `Resolve` method currently always results in a hit with no critical hits, returning a default `HitResult`.
- Future implementation will include accuracy, evasion, critical hit chance, and critical hit multiplier calculations.

# Constraints & Failure Modes
- The current implementation does not account for any hit or critical hit statistics.
- Placeholder methods for accuracy, evasion, critical chance, and critical multiplier are provided but not yet implemented.

# Example
```csharp
HitResult result = HitResolver.Resolve(attacker, defender, skill);
```

# Unknowns
- The actual implementation details for accuracy, evasion, critical chance, and critical multiplier are not defined in this file.
