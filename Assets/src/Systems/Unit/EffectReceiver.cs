using CHAL.Data;
using CHAL.Systems;
using CHAL.Systems.Skill;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Unit
{
    public abstract class EffectReceiver
    {
        public float CurrentHP { get; protected set; }
        public float MaxHP { get; protected set; }

        public List<ActiveStatusEffect> ActiveEffects { get; private set; } = new();

        public ModifierStack ActiveModifiers { get; private set; } = new ModifierStack();

        public UnitTeam Team;

/// <summary>
/// Applies a status effect to the entity, updating existing effects if necessary.
/// </summary>
/// <param name="effect">The active status effect to apply.</param>
        public virtual void ApplyStatusEffect(ActiveStatusEffect effect)
        {
            if (effect == null) return;

            var existing = ActiveEffects.Find(e => e.EffectId == effect.EffectId);
            // — DoT-Case (bestehend): bleibt wie bei dir —
            if (existing is DoTStatusEffect exDot && effect is DoTStatusEffect newDot)
            {
                exDot.TryAddStack(effect.source);
                exDot.RemainingTime = Mathf.Max(exDot.RemainingTime, newDot.BaseDuration);
                return;
            }

            // — Buff-Case: analog zu DoT —
            if (existing is BuffStatusEffect exBuff && effect is BuffStatusEffect newBuff)
            {
                exBuff.TryAddStack(effect.source);
                exBuff.RemainingTime = Mathf.Max(exBuff.RemainingTime, newBuff.BaseDuration);
                return;
            }

            // — Neuer Effekt: beim Buff direkt Modifier aktivieren —
            if (effect is BuffStatusEffect buff)
            {
                ActiveModifiers.AddModifier(buff.Modifier);
                // (Kein doppeltes Add mehr bei Refresh, weil wir oben in den existing-Case gehen)
            }

            // Debuff
            if (existing is DebuffStatusEffect exDeBuff && effect is DebuffStatusEffect newDeBuff)
            {
                exDeBuff.TryAddStack(effect.source);
                exDeBuff.RemainingTime = Mathf.Max(exDeBuff.RemainingTime, newDeBuff.BaseDuration);
                return;
            }

            // — Neuer Effekt: beim Debuff direkt Modifier aktivieren —
            if (effect is DebuffStatusEffect debuff)
            {
                ActiveModifiers.AddModifier(debuff.Modifier);
                // (Kein doppeltes Add mehr bei Refresh, weil wir oben in den existing-Case gehen)
            }



            ActiveEffects.Add(effect);
        }

/// <summary>
/// Removes the specified active status effect from the entity.
/// </summary>
/// <param name="effect">The active status effect to remove.</param>
        public virtual void RemoveEffect(ActiveStatusEffect effect)
        {
            ActiveEffects.Remove(effect);
        }

/// <summary>
/// Applies damage to the entity based on the specified amount and type.
/// </summary>
/// <param name="amount">The amount of damage to apply.</param>
/// <param name="type">The type of damage being inflicted.</param>
        public abstract void TakeDamage(float amount, DamageType type);

        protected abstract void OnDeath();

/// <summary>
/// Updates the active effects based on the elapsed time.
/// </summary>
/// <param name="deltaTime">The time in seconds since the last update.</param>
        public void UpdateEffects(float deltaTime)
        {
            for (int i = ActiveEffects.Count - 1; i >= 0; i--)
            {
                var effect = ActiveEffects[i];
                effect.RemainingTime -= deltaTime;

                if (effect is DoTStatusEffect dot)
                {
                    dot.internalTickTimer -= deltaTime;
                    if (dot.internalTickTimer <= 0f)
                    {
                        float totalDamage = dot.DoTsettings.DamagePerTick * dot.CurrentStacks;
                        TakeDamage(totalDamage, dot.DoTsettings.DamageType);
                        dot.internalTickTimer = dot.DoTsettings.TickInterval;
                    }
                }

                //Buffs
                if (effect is BuffStatusEffect buff && buff.RemainingTime <= 0f)
                {
                    if (buff.Modifier != null && buff.modifierApplied)
                    {
                        ActiveModifiers.RemoveModifier(buff.Modifier);
                        buff.modifierApplied = false;
                    }
                }

                //Debuffs
                if (effect is DebuffStatusEffect debuff && debuff.RemainingTime <= 0f)
                {
                    if (debuff.Modifier != null && debuff.modifierApplied)
                    {
                        ActiveModifiers.RemoveModifier(debuff.Modifier);
                        debuff.modifierApplied = false;
                    }
                }

                if (effect.RemainingTime <= 0)
                {
                    if (effect is BuffStatusEffect be && be.Modifier != null)
                    {
                        ActiveModifiers.RemoveModifier(be.Modifier);
                    }
                    RemoveEffect(effect);
                }
            }
        }
    }
}
