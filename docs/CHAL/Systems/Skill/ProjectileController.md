# CHAL.Systems.Skill.ProjectileController

_Automatically generated/updated from `Assets/src/Systems/Skills/ProjectileController.cs`._

```text
1) Purpose
- Defines ProjectileController, a Unity MonoBehaviour in CHAL.Systems.Skill, to manage skill-related projectiles.
- Exposes Init(SkillInstance inst, EffectReceiver src, EffectReceiver tgt, Vector3 dir, float projSpeed, float life) to initialize internal state.
- Handles per-frame movement, lifetime expiration, and on-hit processing via Unity callbacks.

```

```text
2) Public API
- Namespace/module
  - CHAL.Systems.Skill

- Types
  - public class ProjectileController : MonoBehaviour
    - Public methods
      - public void Init(SkillInstance inst, EffectReceiver src, EffectReceiver tgt, Vector3 dir, float projSpeed, float life)
        - Initializes internal fields: skill, source, target, direction (normalized), speed, lifespan
        - Logs spawn via DebugManager
        - Side effects: sets up projectile state for movement and collision handling
    - Unity lifecycle (implicit public-facing behavior in Unity context)
      - Update()
        - Calls MoveForward()
        - Calls Check_LifetimeExpiration()

```

```text
3) Key Behavior & Side Effects
- Init(...)
  - Sets: skill, source, target, direction (normalized), speed, lifespan
  - Logs: projectile spawn with skill display name, source, and target
- Update()
  - MoveForward(): advances position by direction * speed * Time.deltaTime; decreases lifespan by Time.deltaTime
  - Check_LifetimeExpiration(): if lifespan <= 0, logs expiration and destroys the projectile
- MoveForward()
  - Transform position += direction * speed * deltaTime
  - lifespan -= deltaTime
- Check_LifetimeExpiration()
  - If lifespan <= 0, logs and Destroy(gameObject)
- OnTriggerEnter(Collider other)
  - Validates potential hit via ValidateFastReturns
  - If valid targetReceiver obtained:
    - SkillExecutor.ApplyOnHit(skill, source, targetReceiver)
    - Logs hit
    - Destroy(gameObject)
- ValidateFastReturns(Collider other, out EffectReceiver targRE)
  - targRE = null
  - If other tag != "Unit" -> return
  - If other.layer == LayerMask.NameToLayer("Projectile") -> return
  - Finds unit controller: other.GetComponent<IUnitController>() ?? other.GetComponentInParent<IUnitController>()
  - If no unit controller -> return
  - targRE = unitCtrl.GetEffectReceiver(); if null -> return
  - If self-hit: ReferenceEquals(source, targRE) -> return
  - If not AllowFriendlyFire and source.Team == targRE.Team -> return

```

```text
4) Constraints & Failure Modes
- Guarded targets:
  - Only processes objects tagged "Unit" and not on the "Projectile" layer
  - Requires a valid IUnitController and non-null EffectReceiver
- Self-hit prevention:
  - Uses ReferenceEquals to avoid hitting the source
- Friendly-fire control:
  - Honors BalanceManager.Instance.Config.AllowFriendlyFire
  - Skips hits when disabled and teams match
- Lifespan:
  - Projectile expires when lifespan <= 0 and is destroyed
- Threading/async:
  - No explicit threading; uses Unity's Update loop and physics callbacks
- Performance:
  - Direction is normalized on Init; per-frame movement uses Time.deltaTime

```

```text
5) Example
- Not derivable from this file (no usage example provided)
```

```text
6) Unknowns
- Definitions and behavior of:
  - SkillInstance, EffectReceiver, IUnitController, SkillExecutor
  - DebugManager and its log levels
  - BalanceManager, Config, and the AllowFriendlyFire flag
- Exact semantics of GetEffectReceiver and Team properties
- Collision setup details in the Unity scene (IsTrigger, colliders)
- Any additional side effects of Destroy(gameObject) beyond lifecycle management
```
