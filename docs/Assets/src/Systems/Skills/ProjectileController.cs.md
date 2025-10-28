# Assets/src/Systems/Skills/ProjectileController.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `ProjectileController` class for managing projectile behavior in the game.

# Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public class ProjectileController : MonoBehaviour`
    - Public methods:
      - `void Init(SkillInstance inst, EffectReceiver src, EffectReceiver tgt, Vector3 dir, float projSpeed, float life)`
        - Initializes the projectile with skill instance, source, target, direction, speed, and lifespan.
      - `private void Update()`
        - Updates the projectile's position and checks for lifespan expiration.
      - `private void OnTriggerEnter(Collider other)`
        - Handles collision with other objects and applies skill effects.
      - `private void ValidateFastReturns(Collider other, out EffectReceiver targRE)`
        - Validates the collider and retrieves the target `EffectReceiver`.

# Key Behavior & Side Effects
- Moves the projectile forward based on its direction and speed in the `Update` method.
- Checks for lifespan expiration and destroys the projectile if it exceeds its lifespan.
- On collision, applies skill effects to the target and destroys the projectile.

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
- The implementation details of `EffectReceiver` and `IUnitController` are not provided.
```
