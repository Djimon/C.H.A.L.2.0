using CHAL.Data;
using System;
using System.Collections.Generic;

namespace CHAL.Data
{
    [Serializable]
    public struct EnemyInstance
    {
        public string EnemyId;          // optional: Referenz auf Monster-Def
        public int Count;               // wie oft gespawnt
        public List<string> Tags;       // z. B. {"insect","swarm"}
        public EnemyRank Rank;         // z. B. Elite, Boss ..
    }
}