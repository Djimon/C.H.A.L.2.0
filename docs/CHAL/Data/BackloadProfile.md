# CHAL.Data.BackloadProfile

_Automatically generated/updated from `Assets/src/Data/Defs/WaveDef.cs`._

# WaveDef.cs Documentation

## Purpose
- Defines a `WaveDef` class for configuring enemy wave properties in a game.
- Provides a `BackloadProfile` struct for managing spawn delays based on enemy rank.

## Public API
- Namespace: `CHAL.Data`
- Types:
  - **public class WaveDef : ScriptableObject**
    - Public fields/properties:
      - `int spawnCount`: Number of enemies to spawn.
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
      - `WaveComposition ToComposition(int baseLevel, MapDifficulty difficulty)`: Creates a `WaveComposition` based on the current `WaveDef`.

  - **[Serializable] public struct BackloadProfile**
    - Public fields/properties:
      - `float alphaSpawnDelay`: Delay for spawn enemies.
      - `float alphaNormalDelay`: Delay for normal enemies.
      - `float alphaMagicDelay`: Delay for magic enemies.
      - `float alphaEliteDelay`: Delay for elite enemies.
      - `float alphaBossDelay`: Delay for boss enemies.
      - `float alphaChampionDelay`: Delay for champion enemies.
    - Public methods:
      - `float GetSpawnDelayAlpha(EnemyRank r)`: Returns the spawn delay for the specified enemy rank.

## Key Behavior & Side Effects
- `ToComposition` method constructs a `WaveComposition` with a specified base level and difficulty, initializing an empty list of monsters.

## Constraints & Failure Modes
- The `BackloadProfile` struct uses ranges for delay values (0 to 5).
- No explicit error handling is defined in the methods.

## Example
```csharp
WaveDef waveDef = ScriptableObject.CreateInstance<WaveDef>();
waveDef.spawnCount = 10;
WaveComposition composition = waveDef.ToComposition(1, MapDifficulty.Normal);
```

## Unknowns
- The behavior of `WaveComposition` and `MapDifficulty` is not defined in this file.
- The implementation details of how `WaveManager` fills the `Monsters` list are not provided.

