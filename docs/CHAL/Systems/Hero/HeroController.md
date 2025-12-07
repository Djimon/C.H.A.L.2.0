# Assets/src/Systems/Heroes/HeroController.cs

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroController.cs`._

# Purpose
- Manages the hero's actions and interactions in the game.
- Implements the `IUnitController` interface for unit control functionality.

# Public API
- Namespace: `CHAL.Systems.Hero`
- Types
  - **public class** `HeroController` **: MonoBehaviour, IUnitController**
    - Public fields/properties:
      - `HeroDef`: Defines the hero's characteristics.
      - `target`: Current target (e.g., `EnemyController`).
      - `debugSocketSkills`: List for debugging socketed skills.
      - `RuntimeHeroInstance`: Retrieves the current hero instance.
      - `IsAlive`: Indicates if the hero is alive.
    - Public methods:
      - `void Init(HeroDef def, HeroProgressData progressData = null)`: Initializes the hero with the specified definition and optional progress data.
      - `void TakeDamage(float amount, DamageType type)`: Applies damage to the hero.
      - `EffectReceiver GetEffectReceiver()`: Retrieves the EffectReceiver associated with the hero instance.

# Key Behavior & Side Effects
- Registers and unregisters the hero with `UnitLocator` on enable/disable.
- Initializes the hero and builds skill instances in `Start()`.
- Updates hero status, targeting, movement, and skill cooldowns in `Update()`.
- Handles hero death and invokes `OnHeroDied` event when the hero dies.
- Logs warnings if `HeroDef` is null during initialization or if the auto-attack skill is not set.

# Constraints & Failure Modes
- If `HeroDef` is null during initialization, logs a warning and does not initialize.
- If the hero is dead, methods that affect the hero's state (e.g., `TakeDamage`) will not execute.
- Skills are only executed if the target is within range and alive.

# Example
```csharp
HeroController heroController = gameObject.AddComponent<HeroController>();
heroController.Init(heroDefinition);
heroController.TakeDamage(10, DamageType.Physical);
```

# Unknowns
- The exact implementation details of `SkillInstance`, `HeroInstance`, and `MoveAgent` are not provided in this file.
- The behavior of `UnitLocator` and how it manages enemies is not defined here.
