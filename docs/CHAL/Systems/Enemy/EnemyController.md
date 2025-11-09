# Assets/src/Systems/Enemy/EnemyController.cs

_Automatically generated/updated from `Assets/src/Systems/Enemy/EnemyController.cs`._

# Purpose
- Manages the behavior and state of enemy units in the game.
- Implements the IUnitController interface for unit control functionality.

# Public API
- Namespace: CHAL.Systems.Enemy
- Types
  - public class EnemyController : MonoBehaviour, IUnitController
    - Public fields/properties
      - EnemyDef: Definition of the enemy.
      - EnemyData: The data structure containing enemy information.
      - EnemyInstance: The instance representing the enemy's current state.
      - target: The current target of the enemy.
      - IsAlive: Indicates if the enemy is alive.
    - Public methods
      - void Init(EnemyStruct enemstruct)
      - void TakeDamage(float amount, DamageType type)
      - EffectReceiver GetEffectReceiver()

# Key Behavior & Side Effects
- OnEnable: Registers the enemy controller with the UnitLocator.
- OnDisable: Unregisters the enemy controller from the UnitLocator.
- Start: Initializes the enemy instance if the definition is set.
- Update: Handles enemy behavior including targeting, movement, and skill casting.
- OnDestroy: Unsubscribes from the enemy's death event.
- HandleEnemyDied: Triggers an event when the enemy dies and handles cleanup.

# Constraints & Failure Modes
- If the enemy definition is not found during initialization, an error is logged.
- If the enemy is not alive, damage cannot be applied.
- Targeting logic ensures that the enemy only targets valid heroes within sight range.

# Example
```csharp
EnemyController enemyController = gameObject.AddComponent<EnemyController>();
enemyController.Init(new EnemyStruct { EnemyId = 1 });
enemyController.TakeDamage(10, DamageType.Physical);
```

# Unknowns
- The exact implementation details of the SkillInstance and MoveAgent classes.
- The behavior of the UnitLocator and how it manages enemy targeting.

