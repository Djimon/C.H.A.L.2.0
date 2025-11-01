# CHAL.Data.MapDef

_Automatically generated/updated from `Assets/src/Data/Defs/MapDef.cs`._

Purpose
- Defines MapDef as a ScriptableObject data container for map configuration in CHAL.Data.
- Exposes serialized configuration fields categorized as meta, gameplay, enemy pools, and wave templates.
- Enables asset creation via Unity’s CreateAssetMenu with fileName "MapDef" and menuName "Data/Map Definition".

Public API
- Namespace: CHAL.Data
- Type: public class MapDef : ScriptableObject
  - Public fields (with brief roles):
    - public int mapId
      - Internal/map identifier (e.g., "desert_01")
    - public string displayNameKey
      - Localization key for map display name (e.g., "MAP_DESERT")
    - public Sprite previewImage
      - Image used in map selection UI
    - public GameObject mapPrefab
      - The actual environment instance, instantiated by MapManager
    - public int baseLevel
      - Starting level for enemies
    - public int maxWaves
      - Number of waves per map
    - public MapDifficulty difficulty
      - Base difficulty for this map
    - public int heroSlots
      - Number of hero slots available
    - public List<EnemyDef> allowedEnemies
      - Permitted enemy definitions for this map
    - public List<string> allowedModifiers
      - Permitted modifier keys for this map
    - public List<WaveDef> waveDefs
      - Wave templates (concrete now; constraints later)
    - public int subWaveCount
      - Number of sub-waves per wave
    - public float interSubWaveDelay
      - Delay between sub-waves
    - public int maxConCurrentEnemies
      - Maximum concurrent enemies allowed

Key Behavior & Side Effects
- No runtime methods or behavior defined in this file.
- Behavior implied by usage:
  - mapPrefab is instantiated by MapManager; no instantiation logic present here.
- Overall, this is a data container; all logic is external to this class.

Constraints & Failure Modes
- No explicit guards or validation present.
- Potential null lists if not assigned (allowedEnemies, allowedModifiers, waveDefs).
- Serialized fields default to Unity defaults; several numeric fields have explicit defaults:
  - baseLevel = 1
  - maxWaves = 5
  - heroSlots = 1
  - subWaveCount = 5
  - interSubWaveDelay = 10f
  - maxConCurrentEnemies = 25
- Dependencies on other types (MapDifficulty, EnemyDef, WaveDef) not defined in this file; their behavior/validation is external.

Unknowns
- Definitions and constraints of MapDifficulty, EnemyDef, WaveDef.
- Any runtime validation or loading semantics beyond asset configuration.
- How null/empty configurations are handled by consuming systems.
- Exact usage of localization key displayNameKey beyond being stored as a string.
