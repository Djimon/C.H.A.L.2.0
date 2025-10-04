using CHAL.Data;
using CHAL.Systems.Items;
using CHAL.Systems.Loot;
using CHAL.Systems.Map;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        [SerializeField]
        private GameBalanceConfig config; 

        [SerializeField]
        private InputManager inputManager;

        [SerializeField]
        public HeroDef starterHero { get; private set; }

        public static GameManager Instance { get; private set; }
        public PlayerProfile Profile { get; private set; }

        public UnluckyProtection Unlucky { get; private set; }

        public MapDef pendingMap { get; private set; }



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

            inputManager = FindFirstObjectByType<InputManager>();

            // Falls keiner in der Szene existiert → automatisch erstellen
            if (inputManager == null)
            {
                GameObject go = new GameObject("InputManager");
                inputManager = go.AddComponent<InputManager>();
                go.transform.SetParent(gameObject.transform);
            }

            Unlucky ??= new UnluckyProtection();
        }

        private void Start()
        {
            //Preload all registries
            ItemRegistry.Instance.TriggerInstance();
        }

        public void SaveGame()
        {
            SaveSystem.Save(Profile);
        }

        public void ResetProfile()
        {
            var oldName = Profile.playerName;
            var oldColors = Profile.playerColors;
            Profile = new PlayerProfile();

            Profile.InitializePlayer(oldName, oldColors);
            //InitalizePlayer autosavfes
        }

        // ---------------------------
        // State Machine Logik
        // ---------------------------
        public void SetState(GameState newState)
        {
            DebugManager.Log($"GameState {CurrentState} → {newState}");
            CurrentState = newState;
            // Optional: Events triggern oder UI umschalten
            // EventBus.Publish(new GameStateChanged(newState));
        }

        // ---------------------------
        // Centralized Scene - Management
        // ---------------------------

        internal void StartNewGame(PlayerProfile profile)
        {
            Profile = profile;
            SaveGame();
            SetState(GameState.Hideout);
            SceneManager.LoadScene("03_Hideout"); // zentral!
        }


        public void GoToMainMenu()
        {
            SaveGame();
            SetState(GameState.MainMenu);
            SceneManager.LoadScene("01_MainMenu");
        }

        public void ExitToHideout()
        {
            SetState(GameState.Hideout);
            SceneManager.LoadScene("03_Hideout");
        }

        internal void ContinueGame()
        {
            if (Profile == null)
            {
                DebugManager.Warning("Kein Save zum Fortsetzen gefunden", "System");
                return;
            }

            var starterId = GameManager.Instance.starterHero != null ? GameManager.Instance.starterHero.HeroId : "TestHero";
            Profile.EnsureStarterHeroUnlocked(starterId);

            SetState(GameState.Hideout); 
            SceneManager.LoadScene("03_Hideout");
        }

        internal static void Quit()
        {
            // Vor dem Beenden: persistente Saves / PlayerPrefs sichern
            try { SaveSystem.Save(Instance?.Profile); } catch { /* ignore */ }

#if UNITY_EDITOR
            // Im Editor: Play Mode stoppen
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

        }

        internal void StartMap(string sceneName, MapDef selectedMap)
        {
            pendingMap = selectedMap;
            SetState(GameState.MapPhase);
            SceneManager.LoadScene(sceneName);

        }
    }
}
