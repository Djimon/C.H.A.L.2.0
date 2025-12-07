# Assets/src/Systems/Unit/EffectReceiver.cs

_Automatically generated/updated from `Assets/src/Systems/Unit/EffectReceiver.cs`._

# Purpose
- Defines an abstract class `EffectReceiver` for managing status effects and health for game entities.

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
      - `void TakeDamage(DamagePacket packet)`: Applies damage based on the provided damage packet.

# Key Behavior & Side Effects
- `ApplyStatusEffect` method handles the application of new effects and updates existing effects, including stacking and duration management.
- `RemoveEffect` method removes an effect from the active effects list.
- `TakeDamage(float amount, DamageType type)` applies damage based on the specified amount and type, and calls `TakeDamage(DamagePacket packet)`.
- `TakeDamage(DamagePacket packet)` applies damage based on the provided damage packet and checks for validity.
- `UpdateEffects` method processes the remaining time of active effects and applies damage for damage-over-time effects, removing effects that have expired.

# Constraints & Failure Modes
- `ApplyStatusEffect` will not apply a null effect.
- Effects are managed in a way that prevents double application of modifiers when refreshing existing effects.
- The `UpdateEffects` method iterates backward through the effects list to safely remove expired effects.
- `TakeDamage(DamagePacket packet)` will not apply damage if the packet is invalid or empty.

# Example
```csharp
EffectReceiver receiver = ...; // Instantiate a concrete subclass of EffectReceiver
ActiveStatusEffect effect = new BuffStatusEffect(); // Create a new status effect
receiver.ApplyStatusEffect(effect); // Apply the effect
receiver.UpdateEffects(Time.deltaTime); // Update effects in the game loop
```

# Unknowns
- The specific implementations of `TakeDamage` and `OnDeath` are not defined in this abstract class.
- The structure and behavior of `ActiveStatusEffect`, `DoTStatusEffect`, `BuffStatusEffect`, `DebuffStatusEffect`, and `ModifierStack` are not detailed in this file.
