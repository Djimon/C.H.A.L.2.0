using CHAL.Systems.Unit;

namespace CHAL.Systems.Skill
{

/// <summary>
/// Provides methods for calculating combat outcomes in a game.
/// This class includes functionality for resolving hit results between combatants.
/// </summary>
    public static class CombatCalculator
    {

/// <summary>
/// Resolves the hit outcome between an attacker and defender using a skill.
/// Returns the result of the hit resolution process.
/// </summary>
/// <param name="skill">The skill instance used for the attack.</param>
/// <param name="attacker">The effect receiver representing the attacker.</param>
/// <param name="defender">The effect receiver representing the defender.</param>
/// <returns>The result of the hit resolution.</returns>
        public static HitResult ResolveHit(SkillInstance skill, EffectReceiver attacker, EffectReceiver defender)
        {
            // V1: komplett delegiert an HitResolver, der intern HitContext aufbaut.
            return Resolve(attacker, defender, skill);
        }


/// <summary>
/// Computes the final damage scalar based on the skill and hit result.
/// </summary>
/// <param name="skill">The skill instance containing damage information.</param>
/// <param name="hit">The hit result indicating if the attack was successful.</param>
/// <returns>The final damage scalar as a float.</returns>
        public static float ComputeFinalDamageScalar(SkillInstance skill, HitResult hit)
        {
            if (skill == null)
                return 0f;

            return ComputeFinalDamageScalar(skill.finalSkillData, hit);
        }


        /// <summary>
        /// Computes the final damage scalar based on the skill and hit result.
        /// Returns 0 if the skill has no damage or the hit is not successful.
        /// </summary>
        /// <param name="skill">The skill instance containing damage information.</param>
        /// <param name="hit">The hit result indicating if the attack was successful.</param>
        /// <returns>The computed final damage scalar.</returns>
        public static float ComputeFinalDamageScalar(ResolvedSkill skill, HitResult hit)
        {
            if (skill == null)
                return 0f;

            if (!hit.IsHit)
                return 0f;

            // Primär: Detail-DamageEntries verwenden, falls vorhanden
            float total = 0f;

            if (skill.DamageEntries != null && skill.DamageEntries.Count > 0)
            {
                for (int i = 0; i < skill.DamageEntries.Count; i++)
                {
                    var entry = skill.DamageEntries[i];
                    if (entry.damageOutput > 0f)
                        total += entry.damageOutput;
                }
            }
            else
            {
                // Fallback: aggregierter Damage-Wert
                if (skill.Damage > 0f)
                    total = skill.Damage;
            }

            if (total <= 0f)
                return 0f;

            // CritFactor als zusätzlicher "More"-Layer auf Gesamt-Schaden
            float critMult = 1f;
            if (hit.IsHit && hit.IsCrit && hit.CritMultiplier > 0f)
                critMult = hit.CritMultiplier;

            return total * critMult;
        }


/// <summary>
/// Builds a damage packet using the final skill data and hit result.
/// </summary>
/// <param name="skill">The skill instance used to calculate damage.</param>
/// <param name="attacker">The entity dealing the damage.</param>
/// <param name="defender">The entity receiving the damage.</param>
/// <param name="hit">The result of the hit attempt.</param>
/// <returns>A DamagePacket containing the calculated damage information.</returns>
        public static DamagePacket BuildDamagePacket(SkillInstance skill, EffectReceiver attacker, EffectReceiver defender, HitResult hit)
        {
            return BuildDamagePacket(skill?.finalSkillData, attacker, defender, hit);
        }


        /// <summary>
        /// Builds a damage packet based on the provided skill and hit result.
        /// </summary>
        /// <param name="skill">The skill instance used to calculate damage.</param>
        /// <param name="attacker">The entity dealing the damage.</param>
        /// <param name="defender">The entity receiving the damage.</param>
        /// <param name="hit">The result of the hit attempt.</param>
        /// <returns>A DamagePacket containing the calculated damage information.</returns>
        public static DamagePacket BuildDamagePacket(ResolvedSkill skill, EffectReceiver attacker, EffectReceiver defender, HitResult hit)
        {
            var packet = new DamagePacket
            {
                IsHitBased = true,
                IsDot = false
            };

            if (skill == null)
                return packet;

            if (skill.DamageEntries == null || skill.DamageEntries.Count == 0)
                return packet;

            float critMult = 1f;
            if (hit.IsHit && hit.IsCrit && hit.CritMultiplier > 0f)
                critMult = hit.CritMultiplier;

            for (int i = 0; i < skill.DamageEntries.Count; i++)
            {
                var entry = skill.DamageEntries[i];
                if (entry.damageOutput <= 0f)
                    continue;

                float final = entry.damageOutput * critMult;
                packet.AddDamage(entry.DmgType, final);
            }

            return packet;
        }

        /// <summary>
        /// Resolves the outcome of an attack between an attacker and a defender using a specified skill.
        /// </summary>
        /// <param name="attacker">The entity initiating the attack.</param>
        /// <param name="defender">The entity receiving the attack.</param>
        /// <param name="skill">The skill being used in the attack.</param>
        /// <returns>The result of the hit, indicating success or failure.</returns>
        public static HitResult Resolve(EffectReceiver attacker, EffectReceiver defender, SkillInstance skill)
        {
            var ctx = new HitContext(skill, attacker, defender);

            // ------------------------------------------------------------
            // V1: Keine echten Hit/Crit-Stats vorhanden -> Verhalten:
            //  - Immer Treffer
            //  - Niemals Crit
            //  => identisch zum bisherigen System, nur jetzt explizit.
            // ------------------------------------------------------------
            //TODO: Ausbauen sobald accuracy/evasion steht
            return HitResult.CreateDefault(skill, attacker, defender);

            // ------------------------------------------------------------
            // HINWEIS:
            // Sobald du Accuracy/Evasion/Crit implementierst, kannst du
            // die Logik unterhalb aktivieren und an GetAccuracy/GetEvasion/
            // GetCritChance/GetCritMultiplier anbinden.
            // ------------------------------------------------------------

            // float accuracy = GetAccuracy(attacker, skill, ctx);   // 0..1
            // float evasion  = GetEvasion(defender, ctx);           // 0..1
            //
            // // Typische Designwerte aus dem Dokument:
            // const float accCap = 1.5f;   // 150%
            // const float evaCap = 0.75f;  // 75%
            // const float minHit = 0.05f;  //  5%
            // const float maxHit = 0.99f;  // 99%
            //
            // float accClamped = Mathf.Clamp(accuracy, 0f, accCap);
            // float evaClamped = Mathf.Clamp(evasion,  0f, evaCap);
            //
            // float hitChanceRaw = accClamped - evaClamped;
            // float hitChance = Mathf.Clamp(hitChanceRaw, minHit, maxHit);
            //
            // float rHit = Random.value;
            // bool isHit = rHit <= hitChance;
            //
            // float critChance = GetCritChance(attacker, skill, ctx); // 0..1
            // const float critCap = 0.75f;
            // critChance = Mathf.Clamp(critChance, 0f, critCap);
            //
            // float rCrit = Random.value;
            // bool isCrit = isHit && (rCrit <= critChance);
            //
            // float critMult = GetCritMultiplier(attacker, skill, ctx);
            //
            // return new HitResult(ctx, isHit, isCrit, hitChance, critChance, critMult);
        }

        // --- Hooks für spätere Implementierung (derzeit ungenutzt/placeholder) ---

        private static float GetAccuracy(EffectReceiver attacker, ResolvedSkill skill, HitContext ctx)
        {
            // TODO: Accuracy aus Attributen/Gear/Buffs lesen.
            return 1f;
        }

        private static float GetEvasion(EffectReceiver defender, HitContext ctx)
        {
            // TODO: Evasion aus Defensiv-Stats lesen.
            return 0f;
        }

        private static float GetCritChance(EffectReceiver attacker, ResolvedSkill skill, HitContext ctx)
        {
            // TODO: BaseCritChance + CritChanceMods aus Skill/Gear/Passives/Buffs.
            return 0f;
        }

        private static float GetCritMultiplier(EffectReceiver attacker, ResolvedSkill skill, HitContext ctx)
        {
            // TODO: CritMulti aus Skill/Stats holen (z.B. 1.5f für +50%).
            return 1.5f;
        }
    }
}
