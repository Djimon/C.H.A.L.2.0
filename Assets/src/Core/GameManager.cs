using CHAL.Data;
using UnityEngine;

namespace CHAL.Core
{
    public enum GameState
    {
        MainMenu,
        MapPhase,     // Spieler kämpft auf einer Map
        WaveReward,   // kleiner Reward-Screen
        MapReward,    // großer Reward-Screen
        Hideout
    }
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

        // Aktueller Spielzustand
        public GameState CurrentState { get; private set; } = GameState.MainMenu;

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
                Debug.Log("Kein Save gefunden ");
                //Profile = new PlayerProfile(); //erst im Character Creator
            }

            var xpplvl = Config.economy.xp.xpPerLevel;
            DebugManager.Log($"Xp per level: {xpplvl}");

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

        // ---------------------------
        // State Machine Logik
        // ---------------------------
        public void SetState(GameState newState)
        {
            CurrentState = newState;
            Debug.Log($"GameState → {newState}");

            // Optional: Events triggern oder UI umschalten
            // EventBus.Publish(new GameStateChanged(newState));
        }

        public void GoToMainMenu()
        {
            SetState(GameState.MainMenu);
        }

        public void ExitToHideout()
        {
            SetState(GameState.Hideout);
            // Hier Crafting/Hideout laden
        }

    }
}
