# CHAL.Systems.Skill.ProjectileController

_Automatically generated/updated from `Assets/src/Systems/Skills/ProjectileController.cs`._

# Purpose
- Manages the behavior and movement of projectiles in the game.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public class ProjectileController : MonoBehaviour`
    - Public methods:
      - `public void Init(SkillInstance inst, EffectReceiver src, EffectReceiver tgt, Vector3 dir, float projSpeed, float life)`
        - Initializes the projectile with the specified parameters.
    - Lifecycle methods:
      - `private void Update()`
        - Updates the projectile's position and checks for expiration.
      - `private void OnTriggerEnter(Collider other)`
        - Handles collision with other objects.

# Key Behavior & Side Effects
- The projectile moves forward based on its speed and direction.
- The projectile checks its lifespan and destroys itself if it expires.
- On collision, it applies effects to the target and destroys itself.

# Constraints & Failure Modes
- The projectile will not hit itself or friendly units if friendly fire is not allowed.
- Only colliders tagged as "Unit" are considered valid targets.
- The projectile is destroyed upon expiration or upon hitting a valid target.

# Example
```csharp
ProjectileController projectile = gameObject.AddComponent<ProjectileController>();
projectile.Init(skillInstance, sourceReceiver, targetReceiver, direction, speed, lifespan);
```

# Unknowns
- None.

