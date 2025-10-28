# Assets/src/Data/Defs/WaveDef.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `WaveDef` class for configuring enemy wave properties in a game.
- Provides a method to create a `WaveComposition` based on the defined wave settings.

## Public API
- Namespace: `CHAL.Data`
- Types
  - **public class** `WaveDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `int spawnCount` - Number of enemies to spawn.
      - `int normalCount` - Number of normal enemies.
      - `int magicCount` - Number of magic enemies.
      - `int eliteCount` - Number of elite enemies.
      - `int bossCount` - Number of bosses.
      - `int championCount` - Number of champions.
      - `int maxTagsPerEnemy` - Maximum tags allowed per enemy.
      - `int maxElites` - Maximum number of elite enemies.
      - `int maxBosses` - Maximum number of bosses.
      - `int maxChampions` - Maximum number of champions.
      - `BackloadProfile backload` - Configuration for spawn delays.
    - Public methods:
      - `WaveComposition ToComposition(int baseLevel, MapDifficulty difficulty)` - Creates a `WaveComposition` based on the wave definition.

  - **[Serializable] public struct** `BackloadProfile`
    - Public fields/properties:
      - `float alphaSpawnDelay` - Delay for spawn enemies.
      - `float alphaNormalDelay` - Delay for normal enemies.
      - `float alphaMagicDelay` - Delay for magic enemies.
      - `float alphaEliteDelay` - Delay for elite enemies.
      - `float alphaBossDelay` - Delay for bosses.
      - `float alphaChampionDelay` - Delay for champions.
    - Public methods:
      - `float GetSpawnDelayAlpha(EnemyRank r)` - Returns the spawn delay for the specified enemy rank.

## Key Behavior & Side Effects
- `ToComposition` method constructs a `WaveComposition` with a specified base level and difficulty, initializing an empty list of monsters.

## Constraints & Failure Modes
- The `maxTagsPerEnemy`, `maxElites`, `maxBosses`, and `maxChampions` fields impose limits on the configuration of enemy types.
- The `BackloadProfile` struct uses ranges for delay values, ensuring they remain within specified bounds.

## Example
```csharp
WaveDef waveDef = ScriptableObject.CreateInstance<WaveDef>();
waveDef.spawnCount = 10;
WaveComposition composition = waveDef.ToComposition(1, MapDifficulty.Normal);
```

## Unknowns
- The implementation details of `WaveComposition` and `MapDifficulty` are not provided in this file.
- The behavior of the `WaveManager` that fills the `Monsters` list in `WaveComposition` is not defined here.
```
