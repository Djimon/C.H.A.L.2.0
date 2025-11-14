using CHAL.Data;
using System.Collections.Generic;

namespace CHAL.Systems.Stats
{

    public interface IStatisticsService
    {

        IReadOnlyDictionary<string, long> Counters { get; }


        void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> tags, List<string> bonsutags);


        void OnWaveCompleted(int mapId, int waveIndex, MapDifficulty difficulty);


        void OnMapCompleted(int mapId, MapDifficulty difficultyId);


        void OnCraftExecuted(string recipeId);


        void OnHeroGainedXp(string heroId, long amount);

 
        void OnSessionStarted();
    }
}