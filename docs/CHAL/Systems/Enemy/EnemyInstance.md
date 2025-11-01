# CHAL.Systems.Enemy.EnemyInstance

_Automatically generated/updated from `Assets/src/Systems/Enemy/EnemyInstance.cs`._

```csharp
```

1) Purpose
- Defines EnemyInstance class representing a live enemy instance with runtime state.
- Stores the enemy's Definition (EnemyDef) and runtime data (EnemyStruct).
- Handles damage intake and death, emitting OnDied to listeners.

2) Public API
- Namespace/module
  - CHAL.Systems.Enemy

- Types
  - public class EnemyInstance : EffectReceiver
    - Public properties
      - public EnemyDef Definition { get; private set; }
        - The enemy's static definition data.
      - public EnemyStruct StructData { get; private set; }
        - The enemy's runtime/state data.
    - Public events
      - public event Action<EnemyInstance> OnDied;
        - Invoked when the enemy dies; passes the instance to listeners.
    - Public constructors
      - public EnemyInstance(EnemyDef def, EnemyStruct data)
        - Initializes Definition and StructData; sets HP via base class fields.
    - Public methods
      - public override void TakeDamage(float amount, DamageType type)
        - Reduces CurrentHP by amount; logs damage; triggers death if HP <= 0.
    - Protected methods
      - protected override void OnDeath()
        - Logs death; invokes OnDied?.Invoke(this); placeholder for loot/XP events.

3) Key Behavior & Side Effects
- Construction
  - Sets Definition and StructData from parameters.
  - Sets MaxHP = def.baseHP; CurrentHP = MaxHP.
- Damage handling
  - TakeDamage(amount, type): CurrentHP -= amount; logs via DebugManager.Log; if CurrentHP <= 0, calls OnDeath().
- Death handling
  - OnDeath(): logs death; triggers OnDied event if any listeners exist; placeholder for loot/XP events.

4) Constraints & Failure Modes
- Armor/Resist handling not implemented (TODO in TakeDamage).
- No null checks for def/data in constructor.
- CurrentHP may become negative; death triggers only when <= 0.
- OnDied invocation uses null-conditional operator, so no crash if no listeners.
- Relies on base class members (MaxHP, CurrentHP) and external DebugManager; details not in this file.

5) Example
```csharp
// Example usage
var enemy = new CHAL.Systems.Enemy.EnemyInstance(def, data);
enemy.OnDied += e => Console.WriteLine($"Enemy {e.StructData.EnemyId} died");
enemy.TakeDamage(25f, DamageType.Physical);
```

6) Unknowns
- Definitions and members of:
  - EnemyDef
  - EnemyStruct
  - DamageType
- Base class details of EffectReceiver (where MaxHP/CurrentHP are defined and how they interact with equality/serialization).
- DebugManager implementation and logging behavior.
- Exact lifecycle/triggering of loot/XP events beyond OnDied invocation.
