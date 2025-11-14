using CHAL.Data;
using System;
using System.Collections.Generic;

namespace CHAL.Systems.Stats
{
    public sealed class StatisticsSnapshot
    {
        public Dictionary<string, long> Counters = new Dictionary<string, long>();
    }


    public sealed class StatisticsService : IStatisticsService
    {
        // TODO: Falls du DI verwendest, hier ggf. DebugManager / Config / SaveSystem-Access injizieren.
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
             * END TODO */
        }

        
        public StatisticsSnapshot CreateSnapshot()
        {
            var snapshot = new StatisticsSnapshot();
            foreach (var kvp in _counters)
            {
                snapshot.Counters[kvp.Key] = kvp.Value;
            }

            return snapshot;
        }

        public void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> basetags, List<string> bonustags)
        {
            // TODO: Feingranulare Counter-Strategie definieren (nach Tags, Map, Rank, etc.).
            Increment("kills.total");
            Increment($"kills.id.{enemyId}");
            Increment($"kills.rank.{rank}");
            //Increment($"kills.map.{mapId}");
            //Increment($"kills.wave.{mapId}.{waveIndex}");
            foreach (string tag in basetags)
            { 
                Increment($"kills.tag.{tag}");
            }
            foreach (string t in bonustags)
            {
                Increment($"kills.bonustag.{t}");
            }

            OnEnemyKilledEvent?.Invoke(enemyId, rank, basetags, bonustags);

        }


        public void OnWaveCompleted(int mapId, int waveIndex, MapDifficulty difficulty)
        {
            Increment("waves.completed.total");
            Increment($"waves.completed.map.{mapId}.{difficulty}");
            Increment($"waves.completed.map.{mapId}.{difficulty}.wave.{waveIndex}");

            OnWaveCompletedEvent?.Invoke(mapId, waveIndex, difficulty); 
        }


        public void OnMapCompleted(int mapId, MapDifficulty difficultyId)
        {
            Increment("maps.completed.total");
            Increment($"maps.completed.map.{mapId}.{difficultyId}");
            Increment($"maps.completed.difficulty.{difficultyId}");

            OnMapCompletedEvent?.Invoke(mapId, difficultyId);
        }


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
