using CHAL.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Codex
{
    [Serializable]
    public sealed class DeedProgressSave
    {
        public int waves;

        public int mapsTotal;
        public List<MapCountEntry> mapsByDifficulty = new List<MapCountEntry>(); // (difficulty, count)

        public int killsGeneralWeighted;
        public List<TagCountEntry> killsByTagWeighted = new List<TagCountEntry>(); // (tag, count)

        public int eliteCount;
        public int bossCount;
        public int champCount; // CHAMP (bei dir heißt es champCount im Runtime)

        [Serializable]
        public struct MapCountEntry
        {
            public MapDifficulty difficulty;
            public int count;
        }

        [Serializable]
        public struct TagCountEntry
        {
            public string tag;
            public int count;
        }
    }

    [Serializable]
    public sealed class CodexSnapshot
    {
        public int version = 2;

        // Persistenter Fortschritt pro DeedId
        public List<DeedProgressEntry> deedProgress = new List<DeedProgressEntry>();

        // Persistente ActiveFocus-Slots (DeedId pro Slot)
        public List<FocusSlotEntry> activeFocusSlots = new List<FocusSlotEntry>();

        [Serializable]
        public struct DeedProgressEntry
        {
            public string deedId;

            public float progress01;
            public bool completed;
            public bool claimed;

            public DeedProgressSave counters;
        }

        [Serializable]
        public struct FocusSlotEntry
        {
            public int slotIndex;
            public string deedId;
        }
    }
}