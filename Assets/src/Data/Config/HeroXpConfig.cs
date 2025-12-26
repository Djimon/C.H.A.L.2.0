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

        //TODO: Insert nice Level Curve visual (with movable points) in custom Editor

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
    }
}
