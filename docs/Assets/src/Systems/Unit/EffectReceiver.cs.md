# Assets/src/Systems/Unit/EffectReceiver.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines an abstract class `EffectReceiver` for handling status effects on units in a game.

## Public API
- Namespace: None
- Types
  - `abstract class EffectReceiver`
    - Public fields/properties:
      - `float CurrentHP`: Current health points of the unit.
      - `float MaxHP`: Maximum health points of the unit.
      - `List<ActiveStatusEffect> ActiveEffects`: List of currently active status effects.
      - `ModifierStack ActiveModifiers`: Stack of active modifiers affecting the unit.
      - `UnitTeam Team`: Team affiliation of the unit.
    - Public methods:
      - `virtual void ApplyStatusEffect(ActiveStatusEffect effect)`: Applies a status effect, managing stacking and duration.
      - `virtual void RemoveEffect(ActiveStatusEffect effect)`: Removes a specified status effect.
      - `abstract void TakeDamage(float amount, DamageType type)`: Abstract method to handle damage taken by the unit.
      - `protected abstract void OnDeath()`: Abstract method to handle unit death.
      - `void UpdateEffects(float deltaTime)`: Updates the status effects based on elapsed time.

## Key Behavior & Side Effects
- `ApplyStatusEffect` manages stacking and duration of effects, differentiating between DoTs, buffs, and debuffs.
- `UpdateEffects` processes the remaining time of active effects, applies damage for DoTs, and removes effects when their duration expires.

## Constraints & Failure Modes
- `ApplyStatusEffect` does not apply effects if the provided effect is `null`.
- Effects are only added if they are new or if they are of the same type as an existing effect, which is managed through checks.
- Buffs and debuffs remove their modifiers when their duration expires.

## Example
```csharp
EffectReceiver receiver = ...; // Instantiate a concrete subclass of EffectReceiver
ActiveStatusEffect effect = new BuffStatusEffect(); // Create a new buff effect
receiver.ApplyStatusEffect(effect); // Apply the buff effect
receiver.UpdateEffects(Time.deltaTime); // Update effects in the game loop
```

## Unknowns
- The concrete implementations of `TakeDamage` and `OnDeath` are not defined in this file.
- The structure and behavior of `ActiveStatusEffect`, `DoTStatusEffect`, `BuffStatusEffect`, `DebuffStatusEffect`, and `ModifierStack` are not provided.
```
