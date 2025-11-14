# Assets/src/Core/StatisticService.cs

_Automatically generated/updated from `Assets/src/Core/StatisticService.cs`._

# Purpose
- Defines the `StatisticsService` for tracking various game statistics.
- Provides a `StatisticsSnapshot` for capturing the current state of statistics.

# Public API
- Namespace: `CHAL.Systems.Stats`
- Types
  - `sealed class StatisticsSnapshot`
    - Public fields/properties:
      - `Dictionary<string, long> Counters`: Stores counters for various statistics.
  - `sealed class StatisticsService`
    - Public fields/properties:
      - `IReadOnlyDictionary<string, long> Counters`: Exposes the current counters.
    - Public methods:
      - `StatisticsSnapshot CreateSnapshot()`: Creates a snapshot of current statistics.
      - `void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> basetags, List<string> bonustags)`: Records an enemy kill.
      - `void OnWaveCompleted(int mapId, int waveIndex, MapDifficulty difficulty)`: Records completion of a wave.
      - `void OnMapCompleted(int mapId, MapDifficulty difficultyId)`: Records completion of a map.
      - `void OnCraftExecuted(string recipeId)`: Records execution of a crafting recipe.
      - `void OnHeroGainedXp(string heroId, long amount)`: Records experience gained by a hero.
      - `void OnHeroLeveledUp(string heroId, int level)`: Records leveling up of a hero.
      - `void OnSessionStarted()`: Records the start of a session.
    - Public events:
      - `event Action<string, EnemyRank, List<string>, List<string>> OnEnemyKilledEvent`: Triggered when an enemy is killed.
      - `event Action<int, int, MapDifficulty> OnWaveCompletedEvent`: Triggered when a wave is completed.
      - `event Action<int, MapDifficulty> OnMapCompletedEvent`: Triggered when a map is completed.
      - `event Action<string> OnCraftExecutedEvent`: Triggered when a crafting recipe is executed.

# Key Behavior & Side Effects
- Increments counters for various events such as enemy kills, wave completions, map completions, crafting, hero experience gain, and session starts.
- Triggers corresponding events for other systems when an enemy is killed, a wave is completed, a map is completed, or a crafting action is executed.

# Constraints & Failure Modes
- Counters are stored in a dictionary; if a key does not exist, it initializes to zero before incrementing.
- No explicit error handling is present for invalid inputs or states.

# Example
```csharp
var statsService = new StatisticsService();
statsService.OnEnemyKilled("enemy_1", EnemyRank.Normal, new List<string> { "tag1" }, new List<string> { "bonus1" });
var snapshot = statsService.CreateSnapshot();
```

# Unknowns
- The implementation details of `IStatisticsService`, `EnemyRank`, and `MapDifficulty` are not provided in this file.
- The potential use of dependency injection for accessing other systems is mentioned but not implemented.
