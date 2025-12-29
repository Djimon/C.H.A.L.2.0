using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "HeroXPConfig",menuName = "Config/Hero XP Config")]
/// <summary>
/// Holds configuration settings for hero experience points.
/// This includes level cap and experience requirements.
/// </summary>
    public class HeroXPConfig : ScriptableObject
    {
        [Min(1)]
        public int LevelCap = 100;

        [Min(1)]
        public int baseXpPerStandardWave = 100;

        public int[] wavesRequiredPerLevel = new int[100];

        public AnimationCurve wavesCurve = new AnimationCurve();

        /// <summary>
        /// Calculates the required experience points for a given level.
        /// Returns 0 if the level is invalid or exceeds the cap.
        /// </summary>
        /// <param name="currentLevel">The current level to calculate XP for.</param>
        /// <returns>The required experience points for the specified level.</returns>
        public int GetRequiredXPForLevel(int currentLevel)
        {
            // Kein XP mehr über dem Cap
            if (currentLevel < 1 || currentLevel >= LevelCap)
                return 0;

            if (wavesRequiredPerLevel == null || wavesRequiredPerLevel.Length == 0)
                return 0;

            // Index 0 = Level 1 -> 2, Index 1 = Level 2 -> 3, ...
            int index = currentLevel - 1;
            if (index < 0 || index >= wavesRequiredPerLevel.Length)
                return 0;

            int waves = Mathf.Max(0, wavesRequiredPerLevel[index]);
            return waves * baseXpPerStandardWave;
        }

        /// <summary>
        /// Rebuilds wavesCurve from wavesRequiredPerLevel.
        /// Keys: X = Level (1..LevelCap-1), Y = wavesRequiredPerLevel[level-1]
        /// </summary>
        public void RebuildCurveFromArray()
        {
            int cap = Mathf.Max(1, LevelCap);
            int points = Mathf.Max(0, cap - 1); // Level 1->2 at x=1 ... Level (cap-1)->cap at x=cap-1

            // Ensure array is long enough (we only need cap-1 entries)
            int requiredLen = points;
            if (wavesRequiredPerLevel == null || wavesRequiredPerLevel.Length < requiredLen)
            {
                var old = wavesRequiredPerLevel ?? System.Array.Empty<int>();
                var arr = new int[requiredLen];
                for (int i = 0; i < Mathf.Min(old.Length, arr.Length); i++)
                    arr[i] = old[i];
                wavesRequiredPerLevel = arr;
            }

            var keys = new Keyframe[points];
            for (int lvl = 1; lvl <= points; lvl++)
            {
                int idx = lvl - 1;
                int waves = (wavesRequiredPerLevel != null && idx >= 0 && idx < wavesRequiredPerLevel.Length)
                    ? Mathf.Max(0, wavesRequiredPerLevel[idx])
                    : 0;

                keys[idx] = new Keyframe(lvl, waves);
            }

            wavesCurve = new AnimationCurve(keys);

            // Optional: make it smooth by default (comment out if you want linear)
            for (int i = 0; i < wavesCurve.length; i++)
                wavesCurve.SmoothTangents(i, 0f);
        }

        /// <summary>
        /// Bakes wavesCurve into wavesRequiredPerLevel.
        /// For each level (1..LevelCap-1) it evaluates curve at X=level,
        /// rounds to int and clamps to >= 0.
        /// </summary>
        public void BakeCurveToArray()
        {
            int cap = Mathf.Max(1, LevelCap);
            int points = Mathf.Max(0, cap - 1);

            // Ensure array is long enough (we only need cap-1 entries)
            int requiredLen = points;
            if (wavesRequiredPerLevel == null || wavesRequiredPerLevel.Length < requiredLen)
            {
                var old = wavesRequiredPerLevel ?? System.Array.Empty<int>();
                var arr = new int[requiredLen];
                for (int i = 0; i < Mathf.Min(old.Length, arr.Length); i++)
                    arr[i] = old[i];
                wavesRequiredPerLevel = arr;
            }

            // If curve is missing/empty, build a trivial one from the current array
            if (wavesCurve == null || wavesCurve.length == 0)
            {
                var keys = new Keyframe[points];
                for (int lvl = 1; lvl <= points; lvl++)
                {
                    int idx = lvl - 1;
                    int waves = (wavesRequiredPerLevel != null && idx < wavesRequiredPerLevel.Length)
                        ? Mathf.Max(0, wavesRequiredPerLevel[idx])
                        : 0;

                    keys[idx] = new Keyframe(lvl, waves);
                }
                wavesCurve = new AnimationCurve(keys);
            }

            // Sample strictly at integer X = level (ignores any X-drift of keys)
            for (int lvl = 1; lvl <= points; lvl++)
            {
                float y = wavesCurve.Evaluate(lvl);
                int waves = Mathf.Max(0, Mathf.RoundToInt(y));
                wavesRequiredPerLevel[lvl - 1] = waves;
            }
        }
    }
}
