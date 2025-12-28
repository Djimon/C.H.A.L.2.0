using CHAL.Data;
using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using CHAL.Systems.Loot;
using CHAL.Systems.Map;
using CHAL.Systems.Research;
using CHAL.Systems.Skill;
using CHAL.Systems.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CHAL.Core
{
    public enum GameState
    {
        MainMenu,
        MapPhase,     // Spieler kÃ¤mpft auf einer Map
        WaveReward,   // kleiner Reward-Screen
        MapReward,    // groÃŸer Reward-Screen
        Hideout
    }

/// <summary>
/// Manages the game state and handles game-related logic.
/// </summary>
    public class GameManager : MonoBehaviour
    {

        [SerializeField]
        private GameBalanceConfig config; 

        [SerializeField]
        private InputManager inputManager;

        public HeroDef starterHero { get; private set; }

        [SerializeField]
        private HeroCatalog heroCatalog;
        public HeroCatalog HeroCatalogue => heroCatalog;

        public static GameManager Instance { get; private set; }
        public PlayerProfile Profile { get; private set; }

        public StatisticsService Stats { get; private set; }

        public UnluckyProtection Unlucky { get; private set; }

        public MapDef pendingMap { get; private set; }

        // ---- INVENTORY -----
        private const string InventoryDefsPath = "data/Inventory";
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


        // Gearing
        public ImplicitRegistryDef implicitRegistrySO;
        public AffixRegistryDef affixRegistryDef;
        public GearModRegistry gearModRegistry;
        public GearRoller gearRoller { get; private set; }

        Dictionary<string, GearInstance> _gearInstances = new Dictionary<string, GearInstance>();


        public GameBalanceConfig BalanceConfig
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
                DebugManager.DebugLog("No save found");
                //Profile = new PlayerProfile(); //erst im Character Creator
            }
  

            var xpplvl = BalanceConfig.economy.xp.xpPerLevel;
            DebugManager.Log($"Xp per level: {xpplvl}");

            inputManager = FindFirstObjectByType<InputManager>();

            // Falls keiner in der Szene existiert â†’ automatisch erstellen
            if (inputManager == null)
            {
                GameObject go = new GameObject("InputManager");
                inputManager = go.AddComponent<InputManager>();
                go.transform.SetParent(gameObject.transform);
            }

            Unlucky ??= new UnluckyProtection();
            Stats = new StatisticsService();
            gearModRegistry = new GearModRegistry(implicitRegistrySO,affixRegistryDef);
            gearRoller = new GearRoller(BalanceConfig, gearModRegistry);

            if (Profile != null)
            {
                var statsSnap = SaveSystem.LoadStatistics(Profile.profileId);
                Stats.RestoreFromSnapshot(statsSnap);
            }

        }

        private void Start()
        {
            //Preload all registries
            ItemRegistry.Instance.TriggerInstance();
            SkillRegistry.Instance.TriggertInstanc();
            //TODO Skill-registry
        }

        private void WiringServices()
        {
            if (Stats == null || researchService == null)
            {
                DebugManager.Warning("WiringServices: Stats or researchService is null, wiring skipped.", "System");
                return;
            }

            Stats.OnEnemyKilledEvent += researchService.OnEnemyKilled;
            Stats.OnWaveCompletedEvent += researchService.OnWaveCompleted;
            Stats.OnMapCompletedEvent += researchService.OnMapCompleted;
            Stats.OnCraftExecutedEvent += researchService.OnCraftExecuted;
        }

        /// <summary>
        /// Saves the current game state to persistent storage.
        /// This method updates the saved profile with the latest changes.
        /// </summary>
        public void SaveGame()
        {
            MapDomainToProfile();
            SaveSystem.Save(Profile);

            if (Stats != null && Profile != null)
            {
                var statsSnap = Stats.CreateSnapshot();
                SaveSystem.SaveStatistics(Profile.profileId, statsSnap);
            }

            if (Profile != null && Profile.ResearchRuntime != null)
            {
                var snap = Profile.BuildResearchSnapshotFrom(Profile.ResearchRuntime);
                SaveSystem.SaveResearch(Profile.profileId, snap);
            }
        }

/// <summary>
/// Resets the player profile to its default state.
/// This method initializes a new player profile and retains the old player's name and colors.
/// </summary>
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
/// <summary>
/// Sets the current game state to the specified new state.
/// </summary>
/// <param name="newState">The new game state to set.</param>
        public void SetState(GameState newState)
        {
            DebugManager.Log($"GameState {CurrentState} -> {newState}");
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

            BootstrapInventoryDomain();
            BuildInventoryRoutingMaps();
            MapProfileToDomain();
            InventoryReady = true;

            //Research
            InitResearch(loadExisting: false);

            SaveGame();
            SetState(GameState.Hideout);
            SceneManager.LoadScene("03_Hideout"); // zentral!
        }



/// <summary>
/// Navigates to the main menu of the game.
/// </summary>
        public void GoToMainMenu()
        {
            SaveGame();
            SetState(GameState.MainMenu);
            SceneManager.LoadScene("01_MainMenu");
        }

/// <summary>
/// Exits the current game state and transitions to the hideout scene.
/// </summary>
        public void ExitToHideout()
        {
            SetState(GameState.Hideout);
            SceneManager.LoadScene("03_Hideout");
        }

        internal void ContinueGame()
        {
            if (Profile == null)
            {
                DebugManager.Warning("No save found to continue", "System");
                return;
            }

            //Inventroy
            BootstrapInventoryDomain();
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

/// <summary>
/// Initializes the player inventory for the test.
/// </summary>
        public void TestInitInventory()
        {
            BootstrapInventoryDomain();
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
            SaveGame();
        }

        //INVENTORY
        private void BootstrapInventoryDomain()
        {
            EnsureInventoryDomain();
            LoadInventoryTemplatesIfNeeded();
            RegisterPlayerInventoryInstancesFromTemplates();
        }

        private void EnsureInventoryDomain()
        {
            Inventory ??= new InventoryDomain();
        }

        private void LoadInventoryTemplatesIfNeeded()
        {
            if (_inventoryTemplates.Count > 0) return;

            var defs = Resources.LoadAll<InventoryDef>(InventoryDefsPath);
            foreach (var def in defs)
            {
                if (def == null) continue;
                _inventoryTemplates[def.TypeId] = def;
            }
        }

        private void RegisterPlayerInventoryInstancesFromTemplates()
        {
            foreach (var kv in _inventoryTemplates)
            {
                var type = kv.Key;
                var def = kv.Value;

                if (def == null) continue;
                if (type == PlayerInventoryType.all) continue;

                var instanceId = BuildInstanceId(type);
                if (Inventory.HasInstance(instanceId)) continue;

                var inst = InventoryInstance.Create(instanceId, def);
                Inventory.RegisterInstance(inst);
            }
        }

        /// <summary>
        /// Retrieves the inventory template for the specified player inventory type.
        /// </summary>
        /// <param name="typeId">The type ID of the player inventory.</param>
        /// <returns>The corresponding InventoryDef, or null if not found.</returns>
        public InventoryDef GetTemplate(PlayerInventoryType typeId)
        {
            if (_inventoryTemplates.TryGetValue(typeId, out var def)) return def;

            const string path = "data/Inventory";
            var all = Resources.LoadAll<InventoryDef>(path);
            foreach (var d in all)
                if (d != null)
                    _inventoryTemplates[d.TypeId] = d;

            if (_inventoryTemplates.TryGetValue(typeId, out def)) return def;

            DebugManager.Error($"GetTemplate: no InventoryDef with TypeId='{typeId}' under Resources/{path}");
            return null;
        }

/// <summary>
/// Ensures an inventory instance is created based on the provided instance ID and template type.
/// </summary>
/// <param name="instanceId">The unique identifier for the inventory instance.</param>
/// <param name="templateTypeId">The type of player inventory template to use.</param>
/// <returns>The created inventory instance, or null if the instance could not be created.</returns>
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

/// <summary>
/// Maps domain data to the profile's inventory.
/// </summary>
        public void MapDomainToProfile()
        {
            if (Inventory == null || Profile == null) return;

            Profile.InventorySave ??= new List<InventorySnapshot>();
            Profile.InventorySave.Clear();

            int applied = 0;
            var invs = Profile.Inventories;
            for (int i = 0; i < invs.Count; i++)
            {
                var inv = invs[i];
                if (inv == null || string.IsNullOrEmpty(inv.invID)) continue;

                string instanceId = "player_" + inv.invID.ToLowerInvariant();

                // 1) Slots (positionsgenau + instanceId)
                var slotSnaps = ReadDomainAsSlotSnapshots(instanceId);

                // 2) Legacy dict (für Counts/UI)
                var dict = BuildFlatDict(slotSnaps);

                // 3) GearInstance payloads (nur wenn instanceId gesetzt)
                var gearPayloads = CollectGearPayloads(slotSnaps);

                // optional: alte "Inventory" weiterhin füllen (Legacy UI/Logik)
                inv.FromDictionary(dict);

                Profile.InventorySave.Add(new InventorySnapshot
                {
                    id = inv.invID,
                    items = dict,
                    slots = slotSnaps,
                    gearInstances = gearPayloads
                });

                applied++;
            }

            DebugManager.Log(
                $"MapDomainToProfile: applied {applied} inventories",
                DebugManager.EDebugLevel.Dev, "Inventory", LogType.Log);
        }

/// <summary>
/// Maps the profile data to the domain model.
/// </summary>
        public void MapProfileToDomain()
        {
            if (Inventory == null || Profile == null) 
                return;

            // 1) Restore instance payloads first (so systems can query them immediately)
            _gearInstances ??= new Dictionary<string, GearInstance>();
            _gearInstances.Clear();

            if (Profile.InventorySave != null)
            {
                foreach (var snap in Profile.InventorySave)
                {
                    if (snap.gearInstances == null) continue;
                    foreach (var g in snap.gearInstances)
                    {
                        if (g == null || string.IsNullOrWhiteSpace(g.instanceId)) continue;
                        _gearInstances[g.instanceId] = g;
                    }
                }
            }

            int applied = 0;


            // 2) Prefer InventorySave (domain-accurate). Fallback to old Profile.Inventories if needed.
            if (Profile.InventorySave != null && Profile.InventorySave.Count > 0)
            {
                foreach (var snap in Profile.InventorySave)
                {
                    if (string.IsNullOrEmpty(snap.id)) continue;

                    string instanceId = "player_" + snap.id.ToLowerInvariant();

                    // Ensure instance exists (TryFillDomainFrom already does this, but we need it for slots too)
                    EnsureInstanceByInstanceId(instanceId);

                    // Clear first
                    Inventory.ClearAllSlots(instanceId);

                    // Slots path (preferred)
                    if (snap.slots != null && snap.slots.Count > 0)
                    {
                        TryFillDomainFromSlots(snap.slots, instanceId);
                    }
                    else
                    {
                        // Fallback: items dict
                        TryFillDomainFrom(snap.items ?? new Dictionary<string, int>(), instanceId);
                    }

                    // Optional: keep legacy Inventory objects in sync (if still used somewhere)
                    var legacyInv = Profile.Inventories?.FirstOrDefault(x => x != null && x.invID == snap.id);
                    if (legacyInv != null)
                        legacyInv.FromDictionary(snap.items ?? new Dictionary<string, int>());

                    applied++;
                }
            }
            else
            {
                // Legacy fallback
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
            }

            DebugManager.Log(
                $"MapProfileToDomain: applied {applied} inventories",
                DebugManager.EDebugLevel.Dev, "Inventory", LogType.Log);
        }

        private void EnsureInstanceByInstanceId(string instanceId)
        {
            if (Inventory == null || string.IsNullOrEmpty(instanceId)) return;

            if (Inventory.HasInstance(instanceId))
                return;

            // same logic as TryFillDomainFrom: parse suffix to PlayerInventoryType and EnsureInstance(...)
            var suffix = instanceId.StartsWith("player_") ? instanceId.Substring("player_".Length) : instanceId;
            if (Enum.TryParse<PlayerInventoryType>(suffix, true, out var type))
            {
                EnsureInstance(instanceId, type);
            }
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
                // Konvention: "player_" + enumname_lower  â†’ Enum parsen
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
                    Inventory.TryAdd(instanceId, new ItemStackRef(kv.Key, kv.Value), out _);
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

            // NUR die Typen registrieren, fÃ¼r die du auch tatsÃ¤chlich Defs/Inventare hast
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

        // Ã–ffentliche, einfache Resolver-API fÃ¼r alle Systeme
/// <summary>
/// Attempts to resolve a PlayerInventoryType and instance ID from the given item ID.
/// </summary>
/// <param name="itemId">The item ID to resolve.</param>
/// <param name="type">The resolved PlayerInventoryType.</param>
/// <param name="instanceId">The resolved instance ID.</param>
/// <returns>True if resolution is successful; otherwise, false.</returns>
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

/// <summary>
/// Gets the instance ID for the specified player inventory type.
/// </summary>
/// <param name="t">The player inventory type to get the instance ID for.</param>
/// <returns>The instance ID associated with the specified inventory type.</returns>
        public string InstanceIdFor(PlayerInventoryType t)
        {
            if (_typeToInstanceId.TryGetValue(t, out var id)) return id;
            id = BuildInstanceId(t);
            _typeToInstanceId[t] = id;
            return id;
        }

/// <summary>
/// Initializes the research system, optionally loading existing data.
/// </summary>
/// <param name="loadExisting">Indicates whether to load existing research data.</param>
        public void InitResearch(bool loadExisting)
        {
            EnsureResearchDefsLoaded();

            // Runtime-Container sicherstellen
            if (Profile.ResearchRuntime == null)
                Profile.ResearchRuntime = new ResearchState();

            // Services erstellen (einmalig)
            researchService ??= new ResearchService();
            ResearchUnlocks ??= new ResearchUnlockRegistry();

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

            WiringServices();

        }

        private void EnsureResearchDefsLoaded()
        {
            if (researchTree == null)
                researchTree = Resources.Load<ResearchTreeDef>("data/Research/Tree");

            if (researchNodes == null || researchNodes.Count == 0)
                researchNodes = Resources.LoadAll<ResearchNodeDef>("data/Research/Nodes").ToList();
        }


/// <summary>
/// Registers a new gear instance if it is valid.
/// </summary>
/// <param name="gear">The gear instance to register.</param>
        public void RegisterGearInstance(GearInstance gear)
        {
            if (gear == null)
            {
                DebugManager.Warning("RegisterGearInstance: gear is null.", "Gearing");
                return;
            }

            if (string.IsNullOrWhiteSpace(gear.instanceId))
            {
                DebugManager.Warning("RegisterGearInstance: gear.instanceId is null/empty.", "Gearing");
                return;
            }

            if (_gearInstances.ContainsKey(gear.instanceId))
            {
                    DebugManager.Warning($"RegisterGearInstance: duplicate instanceId '{gear.instanceId}'. Overwriting.", "Gearing");
            }

            _gearInstances[gear.instanceId] = gear;
        }

/// <summary>
/// Tries to get a gear instance by its instance ID.
/// </summary>
/// <param name="instanceId">The ID of the gear instance to retrieve.</param>
/// <param name="gear">The retrieved gear instance, if found.</param>
/// <returns>True if the gear instance was found; otherwise, false.</returns>
        public bool TryGetGearInstance(string instanceId, out GearInstance gear)
        {
            if (string.IsNullOrWhiteSpace(instanceId))
            {
                gear = null;
                return false;
            }
            return _gearInstances.TryGetValue(instanceId, out gear);
        }

/// <summary>
/// Removes a gear instance identified by the given instance ID.
/// </summary>
/// <param name="instanceId">The ID of the gear instance to remove.</param>
/// <returns>True if the instance was successfully removed; otherwise, false.</returns>
        public bool RemoveGearInstance(string instanceId)
        {
            if (string.IsNullOrWhiteSpace(instanceId))
                    return false;

            return _gearInstances.Remove(instanceId);
        }

        private List<InventorySlotSnapshot> ReadDomainAsSlotSnapshots(string instanceId)
        {
            var slotsOut = new List<InventorySlotSnapshot>();
            int slots = Inventory.SlotCount(instanceId);

            for (int i = 0; i < slots; i++)
            {
                var st = Inventory.Peek(instanceId, i);
                if (!st.HasValue || st.Value.count <= 0) continue;

                slotsOut.Add(new InventorySlotSnapshot
                {
                    slot = i,
                    itemId = st.Value.itemID,
                    count = st.Value.count,
                    instanceId = st.Value.instanceId
                });
            }

            return slotsOut;
        }

        private Dictionary<string, int> BuildFlatDict(List<InventorySlotSnapshot> slots)
        {
            var dict = new Dictionary<string, int>();
            if (slots == null) return dict;

            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                if (string.IsNullOrEmpty(s.itemId) || s.count <= 0) continue;

                dict[s.itemId] = (dict.TryGetValue(s.itemId, out var c) ? c : 0) + s.count;
            }

            return dict;
        }

        private List<GearInstance> CollectGearPayloads(List<InventorySlotSnapshot> slots)
        {
            if (slots == null) return null;

            List<GearInstance> result = null;

            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                if (string.IsNullOrWhiteSpace(s.instanceId)) continue;

                if (_gearInstances != null && _gearInstances.TryGetValue(s.instanceId, out var gear) && gear != null)
                {
                    result ??= new List<GearInstance>();
                    result.Add(gear);
                }
                else
                {
                    DebugManager.Log(
                        $"CollectGearPayloads: instanceId '{s.instanceId}' referenced in inventory but not found in _gearInstances.",
                        DebugManager.EDebugLevel.Dev, "Save", LogType.Warning);
                }
            }

            return result;
        }

        private void TryFillDomainFromSlots(List<InventorySlotSnapshot> slots, string instanceId)
        {
            if (Inventory == null || string.IsNullOrEmpty(instanceId)) return;
            if (slots == null) return;

            // assumes instance exists
            for (int i = 0; i < slots.Count; i++)
            {
                var s = slots[i];
                if (s.count <= 0 || string.IsNullOrEmpty(s.itemId)) continue;

                var stack = new ItemStackRef(s.itemId, s.count, s.instanceId);

                // Use domain API to preserve slot AND trigger events
                if (!Inventory.TrySetSlot(instanceId, s.slot, stack))
                {
                    DebugManager.Log(
                        $"TryFillDomainFromSlots: failed to set slot {instanceId}:{s.slot} ({stack})",
                        DebugManager.EDebugLevel.Dev, "Inventory", LogType.Warning);
                }
            }
        }

    }
}
