# Assets/src/Core/IStatisticsService.cs

_Automatically generated/updated from `Assets/src/Core/IStatisticsService.cs`._

# Purpose
- Defines the `IStatisticsService` interface for tracking game statistics.

# Public API
- Namespace: `CHAL.Systems.Stats`
- Types
  - public interface `IStatisticsService`
    - Public fields/properties:
      - `IReadOnlyDictionary<string, long> Counters`: Provides access to the statistics counters.
    - Public methods:
      - `void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> tags, List<string> bonsutags)`: Records an enemy kill event.
      - `void OnWaveCompleted(int mapId, int waveIndex, MapDifficulty difficulty)`: Records the completion of a wave.
      - `void OnMapCompleted(int mapId, MapDifficulty difficultyId)`: Records the completion of a map.
      - `void OnCraftExecuted(string recipeId)`: Records the execution of a crafting recipe.
      - `void OnHeroGainedXp(string heroId, long amount)`: Records experience gained by a hero.
      - `void OnSessionStarted()`: Records the start of a game session.

# Key Behavior & Side Effects
- Each method is intended to log specific game events related to statistics tracking.

# Constraints & Failure Modes
- No explicit guards or error handling are defined in the interface. 
- Assumes valid inputs for all method parameters. 

# Example
```csharp
public class StatisticsService : IStatisticsService
{
    public IReadOnlyDictionary<string, long> Counters { get; private set; }

    public void OnEnemyKilled(string enemyId, EnemyRank rank, List<string> tags, List<string> bonsutags) { /* implementation */ }
    public void OnWaveCompleted(int mapId, int waveIndex, MapDifficulty difficulty) { /* implementation */ }
    public void OnMapCompleted(int mapId, MapDifficulty difficultyId) { /* implementation */ }
    public void OnCraftExecuted(string recipeId) { /* implementation */ }
    public void OnHeroGainedXp(string heroId, long amount) { /* implementation */ }
    public void OnSessionStarted() { /* implementation */ }
}
```

# Unknowns
- No information on the implementation details of the methods or the behavior of the `EnemyRank` and `MapDifficulty` types.
