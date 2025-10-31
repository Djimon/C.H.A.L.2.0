# CHAL.Systems.Skill.ProjectileController

_Automatically generated/updated from `Assets/src/Systems/Skills/ProjectileController.cs`._

# Purpose
- Defines the `ProjectileController` class for managing projectile behavior in the game.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public class `ProjectileController` [extends `MonoBehaviour`]
    - Public methods:
      - `void Init(SkillInstance inst, EffectReceiver src, EffectReceiver tgt, Vector3 dir, float projSpeed, float life)`
        - Initializes the projectile with skill, source, target, direction, speed, and lifespan.
      - `void Update()`
        - Updates the projectile's position and checks for expiration.
      - `void OnTriggerEnter(Collider other)`
        - Handles collision with other objects and applies skill effects.
    - Private methods:
      - `void MoveForward()`
        - Moves the projectile forward based on its speed and direction.
      - `void Check_LifetimeExpiration()`
        - Checks if the projectile's lifespan has expired and destroys it if so.
      - `void ValidateFastReturns(Collider other, out EffectReceiver targRE)`
        - Validates collision with other objects and determines if they are valid targets.

# Key Behavior & Side Effects
- The projectile moves forward each frame until its lifespan expires.
- On collision, it applies the skill effect to the target and destroys itself.
- Logs messages for spawning, expiration, and hits for debugging purposes.

# Constraints & Failure Modes
- The projectile will not hit itself or friendly units if friendly fire is disabled.
- Validates that the collider has the "Unit" tag and is not on the "Projectile" layer.
- Requires that all units have an `IUnitController` component to retrieve the `EffectReceiver`.

# Example
```csharp
ProjectileController projectile = gameObject.AddComponent<ProjectileController>();
projectile.Init(skillInstance, sourceReceiver, targetReceiver, direction, speed, lifespan);
```

# Unknowns
- The behavior of `SkillExecutor.ApplyOnHit` and its side effects are not defined in this file.
- The structure of `SkillInstance`, `EffectReceiver`, and `IUnitController` is not detailed here.

