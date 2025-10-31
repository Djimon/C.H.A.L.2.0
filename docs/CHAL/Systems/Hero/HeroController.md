# CHAL.Systems.Hero.HeroController

_Automatically generated/updated from `Assets/src/Systems/Heroes/HeroController.cs`._

# HeroController.cs

## Purpose
- Defines the `HeroController` class for managing hero behavior in the game.
- Implements the `IUnitController` interface to control unit actions and interactions.

## Public API
- Namespace: `CHAL.Systems.Hero`
- Types
  - `public class HeroController : MonoBehaviour, IUnitController`
    - Public fields/properties:
      - `HeroDef HeroDef` - Definition of the hero.
      - `Transform target` - Current target (e.g., an enemy).
      - `bool IsAlive` - Indicates if the hero is alive.
      - `List<SkillData> debugSocketSkills` - List of skills for debugging.
    - Public methods:
      - `void Init(HeroDef def)` - Initializes the hero with the given definition.
      - `void TakeDamage(float amount, DamageType type)` - Applies damage to the hero.
      - `EffectReceiver GetEffectReceiver()` - Returns the effect receiver for the hero.

## Key Behavior & Side Effects
- Registers and unregisters the hero with the `UnitLocator` on enable/disable.
- Initializes the hero and builds skill instances in the `Start` method.
- Updates hero status, targeting, movement, and skill cooldowns in the `Update` method.
- Handles hero death and invokes the `OnHeroDied` event when the hero dies.

## Constraints & Failure Modes
- If `HeroDef` is null during initialization, a warning is logged.
- If the hero is dead, damage cannot be applied.
- Skills are only executed if the target is valid and within range.

## Example
```csharp
HeroController heroController = gameObject.AddComponent<HeroController>();
heroController.Init(heroDefinition);
heroController.TakeDamage(10, DamageType.Physical);
```

## Unknowns
- The exact implementation details of `SkillInstance`, `HeroInstance`, and `MoveAgent` are not provided in this file.
- The behavior of `UnitLocator` and how it manages enemies is not defined here.

