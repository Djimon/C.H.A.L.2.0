using CHAL.Core;
using UnityEngine;

namespace CHAL.Systems.Loot
{
    public static class LootBudgetCalculator
    {
        /// <summary>
        /// Berechnet das Loot-Budget für eine Welle.
        /// </summary>
        public static int CalculateBudget(
            int spawns=0,int normals=0, int magics=0, int elites=0, int bosses=0, int champions=0, int level=1, float difficultyMultiplier=1)
        {
            var cfg = BalanceManager.Instance.Config;    
            // 1. Raw Budget
            int B_raw = spawns * cfg.enemies.budgetPoints.spawn
                      + normals * cfg.enemies.budgetPoints.normal
                      + magics * cfg.enemies.budgetPoints.magic
                      + elites * cfg.enemies.budgetPoints.elite
                      + bosses * cfg.enemies.budgetPoints.boss
                      + champions * cfg.enemies.budgetPoints.champion;

            // 2. Level-Skalierung
            float levelFactor = 1f + cfg.loot.budget.levelFactor * (level - 1);
            float B_scaled = B_raw * levelFactor * difficultyMultiplier;

            // 3. Varianz
            float variance = Random.Range(-cfg.loot.budget.budgetVariance, cfg.loot.budget.budgetVariance);
            float B_var = B_scaled * (1f + variance);

            // 4. Runden und zurückgeben
            return Mathf.Max(0, Mathf.RoundToInt(B_var));
        }
    }
}
