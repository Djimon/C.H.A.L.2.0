using CHAL.Data;
using CHAL.Systems.Loot;
using CHAL.Systems.Wave;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.Core
{
    public class WaveSimulator
    {
        private readonly LootRoller _roller;
        private readonly WaveLootContext _context;
        private readonly WaveComposition _wave;

        public WaveSimulator(LootRoller lootRoller, WaveComposition wave)
        {
            _roller = lootRoller;
            _context = new WaveLootContext(wave);
            _wave = wave;
        }

        public WaveRewards Simulate(int mapLevel, MapDifficulty difficulty)
        {
            var rewards = new WaveRewards();

            foreach (var monster in _wave.Monsters)
            {
                for (int i = 0; i < monster.Count; i++)
                {
                    // Loot
                    var drops = _roller.RollLootForMonster(monster, _context);
                    DebugManager.Log($"Kill: {monster.EnemyId} ({monster.Rank})", DebugManager.EDebugLevel.Test, "Fight");
                    foreach (var d in drops)
                    {
                        DebugManager.Log($"Dropped: {d.ItemId} (from {d.PickedTag})", DebugManager.EDebugLevel.Test, "Fight");
                        rewards.AddItem(d.ItemId, d.quantity);
                    }

                    // Gold & XP
                    int gg = _roller.RollGoldForMonster(monster, mapLevel);
                    rewards.AddCurrency("gold",gg);
                    DebugManager.Log($"Dropped: {gg} Gold", DebugManager.EDebugLevel.Test, "Fight");
                    int xp = _roller.RollXPForMonster(monster, mapLevel, difficulty, _wave.Level);
                    rewards.AddXP(xp);
                    DebugManager.Log($"Dropped: {xp} Gold", DebugManager.EDebugLevel.Test, "Fight");
                }
            }

            // Abschluss-Processing (Garantien etc.)
            _roller.FinalizeWave(_context);

            // Nach Finalize auch garantierte Drops einsammeln
            foreach (var d in _context.Drops)
                rewards.AddItem(d.ItemId, d.quantity);

            return rewards;
        }

    }
}
