using CHAL.Core;
using CHAL.Systems.Wave;
using UnityEngine;

namespace CHAL.Systems.Map
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance { get; private set; }

        public int CurrentMapId { get; private set; }
        public int CurrentWave { get; private set; }
        public int MaxWaves { get; private set; }
        public MapDifficulty Difficulty { get; private set; }

        private WaveManager _waveManager;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void Init(WaveManager waveManager)
        {
            _waveManager = waveManager;
        }

        public void StartMap(int mapId, MapDifficulty difficulty, int maxWaves)
        {
            CurrentMapId = mapId;
            Difficulty = difficulty;
            MaxWaves = maxWaves;
            CurrentWave = 1;

            GameManager.Instance.SetState(GameState.MapPhase);
            StartWave();
        }

        public void StartWave()
        {
            DebugManager.Log($"Starte Wave {CurrentWave}/{MaxWaves}",DebugManager.EDebugLevel.Test);
            _waveManager.StartWave();
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

        public void NextWave()
        {
            CurrentWave++;
            GameManager.Instance.SetState(GameState.MapPhase);
            StartWave();
        }
    }
}
