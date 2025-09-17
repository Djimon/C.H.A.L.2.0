using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHAL.Data;
using CHAL.Systems.Loot;
using System.Linq;
using CHAL.Systems.Wave;

namespace CHAL.Core
{
    public class WaveSimulator
    {
        private readonly LootRoller _lootRoller;
        private readonly WaveLootContext _context;

        public WaveSimulator(LootRoller lootRoller, WaveComposition wave)
        {
            _lootRoller = lootRoller;
            _context = new WaveLootContext(wave);
        }

        public void RunWave()
        {
            DebugManager.Info("=== Wave started ===");

            // Gehe alle Monster der Wave durch
            foreach (var monster in _context.Wave.Monsters)
            {
                for (int i = 0; i < monster.Count; i++)
                {
                    SimulateKill(monster);
                }
            }

            _lootRoller.FinalizeWave(_context);

            // Debug: Ausgabe aller Drops
            foreach (var g in _context.Drops.GroupBy(d => d.ItemId))
            {
                DebugManager.Log($" - {g.Count()}x {g.Key}",DebugManager.EDebugLevel.Test,"Loot");
            }
        }

        private void SimulateKill(EnemyInstance monster)
        {
            DebugManager.Log($"Kill: {monster.EnemyId} ({monster.Rank})",DebugManager.EDebugLevel.Test,"Fight");
            var drops = _lootRoller.RollLootForMonster(monster, _context);

            foreach (var d in drops)
            {
                DebugManager.Log($"Dropped: {d.ItemId} (from {d.PickedTag})",DebugManager.EDebugLevel.Test,"Fight");
            }
        }
    }
}
