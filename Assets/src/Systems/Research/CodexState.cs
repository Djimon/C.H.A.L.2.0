using CHAL.Data;
using System;
using System.Collections.Generic;

namespace CHAL.Systems.Research
{
    [Serializable]
    public sealed class NodeProgress
    {
        // Waves / Maps
        public int waves;
        public int mapsTotal;
        public Dictionary<MapDifficulty, int> mapsByDifficulty = new Dictionary<MapDifficulty, int>(); // key = (int)MapDifficulty

        // Kills
        public int killsGeneralWeighted;
        public Dictionary<string, int> killsByTagWeighted = new Dictionary<string, int>(StringComparer.Ordinal);

        // Rarities (ungewichtet, reine Stückzahlen für "Elites" / "Bosses"-Requirements)
        public int eliteCount;
        public int bossCount;
        internal int champCount;
    }

    [Serializable]
    public sealed class CodexState
    {
        public string activeNodeId;
        public HashSet<string> completedNodeIds = new HashSet<string>(StringComparer.Ordinal);
        public Dictionary<string, NodeProgress> perNodeProgress = new Dictionary<string, NodeProgress>(StringComparer.Ordinal);
    }
}
