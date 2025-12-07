# Assets/src/Systems/Skills/ProjectileController.cs

_Automatically generated/updated from `Assets/src/Systems/Skills/ProjectileController.cs`._

# Purpose
- Manages the behavior and movement of projectiles in the game.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - public class `ProjectileController` : `MonoBehaviour`
    - Public methods
      - `void Init(SkillInstance inst, EffectReceiver src, EffectReceiver tgt, Vector3 dir, float projSpeed, float life)`
        - Initializes the projectile with the specified parameters.
      - `void OnTriggerEnter(Collider other)`
        - Handles collision with other objects and applies effects on hit.

# Key Behavior & Side Effects
- The projectile moves forward based on its direction and speed, updating its position every frame in `Update()`.
- The projectile checks its lifespan and destroys itself if it expires without hitting a target.
- On collision, it validates the target and applies the skill effect if valid, then destroys itself.
- Logs messages for projectile spawning and expiration.

# Constraints & Failure Modes
- The projectile will not hit itself or friendly units if friendly fire is not allowed.
- Only objects tagged as "Unit" and not on the "Projectile" layer can be considered valid targets.
- The `ValidateFastReturns` method ensures that only valid targets are processed.

# Example
```csharp
ProjectileController projectile = gameObject.AddComponent<ProjectileController>();
projectile.Init(skillInstance, sourceReceiver, targetReceiver, direction, speed, lifespan);
```

# Unknowns
- The behavior of `SkillExecutor.ApplyOnHit` and its side effects are not defined in this file.
- The configuration of `BalanceManager.Instance.Config.AllowFriendlyFire` is not detailed here.
