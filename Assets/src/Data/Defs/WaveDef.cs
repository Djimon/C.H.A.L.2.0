using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{

    [CreateAssetMenu(fileName = "WaveDef", menuName = "Data/WaveDef")]
    public class WaveDef : ScriptableObject
    {
        public int level = 1;
        [Range(0.5f, 2f)] public float difficulty = 1.0f;

        [Header("Monster in dieser Wave")]
        public List<EnemyEntry> monsters = new();

        [System.Serializable]
        public class EnemyEntry
        {
            public string enemyId = "MonsterX";   // für Debug oder Lookup
            public EnemyRank rank = EnemyRank.Normal;
            public int count = 1;
            public List<string> tags = new();     // z. B. { "insect", "swarm" }
        }

        public WaveComposition ToComposition()
        {
            return new WaveComposition
            {
                Level = level,
                Difficulty = difficulty,
                Monsters = monsters.ConvertAll(m => new EnemyInstance
                {
                    EnemyId = m.enemyId,
                    Rank = m.rank,
                    Count = m.count,
                    Tags = new List<string>(m.tags)
                })
            };
        }
    }
}