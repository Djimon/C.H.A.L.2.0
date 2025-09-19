using CHAL.Data;
using UnityEngine;

namespace CHAL.Core
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] private GameBalanceConfig config; // optional: Inspector-Zuweisung

        public static GameManager Instance { get; private set; }
        public PlayerProfile Profile { get; private set; }

        public GameBalanceConfig Config
        {
            get
            {
                if (config == null)
                {
                    config = Resources.Load<GameBalanceConfig>("Config/GameBalanceConfig");
                }
                return config;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Save laden oder neuen Spielstand erstellen
            Profile = SaveSystem.Load();
            if (Profile == null)
            {
                Debug.Log("Kein Save gefunden – neues Profil erstellt.");
                Profile = new PlayerProfile();
            }
        }

        public void SaveGame()
        {
            SaveSystem.Save(Profile);
        }

        public void ResetProfile()
        {
            Profile = new PlayerProfile();
            SaveGame();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
