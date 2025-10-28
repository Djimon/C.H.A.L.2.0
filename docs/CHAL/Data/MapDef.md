# CHAL.Data.MapDef

_Automatically generated/updated from `Assets/src/Data/Defs/MapDef.cs`._

## Purpose
- Defines a `MapDef` class as a ScriptableObject for map definitions in the game.
- Provides metadata, gameplay settings, enemy pools, and wave templates for maps.

## Public API
- Namespace: `CHAL.Data`
- Types
  - public class `MapDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `int mapId` - Internal ID for the map.
      - `string displayNameKey` - Key for localization.
      - `Sprite previewImage` - Image for map selection UI.
      - `GameObject mapPrefab` - Environment prefab instantiated by the MapManager.
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
- The `MapDef` class is used to define the properties and behaviors of a map in the game, which can be instantiated and managed by the `MapManager`.

## Constraints & Failure Modes
- No explicit guards or null/empty handling noted in the file.
- Assumes that all referenced types (e.g., `EnemyDef`, `WaveDef`, `MapDifficulty`) are defined elsewhere.

## Example
```csharp
MapDef myMap = ScriptableObject.CreateInstance<MapDef>();
myMap.mapId = 1;
myMap.displayNameKey = "MAP_DESERT";
myMap.baseLevel = 1;
```

## Unknowns
- The definitions and behaviors of `EnemyDef`, `WaveDef`, and `MapDifficulty` are not provided in this file.

