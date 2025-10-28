# Assets/src/Systems/Enemy/EnemyInstance.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `EnemyInstance` class representing an enemy in the game.
- Provides functionality for handling damage and death events.

# Public API
- Namespace: `CHAL.Systems.Enemy`
- Types
  - `public class EnemyInstance : EffectReceiver`
    - Public fields/properties:
      - `EnemyDef Definition { get; private set; }` - Holds the enemy's definition data.
      - `EnemyStruct StructData { get; private set; }` - Holds the enemy's structural data.
    - Public methods:
      - `public EnemyInstance(EnemyDef def, EnemyStruct data)` - Constructor initializing the enemy with definition and data.
      - `public override void TakeDamage(float amount, DamageType type)` - Applies damage to the enemy; triggers death if HP falls to zero.
      - `protected override void OnDeath()` - Handles the enemy's death; triggers the `OnDied` event.

# Key Behavior & Side Effects
- `TakeDamage` method reduces `CurrentHP` by the specified amount and logs the damage taken.
- If `CurrentHP` drops to zero or below, `OnDeath` is called, logging the death and invoking the `OnDied` event.

# Constraints & Failure Modes
- No explicit guards against negative damage values.
- Assumes `EnemyDef` and `EnemyStruct` are valid and properly initialized.
- No threading or async handling noted.

# Example
```csharp
var enemyDef = new EnemyDef { baseHP = 100 };
var enemyStruct = new EnemyStruct { EnemyId = 1 };
var enemyInstance = new EnemyInstance(enemyDef, enemyStruct);
enemyInstance.TakeDamage(50, DamageType.Physical);
```

# Unknowns
- The structure and properties of `EnemyDef` and `EnemyStruct` are not defined in this file.
- The behavior of `EffectReceiver` and its methods is not detailed here.
```
