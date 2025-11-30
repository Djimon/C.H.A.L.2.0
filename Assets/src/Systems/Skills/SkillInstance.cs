using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Hero;
using CHAL.Systems.Unit;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

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
            skillData = data;
            ownedBy = owner;
            Recalculate();

        }

        /// <summary>
        /// Recalculates the skill's attributes based on current modifiers and data.
        /// </summary>
        public void Recalculate()
        {
            var tags = skillData.Tags ?? new List<SkillTag>();
            var mods = ownedBy != null ? ownedBy.ActiveModifiers : new ModifierStack();

            // --- Phase 2, Step 1: BaseDMG (skalar, noch ohne Typaufschlüsselung) ---
            float baseDamage = Mathf.Max(0f, skillData.BaseDamage);

            // --- Step 2: StatModifier (= DMGEffektModifier) anwenden ---
            float baseEffectiveDamage = ApplyStatScaling(baseDamage);
            float statModifier = (baseDamage > 0f)
                ? (baseEffectiveDamage / baseDamage)
                : 1f;

            // --- Step 3/4: Increased + More Layer über ModifierStack ---
            ApplyFinalModifiers(mods, tags, baseDamage, baseEffectiveDamage, statModifier);

            DebugManager.Log(
                $"Initialized Skill {skillData.SkillId} with DMG:{Damage:F1} (Base:{baseDamage:F1}, StatMod:{statModifier:F2}) CastTime:{CastTime:F2} cd:{Cooldown:F2} range:{Range:F1} dur:{Duration:F2}",
                DebugManager.EDebugLevel.Debug,
                "Skill");
        }

        private void ApplyFinalModifiers(ModifierStack mods, List<SkillTag> tags, float baseDamage, float baseEffectiveDamage, float statModifier)
        {
            // --- Damage-Layering ---

            // V1-Annahme: ModifierStack.Apply(Damage, ...) liefert uns bereits
            // (BaseEffektiveDMG + IncreasedDMG) * MoreDMG.
            // Da das System noch keinen getrennten Zugriff auf "increased" vs "more" bietet,
            // ziehen wir vorerst alles in die Increased-Schicht und setzen MoreDMG=1.
            float preMoreTotal = mods.Apply(ModifierTarget.Damage, baseEffectiveDamage, tags);

            float increasedDamage = Mathf.Max(0f, preMoreTotal - baseEffectiveDamage);

            // TODO: Sobald ModifierStack "more"-Modifier separat liefern kann,
            //       hier MoreDMG als Produkt der Faktoren berechnen
            //       und preMoreTotal entsprechend anpassen.
            float moreMult = 1.0f;

            // Top-Level-Formel aus dem Design-Dokument:
            // FinalDMG = (BaseEffektiveDMG + IncreasedDMG) * MoreDMG
            Damage = (baseEffectiveDamage + increasedDamage) * moreMult;

            // --- Restliche Runtime-Werte wie bisher über ModifierStack ---
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

        private float ApplyStatScaling(float baseDamage)
        {
            float statMod = ComputeStatModifier();
            return baseDamage * statMod;
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
