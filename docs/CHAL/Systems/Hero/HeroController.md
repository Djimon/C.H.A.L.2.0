# CHAL.Systems.Hero.HeroController

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroController.cs`._

# Purpose
- Manages the hero's actions and interactions in the game.
- Implements the `IUnitController` interface for unit control functionality.

# Public API
- Namespace: `CHAL.Systems.Hero`
- Types
  - **public class HeroController : MonoBehaviour, IUnitController**
    - Public fields/properties:
      - `HeroDef HeroDef`: Definition of the hero.
      - `Transform target`: Current target (e.g., EnemyController).
      - `List<SkillData> debugSocketSkills`: Skills for debugging.
      - `bool IsAlive`: Indicates if the hero is alive.
    - Public methods:
      - `void Init(HeroDef def)`: Initializes the hero with the specified definition.
      - `void TakeDamage(float amount, DamageType type)`: Applies damage to the hero based on the specified amount and damage type.
      - `EffectReceiver GetEffectReceiver()`: Retrieves the EffectReceiver associated with the hero instance.

# Key Behavior & Side Effects
- Registers and unregisters the hero with `UnitLocator` on enable/disable.
- Initializes the hero and builds skill instances in `Start()`.
- Updates hero status, targeting, movement, and skill cooldowns in `Update()`.
- Handles hero death and invokes `OnHeroDied` event when the hero dies.

# Constraints & Failure Modes
- If `HeroDef` is null during initialization, a warning is logged.
- If the hero is not alive, damage cannot be taken.
- Skills are only executed if the target is within range and alive.

# Example
```csharp
HeroController heroController = gameObject.AddComponent<HeroController>();
heroController.Init(heroDefinition);
heroController.TakeDamage(10, DamageType.Physical);
```

# Unknowns
- The exact implementation details of `SkillInstance`, `HeroInstance`, and `MoveAgent` are not provided in this file.
- The behavior of `UnitLocator` and how it manages targets is not detailed.

