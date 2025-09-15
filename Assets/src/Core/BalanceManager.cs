using UnityEngine;

namespace CHAL.Core
{
    // Fr�h ausf�hren, damit andere Systeme schon beim Awake darauf zugreifen k�nnen.
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
                Debug.LogWarning("[BalanceManager] Zweite Instanz gefunden � wird zerst�rt.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Fr�h validieren, damit Fehler sofort auffallen
            if (Config == null)
            {
                Debug.LogError("[BalanceManager] Config ist null. Balancing-Werte nicht verf�gbar!");
            }
        }
    }
}

