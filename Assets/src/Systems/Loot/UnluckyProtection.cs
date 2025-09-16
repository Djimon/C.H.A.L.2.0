using CHAL.Core;
using CHAL.Data;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Loot
{
    /// <summary>
    /// Verwaltet Dry-Streaks pro Rarity und berechnet Chance-Multiplikatoren.
    /// </summary>
    public class UnluckyProtection
    {
        private readonly Dictionary<Rarity, int> _streaks = new();

        // Faktoren aus Config
        private float alphaRare => BalanceManager.Instance.Config.loot.unlucky.alphaRare;
        private float alphaEpic => BalanceManager.Instance.Config.loot.unlucky.alphaEpic;
        private float alphaLegendary => BalanceManager.Instance.Config.loot.unlucky.alphaLegendary;
        private float alphaSpecials => BalanceManager.Instance.Config.loot.unlucky.alphaSpecials;

        public UnluckyProtection()
        {
            // Streaks initialisieren
            foreach (Rarity r in System.Enum.GetValues(typeof(Rarity)))
                _streaks[r] = 0;
        }

        /// <summary>
        /// Call wenn ein Item mit Rarity gedroppt ist → Streak resetten.
        /// </summary>
        public void OnDrop(Rarity rarity)
        {
            _streaks[rarity] = 0;
        }

        /// <summary>
        /// Call wenn ein Item mit Rarity NICHT gedroppt ist → Streak erhöhen.
        /// </summary>
        public void OnFail(Rarity rarity)
        {
            _streaks[rarity]++;
        }

        /// <summary>
        /// Liefert den Multiplikator für die aktuelle Rarity.
        /// </summary>
        public float GetMultiplier(Rarity rarity)
        {
            int s = _streaks[rarity];
            return rarity switch
            {
                Rarity.Rare => 1f + alphaRare * s,
                Rarity.Epic => 1f + alphaEpic * s,
                Rarity.Legendary => 1f + alphaLegendary * s,
                Rarity.Daemonic => 1f + alphaSpecials * s,
                Rarity.Holy => 1f + alphaSpecials * s,
                Rarity.Mythic => 1f + alphaSpecials * s,
                _ => 1f // Common → kein Dry-Streak
            };
        }

        /// <summary>
        /// Debug-Info für Logs.
        /// </summary>
        public string DebugInfo()
        {
            return $"Rare={_streaks[Rarity.Rare]}, Epic={_streaks[Rarity.Epic]}, Legendary={_streaks[Rarity.Legendary]}";
        }
    }
}
