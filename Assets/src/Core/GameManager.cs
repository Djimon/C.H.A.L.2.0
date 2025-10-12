using CHAL.Data;
using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using CHAL.Systems.Loot;
using CHAL.Systems.Map;
using System;
using System.Collections.Generic;
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

    public enum PlayerInventoryType
    {
        all,
        Remains,
        Parts,
        Runes,
        Modules,
        Gear
    }


    public class GameManager : MonoBehaviour
    {

        [SerializeField]
        private GameBalanceConfig config; 

        [SerializeField]
        private InputManager inputManager;

        [SerializeField]
        public HeroDef starterHero { get; private set; }
        [SerializeField]
        private HeroCatalog heroCatalog;
        public HeroCatalog HeroCatalogue => heroCatalog;

        public static GameManager Instance { get; private set; }
        public PlayerProfile Profile { get; private set; }

        public UnluckyProtection Unlucky { get; private set; }

        public MapDef pendingMap { get; private set; }

        public InventoryDomain Inventory { get; private set; }
        public bool InventoryReady { get; private set; }

        private readonly Dictionary<string, InventoryDef> _inventoryTemplates = new();



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
            else //PlayerProfiel vorhanden 
            {
                Inventory = new InventoryDomain();
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
            MapDomainToProfile();
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

            if (Inventory == null)
                Inventory = new InventoryDomain();

            BuildPlayerInventoriesFromFolder();
            MapDomainToProfile();
            InventoryReady = true;

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

            if (Inventory == null)
                Inventory = new InventoryDomain();

            BuildPlayerInventoriesFromFolder();
            MapProfileToDomain();
            InventoryReady = true;

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

        //INVENTORY
        private void BuildPlayerInventoriesFromFolder()
        {
            if (Inventory == null) Inventory = new InventoryDomain();
        }

        private void MapDomainToProfile()
        {
            if (Inventory == null || Profile == null) return;

            // Ziel-Instanzen existieren, weil BuildPlayerInventoriesFromTemplates() sie angelegt hat
            Profile.Remains.FromDictionary(ReadDomainAsDict("player_remains"));
            Profile.Parts.FromDictionary(ReadDomainAsDict("player_parts"));
            Profile.Runes.FromDictionary(ReadDomainAsDict("player_runes"));
            Profile.Modules.FromDictionary(ReadDomainAsDict("player_modules"));
        }

        public void MapProfileToDomain()
        {
            if (Inventory == null || Profile == null) return;

            TryFillDomainFrom(Profile.Remains.ToDictionary(), "player_remains");
            TryFillDomainFrom(Profile.Parts.ToDictionary(), "player_parts");
            TryFillDomainFrom(Profile.Runes.ToDictionary(), "player_runes");
            TryFillDomainFrom(Profile.Modules.ToDictionary(), "player_modules");
        }

        private Dictionary<string, int> ReadDomainAsDict(string instanceId)
        {
            var dict = new Dictionary<string, int>();
            int slots = Inventory.SlotCount(instanceId);
            for (int i = 0; i < slots; i++)
            {
                var st = Inventory.Peek(instanceId, i);
                if (!st.HasValue || st.Value.count <= 0) continue;
                dict[st.Value.itemID] = (dict.TryGetValue(st.Value.itemID, out var c) ? c : 0) + st.Value.count;
            }
            return dict;
        }

        private void TryFillDomainFrom(Dictionary<string, int> source, string instanceId)
        {
            int slots = Inventory.SlotCount(instanceId);
            if (slots <= 0) return;

            // leer machen, weil wir beim Laden/Neustart bewusst den Profilzustand spiegeln
            for (int i = 0; i < slots; i++)
                if (Inventory.Peek(instanceId, i).HasValue)
                    Inventory.TryRemove(instanceId, i, int.MaxValue, out _);

            foreach (var kv in source)
            {
                if (kv.Value <= 0) continue;
                Inventory.TryAdd(instanceId, new ItemStack(kv.Key, kv.Value), out _);
            }
        }

    }
}
