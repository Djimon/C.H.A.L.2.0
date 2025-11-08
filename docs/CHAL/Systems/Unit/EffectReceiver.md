# CHAL.Systems.Unit.EffectReceiver

_Automatically generated/updated from `Assets/src/Systems/Unit/EffectReceiver.cs`._

# Purpose
- Defines an abstract class `EffectReceiver` for managing status effects and health in game entities.

# Public API
- Namespace: `CHAL.Systems.Unit`
- Types
  - **abstract class** `EffectReceiver`
    - Public fields/properties:
      - `float CurrentHP`: Current health points of the entity.
      - `float MaxHP`: Maximum health points of the entity.
      - `List<ActiveStatusEffect> ActiveEffects`: List of currently active status effects.
      - `ModifierStack ActiveModifiers`: Stack of active modifiers affecting the entity.
      - `UnitTeam Team`: The team to which the unit belongs.
    - Public methods:
      - `virtual void ApplyStatusEffect(ActiveStatusEffect effect)`: Applies a status effect, updating existing effects if necessary.
      - `virtual void RemoveEffect(ActiveStatusEffect effect)`: Removes a specified active status effect.
      - `abstract void TakeDamage(float amount, DamageType type)`: Applies damage to the entity based on the specified amount and type.
      - `void UpdateEffects(float deltaTime)`: Updates active effects based on elapsed time.

# Key Behavior & Side Effects
- `ApplyStatusEffect` handles the application of new effects and updates existing effects, including stacking and duration management for DoTs, buffs, and debuffs.
- `RemoveEffect` removes an effect from the active effects list.
- `TakeDamage` is an abstract method that must be implemented to define how damage is applied.
- `UpdateEffects` processes the remaining time for each effect, applies damage for DoTs, and removes effects that have expired, also managing modifiers accordingly.

# Constraints & Failure Modes
- `ApplyStatusEffect` checks for null effects before processing.
- Effects are only added if they are not already present or if they are of a different type that can stack.
- Modifiers are removed when their corresponding effects expire.

# Example
```csharp
EffectReceiver receiver = ...; // Instantiate a concrete subclass of EffectReceiver
ActiveStatusEffect effect = new BuffStatusEffect(); // Create a new status effect
receiver.ApplyStatusEffect(effect); // Apply the effect
receiver.UpdateEffects(Time.deltaTime); // Update effects in the game loop
```

# Unknowns
- The implementation details of `ActiveStatusEffect`, `DoTStatusEffect`, `BuffStatusEffect`, `DebuffStatusEffect`, and `ModifierStack` are not provided in this file.
- The behavior of `TakeDamage` is not defined as it is an abstract method.

