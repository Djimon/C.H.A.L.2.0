# CHAL.Systems.Enemy.EnemyInstance

_Automatically generated/updated from `Assets/src/Systems/Enemy/EnemyInstance.cs`._

# Purpose
- Represents an instance of an enemy in the game, inheriting from `EffectReceiver`.
- Serves as a base for enemy effects and behaviors.

# Public API
- Namespace: `CHAL.Systems.Enemy`
- Types
  - public class `EnemyInstance` : `EffectReceiver`
    - Public fields/properties:
      - `EnemyDef Definition`: The definition of the enemy.
      - `EnemyStruct StructData`: The structural data of the enemy.
    - Public methods:
      - `EnemyInstance(EnemyDef def, EnemyStruct data)`: Constructor that initializes the enemy instance with a definition and data.
      - `override void TakeDamage(float amount, DamageType type)`: Applies damage to the enemy and updates its health.
      - `protected override void OnDeath()`: Handles the enemy's death event.

# Key Behavior & Side Effects
- `TakeDamage` method reduces the enemy's health and logs the damage taken. If health drops to zero or below, it triggers the `OnDeath` method.
- `OnDeath` method logs the death of the enemy and invokes the `OnDied` event.

# Constraints & Failure Modes
- No explicit guards against negative damage values are present.
- Assumes `CurrentHP` and `MaxHP` are properly initialized.
- The `OnDied` event may not be subscribed to, leading to no action on death.

# Example
```csharp
var enemyDef = new EnemyDef { baseHP = 100 };
var enemyData = new EnemyStruct { EnemyId = 1 };
var enemyInstance = new EnemyInstance(enemyDef, enemyData);
enemyInstance.TakeDamage(20, DamageType.Physical);
```

# Unknowns
- The implementation details of `EffectReceiver`, `EnemyDef`, `EnemyStruct`, and `DamageType` are not provided in this file.
