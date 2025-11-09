# Assets/src/Systems/Enemy/EnemyInstance.cs

_Automatically generated/updated from `Assets/src/Systems/Enemy/EnemyInstance.cs`._

# Purpose
- Represents an instance of an enemy in the game, inheriting from EffectReceiver.
- Serves as a base for enemy effects and behaviors.

# Public API
- Namespace: CHAL.Systems.Enemy
- Types
  - public class EnemyInstance : EffectReceiver
    - Public fields/properties:
      - EnemyDef Definition { get; private set; }
      - EnemyStruct StructData { get; private set; }
    - Public methods:
      - EnemyInstance(EnemyDef def, EnemyStruct data)
      - override void TakeDamage(float amount, DamageType type)
        - Applies damage to the enemy and updates its health.
      - protected override void OnDeath()
        - Handles enemy death and triggers the OnDied event.

# Key Behavior & Side Effects
- Takes damage and updates health; logs damage taken.
- Triggers OnDied event when health reaches zero.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes valid EnemyDef and EnemyStruct are provided during instantiation.

# Example
```csharp
var enemy = new EnemyInstance(enemyDefinition, enemyData);
enemy.TakeDamage(10f, DamageType.Physical);
```

# Unknowns
- No information on the implementation of EffectReceiver or the structure of EnemyDef and EnemyStruct.

