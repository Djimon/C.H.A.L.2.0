using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Wave;
using UnityEngine;

namespace CHAL.Systems.Map
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        [Header("Runtime")]
        public MapDef CurrentMap { get; private set; }
        private GameObject _mapInstancedPrefab;


        private WaveManager _waveManager;
        public int CurrentWave { get; private set; } = 1;
        public int MaxWaves => CurrentMap != null ? CurrentMap.maxWaves : 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }


        public void StartMap(MapDef mapDef)
        {
            CurrentMap = mapDef;
            CurrentWave = 1;

            DebugManager.Log($"Starting Map {mapDef.mapId} (Level {mapDef.baseLevel}, Waves {mapDef.maxWaves})",
                             DebugManager.EDebugLevel.Test, "Map");

            // Szene "04_Map" muss geladen sein → dann Prefab instanzieren
            if (_mapInstancedPrefab != null)
                Destroy(_mapInstancedPrefab);

            if (mapDef.mapPrefab != null)
                _mapInstancedPrefab = Instantiate(mapDef.mapPrefab);

            StartWave();
        }


        public void StartWave()
        {
            _waveManager = _mapInstancedPrefab.GetComponentInChildren<WaveManager>();

            if (_waveManager == null)
            {
                DebugManager.Error("WaveManager not set!", "Map");
                GameManager.Instance.ExitToHideout();
                return;
            }

            DebugManager.Log($"Starte Wave {CurrentWave}/{MaxWaves}", DebugManager.EDebugLevel.Test, "Map");

            _waveManager.StartWave(CurrentMap, CurrentWave);
        }

        public void OnWaveCompleted(bool success)
        {
            if (!success)
            {
                GameManager.Instance.SetState(GameState.WaveReward);
                return;
            }

            if (CurrentWave < MaxWaves)
            {
                GameManager.Instance.SetState(GameState.WaveReward);
            }
            else
            {
                GameManager.Instance.SetState(GameState.MapReward);
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
