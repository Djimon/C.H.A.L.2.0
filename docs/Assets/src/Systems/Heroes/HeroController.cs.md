# Assets/src/Systems/Heroes/HeroController.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `HeroController` class for managing hero behavior in the game.

## Public API
- Namespace: `CHAL.Systems.Hero`
- Types
  - `public class HeroController : MonoBehaviour, IUnitController`
    - Public fields/properties:
      - `HeroDef` (Hero definition data)
      - `target` (Current target transform)
      - `debugSocketSkills` (List of skills for debugging)
      - `IsAlive` (Boolean indicating if the hero is alive)
    - Public methods:
      - `void Init(HeroDef def)` (Initializes the hero with the given definition)
      - `void TakeDamage(float amount, DamageType type)` (Applies damage to the hero)
      - `EffectReceiver GetEffectReceiver()` (Returns the effect receiver of the hero)

## Key Behavior & Side Effects
- Registers and unregisters the hero with `UnitLocator` on enable/disable.
- Initializes the hero and builds skill instances on start.
- Updates hero status, targeting, movement, and skill cooldowns in `Update()`.
- Handles casting of skills and executes them if conditions are met.
- Triggers `OnHeroDied` event when the hero dies.

## Constraints & Failure Modes
- If `HeroDef` is null during initialization, logs a warning and does not initialize.
- If the hero is dead, methods like `TakeDamage` and skill execution are bypassed.
- Skills are only executed if the target is valid and within range.

## Example
```csharp
HeroController heroController = gameObject.AddComponent<HeroController>();
heroController.Init(heroDefinition);
```

## Unknowns
- The exact implementation details of `SkillInstance`, `HeroInstance`, and `MoveAgent`.
- The behavior of `UnitLocator` and how it manages targets.
- The full structure of `HeroDef` and its properties.
```
