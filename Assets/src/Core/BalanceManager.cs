using CHAL.Data;
using UnityEngine;

namespace CHAL.Core
{
    // Früh ausführen, damit andere Systeme schon beim Awake darauf zugreifen können.
    [DefaultExecutionOrder(-1000)]
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
                        Debug.LogError("[BalanceManager] Keine GameBalanceConfig gefunden. " +
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
                Debug.LogWarning("[BalanceManager] Zweite Instanz gefunden – wird zerstört.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Früh validieren, damit Fehler sofort auffallen
            if (Config == null)
            {
                Debug.LogError("[BalanceManager] Config ist null. Balancing-Werte nicht verfügbar!");
            }
        }

        public static int GetXpForLevel(int level)
        {
            var xpConfig = Instance.config.economy.xp;
            float scale = xpConfig.levelCurveFactor * 0.005f;
            return Mathf.RoundToInt(
                xpConfig.baseLevelUpXp * Mathf.Pow(1 + scale * (level - 1), 2)
            );
        }

        [ContextMenu("Debug XP Progression")]
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

    }
}

