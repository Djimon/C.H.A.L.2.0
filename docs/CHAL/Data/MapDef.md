# CHAL.Data.MapDef

_Automatically generated/updated from `Assets/src/Data/Defs/MapDef.cs`._

# Purpose
- Defines a map definition used in the game, including metadata and gameplay settings.

# Public API
- Namespace: CHAL.Data
- Types
  - public class MapDef : ScriptableObject
    - Public fields/properties:
      - int mapId: Internal ID (e.g., "desert_01").
      - string displayNameKey: Key for localization (e.g., "MAP_DESERT").
      - Sprite previewImage: For MapSelectionUI.
      - GameObject mapPrefab: The actual environment, instantiated by MapManager.
      - int baseLevel: Start level of enemies (default is 1).
      - int maxWaves: Number of waves per map (default is 5).
      - MapDifficulty difficulty: Base difficulty of this map.
      - int heroSlots: Number of hero slots (default is 1).
      - List<EnemyDef> allowedEnemies: List of enemy definitions allowed on this map.
      - List<string> allowedModifiers: List of allowed modifiers for the map.
      - List<WaveDef> waveDefs: Currently specific wave definitions.
      - int subWaveCount: Number of sub-waves (default is 5).
      - float interSubWaveDelay: Delay between sub-waves (default is 10f).
      - int maxConCurrentEnemies: Maximum concurrent enemies (default is 25).

# Key Behavior & Side Effects
- None explicitly defined in the file.

# Constraints & Failure Modes
- None explicitly defined in the file.

# Example
- Not applicable.

# Unknowns
- None.

