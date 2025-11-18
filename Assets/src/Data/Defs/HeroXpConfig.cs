using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "HeroXPConfig",menuName = "Config/Hero XP Config")]
    public class HeroXPConfig : ScriptableObject
    {
        [Min(1)]
        public int LevelCap = 100;

        [Min(1)]
        public int baseXpPerStandardWave = 100;

        public int[] wavesRequiredPerLevel = new int[100];

        //TODO: Insert nice Level Curve visual (with movable points) in custom Editor

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
