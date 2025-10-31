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

        public bool IsReady() //→ prüft, ob cooldownRemaining <= 0.
        {
            if(cooldownRemaining <= 0)
            {
                cooldownRemaining = 0;
                return true;
            }

            return false;
        }

        public void StartCooldown() //→ setzt cooldownRemaining = Cooldown.
        {
            cooldownRemaining = Cooldown;
        }

        public void TickCooldown(float deltaTime) //→ reduziert cooldownRemaining.
        {
            cooldownRemaining -= deltaTime;
        }

        public float GetCooldownRemaining() => Mathf.Max(0f, cooldownRemaining);


        public override string ToString()
        {
            return $"{Data.DisplayName}: Dmg={Damage}, CD={Cooldown}, Range={Range}, " +
                   $"Dur={Duration}, ProjSpeed={ProjectileSpeed}, AoE={AoERadius}";
        }
    }
}
