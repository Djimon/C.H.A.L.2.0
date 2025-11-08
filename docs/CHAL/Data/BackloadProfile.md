# CHAL.Data.BackloadProfile

_Automatically generated/updated from `Assets/src/Data/Defs/WaveDef.cs`._

1) Purpose
- Defines a wave configuration for spawning enemies in the game.
- Contains parameters for different enemy types and their constraints.

2) Public API
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
      - `WaveComposition ToComposition(int baseLevel, MapDifficulty difficulty)`: Creates a `WaveComposition` based on the wave definition.

  - [Serializable] public struct `BackloadProfile`
    - Public fields/properties:
      - `float alphaSpawnDelay`: Delay for spawn enemies.
      - `float alphaNormalDelay`: Delay for normal enemies.
      - `float alphaMagicDelay`: Delay for magic enemies.
      - `float alphaEliteDelay`: Delay for elite enemies.
      - `float alphaBossDelay`: Delay for boss enemies.
      - `float alphaChampionDelay`: Delay for champion enemies.
    - Public methods:
      - `float GetSpawnDelayAlpha(EnemyRank r)`: Returns the spawn delay alpha based on the enemy rank.

3) Key Behavior & Side Effects
- The `ToComposition` method constructs a `WaveComposition` object initialized with the specified base level and difficulty, and an empty list of monsters.

4) Constraints & Failure Modes
- The `BackloadProfile` fields are constrained to a range of 0 to 5 for spawn delays.
- The `maxTagsPerEnemy`, `maxElites`, `maxBosses`, and `maxChampions` fields define limits on enemy types.

5) Example
```csharp
WaveDef waveDef = ScriptableObject.CreateInstance<WaveDef>();
waveDef.spawnCount = 10;
waveDef.normalCount = 5;
WaveComposition composition = waveDef.ToComposition(1, MapDifficulty.Normal);
```

6) Unknowns
- The implementation details of `WaveComposition` and `MapDifficulty` are not provided in this file.
