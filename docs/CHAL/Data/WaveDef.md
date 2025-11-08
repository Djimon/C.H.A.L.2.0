# CHAL.Data.WaveDef

_Automatically generated/updated from `Assets/src/Data/Defs/WaveDef.cs`._

# Purpose
- Defines a wave definition for spawning enemies in the game, including parameters for different enemy types and constraints.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `WaveDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `int spawnCount`: Total number of enemies to spawn.
      - `int normalCount`: Number of normal enemies.
      - `int magicCount`: Number of magic enemies.
      - `int eliteCount`: Number of elite enemies.
      - `int bossCount`: Number of boss enemies.
      - `int championCount`: Number of champion enemies.
      - `int maxTagsPerEnemy`: Maximum tags allowed per enemy.
      - `int maxElites`: Maximum number of elite enemies.
      - `int maxBosses`: Maximum number of boss enemies.
      - `int maxChampions`: Maximum number of champion enemies.
      - `BackloadProfile backload`: Configuration for spawn delays.
    - Public methods:
      - `WaveComposition ToComposition(int baseLevel, MapDifficulty difficulty)`: Creates a `WaveComposition` from this template.

  - [Serializable] public struct `BackloadProfile`
    - Public fields/properties:
      - `float alphaSpawnDelay`: Delay for spawn enemies.
      - `float alphaNormalDelay`: Delay for normal enemies.
      - `float alphaMagicDelay`: Delay for magic enemies.
      - `float alphaEliteDelay`: Delay for elite enemies.
      - `float alphaBossDelay`: Delay for boss enemies.
      - `float alphaChampionDelay`: Delay for champion enemies.
    - Public methods:
      - `float GetSpawnDelayAlpha(EnemyRank r)`: Gets the spawn delay alpha based on the enemy rank.

# Key Behavior & Side Effects
- The `ToComposition` method constructs a `WaveComposition` object with specified base level and difficulty, initializing the `Monsters` list.

# Constraints & Failure Modes
- The `BackloadProfile` struct uses ranges for delay values (0 to 5).
- The `maxTagsPerEnemy`, `maxElites`, `maxBosses`, and `maxChampions` fields impose constraints on enemy types.

# Example
```csharp
WaveDef waveDef = ScriptableObject.CreateInstance<WaveDef>();
waveDef.spawnCount = 10;
waveDef.normalCount = 5;
WaveComposition composition = waveDef.ToComposition(1, MapDifficulty.Normal);
```

# Unknowns
- The implementation details of `WaveComposition` and `MapDifficulty` are not provided in this file.
- The behavior of the `WaveManager` that populates the `Monsters` list is not defined here.

