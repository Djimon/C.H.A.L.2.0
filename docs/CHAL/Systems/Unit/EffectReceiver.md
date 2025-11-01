# CHAL.Systems.Unit.EffectReceiver

_Automatically generated/updated from `Assets/src/Systems/Unit/EffectReceiver.cs`._

```text
Purpose
- Defines an abstract base class for units that receive and manage status effects.
- Tracks HP, active status effects, and active modifiers; provides hooks for taking damage and death.
- Provides flow for applying, removing, and updating status effects over time.

Public API
- Namespace/module
  - CHAL.Systems.Unit

- Types
  - public abstract class EffectReceiver
    - Public fields/properties
      - public float CurrentHP { get; protected set; }
        - Current hit points of the unit
      - public float MaxHP { get; protected set; }
        - Maximum hit points of the unit
      - public List<ActiveStatusEffect> ActiveEffects { get; private set; } = new();
        - List of currently active status effects
      - public ModifierStack ActiveModifiers { get; private set; } = new ModifierStack();
        - Modifier stack currently applied to the unit
      - public UnitTeam Team;
        - Team affiliation of the unit

    - Public methods
      - public virtual void ApplyStatusEffect(ActiveStatusEffect effect)
        - Applies or refreshes a status effect
        - Returns early if effect is null
        - DoT-Case: if existing DoTStatusEffect and incoming DoTStatusEffect, stacks and refreshes duration
        - Buff-Case: if existing BuffStatusEffect and incoming BuffStatusEffect, stacks and refreshes duration
        - Neuer Buff: if effect is BuffStatusEffect, activates its Modifier immediately
        - Debuff-Case: if existing DebuffStatusEffect and incoming DebuffStatusEffect, stacks and refreshes duration
        - Neuer Debuff: if effect is DebuffStatusEffect, activates its Modifier immediately
        - Adds new effect to ActiveEffects otherwise

      - public virtual void RemoveEffect(ActiveStatusEffect effect)
        - Removes effect from ActiveEffects

      - public abstract void TakeDamage(float amount, DamageType type)
        - Apply damage to the unit (implementation provided by subclass)

      - protected abstract void OnDeath()
        - Hook for death handling (implementation provided by subclass)

      - public void UpdateEffects(float deltaTime)
        - Advances effect timers and handles per-effect behavior
        - Decrements RemainingTime for all ActiveEffects
        - DoT: ticks based on internalTickTimer and applies DoT damage
        - Buff: removes modifier when RemainingTime <= 0 and modifierApplied
        - Debuff: removes modifier when RemainingTime <= 0 and modifierApplied
        - On effect expiration: removes associated modifier (if any) and removes the effect

Key Behavior & Side Effects
- ApplyStatusEffect
  - Handles stacking for DoT, Buff, and Debuff via TryAddStack(effect.source)
  - Keeps RemainingTime as the max of existing and new duration (per type)
  - Applies modifiers on initial Buff/Debuff addition
  - Adds new effects to ActiveEffects when not handled by existing-case logic

- UpdateEffects
  - DoT effects: damage applied at intervals: DamagePerTick * CurrentStacks
  - DoT: internalTickTimer decremented; on zero or below, damage applied and timer reset
  - Buffs/Debuffs: modifiers removed when RemainingTime <= 0
  - Expired buffs/debuffs cause modifier removal and final effect removal
  - Expiration path: if RemainingTime <= 0, remove associated modifier (if any) and RemoveEffect(effect)

- RemoveEffect
  - Simply removes the effect from the ActiveEffects list

- TakeDamage / OnDeath
  - Abstracts: concrete unit types must implement damage handling and death behavior

Constraints & Failure Modes
- Null handling
  - ApplyStatusEffect exits early if effect is null

- Time/Update semantics
  - UpdateEffects(deltaTime) assumes positive deltaTime; negative values are not handled explicitly

- Modifier lifecycle
  - Modifiers are added on Buff/Debuff application and removed when corresponding effect expires or is cleared

- Iteration safety
  - UpdateEffects iterates from end to start to safely remove effects during iteration

- Surface coupling
  - Depends on external types: ActiveStatusEffect, DoTStatusEffect, BuffStatusEffect, DebuffStatusEffect, ModifierStack, UnitTeam, DamageType, and effect-specific fields/members (e.g., DoTsettings, RemainingTime, modifierApplied)

Example
- Not derivable from this file alone; no runnable usage snippet provided.

Unknowns
- Definitions and structures of:
  - ActiveStatusEffect and all derived types (DoTStatusEffect, BuffStatusEffect, DebuffStatusEffect)
  - DoTStatusEffect internals (internalTickTimer, DoTsettings, DamagePerTick, TickInterval, CurrentStacks, etc.)
  - BuffStatusEffect and DebuffStatusEffect fields (Modifier, modifierApplied, RemainingTime)
  - ModifierStack.AddModifier/RemoveModifier behavior and side effects
  - Effect identifiers (EffectId) and effect.source usage
  - DamageType enum and its integration with TakeDamage
  - UnitTeam type and its semantics in game logic
```
