using CHAL.Data;
using UnityEngine;

namespace CHAL.Core
{
    // Früh ausführen, damit andere Systeme schon beim Awake darauf zugreifen können.
    [DefaultExecutionOrder(-1000)]
/// <summary>
/// Manages the game balance configuration and provides access to it.
/// </summary>
    public class BalanceManager : MonoBehaviour
    {
        public static BalanceManager Instance { get; private set; }

        [Header("Zentrales Balancing-ScriptableObject")]
        [SerializeField] private GameBalanceConfig config;   // per Inspector setzen

        public GameBalanceConfig Config
        {
            get
            {
                if (config == null)
                {
                    // Fallback: versucht, aus Resources/Config/GameBalanceConfig zu laden
                    config = Resources.Load<GameBalanceConfig>("Config/GameBalanceConfig");
                    if (config == null)
                    {
                        DebugManager.Error("[BalanceManager] No GameBalanceConfig found." +
                                        "Bitte im Inspector zuweisen oder unter Resources/Config/GameBalanceConfig ablegen.");
                    }
                }
                return config;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DebugManager.Warning("[BalanceManager] Second instance found – will be destroyed.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Früh validieren, damit Fehler sofort auffallen
            if (Config == null)
            {
                DebugManager.Error("[BalanceManager] Config is null. Balancing values unavailable!");
            }
        }

        // NEU: zentraler Zugriff auf HeroXPConfig
        public HeroXPConfig HeroXPConfig
            => Config != null ? Config.heroXP : null;

        // Optional: statischer Helfer, falls du aus nicht-Mono-Klassen ranwillst
        public static HeroXPConfig GetHeroXP()
        {
            return Instance != null ? Instance.HeroXPConfig : null;
        }

        /// <summary>
        /// Calculates the experience points required for a given level.
        /// </summary>
        /// <param name="level">The level for which to calculate the experience points.</param>
        /// <returns>The experience points needed to reach the specified level.</returns>
        public static int GetXpForLevel(int level)
        {
            var xpConfig = Instance.config.economy.xp;
            float scale = xpConfig.levelCurveFactor * 0.005f;
            return Mathf.RoundToInt(
                xpConfig.baseLevelUpXp * Mathf.Pow(1 + scale * (level - 1), 2)
            );
        }

        [ContextMenu("Debug XP Progression")]
/// <summary>
/// Debugs the experience progression for specified levels.
/// This method calculates and displays the total experience points up to each checkpoint level.
/// </summary>
        public void DebugXpProgression()
        {
            int[] checkpoints = {1, 10, 50, 100 };

            int total = 0;
            foreach (int lvl in checkpoints)
            {
                // kumulierte XP bis zu diesem Level
                total = 0;
                for (int i = 1; i <= lvl; i++)
                {
                    total += GetXpForLevel(i);
                }

                int levelXp = GetXpForLevel(lvl);
                DebugManager.Log($"Level {lvl}: {total:N0} total XP | XP for level: {levelXp:N0}",DebugManager.EDebugLevel.Debug);
            }
        }

/// <summary>
/// Gets the range value based on the specified skill range.
/// </summary>
/// <param name="range">The skill range to evaluate.</param>
/// <returns>The corresponding range value as a float.</returns>
        public float GetRangeValue(SkillRange range)
        {
            var skillRangeCOnfig = Instance.Config.skillRanges;
            return range switch
            {
                SkillRange.Self => skillRangeCOnfig.selfRange,
                SkillRange.MeleeRange => skillRangeCOnfig.meleeRange,
                SkillRange.Reach => skillRangeCOnfig.reachRange,
                SkillRange.MidDistance => skillRangeCOnfig.midDistanceRange,
                SkillRange.FarDistance => skillRangeCOnfig.farDistanceRange,
                _ => skillRangeCOnfig.meleeRange
            };
        }

    }
}

