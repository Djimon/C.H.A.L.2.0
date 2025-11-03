using System.Collections.Generic;
using CHAL.Data;

namespace CHAL.Systems.Research
{
    public sealed class ResearchEventBridge
    {
        private readonly ResearchService _service;

        public ResearchEventBridge(ResearchService service)
        {
            _service = service;
        }

        // Aufruf z.B. im WaveManager:
/// <summary>
/// Called when a wave is completed.
/// </summary>
        public void OnWaveCompleted()
        {
            _service.ApplyWaveCompleted();
        }

        // Aufruf z.B. im MapManager (falls du MapDifficulty schon fÃ¼hrst; sonst boolâ†’Mapping machen)
/// <summary>
/// Called when the map is completed, applying the specified difficulty.
/// </summary>
/// <param name="difficulty">The difficulty level of the completed map.</param>
        public void OnMapCompleted(MapDifficulty difficulty)
        {
            _service.ApplyMapCompleted(difficulty);
        }

        // Aufruf z.B. im Death/Combat-Handler
/// <summary>
/// Handles the event when an enemy is killed.
/// </summary>
/// <param name="enemyTags">A list of tags associated with the killed enemy.</param>
/// <param name="rank">The rank of the killed enemy.</param>
        public void OnEnemyKilled(IReadOnlyList<string> enemyTags, EnemyRank rank)
        {
            _service.ApplyEnemyKilled(enemyTags, rank);
        }
    }
}
