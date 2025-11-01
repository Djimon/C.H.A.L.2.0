# CHAL.Systems.Unit.UnitLocator

_Automatically generated/updated from `Assets/src/Systems/Unit/UnitLocator.cs`._

```text
1) Purpose
- Scene-scoped locator component that tracks active units (heroes and enemies) in the current scene.
- Provides registration helpers (Register/Unregister) for HeroController and EnemyController.
- Exposes read-only collections of currently active heroes/enemies for consumers.

2) Public API
- Namespace: CHAL.Systems.Unit
- Types
  - public sealed class UnitLocator : MonoBehaviour
    - Public properties
      - public static UnitLocator Instance { get; private set; }
        - Scene-scoped singleton-like access to the locator instance.
      - public IReadOnlyCollection<HeroController> ActiveHeroes => _heroes;
        - Read-only view of currently registered heroes.
      - public IReadOnlyCollection<EnemyController> ActiveEnemies => _enemies;
        - Read-only view of currently registered enemies.
    - Public methods
      - public void Register(HeroController hero)
        - Adds hero to the internal set and logs the registration.
      - public void Unregister(HeroController hero)
        - Removes hero from the internal set and logs the unregistration.
      - public void Register(EnemyController enemy)
        - Adds enemy to the internal set and logs the registration.
      - public void Unregister(EnemyController enemy)
        - Removes enemy from the internal set and logs the unregistration.
      - public Transform GetNearestEnemy(Vector3 origin, UnitTeam myTeam, float sightRange)
        - Returns the Transform of the nearest visible enemy within sightRange for a given team; null if none found.
      - public Transform GetHighestHPEnemy(Vector3 origin, UnitTeam myTeam, float sightRange)
        - Returns the Transform of the enemy (within sightRange) with the highest current HP for a given team; null if none found.

3) Key Behavior & Side Effects
- Awake
  - Ensures a single in-scene instance; if another instance exists, logs an error.
  - Sets Instance = this.
  - Logs readiness for the scene-scoped locator.
- OnDestroy
  - If this is the current Instance, clears Instance.
  - Clears internal hero/enemy sets.
- Register / Unregister (HeroController)
  - Null check; adds/removes hero to/from _heroes.
  - Logs registration/unregistration events with updated counts.
- Register / Unregister (EnemyController)
  - Null check; adds/removes enemy to/from _enemies.
  - Logs registration/unregistration events with updated counts.
- GetNearestEnemy
  - Depending on myTeam, cleans up dead units in the relevant set.
  - Iterates through valid units, filters by squared distance <= sightRange^2, tracks closest one.
  - Returns Transform of the nearest valid target or null if none found.
- GetHighestHPEnemy
  - Depending on myTeam, cleans up dead units in the relevant set.
  - Iterates through valid units within sightRange^2, reads current HP (Enemy HP for enemies; hero HP via effect receiver), selects the one with the highest HP.
  - Returns Transform of the target with highest HP or null if none found.
- Helpers (implementation)
  - IsValid(HeroController) and IsValid(EnemyController)
    - Consider non-null and IsAlive.
  - CleanupDead(HashSet<HeroController>) and CleanupDead(HashSet<EnemyController>)
    - Remove dead or null entries from sets.

4) Constraints & Failure Modes
- Null checks on Register/Unregister prevent NREs when controllers are missing.
- GetNearestEnemy/GetHighestHPEnemy return null if no valid target found.
- Distance checks use squared distance (sqrMagnitude) for performance.
- Dead units are pruned via CleanupDead prior to queries.
- Single-instance enforcement is best-effort: multiple instances log an error but do not crash.
- Threading: Unity main-thread only; lack of explicit synchronization.

5) Example
```csharp
// Example usage (conceptual)
public class MyHeroBehaviour : MonoBehaviour
{
    public HeroController Controller;

    void OnEnable()
    {
        UnitLocator.Instance?.Register(Controller);
    }

    void OnDisable()
    {
        UnitLocator.Instance?.Unregister(Controller);
    }
}
```

6) Unknowns
- Details of HeroController, EnemyController, UnitTeam enum, and GetEffectReceiver behavior.
- Exact semantics of IsAlive, CurrentHP, and how EnemyInstance/CurrentHP are populated.
- DebugManager implementation and log output specifics beyond what is shown.
- Any multi-scene behavior or persistence across scenes beyond the described scene-scoped approach.
