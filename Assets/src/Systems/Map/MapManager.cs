using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Wave;
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

            DebugManager.Log($"Starte Wave {CurrentWave}/{MaxWaves}", DebugManager.EDebugLevel.Test, "Map");

            _waveManager.StartWave(CurrentMap, CurrentWave, this);
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
