using CHAL.Core;
using CHAL.Data;
using UnityEngine;

namespace CHAL.Systems.Loot
{
    public static class LootBudgetModulator
    {
        /// <summary>
        /// Berechnet den Multiplikator für die Dropchance basierend auf Budget-Overflow.
        /// </summary>
        public static float GetModifier(int U, int v_i, int B, Rarity rarity)
        {
            var cfg = BalanceManager.Instance.Config.loot.budget;
            var floors = BalanceManager.Instance.Config.loot.floors;
            float beta = cfg.beta;

            // passt ins Budget ? kein Effekt
            if (U + v_i <= B)
                return 1f;

            // Overflow berechnen
            float overflow = (float)(U + v_i - B) / B;
            float expVal = Mathf.Exp(-beta * overflow);

            // Floor nach Rarity
            float floor = rarity switch
            {
                Rarity.Rare => floors.rare,
                Rarity.Epic => floors.epic,
                Rarity.Legendary => floors.legendary,
                _ => 0.0f // Common brauchen keinen Floor
            };

            return Mathf.Max(floor, expVal);
        }
    }
}
