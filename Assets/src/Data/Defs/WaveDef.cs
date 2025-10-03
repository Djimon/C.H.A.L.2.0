using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "WaveDef", menuName = "Data/Wave Definition")]
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

        [Header("Spawn Delay (0=none, 2=spawn later, 5=spawn last)")]
        public BackloadProfile backload = new BackloadProfile
        {
            alphaSpawnDelay = 0f,
            alphaNormalDelay = 0f,
            alphaMagicDelay =0f,
            alphaEliteDelay = 1.5f,
            alphaBossDelay = 2f,
            alphaChampionDelay = 5f
        };


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
                Monsters = new List<EnemyStruct>() //wird von WaveManager befüllt
            };         
        }

        
    }

    [Serializable]
    public struct BackloadProfile
    {
        [Range(0f, 5f)] public float alphaSpawnDelay;
        [Range(0f, 5f)] public float alphaNormalDelay;
        [Range(0f, 5f)] public float alphaMagicDelay;
        [Range(0f, 5f)] public float alphaEliteDelay;
        [Range(0f, 5f)] public float alphaBossDelay;
        [Range(0f, 5f)] public float alphaChampionDelay;

        public float GetSpawnDelayAlpha(EnemyRank r) => r switch
        {
            EnemyRank.Spawn => alphaSpawnDelay,
            EnemyRank.Normal => alphaNormalDelay,
            EnemyRank.Magic => alphaMagicDelay,
            EnemyRank.Elite => alphaEliteDelay,
            EnemyRank.Boss => alphaBossDelay,
            EnemyRank.Champion => alphaChampionDelay,
            _ => 0f
        };
    }
    }
