# CHAL.Data.BackloadProfile

_Automatically generated/updated from `Assets/src/Data/Defs/WaveDef.cs`._

1) Purpose
- Defines a WaveDef ScriptableObject as a template for game waves, including structure counts and spawn constraints.
- Encapsulates per-rank spawn delay configuration via BackloadProfile.
- Provides a method to build a WaveComposition from the template.

2) Public API
- Namespace/module
  - CHAL.Data

- Types

  - public class WaveDef : ScriptableObject
    - Public fields
      - int spawnCount
      - int normalCount
      - int magicCount
      - int eliteCount
      - int bossCount
      - int championCount
      - int maxTagsPerEnemy (default 2)
      - int maxElites (default 2)
      - int maxBosses (default 1)
      - int maxChampions (default 0)
      - BackloadProfile backload (default instance with specific per-rank delays)
    - Public methods
      - WaveComposition ToComposition(int baseLevel, MapDifficulty difficulty)
        - Returns a new WaveComposition with Level = baseLevel, Difficulty = difficulty, Monsters = new List<EnemyStruct>()

  - public struct BackloadProfile [Serializable]
    - Public fields
      - float alphaSpawnDelay [Range(0f, 5f)]
      - float alphaNormalDelay [Range(0f, 5f)]
      - float alphaMagicDelay [Range(0f, 5f)]
      - float alphaEliteDelay [Range(0f, 5f)]
      - float alphaBossDelay [Range(0f, 5f)]
      - float alphaChampionDelay [Range(0f, 5f)]
    - Public methods
      - float GetSpawnDelayAlpha(EnemyRank r)
        - Returns the corresponding rank delay (Spawn/Normal/Magic/Elite/Boss/Champion) or 0f for unknown ranks

Notes
- WaveDef has [CreateAssetMenu] attribute, enabling asset creation in Unity.
- BackloadProfile is [Serializable] and fields have [Range] attributes for editor validation.

3) Key Behavior & Side Effects
- ToComposition constructs and returns a WaveComposition with:
  - Level set to baseLevel
  - Difficulty set to difficulty
  - Monsters initialized as an empty List<EnemyStruct>() (to be filled later by WaveManager per comment)
- GetSpawnDelayAlpha maps an EnemyRank to its associated delay field via a switch expression.

4) Constraints & Failure Modes
- Editor-enforced ranges: all BackloadProfile delays are constrained to [0f, 5f] via [Range] attributes.
- No runtime guards in ToComposition; it always returns a new WaveComposition with an empty Monsters list.
- Dependencies on external types not defined in this file: WaveComposition, EnemyStruct, MapDifficulty, EnemyRank.

5) Example
```csharp
// Example usage: create a composition from a WaveDef asset
WaveComposition composition = someWaveDef.ToComposition(baseLevel: 5, difficulty: MapDifficulty.Hard);
```

6) Unknowns
- Definitions and behavior of WaveComposition, EnemyStruct, MapDifficulty, and EnemyRank beyond their usage here.
- How WaveManager populates WaveComposition.Monsters or uses the spawn/constraint counts at runtime.
- Any additional editor or runtime validation beyond provided attributes.
