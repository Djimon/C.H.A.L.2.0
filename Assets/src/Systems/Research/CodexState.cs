using CHAL.Data;
using System;
using System.Collections.Generic;

namespace CHAL.Systems.Codex
{
    [Serializable]
    public sealed class DeedProgress
    {
        // Waves / Maps
        public int waves;
        public int mapsTotal;
        public Dictionary<MapDifficulty, int> mapsByDifficulty = new Dictionary<MapDifficulty, int>();

        // Kills
        public int killsGeneralWeighted;
        public Dictionary<string, int> killsByTagWeighted = new Dictionary<string, int>(StringComparer.Ordinal);

        // Rarities (ungewichtet, reine Stückzahlen)
        public int eliteCount;
        public int bossCount;
        public int champCount;
    }

    [Serializable]
    public sealed class CodexState
    {
        // Progress pro DeedId
        public Dictionary<string, DeedProgressState> deedProgress = new Dictionary<string, DeedProgressState>(StringComparer.Ordinal);

        // Aktive Fokus-Slots (UI/Gameplay)
        public List<ActiveFocusSlotState> activeFocusSlots = new List<ActiveFocusSlotState>();

        // Optionaler Cache (recompute on demand reicht).
        public Dictionary<string, DeedGateState> gateCache =
            new Dictionary<string, DeedGateState>(StringComparer.Ordinal);
    }
}
