using CHAL.Data;
using System;
using System.Collections.Generic;

namespace CHAL.Data
{
    [Serializable]
    public struct EnemyStruct
    {
        public string EnemyId;          // optional: Referenz auf Monster-Def
        public int Count;               // wie oft gespawnt
        public List<string> bonusTags;       // z. B. {"insect","swarm"}
        public EnemyRank Rank;         // z. B. Elite, Boss ..

        //public EnemyStruct(string id, int n,List<string> tags ,EnemyRank rank) { EnemyId = id; Count = n;Tags = tags; Rank = rank; }

    }
}