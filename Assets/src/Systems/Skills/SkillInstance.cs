using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Hero;
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
        public SkillModuleDef skillModule { get; private set; }

        private EffectReceiver ownedBy;

        public ResolvedSkill finalSkillData { get; private set; }

        // berechnete Werte
        public List<DamageEntry> Damage { get; private set; }
        public float CastTime { get; private set; }
        public float Cooldown { get; private set; }
        public SkillRange Range { get; private set; }
        public float Duration { get; private set; }
        public float ProjectileSpeed { get; private set; }
        public int ProjectileCount { get; private set; }
        public float AoERadius { get; private set; }

        //Runtime Felder
        float cooldownRemaining = 0;


        public SkillInstance(SkillModuleDef data, EffectReceiver owner)
        {
            skillModule = data;
            ownedBy = owner;
            Recalculate();

        }

        private ArchetypeModuleOverrideDef GetArchetypeOverride()
        {
            // TODO: Lookup nach module.Id + ownedBy.ArchetypeId in deinem Registry/Service
            return null;
        }

        /// <summary>
        /// Recalculates the skill's attributes based on current modifiers and data.
        /// </summary>
        public void Recalculate()
        {
            var overrideDef = GetArchetypeOverride();
            var archetypeId = ownedBy != null ? (ownedBy as HeroInstance)?.Archetype.ArchetypeId : string.Empty;

            finalSkillData = SkillResolveUtility.ResolveBaseSkill(skillModule, overrideDef, archetypeId);

            DebugManager.DebugLog($"range? Module: {skillModule.Range} <-> finalSKill: {finalSkillData.Range}");

            UpdateInstance();

            var mods = ownedBy != null ? ownedBy.ActiveModifiers : new ModifierStack();

            var tags = finalSkillData.tagContext;
            var tagsStrings = new List<string>(tags.GetModifierTags());

            // --- Phase 2, Step 1: BaseDMG  --
            float baseDamage = Mathf.Max(0f, finalSkillData.Damage) * ownedBy.GetBaseDamage(); ;

            // -- Step 1: Added, converted, Gain Dmg ---
            var dmgpertype = ApplyBaseDmgModfier(mods, tagsStrings, baseDamage);

            // --- Step 2: StatModifier (= DMGEffektModifier) anwenden ---
            ApplyStatScaling(dmgpertype);

            // --- Step 3/4: Increased + More Layer über ModifierStack ---
            ApplyFinalDmgModifiers(mods, tagsStrings, dmgpertype);

            // --- Mods like casttime, cooldown, ragne, etc.
            ApplyOtherModifier(mods, tagsStrings);

            var dmgList = new List<DamageEntry>(dmgpertype.Count);
            foreach (var kv in dmgpertype)
            {
                dmgList.Add(new DamageEntry(kv.Key, kv.Value));
            }

            //Most Important assign all the calculationa bove would be lost if this is misssing
            Damage = dmgList;
            finalSkillData.AddOrReplaceDamageEntries(Damage);

            float totalDmg = 0f;
            if (Damage != null)
            {
                for (int i = 0; i < Damage.Count; i++)
                    totalDmg += Damage[i].damageOutput;
            }

            finalSkillData.UpdateRuntimeValues(
                totalDmg,
                AoERadius,          // oder AoERadius/Radius je nach Semantik
                Duration,
                Cooldown,
                CastTime,
                ProjectileSpeed,
                Range,
                AoERadius,
                ProjectileCount);

            DebugManager.Log(
                $"Initialized Skill {finalSkillData.SkillId} with DMG:{totalDmg:F1} (Base:{baseDamage:F1}, CastTime:{CastTime:F2} cd:{Cooldown:F2} range:{Range:F} dur:{Duration:F2}",
                DebugManager.EDebugLevel.Debug,
                "Skill");
        }

        private void UpdateInstance()
        {
            AoERadius = finalSkillData.AoERadius;          // oder AoERadius/Radius je nach Semantik
            Duration = finalSkillData.Duration;
            Cooldown = finalSkillData.Cooldown;
            CastTime = finalSkillData.CastTime;
            ProjectileSpeed = finalSkillData.ProjectileSpeed;
            ProjectileCount = finalSkillData.ProjectileCount;
            Range = finalSkillData.Range;
        }

        private Dictionary<DamageType, float> ApplyBaseDmgModfier(ModifierStack mods, List<string> tags, float baseDamage)
        {
            DamageType baseType = skillModule.BaseDamageType; ;

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

        private void ApplyFinalDmgModifiers(ModifierStack mods, List<string> tags, Dictionary<DamageType,float> dmgPerType)
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

            //Crit is handled by the CombatCalculator
        }

        private void ApplyOtherModifier(ModifierStack mods, List<string> tags)
        {
            // -----------------------
            // Restliche Runtime-Werte unverändert über ModifierStack.Apply
            // -----------------------
            CastTime = mods.Apply(ModifierTarget.CastTime, finalSkillData.CastTime, tags);
            Cooldown = mods.Apply(ModifierTarget.Cooldown, finalSkillData.Cooldown, tags);
            //TODO: ocncept hoe to convert from float to next higher Range
            //Range = mods.Apply(ModifierTarget.Range, BalanceManager.Instance.GetRangeValue(finalSkillData.Range), tags);
            Duration = mods.Apply(ModifierTarget.Duration, finalSkillData.Duration, tags);
            ProjectileSpeed = mods.Apply(ModifierTarget.ProjectileSpeed, finalSkillData.ProjectileSpeed, tags);
            ProjectileCount = (int)mods.Apply(ModifierTarget.ProjectileCount, finalSkillData.ProjectileCount, tags);
            AoERadius = mods.Apply(ModifierTarget.AoERadius, finalSkillData.AoERadius, tags);
            //TODO weitere properties anpassen
            //CastTime
            /*
                AttackSpeed,
                PierceChance,
                DoTMaxStacks,
                DoTDuration,
                DotDamage,
                TicksPerSecond,
                SummonCount,
                SummonHP,
                SummonDamage,
                AuraRange,
                MovementSpeed,
                HealAmount,
                StackLimit
             */


            // Optional: Wenn du später Debug-Infos für die Layer loggen willst,
            // kannst du hier BaseEffektiveDMG/Increased/More cachen.
        }

        private static bool AppliesToTags(DamageModifier mod, List<string> tags)
        {
            if (mod.AppliesToTags == null || mod.AppliesToTags.Count == 0)
                return true;


            if (tags == null || tags.Count == 0)
                return false;

            foreach (var tag in tags)
            {
                if (mod.AppliesToTags.Contains(tag))
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

            var keys = new List<DamageType>(dmgpertype.Keys);

            foreach (var t in keys)
            {
                var baseVal = dmgpertype[t];
                dmgpertype[t] = baseVal * statMod;
            }
        }

        private float ComputeStatModifier()
        {
            if (ownedBy == null)
                return 1f;

            if (ownedBy is not IAttributeHolder attributeProvider)
                return 1f;

            var mainStatType = skillModule.AttributeAffinity;
            var mainStatValue = attributeProvider.GetAttributeValue(mainStatType);
            var scalingFactor = skillModule.damageAttributeScalingFactor;

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
            return $"{skillModule.DisplayName}: Dmg={Damage}, CD={Cooldown}, Range={Range}, " +
                   $"Dur={Duration}, ProjSpeed={ProjectileSpeed}, AoE={AoERadius}";
        }
    }
}
