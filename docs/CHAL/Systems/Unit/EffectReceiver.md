# CHAL.Systems.Unit.EffectReceiver

_Automatically generated/updated from `Assets/src/Systems/Unit/EffectReceiver.cs`._

```text
1) Purpose
- Defines an abstract base class EffectReceiver for unit-like entities that manage health, status effects, and modifiers.
- Provides HP properties, team association, and containers for active status effects and active modifiers.
- Encapsulates application/removal of status effects and periodic updates (DoT ticks, buff/debuff expirations) with required overrides for damage and death behavior.

2) Public API
- Namespace/module
  - CHAL.Systems.Unit

- Types
  - public abstract class EffectReceiver
    - Public fields/properties
      - public float CurrentHP { get; protected set; }
      - public float MaxHP { get; protected set; }
      - public List<ActiveStatusEffect> ActiveEffects { get; private set; } = new();
      - public ModifierStack ActiveModifiers { get; private set; } = new ModifierStack();
      - public UnitTeam Team;
    - Public methods (signatures; side effects)
      - public virtual void ApplyStatusEffect(ActiveStatusEffect effect)
      - public virtual void RemoveEffect(ActiveStatusEffect effect)
      - public abstract void TakeDamage(float amount, DamageType type)
      - protected abstract void OnDeath()
      - public void UpdateEffects(float deltaTime)

3) Key Behavior & Side Effects
- ApplyStatusEffect(ActiveStatusEffect effect)
  - If effect is null: no-op.
  - If an existing effect with the same EffectId exists:
    - DoTStatusEffect existing and DoTStatusEffect new: exDot.TryAddStack(effect.source); exDot.RemainingTime = max(exDot.RemainingTime, newDot.BaseDuration); return;
    - BuffStatusEffect existing and BuffStatusEffect new: exBuff.TryAddStack(effect.source); exBuff.RemainingTime = max(exBuff.RemainingTime, newBuff.BaseDuration); return;
    - DebuffStatusEffect existing and DebuffStatusEffect new: exDeBuff.TryAddStack(effect.source); exDeBuff.RemainingTime = max(exDeBuff.RemainingTime, newDeBuff.BaseDuration); return;
  - New BuffStatusEffect: add its modifier to ActiveModifiers (prevents double-add on refresh due to above path).
  - New DebuffStatusEffect: add its modifier to ActiveModifiers (prevents double-add on refresh due to above path).
  - Final: add effect to ActiveEffects if not already returned from above.

- RemoveEffect(ActiveStatusEffect effect)
  - Removes effect from ActiveEffects.

- TakeDamage(float amount, DamageType type)
  - Abstract; implemented by derived types.

- OnDeath()
  - Abstract; implemented by derived types.

- UpdateEffects(float deltaTime)
  - Iterates ActiveEffects from end to start.
  - Decrements effect.RemainingTime by deltaTime.
  - DoT handling:
    - If effect is DoTStatusEffect: decrement internalTickTimer; when <= 0, deal DoT periodic damage (DoTsettings.DamagePerTick * CurrentStacks) of DoTsettings.DamageType; reset internalTickTimer to DoTsettings.TickInterval.
  - Buff handling:
    - If buff.RemainingTime <= 0: if buff.Modifier != null && buff.modifierApplied, remove modifier and mark modifierApplied = false.
  - Debuff handling:
    - If debuff.RemainingTime <= 0: if debuff.Modifier != null && debuff.modifierApplied, remove modifier and mark modifierApplied = false.
  - Expiration/removal:
    - If effect.RemainingTime <= 0: if effect is BuffStatusEffect be with Modifier, remove modifier; then RemoveEffect(effect).

4) Constraints & Failure Modes
- Defensive checks
  - ApplyStatusEffect silently ignores null effects.
- Safe collection modification
  - UpdateEffects iterates in reverse to safely remove effects during iteration.
- DoT/Buff/Debuff state dependencies
  - DoT and buff/debuff stacking and duration logic depend on specific fields (e.g., base duration, internalTickTimer, modifier references) defined in derived status effect types.
- Damage/death contract
  - TakeDamage and OnDeath are abstract; behavior defined by concrete subclasses.
- Modifiers
  - Buffs/debuffs add/remove modifiers when created/expired; ensures modifiers are not double-applied on refresh paths.
- Threading
  - No explicit threading safeguards; UpdateEffects is a synchronous, per-entity update.

5) Example
- Not derivable from this file alone (no concrete subclass or status definitions provided).

6) Unknowns
- Definitions and members of:
  - ActiveStatusEffect and its derived types (DoTStatusEffect, BuffStatusEffect, DebuffStatusEffect)
  - DoTStatusEffect fields (e.g., EffectId, source, BaseDuration, RemainingTime, internalTickTimer, DoTsettings)
  - BuffStatusEffect and DebuffStatusEffect fields (e.g., Modifier, modifierApplied, RemainingTime)
  - ActiveStatusEffect.EffectId, ActiveStatusEffect.source
  - DoTSettings structure (DamagePerTick, TickInterval, DamageType)
  - ModifierStack implementation and Modifier type
  - UnitTeam enum/class
- Exact interactions beyond this file (e.g., how HP interacts with other systems, death handling timing) are not specified here.
```
