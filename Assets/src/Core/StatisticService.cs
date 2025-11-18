using CHAL.Data;
using System;
using System.Collections.Generic;

namespace CHAL.Systems.Stats
{
    public sealed class StatisticsSnapshot
    {
        public Dictionary<string, long> Counters = new Dictionary<string, long>();
    }


    public sealed class StatisticsService
    {
        private readonly Dictionary<string, long> _counters = new Dictionary<string, long>();

        
        public IReadOnlyDictionary<string, long> Counters => _counters;

        // --- Events nach oben für andere Systeme (Research, Achievements, HUD etc.) ---
        public event Action<string, EnemyRank, List<string>, List<string>> OnEnemyKilledEvent;
        public event Action<int, int, MapDifficulty> OnWaveCompletedEvent;
        public event Action<int, MapDifficulty> OnMapCompletedEvent;
        public event Action<string> OnCraftExecutedEvent;


        public StatisticsService()
        {
            /* TODO
             * - Optional: Initiale Counter aus einem SaveDTO laden
             * - Optional: Basis-Counter vordefinieren (kills.total, maps.completed, etc.)
            */
        }

        
/// <summary>
/// Creates a new snapshot of the current statistics.
/// </summary>
/// <returns>A new instance of <see cref="StatisticsSnapshot"/> containing the current counters.</returns>
        public StatisticsSnapshot CreateSnapshot()
        {
            var snapshot = new StatisticsSnapshot();
            foreach (var kvp in _counters)
            {
                snapshot.Counters[kvp.Key] = kvp.Value;
            }

            return snapshot;
        }

/// <summary>
/// Restores the statistics from a given snapshot.
/// Clears current counters and updates them with values from the snapshot.
/// </summary>
/// <param name="snapshot">The snapshot containing the statistics to restore.</param>
        public void RestoreFromSnapshot(StatisticsSnapshot snapshot)
        {
            _counters.Clear();

            if (snapshot == null || snapshot.Counters == null)
                return;

            foreach (var kvp in snapshot.Counters)
            {
                _counters[kvp.Key] = kvp.Value;
            }
        }

/// <summary>
/// Increments kill statistics when an enemy is killed.
/// </summary>
/// <param name="enemyId">The unique identifier of the enemy.</param>
/// <param name="rank">The rank of the enemy.</param>
/// <param name="basetags">A list of base tags associated with the kill.</param>
/// <param name="bonustags">A list of bonus tags associated with the kill.</param>
/// <summary>
/// Handles the event when an enemy is killed, updating various kill statistics.
/// </summary>
/// <param name="enemyId">The unique identifier of the enemy that was killed.</param>
/// <param name="rank">The rank of the enemy that was killed.</param>
/// <param name="basetags">A list of base tags associated with the kill.</param>
/// <param name="bonustags">A list of bonus tags associated with the kill.</param>
        public void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> basetags, List<string> bonustags)
        {
            Increment("kills.total");
            Increment($"kills.id.{enemyId}");
            Increment($"kills.rank.{rank}");
            //Increment($"kills.map.{mapId}");
            //Increment($"kills.wave.{mapId}.{waveIndex}");
            if (basetags != null)
                foreach (string tag in basetags)
                { 
                    Increment($"kills.tag.{tag}");
                }

            if (bonustags != null)
                foreach (string t in bonustags)
                {
                    Increment($"kills.bonustag.{t}");
                }

            OnEnemyKilledEvent?.Invoke(enemyId, rank, basetags, bonustags);

        }


/// <summary>
/// Invoked when a wave is completed in a map.
/// </summary>
/// <param name="mapId">The identifier of the completed map.</param>
/// <param name="waveIndex">The index of the completed wave.</param>
/// <param name="difficulty">The difficulty level of the completed map.</param>
        public void OnWaveCompleted(int mapId, int waveIndex, MapDifficulty difficulty)
        {
            Increment("waves.completed.total");
            Increment($"waves.completed.map.{mapId}.{difficulty}");
            Increment($"waves.completed.map.{mapId}.{difficulty}.wave.{waveIndex}");

            OnWaveCompletedEvent?.Invoke(mapId, waveIndex, difficulty); 
        }


/// <summary>
/// Called when a map is completed.
/// </summary>
/// <param name="mapId">The identifier of the completed map.</param>
/// <param name="difficultyId">The difficulty level of the completed map.</param>
        public void OnMapCompleted(int mapId, MapDifficulty difficultyId)
        {
            Increment("maps.completed.total");
            Increment($"maps.completed.map.{mapId}.{difficultyId}");
            Increment($"maps.completed.difficulty.{difficultyId}");

            OnMapCompletedEvent?.Invoke(mapId, difficultyId);
        }


/// <summary>
/// Executes the crafting process for a given recipe.
/// </summary>
/// <param name="recipeId">The identifier of the recipe being crafted.</param>
        public void OnCraftExecuted(string recipeId)
        {
            Increment("crafts.total");
            Increment($"crafts.recipe.{recipeId}");

            OnCraftExecutedEvent?.Invoke(recipeId);
        }


        public void OnHeroGainedXp(string heroId, long amount)
        {
            Increment("hero.xp.total", amount);
            Increment($"hero.{heroId}.xp.total", amount);
        }

        public void OnHeroLeveledUp(string heroId,int level)
        {
            Increment("hero.level.total");
            Replace($"hero.{heroId}.level", level);
        }


        public void OnSessionStarted()
        {
            Increment("session.starts");
        }


        private void Increment(string key)
        {
            Increment(key, 1);
        }


        private void Increment(string key, long amount)
        {
            if (!_counters.TryGetValue(key, out var current))
            {
                current = 0;
            }

            _counters[key] = current + amount;

            // TODO: Optional Change-Event raisen oder Debounced-Update für ein HUD triggern.
            // DebugManager.Info($"Statistics: {key} = {_counters[key]}");
        }

        private void Replace(string key, long amount)
        {
            _counters[key] = amount;
        }
    }
}
