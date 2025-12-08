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
/// Gets the base damage value for the entity.
/// </summary>
/// <returns>The base damage as a float.</returns>
        public abstract float GetBaseDamage();

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
                ActiveModifiers.AddGenericModifier(buff.Modifier);
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
                ActiveModifiers.AddGenericModifier(debuff.Modifier);
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
/// <summary>
/// Applies damage to the entity based on the specified amount and damage type.
/// </summary>
/// <param name="amount">The amount of damage to apply.</param>
/// <param name="type">The type of damage being inflicted.</param>
        public virtual void TakeDamage(float amount, DamageType type)
        {
            if (amount <= 0f)
                return;

            var packet = new DamagePacket
            {
                IsHitBased = true,
                IsDot = false
            };
            packet.AddDamage(type, amount);

            TakeDamage(packet);
        }

/// <summary>
/// Applies damage to the entity based on the provided damage packet.
/// If the packet is invalid, no damage is applied.
/// </summary>
/// <param name="packet">The damage packet containing damage information.</param>
        public void TakeDamage(DamagePacket packet)
        {
            if (packet == null ||
                packet.DamagePerType == null ||
                packet.DamagePerType.Count == 0)
                return;

            float netDamage = ComputeNetDamageAfterMitigation(packet);
            ApplyNetDamageToPools(netDamage, packet);
        }

        protected virtual float ComputeNetDamageAfterMitigation(DamagePacket packet)
        {
            float armor = GetArmor();
            float elemRes = GetElementalResist(); // 0..1 = 0..100% Reduktion (V1: shared ElemRes)
            float damageTakenMult = GetDamageTakenMultiplier(packet);

            float net = 0f;

            foreach (var kv in packet.DamagePerType)
            {
                var type = kv.Key;
                var incoming = Mathf.Max(0f, kv.Value);
                float afterTypeMitigation = incoming;

                if (incoming <= 0f)
                    continue;

                // --- Physisch ---
                if (type == DamageType.Physical)
                {
                    float drPhys = ComputePhysicalDR(armor, incoming);
                    drPhys = Mathf.Clamp01(drPhys);
                    afterTypeMitigation = incoming * (1f - drPhys);
                }
                else
                {
                    // V1: alle Nicht-Physical laufen über ein gemeinsames ElemRes-Flag.
                    // Da ElemRes aktuell noch nirgendwo gesetzt wird, ist der Effekt faktisch 0.
                    float drElem = Mathf.Clamp01(elemRes);
                    afterTypeMitigation = incoming * (1f - drElem);
                }

                afterTypeMitigation *= Mathf.Max(0f, damageTakenMult);
                net += afterTypeMitigation;
            }

            return Mathf.Max(0f, net);
        }

        protected virtual float GetArmor() => 0f;

        protected virtual float GetElementalResist() => 0f;

        protected virtual float GetDamageTakenMultiplier(DamagePacket packet) => 1f;

        protected virtual float ComputePhysicalDR(float armor, float incomingDamage)
        {
            if (armor <= 0f)
                return 0f;

            const float k = 10f; // Design-Konstante, später balancen/aus Config holen.
            float dr = armor / (armor + k * Mathf.Max(1f, incomingDamage));
            return Mathf.Clamp01(dr);
        }

        protected virtual void ApplyNetDamageToPools(float netDamage, DamagePacket packet)
        {
            DebugManager.DevLog($"{this.Team} takes {netDamage} dmg.","Combat");

            if (netDamage <= 0f)
                return;

            float remaining = netDamage;

            // TODO: Barrier-System einführen (Feld/Eigenschaft auf EffectReceiver oder Subclass)
            float barrier = 0f;

            if (barrier > 0f)
            {
                float absorbed = Mathf.Min(barrier, remaining);
                barrier -= absorbed;
                remaining -= absorbed;

                // TODO: OnBarrierBroken-Event, falls barrier von >0 auf 0 fällt
            }

            if (remaining <= 0f)
                return;

            CurrentHP -= remaining;

            // (Optional: später OnDamageTaken-Event hier platzieren)

            if (CurrentHP <= 0f)
            {
                CurrentHP = 0f;
                OnDeath();
            }
        }


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

                //DoTs
                if (effect is DoTStatusEffect dot)
                {
                    dot.internalTickTimer -= deltaTime;
                    if (dot.internalTickTimer <= 0f)
                    {
                        float totalDamage = dot.DoTsettings.DamagePerTick * dot.CurrentStacks;
                        if (totalDamage > 0f)
                        {
                            var packet = new DamagePacket
                            {
                                IsHitBased = false,
                                IsDot = true
                            };
                            packet.AddDamage(dot.DoTsettings.DamageType, totalDamage);
                            TakeDamage(packet);
                        }

                        dot.internalTickTimer = dot.DoTsettings.TickInterval;
                    }
                }

                //Buffs
                if (effect is BuffStatusEffect buff && buff.RemainingTime <= 0f)
                {
                    if (buff.Modifier != null && buff.modifierApplied)
                    {
                        ActiveModifiers.RemoveGenericModifier(buff.Modifier);
                        buff.modifierApplied = false;
                    }
                }

                //Debuffs
                if (effect is DebuffStatusEffect debuff && debuff.RemainingTime <= 0f)
                {
                    if (debuff.Modifier != null && debuff.modifierApplied)
                    {
                        ActiveModifiers.RemoveGenericModifier(debuff.Modifier);
                        debuff.modifierApplied = false;
                    }
                }

                if (effect.RemainingTime <= 0)
                {
                    if (effect is BuffStatusEffect be && be.Modifier != null)
                    {
                        ActiveModifiers.RemoveGenericModifier(be.Modifier);
                    }
                    RemoveEffect(effect);
                }
            }
        }
    }
}
