using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Research
{
    [Serializable]
    public sealed class NodeProgressSave
    {
        public int waves;
        public int mapsTotal;
        public List<MapRequirement> mapsByDifficulty = new List<MapRequirement>();   // (difficulty, count)
        public int killsGeneralWeighted;
        public List<KillTagCount> killsByTagWeighted = new List<KillTagCount>(); // (tag, count)
        public int eliteCount;
        public int bossCount;
        public int championCount;

    }

    [Serializable]
    public sealed class ResearchSnapshot
    {
        public int version = 1;
        public string activeNodeId;
        public List<string> completedNodeIds = new List<string>();
        public List<NodeProgressEntry> perNodeProgress = new List<NodeProgressEntry>(); // key-value als Liste (JSON-freundlich)

        [Serializable]
        public struct NodeProgressEntry
        {
            public string nodeId;
            public NodeProgressSave progress;
        }
    }
}