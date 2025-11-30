using CHAL.Systems.Unit;
using UnityEngine;

namespace CHAL.Systems.Skill
{

    // TODO:
    // - Accuracy/Evasion-Statquellen anbinden
    // - CritChance/CritMultiplier aus Skill/Stats holen
    // - Formel aus dem Combat-Dokument hier implementieren

    public static class HitResolver
    {

        public static HitResult Resolve(EffectReceiver attacker, EffectReceiver defender, SkillInstance skill)
        {
            var ctx = new HitContext(skill, attacker, defender);

            // ------------------------------------------------------------
            // V1: Keine echten Hit/Crit-Stats vorhanden -> Verhalten:
            //  - Immer Treffer
            //  - Niemals Crit
            //  => identisch zum bisherigen System, nur jetzt explizit.
            // ------------------------------------------------------------
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

        // private static float GetAccuracy(EffectReceiver attacker, SkillInstance skill, HitContext ctx)
        // {
        //     // TODO: Accuracy aus Attributen/Gear/Buffs lesen.
        //     return 1f;
        // }
        //
        // private static float GetEvasion(EffectReceiver defender, HitContext ctx)
        // {
        //     // TODO: Evasion aus Defensiv-Stats lesen.
        //     return 0f;
        // }
        //
        // private static float GetCritChance(EffectReceiver attacker, SkillInstance skill, HitContext ctx)
        // {
        //     // TODO: BaseCritChance + CritChanceMods aus Skill/Gear/Passives/Buffs.
        //     return 0f;
        // }
        //
        // private static float GetCritMultiplier(EffectReceiver attacker, SkillInstance skill, HitContext ctx)
        // {
        //     // TODO: CritMulti aus Skill/Stats holen (z.B. 1.5f für +50%).
        //     return 1.5f;
        // }
    }
}
