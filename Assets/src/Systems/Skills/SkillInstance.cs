using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Skill
{
/// <summary>
/// Represents an instance of a skill with various attributes and effects.
/// </summary>
    public class SkillInstance
    {
        public SkillData skillData { get; private set; }

        private EffectReceiver ownedBy;

        // berechnete Werte
        public List<DamageEntry> Damage { get; private set; }
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
            skillData = data;
            ownedBy = owner;
            Recalculate();

        }

        /// <summary>
        /// Recalculates the skill's attributes based on current modifiers and data.
        /// </summary>
        public void Recalculate()
        {
            var tags = skillData.Tags ?? new List<SkillDeliveryTag>();
            //TODO: ocmplete wrapper of skill (SkillData->skillfamily->Archetype)
            //TODO: Umbauen auf TagOcntext mit -> ctx.GetModifierTags()
            

            var mods = ownedBy != null ? ownedBy.ActiveModifiers : new ModifierStack();

            // --- Phase 2, Step 1: BaseDMG  ---
            float baseDamage = Mathf.Max(0f, skillData.BaseDamage);

            // -- Step 1: Added, converted, Gain Dmg ---
            var dmgpertype = ApplyBaseDmgModfier(mods, tags, baseDamage);

            // --- Step 2: StatModifier (= DMGEffektModifier) anwenden ---
            ApplyStatScaling(dmgpertype);

            // --- Step 3/4: Increased + More Layer über ModifierStack ---
            ApplyFinalDmgModifiers(mods, tags, dmgpertype);

            // --- Mods like casttime, cooldown, ragne, etc.
            ApplyOtherModifier(mods, tags);

            var dmgList = new List<DamageEntry>(dmgpertype.Count);
            foreach (var kv in dmgpertype)
            {
                dmgList.Add(new DamageEntry(kv.Key, kv.Value));
            }

            //Most Important assign all the calculationa bove would be lost if this is misssing
            Damage = dmgList;

            float totalDmg = 0f;
            if (Damage != null)
            {
                for (int i = 0; i < Damage.Count; i++)
                    totalDmg += Damage[i].damageOutput;
            }

            DebugManager.Log(
                $"Initialized Skill {skillData.SkillId} with DMG:{totalDmg:F1} (Base:{baseDamage:F1}, CastTime:{CastTime:F2} cd:{Cooldown:F2} range:{Range:F1} dur:{Duration:F2}",
                DebugManager.EDebugLevel.Debug,
                "Skill");
        }

        private Dictionary<DamageType, float> ApplyBaseDmgModfier(ModifierStack mods, List<SkillDeliveryTag> tags, float baseDamage)
        {
            DamageType baseType = skillData.BaseDamageType; ;

            // BaseEffektiveDMG_T: wir starten mit genau einem Typ
            var baseEffectivePerType = new Dictionary<DamageType, float>
            {
                [baseType] = Mathf.Max(0f, baseDamage)
            };

            var damageMods = mods.DamageModifiers; // oder _damageMods, je nachdem wie du es benannt hast


            if (damageMods != null)
            {
                foreach (var dm in damageMods)
                {
                    if (!AppliesToTags(dm, tags))
                        continue;

                    switch (dm.Type)
                    {
                        case DamageModifierType.Added:
                            {
                                var t = dm.TargetType;
                                if (!baseEffectivePerType.TryGetValue(t, out var current))
                                    current = 0f;

                                baseEffectivePerType[t] = current + dm.Value;
                                break;
                            }

                        case DamageModifierType.Convert:
                            {
                                var s = dm.SourceType;
                                if (!baseEffectivePerType.TryGetValue(s, out var sourceCurrent))
                                    sourceCurrent = 0f;

                                if (sourceCurrent == 0f)
                                    continue;

                                var t = dm.TargetType;
                                if (!baseEffectivePerType.TryGetValue(t, out var targetCurrent))
                                    targetCurrent = 0f;

                                var conversion = sourceCurrent * dm.Value;

                                baseEffectivePerType[s] = sourceCurrent - conversion;
                                baseEffectivePerType[t] = targetCurrent + conversion;
                                // -> Später: mehrere Conversions normalisieren (Summe <= 1)
                                break;
                            }

                        case DamageModifierType.Gain:
                            {
                                var s = dm.SourceType;
                                if (!baseEffectivePerType.TryGetValue(s, out var sourceCurrent))
                                    sourceCurrent = 0f;

                                if (sourceCurrent == 0f)
                                    continue;

                                var t = dm.TargetType;
                                if (!baseEffectivePerType.TryGetValue(t, out var targetCurrent))
                                    targetCurrent = 0f;

                                var gainedDmg = sourceCurrent * dm.Value;

                                baseEffectivePerType[t] = targetCurrent + gainedDmg;
                                break;
                            }
                        default: continue;
                    }
                }
            }
           
            return baseEffectivePerType;
        }

        private void ApplyFinalDmgModifiers(ModifierStack mods, List<SkillDeliveryTag> tags, Dictionary<DamageType,float> dmgPerType)
        {

            // Vorbereitung für Increased / More
            float globalMoreMult = 1f;

            var damageMods = mods.DamageModifiers; // oder _damageMods, je nachdem wie du es benannt hast
            if (damageMods == null || dmgPerType == null || dmgPerType.Count == 0)
                return;

            var incPerType = new Dictionary<DamageType, float>();

            foreach (var dm in damageMods)
            {
                if (!AppliesToTags(dm, tags))
                    continue;

                switch (dm.Type)
                {
                    case DamageModifierType.Increased:
                        {
                            var t = dm.TargetType;

                            // Nur interessant, wenn für den Typ überhaupt Damage existiert
                            if (!dmgPerType.ContainsKey(t))
                                continue;

                            if (!incPerType.TryGetValue(t, out var current))
                                current = 0f;

                            // dm.Value = 0.2f -> +20% Increased Damage
                            incPerType[t] = current + dm.Value;
                            break;
                        }

                    case DamageModifierType.More:
                        {
                            // dm.Value = 0.2f -> 20% more = *1.2
                            globalMoreMult *= 1f + dm.Value;
                            break;
                        }
                    default: continue;
                }
            }

            //guard
            if (globalMoreMult < 0)
            {
                DebugManager.Error($"Should not happen: globalMoreMult < 0 :{globalMoreMult}");
                globalMoreMult = 0;
            }


            //Apply IncreasedDmg Mods
            var keys = new List<DamageType>(dmgPerType.Keys);
            foreach (var t in keys)
            {
                var baseEff = dmgPerType[t];
                incPerType.TryGetValue(t, out var incSum); // 0 wenn keiner

                dmgPerType[t] = baseEff * (1f + incSum) * globalMoreMult;
            }
        }

        private void ApplyOtherModifier(ModifierStack mods, List<SkillDeliveryTag> tags)
        {
            //TODO: use Tag-Context ctx -> ctx.GetModifierTags()

            // -----------------------
            // Restliche Runtime-Werte unverändert über ModifierStack.Apply
            // -----------------------
            CastTime = mods.Apply(ModifierTarget.CastTime, skillData.CastTime, tags);
            Cooldown = mods.Apply(ModifierTarget.Cooldown, skillData.Cooldown, tags);
            Range = mods.Apply(ModifierTarget.Range, BalanceManager.Instance.GetRangeValue(skillData.Range), tags);
            Duration = mods.Apply(ModifierTarget.Duration, skillData.Duration, tags);
            ProjectileSpeed = mods.Apply(ModifierTarget.ProjectileSpeed, skillData.ProjectileSpeed, tags);
            ProjectileCount = (int)mods.Apply(ModifierTarget.ProjectileCount, skillData.ProjectileCount, tags);
            AoERadius = mods.Apply(ModifierTarget.AoERadius, skillData.AoERadius, tags);

            // Optional: Wenn du später Debug-Infos für die Layer loggen willst,
            // kannst du hier BaseEffektiveDMG/Increased/More cachen.
        }

        private static bool AppliesToTags(DamageModifier mod, List<SkillDeliveryTag> tags)
        {
            if (mod.AppliesTo == null || mod.AppliesTo.Count == 0)
                return true;

            if (tags == null || tags.Count == 0)
                return false;

            for (int i = 0; i < tags.Count; i++)
            {
                if (mod.AppliesTo.Contains(tags[i]))
                    return true;
            }

            return false;
        }


        private float ComputeStatScalingMultiplier(float mainStat, float damageScalingFactor)
        {
            const float baselineStat = 20f;      // Design-Baseline, kann später aus Config kommen.
            const float perPointFactor = 0.05f;  // 5% pro Punkt über/unter baseline bei factor = 1.0

            float delta = mainStat - baselineStat;
            float multiplier = 1.0f + (delta * perPointFactor * damageScalingFactor);

            if (multiplier < 0.1f)
                multiplier = 0.1f;

            return multiplier;
        }

        private void ApplyStatScaling(Dictionary<DamageType,float> dmgpertype)
        {
            float statMod = ComputeStatModifier();
            foreach (var kv in dmgpertype)
            {
                dmgpertype[kv.Key] = kv.Value * statMod;
            }
        }

        private float ComputeStatModifier()
        {
            if (ownedBy == null)
                return 1f;

            if (ownedBy is not IAttributeHolder attributeProvider)
                return 1f;

            var mainStatType = skillData.AttributeAffinity;
            var mainStatValue = attributeProvider.GetAttributeValue(mainStatType);
            var scalingFactor = skillData.damageAttributeScalingFactor;

            return ComputeStatScalingMultiplier(mainStatValue, scalingFactor);
        }


        /// <summary>
        /// Checks if the cooldown period has ended.
        /// </summary>
        /// <returns>True if cooldownRemaining is less than or equal to zero; otherwise, false.</returns>
        public bool IsReady() //prüft, ob cooldownRemaining <= 0.
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
            return $"{skillData.DisplayName}: Dmg={Damage}, CD={Cooldown}, Range={Range}, " +
                   $"Dur={Duration}, ProjSpeed={ProjectileSpeed}, AoE={AoERadius}";
        }
    }
}
