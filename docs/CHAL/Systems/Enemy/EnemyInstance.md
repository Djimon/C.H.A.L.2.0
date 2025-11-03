# CHAL.Systems.Enemy.EnemyInstance

_Automatically generated/updated from `Assets/src/Systems/Enemy/EnemyInstance.cs`._

```csharp
Purpose
- Defines an in-game enemy instance that tracks health and death logic.
- Holds references to its definition (EnemyDef) and runtime data (EnemyStruct).
- Extends EffectReceiver to participate in the effect system.

Public API
- Namespace: CHAL.Systems.Enemy
- Types
  - public class EnemyInstance : EffectReceiver
    - public EnemyDef Definition { get; private set; }
      - The enemy's definition data.
    - public EnemyStruct StructData { get; private set; }
      - The runtime data for this specific enemy instance.
    - public event Action<EnemyInstance> OnDied;
      - Invoked when the enemy dies; passes the instance.
    - public EnemyInstance(EnemyDef def, EnemyStruct data)
      - Constructor. Assigns Definition and StructData; initializes HP from def.baseHP.
    - public override void TakeDamage(float amount, DamageType type)
      - Reduces CurrentHP by amount; logs damage; triggers death if HP <= 0.
    - protected override void OnDeath()
      - Logs death; raises OnDied event; placeholder for loot/XP events.

Key Behavior & Side Effects
- Construction
  - Definition = def
  - StructData = data
  - MaxHP = def.baseHP
  - CurrentHP = MaxHP
- Damage handling
  - CurrentHP -= amount
  - Debug log: damage details (HP, type, amount)
  - If CurrentHP <= 0, call OnDeath()
- Death handling
  - Debug log: death message
  - OnDied?.Invoke(this)
  - Placeholder for loot/XP events

Constraints & Failure Modes
- Armor/resist handling not implemented (TODO in TakeDamage).
- No null checks for constructor parameters (def/data); passing null could NRE.
- OnDied invocation uses null-conditional operator (safe if no subscribers).
- Relies on base class members MaxHP and CurrentHP (not defined in this file).

Example
- Minimal usage pattern:
```csharp
// Example: create enemy and subscribe to death
EnemyInstance enemy = new EnemyInstance(def, data);
enemy.OnDied += e => DebugManager.Log($"Enemy died: {e.StructData.EnemyId}", DebugManager.EDebugLevel.Dev, "Combat");

// Later, damage would be applied via TakeDamage(...)
```

Unknowns
- Details of EnemyDef (especially baseHP) and EnemyStruct contents.
- Definition of EffectReceiver base class (HP fields and behavior).
- Behavior of DebugManager, DamageType, and how armor/resist would be modeled.
- Any additional lifecycle events (beyond OnDied) or loot/XP handling specifics.
```
