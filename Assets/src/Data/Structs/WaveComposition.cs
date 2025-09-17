using System;
using System.Collections.Generic;
using System.Linq;

namespace CHAL.Data
{
    [Serializable]
    public struct WaveComposition
    {
        public int Level;
        public float Difficulty;

        public List<EnemyInstance> Monsters;

        // Dynamische Properties (keine Redundanz mehr)
        public int TotalSpawns => Monsters?.Where(m => m.Rank == EnemyRank.Spawn).Sum(m => m.Count) ?? 0;
        public int TotalNormals => Monsters?.Where(m => m.Rank == EnemyRank.Normal).Sum(m => m.Count) ?? 0;
        public int TotalMagics => Monsters?.Where(m => m.Rank == EnemyRank.Magic).Sum(m => m.Count) ?? 0;
        public int TotalElites => Monsters?.Where(m => m.Rank == EnemyRank.Elite).Sum(m => m.Count) ?? 0;
        public int TotalBosses => Monsters?.Where(m => m.Rank == EnemyRank.Boss).Sum(m => m.Count) ?? 0;
        public int TotalChampions => Monsters?.Where(m => m.Rank == EnemyRank.Champion).Sum(m => m.Count) ?? 0;

        public WaveComposition Clone() =>
        new WaveComposition
        {
            Level = this.Level,
            Difficulty = this.Difficulty,
            Monsters = this.Monsters
                .Select(m => new EnemyInstance
                {
                    EnemyId = m.EnemyId,
                    Rank = m.Rank,
                    Count = m.Count,
                    Tags = new List<string>(m.Tags)
                })
                .ToList()
        };
    }

    [Serializable]
    public struct EnemyInstance
    {
        public string EnemyId;          // optional: Referenz auf Monster-Def
        public int Count;               // wie oft gespawnt
        public List<string> Tags;       // z. B. {"insect","swarm"}
        public EnemyRank Rank;         // z. B. Elite, Boss ..
    }

}