using CHAL.Data;
using CHAL.Systems;
using CHAL.Systems.Skill;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectReceiver
{
    public float CurrentHP { get; protected set; }
    public float MaxHP { get; protected set; }

    public List<ActiveEffect> ActiveEffects { get; private set; } = new();

    public ModifierStack ActiveModifiers { get; private set; } = new ModifierStack();

    public UnitTeam Team;

    public virtual void ApplyEffect(ActiveEffect effect)
    {
        if (effect == null) return;

        var existing = ActiveEffects.Find(e => e.EffectId == effect.EffectId);
        // — DoT-Case (bestehend): bleibt wie bei dir —
        if (existing is DoTEffect exDot && effect is DoTEffect newDot)
        {
            exDot.TryAddStack(effect.source);
            exDot.RemainingTime = Mathf.Max(exDot.RemainingTime, newDot.BaseDuration);
            return;
        }

        // — Buff-Case: analog zu DoT —
        if (existing is BuffEffect exBuff && effect is BuffEffect newBuff)
        {
            exBuff.TryAddStack(effect.source);
            exBuff.RemainingTime = Mathf.Max(exBuff.RemainingTime, newBuff.BaseDuration);
            return;
        }

        // — Neuer Effekt: beim Buff direkt Modifier aktivieren —
        if (effect is BuffEffect buff)
        {
            ActiveModifiers.AddModifier(buff.Modifier);
            // (Kein doppeltes Add mehr bei Refresh, weil wir oben in den existing-Case gehen)
        }

        // Debuff: später
        ActiveEffects.Add(effect);
    }

    public virtual void RemoveEffect(ActiveEffect effect)
    {
        ActiveEffects.Remove(effect);
    }

    public abstract void TakeDamage(float amount, DamageType type);

    protected abstract void OnDeath();

    public void UpdateEffects(float deltaTime)
    {
        for (int i = ActiveEffects.Count - 1; i >= 0; i--)
        {
            var effect = ActiveEffects[i];
            effect.RemainingTime -= deltaTime;

            if (effect is DoTEffect dot)
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
            if (effect is BuffEffect buff && buff.RemainingTime <= 0f)
            {
                if (buff.Modifier != null && buff.modifierApplied)
                {
                    ActiveModifiers.RemoveModifier(buff.Modifier);
                    buff.modifierApplied = false;
                }
            }

            //ToDo: Debuffs

            if (effect.RemainingTime <= 0)
            {
                if (effect is BuffEffect be && be.Modifier != null)
                {
                    ActiveModifiers.RemoveModifier(be.Modifier);
                }
                RemoveEffect(effect);
            }
        }
    }
}