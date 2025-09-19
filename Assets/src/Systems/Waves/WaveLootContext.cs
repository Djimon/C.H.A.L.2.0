using CHAL.Data;
using CHAL.Systems.Items;
using CHAL.Systems.Loot;
using CHAL.Systems.Loot.Models;
using System.Collections.Generic;

namespace CHAL.Systems.Wave
{
    public class WaveLootContext
    {
        public WaveComposition Wave { get; }
        public int TotalBudget { get; }      // B
        public int SpentBudget { get; set; } // U
        public int RemainingBudget => TotalBudget - SpentBudget;
        public List<LootResultEntry> Drops { get; } = new List<LootResultEntry>();
        //public UnluckyProtection Unlucky { get; }

        public WaveLootContext(WaveComposition wave)
        {
            Wave = wave;
            //Unlucky = new UnluckyProtection();

            TotalBudget = LootBudgetCalculator.CalculateBudget(
                wave.TotalSpawns, wave.TotalNormals, wave.TotalMagics,
                wave.TotalElites, wave.TotalBosses, wave.TotalChampions,
                wave.Level, wave.Difficulty
            );
        }

    }
}
