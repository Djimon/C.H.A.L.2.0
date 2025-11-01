# CHAL.Systems.Skill.ProjectileController

_Automatically generated/updated from `Assets/src/Systems/Skills/ProjectileController.cs`._

```text
1) Purpose
- Unity MonoBehaviour that drives a skill projectile: movement, lifetime, and collision handling.
- Initialize projectile state via Init with skill instance, source/target receivers, direction, speed, and lifespan.
- On hit or expiry, apply damage/effects via SkillExecutor and destroy the projectile; logs events.

2) Public API
- Namespace/module
  - CHAL.Systems.Skill

- Types
  - public class ProjectileController : MonoBehaviour
    - Public fields/properties
      - (none)
    - Public methods
      - public void Init(SkillInstance inst, EffectReceiver src, EffectReceiver tgt, Vector3 dir, float projSpeed, float life)

3) Key Behavior & Side Effects
- Init(...)
  - stores inst, src, tgt, direction (normalized), speed, lifespan
  - logs spawn with skill display name and sources
- Update()
  - calls MoveForward()
  - calls Check_LifetimeExpiration()
- MoveForward()
  - moves transform.position by direction * speed * deltaTime
  - subtracts deltaTime from lifespan
- Check_LifetimeExpiration()
  - if lifespan <= 0, logs expiration and Destroy(gameObject)
- OnTriggerEnter(Collider other)
  - validates potential target via ValidateFastReturns(...)
  - applies on-hit: SkillExecutor.ApplyOnHit(skill, source, targetReceiver)
  - logs hit
  - Destroy(gameObject)
- ValidateFastReturns(Collider other, out EffectReceiver targRE)
  - returns early unless other is tagged "Unit" and not on layer "Projectile"
  - finds IUnitController on collider or parent; returns if null
  - gets targRE from unit controller; returns if null
  - prevents self-hit (ReferenceEquals(source, targRE))
  - respects global friendly-fire setting; returns if disabled and teams match
  - otherwise returns with targRE set
- Side effects spread across
  - Destroy(gameObject) on expiry and on hit
  - Debug logs via DebugManager
  - Potential null-targetReceiver risk in OnTriggerEnter if validation misses

4) Constraints & Failure Modes
- Guards and filtering
  - Only process units (tag "Unit"), ignore projectiles
  - Requires unit to provide an EffectReceiver
  - Guards against self-hit
  - Honors BalanceManager.Instance.Config.AllowFriendlyFire
- Null handling
  - Many guards exist in ValidateFastReturns; however OnTriggerEnter uses targetReceiver after validation without an explicit null check
- Performance
  - Movement uses Time.deltaTime to ensure frame-rate independent updates
- Unity specifics
  - relies on physics trigger events; uses transform-based movement
  - uses Destroy to remove the projectile
  - uses LayerMask and tags for quick gating

5) Example
```csharp
using CHAL.Systems.Skill;

public class ExampleUsage : MonoBehaviour
{
    void SpawnProjectile(ProjectileController prefab, SkillInstance skill, EffectReceiver src, EffectReceiver tgt, Vector3 dir, float speed, float life)
    {
        var go = new GameObject("Projectile");
        var proj = go.AddComponent<ProjectileController>();
        proj.Init(skill, src, tgt, dir, speed, life);
        // position the projectile as needed; e.g., go.transform.position = src.Position;
    }
}
```

6) Unknowns
- Details of SkillInstance, EffectReceiver, and IUnitController implementations
- Behavior of SkillExecutor.ApplyOnHit and its side effects
- Structure and contents of DebugManager, BalanceManager, and their config fields
- Exact behavior of GetEffectReceiver() and how teams are represented
- Any additional runtime constraints not visible in this file (e.g., collision layers beyond what's checked)

```
