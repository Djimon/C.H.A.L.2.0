# Assets/src/Systems/Skills/SkillImpactBase.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines an abstract base class for skill impact effects in a game.

## Public API
- Namespace: `CHAL.Systems.Skill`
- Types
  - `public abstract class SkillImpactBase : ScriptableObject`
    - Public fields/properties:
      - `public string EffectId`: Optional unique identifier for debugging or balancing.
    - Public methods:
      - `public abstract void Apply(SkillInstance skill, EffectReceiver source, EffectReceiver target)`: Executes the effect from source to target.

## Key Behavior & Side Effects
- The `Apply` method is intended to be overridden in derived classes to implement specific skill effects.

## Constraints & Failure Modes
- No explicit guards or null handling are defined in this file.

## Example
- Not applicable.

## Unknowns
- Specific implementations of `SkillImpactBase` and their behaviors cannot be determined from this file.
```
