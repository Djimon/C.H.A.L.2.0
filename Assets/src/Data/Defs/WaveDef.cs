using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "WaveDef", menuName = "CHAL/Wave Definition")]
    public class WaveDef : ScriptableObject
    {
        [Header("Structure")]
        public int spawnCount;
        public int normalCount;
        public int magicCount;
        public int eliteCount;
        public int bossCount;
        public int championCount;

        [Header("Constraints")]
        public int maxTagsPerEnemy = 2;
        public int maxBosses = 1;

        // Später: zusätzliche Constraints (z. B. verbotene Tags, garantierte Tags, etc.)

        /// <summary>
        /// Baut eine WaveComposition aus diesem Template.
        /// </summary>
        public WaveComposition ToComposition(int baseLevel, MapDifficulty difficulty)
        {
            var wave = new WaveComposition
            {
                Level = baseLevel,
                Difficulty = DifficultyToFloat(difficulty),
                Monsters = new List<EnemyInstance>()
            };

            // Aktuell: fülle mit Dummy-Monstern, die nur die Ranks widerspiegeln
            AddMonsters(wave, "SpawnEnemy", spawnCount, EnemyRank.Spawn, new[] { "swarm" , "insectoid" });
            AddMonsters(wave, "NormalEnemy", normalCount, EnemyRank.Normal, new[] { "insectoid", "reptiloid", "swarm", });
            AddMonsters(wave, "MagicEnemy", magicCount, EnemyRank.Magic, new[] { "elemental", "caster" });
            AddMonsters(wave, "EliteEnemy", eliteCount, EnemyRank.Elite, new[] { "elite", "molten" });
            AddMonsters(wave, "BossEnemy", bossCount, EnemyRank.Boss, new[] { "boss", "armored", "winged" });
            AddMonsters(wave, "ChampionEnemy", championCount, EnemyRank.Champion, new[] { "champion", "undead" });

            return wave;
        }

        private void AddMonsters(WaveComposition wave, string id, int count, EnemyRank rank, string[] defaultTags)
        {
            if (count <= 0) return;
            wave.Monsters.Add(new EnemyInstance
            {
                EnemyId = id,
                Count = count,
                Rank = rank,
                Tags = new List<string>(defaultTags)
            });
        }

        private float DifficultyToFloat(MapDifficulty diff) => diff switch
        {
            MapDifficulty.Stable => 0.8f,
            MapDifficulty.Strained => 1.0f,
            MapDifficulty.Volatile => 1.2f,
            MapDifficulty.Chaos => 1.5f,
            _ => 1.0f
        };
    }
}
