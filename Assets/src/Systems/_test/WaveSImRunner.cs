using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Loot;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class WaveSimRunner
{
    /// <summary>
    /// Führt die Wave-Simulation N-mal aus und gibt Durchschnittswerte zurück.
    /// </summary>
    public static void RunStats(
        LootRoller roller,
        WaveComposition wave,
        int mapLevel,
        MapDifficulty difficulty,
        int runs = 1000)
    {
        var totalGold = 0;
        var totalXp = 0;
        var itemCounts = new Dictionary<string, int>();

        for (int i = 0; i < runs; i++)
        {
            var sim = new WaveSimulator(roller, wave);
            var rewards = sim.Simulate(mapLevel, difficulty);

            totalGold += rewards.Currencies["gold"];
            totalXp += rewards.XP;

            foreach (var kv in rewards.Items)
            {
                if (!itemCounts.ContainsKey(kv.Key))
                    itemCounts[kv.Key] = 0;
                itemCounts[kv.Key] += kv.Value;
            }
        }

        // Ergebnisse ausgeben
        Debug.Log($"--- WaveSimStats ({runs} runs) ---");
        Debug.Log($"Avg Gold: {totalGold / (float)runs}");
        Debug.Log($"Avg XP:   {totalXp / (float)runs}");

        foreach (var kv in itemCounts)
        {
            float avg = kv.Value / (float)runs;
            Debug.Log($"Avg Item {kv.Key}: {avg:F2}");
        }
    }
}


