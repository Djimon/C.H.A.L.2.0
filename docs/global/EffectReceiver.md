# global.EffectReceiver

_Automatically generated/updated from `Assets/src/Systems/Unit/EffectReceiver.cs`._

# Purpose
- Defines an abstract class `EffectReceiver` for managing status effects on units.

# Public API
- Namespace: None specified
- Types
  - abstract class EffectReceiver
    - Public fields/properties:
      - `float CurrentHP`: Current health points of the unit.
      - `float MaxHP`: Maximum health points of the unit.
      - `List<ActiveStatusEffect> ActiveEffects`: List of currently active status effects.
      - `ModifierStack ActiveModifiers`: Stack of active modifiers affecting the unit.
      - `UnitTeam Team`: The team to which the unit belongs.
    - Public methods:
      - `virtual void ApplyStatusEffect(ActiveStatusEffect effect)`: Applies a status effect, managing existing effects and modifiers.
      - `virtual void RemoveEffect(ActiveStatusEffect effect)`: Removes a specified status effect.
      - `abstract void TakeDamage(float amount, DamageType type)`: Abstract method to handle damage taken by the unit.
      - `void UpdateEffects(float deltaTime)`: Updates the status effects over time, applying damage and removing expired effects.

# Key Behavior & Side Effects
- `ApplyStatusEffect` manages stacking and duration of status effects (DoTs, buffs, debuffs).
- `RemoveEffect` removes a specified effect from the active effects list.
- `UpdateEffects` processes each active effect, applying damage for DoTs and removing expired buffs/debuffs.

# Constraints & Failure Modes
- `ApplyStatusEffect` guards against null effects.
- Effects are managed in a way that prevents double application of modifiers on refresh.
- `UpdateEffects` iterates backward through the effects list to safely remove expired effects.

# Example
```csharp
EffectReceiver receiver = ...; // Instantiate a concrete subclass
ActiveStatusEffect effect = new BuffStatusEffect(); // Create a new effect
receiver.ApplyStatusEffect(effect); // Apply the effect
receiver.UpdateEffects(Time.deltaTime); // Update effects in the game loop
```

# Unknowns
- The concrete implementation of `EffectReceiver` is not provided.
- Details of `ActiveStatusEffect`, `ModifierStack`, `DoTStatusEffect`, `BuffStatusEffect`, and `DebuffStatusEffect` are not defined in this file.

