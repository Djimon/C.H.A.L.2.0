using CHAL.Data;
using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using CHAL.Systems.Loot;
using CHAL.Systems.Map;
using CHAL.Systems.Research;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField]
        private HeroCatalog heroCatalog;
        public HeroCatalog HeroCatalogue => heroCatalog;

        public static GameManager Instance { get; private set; }
        public PlayerProfile Profile { get; private set; }

        public UnluckyProtection Unlucky { get; private set; }

        public MapDef pendingMap { get; private set; }

        // ---- INVENTORY -----
        public InventoryDomain Inventory { get; private set; }
        public bool InventoryReady { get; private set; }

        private readonly Dictionary<PlayerInventoryType, InventoryDef> _inventoryTemplates = new();

        // Routing-Maps: prefix (enumname lower) -> type, type -> instanceId
        private readonly Dictionary<string, PlayerInventoryType> _prefixToType =
            new Dictionary<string, PlayerInventoryType>(StringComparer.Ordinal);
        private readonly Dictionary<PlayerInventoryType, string> _typeToInstanceId =
            new Dictionary<PlayerInventoryType, string>();

        private static string BuildInstanceId(PlayerInventoryType t)
            => "player_" + t.ToString().ToLowerInvariant();

        // --- Research ---
        [SerializeField] private ResearchTreeDef researchTree;
        [SerializeField] private List<ResearchNodeDef> researchNodes = new();

        public ResearchService researchService { get; private set; }
        public ResearchUnlockRegistry ResearchUnlocks { get; private set; }
        public ResearchEventBridge ResearchBridge { get; private set; }

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

            // Save laden falls vorhanden
            Profile = SaveSystem.Load();

            if (Profile == null)
            {
                DebugManager.DebugLog("Kein Save gefunden ");
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
            MapDomainToProfile();
            SaveSystem.Save(Profile);
        }

        public void ResetProfile()
        {
            var oldName = Profile.playerName;
            var oldColors = Profile.playerColors;

            //SaveSystem.DeleteProfileData();

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

            //Inventory
            if (Inventory == null)
                Inventory = new InventoryDomain();

            BuildPlayerInventoriesFromFolder();
            BuildInventoryRoutingMaps();
            MapProfileToDomain();
            InventoryReady = true;

            //Research
            InitResearch(loadExisting: false);

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

            //Inventroy
            BuildPlayerInventoriesFromFolder();
            BuildInventoryRoutingMaps();
            MapProfileToDomain();
            InventoryReady = true;

            //Research
            InitResearch(loadExisting: true);

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

        public void TestInitInventory()
        {
            BuildPlayerInventoriesFromFolder();
            MapProfileToDomain();
        }

        internal void StartMap(string sceneName, MapDef selectedMap)
        {
            pendingMap = selectedMap;
            SetState(GameState.MapPhase);
            SceneManager.LoadScene(sceneName);

        }


        private void OnApplicationQuit()
        {
            SaveSystem.Save(Instance?.Profile);

            if (Profile != null && Profile.ResearchRuntime != null)
            {
                var snap = Profile.BuildResearchSnapshotFrom(Profile.ResearchRuntime);
                SaveSystem.SaveResearch(Profile.profileId, snap);
            }
        }

        //INVENTORY
        private void BuildPlayerInventoriesFromFolder()
        {
            if (Inventory == null) Inventory = new InventoryDomain();

            const string path = "data/Inventory";
            var defs = Resources.LoadAll<InventoryDef>(path);
            foreach (var def in defs)
            {
                if (def == null) continue;

                // Player-Inventare: alles außer 'all' (du hast Remains/Parts/Runes/Modules/Gear)
                if (def.TypeId == PlayerInventoryType.all) continue;

                // Konvention: instanceId = "player_" + enum-name in lowercase
                string instanceId = "player_" + def.TypeId.ToString().ToLowerInvariant();
                if (Inventory.HasInstance(instanceId)) continue;

                var inst = InventoryInstance.Create(instanceId, def);
                Inventory.RegisterInstance(inst);
            }
        }

        public InventoryDef GetTemplate(PlayerInventoryType typeId)
        {
            if (_inventoryTemplates.TryGetValue(typeId, out var def)) return def;

            const string path = "data/Inventory";
            var all = Resources.LoadAll<InventoryDef>(path);
            foreach (var d in all)
                if (d != null)
                    _inventoryTemplates[d.TypeId] = d;

            if (_inventoryTemplates.TryGetValue(typeId, out def)) return def;

            DebugManager.Error($"GetTemplate: kein InventoryDef mit TypeId='{typeId}' unter Resources/{path}");
            return null;
        }

        public InventoryInstance EnsureInstance(string instanceId, PlayerInventoryType templateTypeId)
        {
            if (string.IsNullOrEmpty(instanceId))
            {
                DebugManager.Error("EnsureInstance: instanceId leer.");
                return null;
            }

            var domain = Inventory;
            if (domain == null)
            {
                DebugManager.Error("EnsureInstance: InventoryDomain fehlt (GameManager.Inventory == null).");
                return null;
            }

            if (domain.HasInstance(instanceId))
                return domain.GetInstance(instanceId);

            var def = GetTemplate(templateTypeId);
            if (def == null) return null;

            var inst = InventoryInstance.Create(instanceId, def);
            domain.RegisterInstance(inst);
            return inst;
        }

        public void MapDomainToProfile()
        {
            if (Inventory == null || Profile == null) return;

            int applied = 0;
            var invs = Profile.Inventories;
            for (int i = 0; i < invs.Count; i++)
            {
                var inv = invs[i];
                if (inv == null || string.IsNullOrEmpty(inv.invID)) continue;

                string instanceId = "player_" + inv.invID.ToLowerInvariant();
                var dict = ReadDomainAsDict(instanceId);
                inv.FromDictionary(dict);
                applied++;
            }

            DebugManager.Log(
                $"MapDomainToProfile: applied {applied} inventories",
                DebugManager.EDebugLevel.Dev, "Inventory", LogType.Log);
        }

        public void MapProfileToDomain()
        {
            if (Inventory == null || Profile == null) return;

            int applied = 0;
            var invs = Profile.Inventories;
            for (int i = 0; i < invs.Count; i++)
            {
                var inv = invs[i];
                if (inv == null || string.IsNullOrEmpty(inv.invID)) continue;

                string instanceId = "player_" + inv.invID.ToLowerInvariant();
                var dict = inv.ToDictionary();
                TryFillDomainFrom(dict, instanceId);
                applied++;
            }

            DebugManager.Log(
                $"MapProfileToDomain: applied {applied} inventories",
                DebugManager.EDebugLevel.Dev, "Inventory", LogType.Log);
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
            if (Inventory == null || string.IsNullOrEmpty(instanceId)) return;

            int slots = Inventory.SlotCount(instanceId);
            if (slots <= 0)
            {
                // Versuche die Instanz on-demand zu erstellen:
                // Konvention: "player_" + enumname_lower  → Enum parsen
                var suffix = instanceId.StartsWith("player_") ? instanceId.Substring("player_".Length) : instanceId;
                if (Enum.TryParse<PlayerInventoryType>(suffix, true, out var type))
                {
                    var created = EnsureInstance(instanceId, type);
                    if (created != null)
                        slots = created.SlotCount;

                    DebugManager.Log($"try to create isntance for  {suffix}");
                }

                if (slots <= 0)
                {
                    DebugManager.Log(
                        $"TryFillDomainFrom: instance '{instanceId}' has no slots / not found (after ensure).",
                        DebugManager.EDebugLevel.Dev, "Inventory", LogType.Warning);
                    return;
                }
            }

            // 1) Clear existing stacks
            Inventory.ClearAllSlots(instanceId);

            // 2) Refill
            if (source != null)
            {
                int x = 0;
                foreach (var kv in source)
                {
                    if (kv.Value <= 0) continue;
                    Inventory.TryAdd(instanceId, new ItemStack(kv.Key, kv.Value), out _);
                    x += kv.Value;
                }
                DebugManager.Log($"Refilled Inventory {instanceId} (amount:{x})", DebugManager.EDebugLevel.Dev, "Inventory");
            }
        }

        // Einmal beim Boot/Def-Load aufrufen
        private void BuildInventoryRoutingMaps()
        {
            _prefixToType.Clear();
            _typeToInstanceId.Clear();

            // NUR die Typen registrieren, für die du auch tatsächlich Defs/Inventare hast
            // Falls du schon eine Liste deiner geladenen InventoryDefs hast, nimm die.
            // Minimalvariante: alle Enumwerte erlauben.
            foreach (PlayerInventoryType t in Enum.GetValues(typeof(PlayerInventoryType)))
            {
                var prefix = t.ToString().ToLowerInvariant(); // 1:1-Regel
                if (!_prefixToType.ContainsKey(prefix))
                    _prefixToType.Add(prefix, t);

                if (!_typeToInstanceId.ContainsKey(t))
                    _typeToInstanceId.Add(t, BuildInstanceId(t));
            }

            DebugManager.Log(
                $"Inventory routing ready: prefixes={_prefixToType.Count}, instanceIds={_typeToInstanceId.Count}",
                DebugManager.EDebugLevel.Dev, "Inventory", LogType.Log);
        }

        // Öffentliche, einfache Resolver-API für alle Systeme
        public bool TryResolveByItemId(string itemId, out PlayerInventoryType type, out string instanceId)
        {
            type = default;
            instanceId = null;
            if (string.IsNullOrEmpty(itemId)) return false;

            int colon = itemId.IndexOf(':');
            if (colon <= 0) return false;

            var prefix = itemId.Substring(0, colon).Trim().ToLowerInvariant(); // exakt enumname lower

            if (!_prefixToType.TryGetValue(prefix, out type))
                return false;

            if (!_typeToInstanceId.TryGetValue(type, out instanceId))
            {
                instanceId = "player_" + type.ToString().ToLowerInvariant();
                _typeToInstanceId[type] = instanceId;
            }

            return true;
        }

        public string InstanceIdFor(PlayerInventoryType t)
        {
            if (_typeToInstanceId.TryGetValue(t, out var id)) return id;
            id = BuildInstanceId(t);
            _typeToInstanceId[t] = id;
            return id;
        }

        public void InitResearch(bool loadExisting)
        {
            EnsureResearchDefsLoaded();

            // Runtime-Container sicherstellen
            if (Profile.ResearchRuntime == null)
                Profile.ResearchRuntime = new ResearchState();

            // Services erstellen (einmalig)
            researchService ??= new ResearchService();
            ResearchUnlocks ??= new ResearchUnlockRegistry();
            ResearchBridge = new ResearchEventBridge(researchService);

            // Laden oder frischen Stand anlegen
            if (loadExisting)
            {
                var snap = SaveSystem.LoadResearch(Profile.profileId);
                Profile.RestoreResearchInto(Profile.ResearchRuntime, snap);
            }
            else
            {
                Profile.ResearchRuntime.activeNodeId = null;
                Profile.ResearchRuntime.completedNodeIds.Clear();
                Profile.ResearchRuntime.perNodeProgress.Clear();

                // alte Datei optional entfernen, dann leeren Snapshot sofort anlegen
                SaveSystem.DeleteResearch(Profile.profileId);
                SaveSystem.SaveResearch(Profile.profileId, Profile.BuildResearchSnapshotFrom(Profile.ResearchRuntime));
            }


            researchService.OnAlwaysUnlockedReady += ids => ResearchUnlocks.ApplyAlwaysUnlocked(ids);
            // Speichern beim Abschluss & Registry pflegen
            researchService.OnNodeCompleted += (nodeId, unlocks) =>
            {
                ResearchUnlocks.ApplyNodeUnlocks(nodeId, unlocks);
                var snapNow = Profile.BuildResearchSnapshotFrom(Profile.ResearchRuntime);
                SaveSystem.SaveResearch(Profile.profileId, snapNow);
            };

            // Service + Registry richtig initialisieren
            researchService.InitFromTree(researchTree, Profile.ResearchRuntime);
            ResearchUnlocks.RebuildFrom(researchNodes, Profile.ResearchRuntime.completedNodeIds);
            ResearchUnlocks.ApplyAlwaysUnlocked(researchTree.alwaysUnlockedIds);


        }

        private void EnsureResearchDefsLoaded()
        {
            if (researchTree == null)
                researchTree = Resources.Load<ResearchTreeDef>("data/Research/Tree");

            if (researchNodes == null || researchNodes.Count == 0)
                researchNodes = Resources.LoadAll<ResearchNodeDef>("data/Research/Nodes").ToList();
        }
    }
}
