using CHAL.Data;
using CHAL.Systems.Skill;
using System.Collections.Generic;

public abstract class EffectReceiver
{
    public float CurrentHP { get; protected set; }
    public float MaxHP { get; protected set; }

    public List<ActiveEffect> ActiveEffects { get; private set; } = new();

    public virtual void ApplyEffect(ActiveEffect effect)
    {
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

            //ToDo: Buffs

            //ToDo: Debuffs

            if (effect.RemainingTime <= 0)
            {
                RemoveEffect(effect);
            }
        }
    }
}