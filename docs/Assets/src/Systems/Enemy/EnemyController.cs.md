# Assets/src/Systems/Enemy/EnemyController.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `EnemyController` class for managing enemy behavior in the game.

## Public API
- Namespace: `CHAL.Systems.Enemy`
- Types
  - `public class EnemyController : MonoBehaviour, IUnitController`
    - Public fields/properties:
      - `EnemyDef`: Definition of the enemy.
      - `EnemyData`: Current data of the enemy (read-only).
      - `EnemyInstance`: Instance representing the enemy's state (read-only).
      - `Transform target`: Current target of the enemy.
      - `bool IsAlive`: Indicates if the enemy is alive.
    - Public methods:
      - `void Init(EnemyStruct enemstruct)`: Initializes the enemy with the provided structure.
      - `void TakeDamage(float amount, DamageType type)`: Applies damage to the enemy.
      - `EffectReceiver GetEffectReceiver()`: Returns the effect receiver of the enemy.

## Key Behavior & Side Effects
- Registers and unregisters itself with `UnitLocator` on enable/disable.
- Initializes enemy data and skills in `Init`.
- Updates enemy status, targeting, movement, and skill cooldowns in `Update`.
- Handles enemy death and triggers an event when the enemy is killed.
- Applies damage and logs the action in `TakeDamage`.

## Constraints & Failure Modes
- If `EnemyDef` is null during initialization, logs an error and does not proceed.
- If no valid target is found, the enemy will not move.
- If the enemy is not alive, methods like `TakeDamage` and `Update` will not execute their main logic.

## Example
```csharp
EnemyController enemyController = gameObject.AddComponent<EnemyController>();
enemyController.Init(new EnemyStruct { EnemyId = 1 });
```

## Unknowns
- The exact implementation details of `SkillInstance`, `EnemyInstance`, and other referenced classes/interfaces.
- The behavior of the `MoveAgent` component and how it interacts with `EnemyController`.
```
