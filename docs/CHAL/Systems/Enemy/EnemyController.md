# CHAL.Systems.Enemy.EnemyController

_Automatically generated/updated from `Assets/src/Systems/Enemy/EnemyController.cs`._

# Purpose
- Defines the `EnemyController` class for managing enemy behavior in the game.

# Public API
- Namespace: `CHAL.Systems.Enemy`
- Types
  - `public class EnemyController : MonoBehaviour, IUnitController`
    - Public fields/properties:
      - `public EnemyDef EnemyDef;`
      - `public EnemyStruct EnemyData { get; private set; }`
      - `public EnemyInstance EnemyInstance { get; private set; }`
      - `public Transform target;`
      - `public bool IsAlive { get; }`
    - Public methods:
      - `public void Init(EnemyStruct enemstruct);`
      - `public void TakeDamage(float amount, DamageType type);`
      - `public EffectReceiver GetEffectReceiver();`

# Key Behavior & Side Effects
- Registers and unregisters itself with `UnitLocator` on enable/disable.
- Initializes enemy data and instance in `Init()`.
- Updates enemy state in `Update()`, including targeting, movement, and skill cooldowns.
- Handles enemy death and invokes `OnEnemyKilled` event in `HandleEnemyDied()`.

# Constraints & Failure Modes
- `Init()` logs an error if the enemy definition is not found.
- `TakeDamage()` does nothing if the enemy is not alive.
- Uses `DebugManager` for logging various states and errors.

# Example
```csharp
EnemyController enemyController = gameObject.AddComponent<EnemyController>();
enemyController.Init(new EnemyStruct { EnemyId = 1 });
```

# Unknowns
- The exact implementation details of `SkillInstance`, `EnemyInstance`, and other referenced classes/interfaces.
- The behavior of `UnitLocator` and how it manages enemy targeting.

