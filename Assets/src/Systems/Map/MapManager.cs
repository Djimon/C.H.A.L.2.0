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
                    hc.Init(def); // setzt Team=Player u.a. und baut AutoAttack/Skills im Start() auf
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
                //Show missed rewards

                return;
            }

            if (CurrentWave < MaxWaves)
            {
                GameManager.Instance.SetState(GameState.WaveReward);
                var rewardUI = waveRewardUI.GetComponent<WaveRewardUI>();
                rewardUI.Show(true);
                rewardUI.populateText(success);
                //show collected rewards

            }
            else
            {
                GameManager.Instance.SetState(GameState.MapReward);
                var maprewardUI = mapRewardUI.GetComponent<MapRewardUI>();
                maprewardUI.Show(true);
                maprewardUI.populateText(success);

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
