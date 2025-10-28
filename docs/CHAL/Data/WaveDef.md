# CHAL.Data.WaveDef

_Automatically generated/updated from `Assets/src/Data/Defs/WaveDef.cs`._

# Purpose
- Defines a `WaveDef` class for configuring enemy wave parameters in a game.
- Provides a method to create a `WaveComposition` based on the defined wave settings.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class WaveDef : ScriptableObject**
    - Public fields/properties:
      - `int spawnCount` - Number of enemies to spawn.
      - `int normalCount` - Number of normal enemies.
      - `int magicCount` - Number of magic enemies.
      - `int eliteCount` - Number of elite enemies.
      - `int bossCount` - Number of boss enemies.
      - `int championCount` - Number of champion enemies.
      - `int maxTagsPerEnemy` - Maximum tags allowed per enemy.
      - `int maxElites` - Maximum number of elite enemies.
      - `int maxBosses` - Maximum number of boss enemies.
      - `int maxChampions` - Maximum number of champion enemies.
      - `BackloadProfile backload` - Configuration for spawn delays.
    - Public methods:
      - `WaveComposition ToComposition(int baseLevel, MapDifficulty difficulty)` - Creates a `WaveComposition` based on the wave definition.

  - **[Serializable] public struct BackloadProfile**
    - Public fields/properties:
      - `float alphaSpawnDelay` - Delay for spawn enemies.
      - `float alphaNormalDelay` - Delay for normal enemies.
      - `float alphaMagicDelay` - Delay for magic enemies.
      - `float alphaEliteDelay` - Delay for elite enemies.
      - `float alphaBossDelay` - Delay for boss enemies.
      - `float alphaChampionDelay` - Delay for champion enemies.
    - Public methods:
      - `float GetSpawnDelayAlpha(EnemyRank r)` - Returns the spawn delay for the specified enemy rank.

# Key Behavior & Side Effects
- The `ToComposition` method constructs a `WaveComposition` object initialized with the provided base level and difficulty, and an empty list of monsters.

# Constraints & Failure Modes
- The `BackloadProfile` struct uses ranges for delay values (0 to 5).
- The `maxTagsPerEnemy`, `maxElites`, `maxBosses`, and `maxChampions` fields impose limits on enemy configurations.

# Example
```csharp
WaveDef waveDef = ScriptableObject.CreateInstance<WaveDef>();
waveDef.spawnCount = 10;
WaveComposition composition = waveDef.ToComposition(1, MapDifficulty.Normal);
```

# Unknowns
- The implementation details of `WaveComposition` and `EnemyStruct` are not provided.
- The `MapDifficulty` type is not defined in this file.

