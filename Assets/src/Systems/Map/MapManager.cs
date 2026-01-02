using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Hero;
using CHAL.Systems.Wave;
using CHAL.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Map
{
/// <summary>
/// Manages the map and its related functionalities in the game.
/// </summary>
    public class MapManager : MonoBehaviour
    {
        //public static MapManager Instance { get; private set; }

        [Header("Runtime")]
        public MapDef CurrentMap { get; private set; }
        private GameObject _mapInstancedPrefab;
        public GameObject waveRewardUI;
        public GameObject mapRewardUI;
        public GameObject selectHeroUI;

        [Header("Heroes")]
        [SerializeField] private GameObject heroFallbackPrefab;   // optionaler Fallback
        private Dictionary<string, HeroDef> _heroById;
        private List<string> _pendingSelectedHeroes;
        private List<GameObject> _activeHeroes;
        private readonly HashSet<string> _heroesDiedThisWave = new HashSet<string>(StringComparer.Ordinal);

        private bool _autoStartAllWaves = false;
        public bool AutoStartAllWaves => _autoStartAllWaves;


        private WaveManager _waveManager;
        public int CurrentWave { get; private set; } = 1;
        public int MaxWaves => CurrentMap != null ? CurrentMap.maxWaves : 0;

        private void Awake()
        {
            HideUI();
        }

        private void Start()
        {
            PrepareMap();
        }

/// <summary>
/// Hides the user interface elements for the current wave.
/// This method is called to prepare the game for the next wave.
/// </summary>
        public void HideUI()
        {
            waveRewardUI.GetComponent<WaveRewardUI>().Show(false);
            mapRewardUI.GetComponent<MapRewardUI>().Show(false);
        }


/// <summary>
/// Prepares the game map for the current wave.
/// This method initializes the map and instantiates the necessary prefabs.
/// </summary>
        public void PrepareMap()
        {
            CurrentMap = GameManager.Instance.pendingMap;
            CurrentWave = 1;

            DebugManager.Log($"Starting Map {CurrentMap.mapId} (Level {CurrentMap.baseLevel}, Waves {CurrentMap.maxWaves})",
                             DebugManager.EDebugLevel.Test, "Map");

            // Szene "04_Map" muss geladen sein â†’ dann Prefab instanzieren
            if (_mapInstancedPrefab != null)
                Destroy(_mapInstancedPrefab);

            if (CurrentMap.mapPrefab != null)
                _mapInstancedPrefab = Instantiate(CurrentMap.mapPrefab);
            else
                DebugManager.Warning("Missing MapPrefab");


            var selectUI = selectHeroUI.GetComponent<HeroSelectionUI>();
            selectUI.Init(this);
            selectUI.Show(true);

            _activeHeroes = new List<GameObject>();

            _autoStartAllWaves = false; // Default für neue Map
            DebugManager.Info("AutoStartAllWaves reset for new map","Wave");
        }

/// <summary>
/// Resets the current wave to the first wave.
/// Prepares the game for a new wave to start.
/// </summary>
        public void ResetWave() 
        {
            CurrentWave = 1;
            StartWave();
        }

        [ContextMenu("Debug/StartWave")]
/// <summary>
/// Starts a new wave in the game.
/// Initializes the wave manager and resets heroes for the new wave.
/// </summary>
        public void StartWave()
        {
            HideUI();

            _heroesDiedThisWave.Clear();

            _waveManager = _mapInstancedPrefab.GetComponentInChildren<WaveManager>();

            if (_waveManager == null)
            {
                DebugManager.Error("WaveManager not set!", "Map");
                GameManager.Instance.ExitToHideout();
                return;
            }


            ResetHeroesForNewWave(_waveManager); 
            SpawnSelectedHeroesAtSlots(_pendingSelectedHeroes, _waveManager);

            DebugManager.Log($"Starting wave {CurrentWave}/{MaxWaves}", DebugManager.EDebugLevel.Test, "Map");

            _waveManager.StartWave(CurrentMap, CurrentWave, this);
        }

        private void ResetHeroesForNewWave(WaveManager waveMgr)
        {
            if (waveMgr == null) return;

            // Destroy only heroes that belong to the active map instance to avoid touching editor objects or other scenes.
            int count = 0;
            foreach (var ah in _activeHeroes)
            {
                if (ah != null)
                {
                    Destroy(ah);
                    count++;
                }
            }

            _activeHeroes.Clear();

            DebugManager.Log($"ResetHeroesForNewWave: cleared {count} existing hero instance(s).",
                DebugManager.EDebugLevel.Test, "Map");
        }

        private void SpawnSelectedHeroesAtSlots(List<string> heroIds, WaveManager waveMgr)
        {
            if (waveMgr == null) return;
            var spawns = waveMgr.HeroSpawns; // kommen aus dem Map-Prefab (am WaveManager)
            if (spawns == null || spawns.Count == 0) return;      // keine Slots auf der Map

            int max = Mathf.Min(spawns.Count, heroIds != null ? heroIds.Count : 0);
            for (int i = 0; i < max; i++)
            {
                var heroId = heroIds[i];
                if (string.IsNullOrEmpty(heroId)) continue;

                var def = ResolveHeroDef(heroId);
                var prefab = GetHeroPrefab(def);
                if (prefab == null)
                {
                    DebugManager.Warning($"No prefab for hero '{heroId}'. Skipping.", "Map");
                    continue;
                }

                var spawnTr = spawns[i];
                var go = Instantiate(prefab, spawnTr.position, spawnTr.rotation);
                var hc = go.GetComponent<HeroController>();
                if (hc != null)
                {
                    //Progress übertragen
                    HeroProgressData hpg = GameManager.Instance.Profile.GetOrCreateHeroProgress(heroId);

                    hc.Init(def,hpg); // setzt Team=Player u.a. und baut AutoAttack/Skills im Start() auf
                    hc.OnHeroDied += OnHeroDiedInWave;
                }
                else
                {
                    DebugManager.Warning($"Spawned hero '{heroId}' has no HeroController!", "Map");
                }

                _activeHeroes.Add(hc.gameObject);
            }
        }

        private HeroDef ResolveHeroDef(string heroId)
        {
            if (string.IsNullOrEmpty(heroId)) return null;
            return GameManager.Instance.HeroCatalogue != null ? GameManager.Instance.HeroCatalogue.GetById(heroId) : null;
        }

        private GameObject GetHeroPrefab(HeroDef def)
        {
            // Annahme: Dein HeroDef enthÃ¤lt ein Prefab-Feld (falls nicht, nutze heroFallbackPrefab)
            var prefab = def != null ? def.Prefab : null; // falls dein Feld anders heiÃŸt: anpassen
            return prefab != null ? prefab : heroFallbackPrefab;
        }

        internal void SetSelectedHeroes(List<string> heroIds)
        {
            _pendingSelectedHeroes = heroIds != null ? new List<string>(heroIds) : null;
        }

/// <summary>
/// Checks if there is a next wave available.
/// </summary>
/// <returns>True if there is a next wave; otherwise, false.</returns>
        public bool HasNextWave()
        {
            return CurrentWave < MaxWaves;
        }

/// <summary>
/// Called when a wave is completed, handling success and rewards.
/// </summary>
/// <param name="success">Indicates if the wave was completed successfully.</param>
/// <param name="rewards">The rewards earned from the completed wave.</param>
        public void OnWaveCompleted(bool success, WaveRewards rewards)
        {
            
            if (!success)
            {
                GameManager.Instance.SetState(GameState.WaveReward);
                var rewardUI = waveRewardUI.GetComponent<WaveRewardUI>();
                rewardUI.Show(true);
                rewardUI.populateText(success);
                //TODO: Show missed rewards
                return;
            }

            // Hero-XP wurde bereits in WaveManager.TransferRewardsToProfile → GrantHeroXpForWave verteilt.
            // Die Runtime-HeroInstanzen haben keine zusätzlichen Änderungen, die zurück ins Profil müssen.
            //SyncActiveHeroesToProfile();

            if (CurrentWave < MaxWaves)
            {
                GameManager.Instance.SetState(GameState.WaveReward);
                var rewardUI = waveRewardUI.GetComponent<WaveRewardUI>();
                rewardUI.Show(true);
                rewardUI.populateText(success);
                //show collected rewards
                GameManager.Instance.Stats.OnWaveCompleted(CurrentMap.mapId, CurrentWave, CurrentMap.difficulty);
            }
            else
            {
                GameManager.Instance.SetState(GameState.MapReward);
                var maprewardUI = mapRewardUI.GetComponent<MapRewardUI>();
                maprewardUI.Show(true);
                maprewardUI.populateText(success);
                GameManager.Instance.Stats.OnWaveCompleted(CurrentMap.mapId, CurrentWave, CurrentMap.difficulty);
                GameManager.Instance.Stats.OnMapCompleted(CurrentMap.mapId, CurrentMap.difficulty);
            }

        }

        private void SyncActiveHeroesToProfile()
        {
            var gm = GameManager.Instance;
            var profile = gm != null ? gm.Profile : null;
            if (profile == null)
                return;

            if (_activeHeroes == null || _activeHeroes.Count == 0)
                return;

            int synced = 0;

            foreach (var go in _activeHeroes)
            {
                if (go == null) continue;

                var hc = go.GetComponent<HeroController>();
                if (hc == null) continue;

                var inst = hc.RuntimeHeroInstance;
                if (inst == null || inst.heroDef == null) continue;

                profile.UpdateHeroProgressFromInstance(inst);
                synced++;
            }

            if (synced > 0)
            {
                DebugManager.Log($"[MapManager] Synced progress for {synced} hero instance(s) to PlayerProfile.",
                    DebugManager.EDebugLevel.Debug, "Hero");
            }
        }

        private void ApplyHeroXpViaTempInstance(HeroProgressData progress, int xpAmount)
        {
            if (progress == null || xpAmount <= 0)
                return;

            var gm = GameManager.Instance;
            if (gm == null || gm.HeroCatalogue == null)
            {
                DebugManager.Warning("[MapManager] ApplyHeroXpViaTempInstance: GameManager or HeroCatalogue missing.", "Hero");
                return;
            }

            var heroDef = gm.HeroCatalogue.GetById(progress.HeroId);
            if (heroDef == null)
            {
                DebugManager.Warning($"[MapManager] ApplyHeroXpViaTempInstance: No HeroDef found for HeroId='{progress.HeroId}'.", "Hero");
                return;
            }

            // temporäre HeroInstance NUR für XP-/Level-Logik
            var tempInstance = new HeroInstance(heroDef, progress);
            tempInstance.AddXP(xpAmount);          // nutzt HeroXpConfig
            tempInstance.FillProgressData(progress); // schreibt Level/XP/Orbit zurück in Progress
        }

/// <summary>
/// Grants experience points to the hero for completing a wave.
/// </summary>
/// <param name="totalWaveXp">The total experience points to grant.</param>
        public void GrantHeroXpForWave(int totalWaveXp)
        {
            if (totalWaveXp <= 0)
                return;

            var gm = GameManager.Instance;
            var profile = gm != null ? gm.Profile : null;
            if (profile == null)
            {
                DebugManager.Warning("[MapManager] GrantHeroXpForWave: No PlayerProfile available.", "Hero");
                return;
            }

            if (_pendingSelectedHeroes == null || _pendingSelectedHeroes.Count == 0)
            {
                DebugManager.Warning("[MapManager] GrantHeroXpForWave: No selected heroes for this map.", "Hero");
                return;
            }

            // Nur valide IDs zählen
            int heroCount = 0;
            foreach (var heroId in _pendingSelectedHeroes)
            {
                if (!string.IsNullOrEmpty(heroId))
                    heroCount++;
            }

            if (heroCount <= 0)
                return;

            // Basis-Share pro Held (Team teilt die XP)
            int baseShare = Mathf.Max(1, totalWaveXp / heroCount);

            // Safety: HeroXPConfig vorhanden?
            var heroXpConfig = BalanceManager.GetHeroXP();
            if (heroXpConfig == null)
            {
                DebugManager.Warning("[MapManager] GrantHeroXpForWave: No HeroXPConfig set in BalanceManager.", "Hero");
                return;
            }

            int aliveCount = 0;
            int deadCount = 0;

            foreach (var heroId in _pendingSelectedHeroes)
            {
                if (string.IsNullOrEmpty(heroId))
                    continue;

                bool diedThisWave = _heroesDiedThisWave.Contains(heroId);

                int xpForHero = diedThisWave
                    ? Mathf.FloorToInt(baseShare * 0.25f) // 25% für Tote
                    : baseShare;                           // 100% für Überlebende

                if (xpForHero <= 0)
                    continue;

                var progress = profile.GetOrCreateHeroProgress(heroId);
                if (progress == null)
                    continue;

                ApplyHeroXpViaTempInstance(progress, xpForHero);

                if (diedThisWave) deadCount++;
                else aliveCount++;
            }

            DebugManager.Log(
                $"[MapManager] Hero XP for wave {CurrentWave}: totalXP={totalWaveXp}, baseShare={baseShare}, alive={aliveCount}, dead={deadCount}.",
                DebugManager.EDebugLevel.Debug,
                "Hero");
        }

        private void OnHeroDiedInWave(HeroController ctrl)
        {
            if (ctrl == null || ctrl.HeroDef == null)
                return;

            var heroId = ctrl.HeroDef.HeroId;
            if (string.IsNullOrEmpty(heroId))
                return;

            if (_heroesDiedThisWave.Add(heroId))
            {
                DebugManager.Log(
                    $"[MapManager] Hero {heroId} marked as dead for wave {CurrentWave}.",
                    DebugManager.EDebugLevel.Debug,
                    "Hero");
            }
        }

        /// <summary>
        /// Sets whether all waves should start automatically.
        /// </summary>
        /// <param name="enabled">True to enable auto-start for all waves; false to disable.</param>
        public void SetAutoStartAllWaves(bool enabled)
        {
            _autoStartAllWaves = enabled;
            DebugManager.Info($"AutoStartAllWaves set to {enabled}","UI");
        }

        [ContextMenu("Debug/Start Next Wave")]
/// <summary>
/// Advances to the next wave in the game.
/// </summary>
        public void NextWave()
        {
            CurrentWave++;
            GameManager.Instance.SetState(GameState.MapPhase);
            StartWave();
        }

    }
}
