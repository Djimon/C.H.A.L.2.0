# Assets/src/Data/Defs/MapDef.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `MapDef` class as a ScriptableObject for map definitions in the game.
- Provides metadata, gameplay settings, enemy pools, and wave templates for each map.

## Public API
- Namespace: `CHAL.Data`
- Types
  - `public class MapDef : ScriptableObject`
    - Public fields/properties:
      - `int mapId` - Internal ID for the map.
      - `string displayNameKey` - Key for localization.
      - `Sprite previewImage` - Image for map selection UI.
      - `GameObject mapPrefab` - Environment prefab instantiated by MapManager.
      - `int baseLevel` - Starting level of enemies.
      - `int maxWaves` - Number of waves per map.
      - `MapDifficulty difficulty` - Base difficulty of the map.
      - `int heroSlots` - Number of hero slots available.
      - `List<EnemyDef> allowedEnemies` - List of enemy definitions allowed on the map.
      - `List<string> allowedModifiers` - List of modifiers allowed on the map.
      - `List<WaveDef> waveDefs` - List of wave definitions for the map.
      - `int subWaveCount` - Number of sub-waves.
      - `float interSubWaveDelay` - Delay between sub-waves.
      - `int maxConCurrentEnemies` - Maximum concurrent enemies allowed.

## Key Behavior & Side Effects
- The `MapDef` class is used to define properties and settings for different maps in the game.
- Instances of `MapDef` are created as ScriptableObjects, allowing for easy configuration and management of map data.

## Constraints & Failure Modes
- No explicit guards or null/empty handling are defined in the provided code.
- Assumes that all lists (e.g., `allowedEnemies`, `allowedModifiers`, `waveDefs`) are initialized before use.

## Example
```csharp
MapDef myMap = ScriptableObject.CreateInstance<MapDef>();
myMap.mapId = 1;
myMap.displayNameKey = "MAP_DESERT";
myMap.baseLevel = 1;
```

## Unknowns
- The behavior of `MapDifficulty`, `EnemyDef`, and `WaveDef` types is not defined in this file.
- No information on how `MapManager` interacts with `mapPrefab`.
```
