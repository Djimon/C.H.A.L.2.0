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


        private WaveManager _waveManager;
        public int CurrentWave { get; private set; } = 1;
        public int MaxWaves => CurrentMap != null ? CurrentMap.maxWaves : 0;

        private void Awake()
        {
            //if (Instance != null && Instance != this)
            //{
            //    Destroy(gameObject);
            //    return;
            //}
            //Instance = this;
            //DontDestroyOnLoad(gameObject);

            //if (waveRewardUI = null)
            //    waveRewardUI = FindFirstObjectByType<WaveRewardUI>().gameObject;

            //if(mapRewardUI = null)
            //    mapRewardUI = FindFirstObjectByType<MapRewardUI>().gameObject;

            HideUI();

        }

        private void Start()
        {
            PrepareMap();
        }

        public void HideUI()
        {
            waveRewardUI.GetComponent<WaveRewardUI>().Show(false);
            mapRewardUI.GetComponent<MapRewardUI>().Show(false);
        }


        public void PrepareMap()
        {
            CurrentMap = GameManager.Instance.pendingMap;
            CurrentWave = 1;

            DebugManager.Log($"Starting Map {CurrentMap.mapId} (Level {CurrentMap.baseLevel}, Waves {CurrentMap.maxWaves})",
                             DebugManager.EDebugLevel.Test, "Map");

            // Szene "04_Map" muss geladen sein → dann Prefab instanzieren
            if (_mapInstancedPrefab != null)
                Destroy(_mapInstancedPrefab);

            if (CurrentMap.mapPrefab != null)
                _mapInstancedPrefab = Instantiate(CurrentMap.mapPrefab);
            else
                DebugManager.Warning("Missing MapPrefab");


            var selectUI = selectHeroUI.GetComponent<HeroSelectionUI>();
            selectUI.Init(this);
            selectUI.Show(true);   
        }

        public void ResetWave() 
        {
            CurrentWave = 1;
            StartWave();
        }

        [ContextMenu("Debug/StartWave")]
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

            DebugManager.Log($"Starte Wave {CurrentWave}/{MaxWaves}", DebugManager.EDebugLevel.Test, "Map");

            _waveManager.StartWave(CurrentMap, CurrentWave, this);
        }

        private void ResetHeroesForNewWave(WaveManager waveMgr)
        {
            if (waveMgr == null) return;

            // Destroy only heroes that belong to the active map instance to avoid touching editor objects or other scenes.
            var existing = _mapInstancedPrefab != null
                ? _mapInstancedPrefab.GetComponentsInChildren<HeroController>(true)
                : Array.Empty<HeroController>();

            int count = 0;
            for (int i = 0; i < existing.Length; i++)
            {
                if (existing[i] != null && existing[i].gameObject != null)
                {
                    Destroy(existing[i].gameObject);
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
            }
        }

        private HeroDef ResolveHeroDef(string heroId)
        {
            if (string.IsNullOrEmpty(heroId)) return null;
            return GameManager.Instance.HeroCatalogue != null ? GameManager.Instance.HeroCatalogue.GetById(heroId) : null;
        }

        private GameObject GetHeroPrefab(HeroDef def)
        {
            // Annahme: Dein HeroDef enthält ein Prefab-Feld (falls nicht, nutze heroFallbackPrefab)
            var prefab = def != null ? def.Prefab : null; // falls dein Feld anders heißt: anpassen
            return prefab != null ? prefab : heroFallbackPrefab;
        }

        internal void SetSelectedHeroes(List<string> heroIds)
        {
            _pendingSelectedHeroes = heroIds != null ? new List<string>(heroIds) : null;
        }

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
                DebugManager.Log("Reward");
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

        [ContextMenu("Debug/Start Next Wave")]
        public void NextWave()
        {
            CurrentWave++;
            GameManager.Instance.SetState(GameState.MapPhase);
            StartWave();
        }

    }
}
