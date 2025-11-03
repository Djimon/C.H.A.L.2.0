using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Hero;
using CHAL.Systems.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace CHAL.Systems.Skill
{
/// <summary>
/// Represents an instance of a skill with various attributes and effects.
/// </summary>
    public class SkillInstance
    {
        public SkillData Data { get; private set; }

        private EffectReceiver ownedBy;

        // berechnete Werte
        public float Damage { get; private set; }
        public float CastTime { get; private set; }
        public float Cooldown { get; private set; }
        public float Range { get; private set; }
        public float Duration { get; private set; }
        public float ProjectileSpeed { get; private set; }
        public int ProjectileCount { get; private set; }
        public float AoERadius { get; private set; }

        //Runtime Felder
        float cooldownRemaining = 0;


        public SkillInstance(SkillData data, EffectReceiver owner)
        {
            Data = data;
            ownedBy = owner;
            Recalculate();

        }

/// <summary>
/// Recalculates the skill's attributes based on current modifiers and data.
/// </summary>
        public void Recalculate()
        {
            var tags = Data.Tags ?? new List<SkillTag>();
            var mods = ownedBy.ActiveModifiers;

            Damage = mods.Apply(ModifierTarget.Damage, Data.BaseDamage, tags);
            CastTime = mods.Apply(ModifierTarget.CastTime, Data.CastTime, tags);
            Cooldown = mods.Apply(ModifierTarget.Cooldown, Data.Cooldown, tags);
            Range = mods.Apply(ModifierTarget.Range,BalanceManager.Instance.GetRangeValue(Data.Range), tags);
            Duration = mods.Apply(ModifierTarget.Duration, Data.Duration, tags);
            ProjectileSpeed = mods.Apply(ModifierTarget.ProjectileSpeed, Data.ProjectileSpeed, tags);
            ProjectileCount = (int)mods.Apply(ModifierTarget.ProjectileCount, Data.ProjectileCount, tags);
            AoERadius = mods.Apply(ModifierTarget.AoERadius, Data.AoERadius, tags);

            DebugManager.Log($"Initialized Skill {Data.SkillId} with DMG:{Damage} CastTime:{CastTime} cd:{Cooldown} range:{Range} dur:{Duration} ", DebugManager.EDebugLevel.Debug,"Skill");
        }

/// <summary>
/// Checks if the cooldown period has ended.
/// </summary>
/// <returns>True if cooldownRemaining is less than or equal to zero; otherwise, false.</returns>
        public bool IsReady() //â†’ prÃ¼ft, ob cooldownRemaining <= 0.
        {
            if(cooldownRemaining <= 0)
            {
                cooldownRemaining = 0;
                return true;
            }

            return false;
        }

/// <summary>
/// Starts the cooldown by setting the remaining time to the full cooldown duration.
/// </summary>
        public void StartCooldown() //â†’ setzt cooldownRemaining = Cooldown.
        {
            cooldownRemaining = Cooldown;
        }

/// <summary>
/// Reduces the remaining cooldown time by the specified delta time.
/// </summary>
/// <param name="deltaTime">The amount of time to reduce from the cooldown.</param>
        public void TickCooldown(float deltaTime) //â†’ reduziert cooldownRemaining.
        {
            cooldownRemaining -= deltaTime;
        }

/// <summary>
/// Gets the remaining cooldown time, ensuring it is not negative.
/// </summary>
/// <returns>The remaining cooldown time as a float.</returns>
        public float GetCooldownRemaining() => Mathf.Max(0f, cooldownRemaining);


/// <summary>
/// Returns a string representation of the object, including its properties.
/// </summary>
/// <returns>A formatted string with the object's data.</returns>
        public override string ToString()
        {
            return $"{Data.DisplayName}: Dmg={Damage}, CD={Cooldown}, Range={Range}, " +
                   $"Dur={Duration}, ProjSpeed={ProjectileSpeed}, AoE={AoERadius}";
        }
    }
}
