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
        public int maxElites = 2;
        public int maxBosses = 1;
        public int maxChampions = 0;

        // Später: zusätzliche Constraints (z. B. verbotene Tags, garantierte Tags, etc.)

        /// <summary>
        /// Baut eine WaveComposition aus diesem Template.
        /// </summary>
        public WaveComposition ToComposition(int baseLevel, MapDifficulty difficulty)
        {
            return new WaveComposition
            {
                Level = baseLevel,
                Difficulty = difficulty,
                Monsters = new List<EnemyInstance>() //wird von WaveManager befüllt
            };         
        }
    }
}
