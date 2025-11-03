# CHAL.Data.WaveDef

_Automatically generated/updated from `Assets/src/Data/Defs/WaveDef.cs`._

1) Purpose
- Define a WaveDef as a ScriptableObject that describes a template for a wave of enemies.
- Expose structure counts, constraints, and per-rank spawn-delay configuration.
- Provide a method to build a WaveComposition from this template.

2) Public API

- Namespace: CHAL.Data

- public class WaveDef : ScriptableObject
  - Public fields
    - int spawnCount
    - int normalCount
    - int magicCount
    - int eliteCount
    - int bossCount
    - int championCount
    - int maxTagsPerEnemy = 2
    - int maxElites = 2
    - int maxBosses = 1
    - int maxChampions = 0
    - BackloadProfile backload = new BackloadProfile
      - alphaSpawnDelay = 0f
      - alphaNormalDelay = 0f
      - alphaMagicDelay = 0f
      - alphaEliteDelay = 1.5f
      - alphaBossDelay = 2f
      - alphaChampionDelay = 5f
  - Public methods
    - WaveComposition ToComposition(int baseLevel, MapDifficulty difficulty)
      - Returns a new WaveComposition with:
        - Level = baseLevel
        - Difficulty = difficulty
        - Monsters = new List<EnemyStruct>() // wird von WaveManager befllt

- public struct BackloadProfile
  - Public fields
    - [Range(0f, 5f)] public float alphaSpawnDelay
    - [Range(0f, 5f)] public float alphaNormalDelay
    - [Range(0f, 5f)] public float alphaMagicDelay
    - [Range(0f, 5f)] public float alphaEliteDelay
    - [Range(0f, 5f)] public float alphaBossDelay
    - [Range(0f, 5f)] public float alphaChampionDelay
  - Public methods
    - public float GetSpawnDelayAlpha(EnemyRank r) => r switch
      - EnemyRank.Spawn => alphaSpawnDelay
      - EnemyRank.Normal => alphaNormalDelay
      - EnemyRank.Magic => alphaMagicDelay
      - EnemyRank.Elite => alphaEliteDelay
      - EnemyRank.Boss => alphaBossDelay
      - EnemyRank.Champion => alphaChampionDelay
      - _ => 0f

3) Key Behavior & Side Effects
- WaveDef.ToComposition(baseLevel, difficulty)
  - Produces a WaveComposition with Level, Difficulty, and an empty Monsters list (to be filled by WaveManager).
- BackloadProfile.GetSpawnDelayAlpha(r)
  - Returns the per-rank alpha delay corresponding to r via a switch expression.
  - Returns 0f for an unknown rank.

4) Constraints & Failure Modes
- Public fields are not validated in this file; UI ranges are suggested by [Range] attributes on BackloadProfile fields.
- BackloadProfile fields default to 0f for most delays, except defined defaults in WaveDef.backload initializer.
- The code relies on external types (WaveComposition, EnemyStruct, EnemyRank, MapDifficulty) defined elsewhere; their behavior is not specified here.
- CreateAssetMenu attribute enables Unity editor creation of WaveDef assets; runtime usage depends on project setup.

5) Example
- Not derivable from this file alone; no standalone usage snippet provided.

6) Unknowns
- Definitions and behavior of WaveComposition, EnemyStruct, EnemyRank, MapDifficulty.
- How WaveManager consumes and populates WaveComposition.Monsters.
- Any runtime validation or interoperability rules beyond what is explicit here.

