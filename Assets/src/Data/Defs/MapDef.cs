using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "MapDef", menuName = "Data/Map Definition")]
/// <summary>
/// Represents a map definition used in the game, including metadata and gameplay settings.
/// </summary>
    public class MapDef : ScriptableObject
    {
        [Header("Meta")]
        public int mapId;               // interne ID (z. B. "desert_01")
        public string displayNameKey;      // Key für Localization (z. B. "MAP_DESERT")
        public Sprite previewImage;        // für MapSelectionUI
        public GameObject mapPrefab;       // eigentliche Umgebung, wird von MapManager instanziert

        [Header("Gameplay")]
        public int baseLevel = 1;          // Start-Level der Gegner
        public int maxWaves = 5;           // Anzahl Waves pro Map
        public MapDifficulty difficulty;   // Basis-Schwierigkeit dieser Map
        public int heroSlots = 1;

        [Header("Enemy Pools")]
        public List<EnemyDef> allowedEnemies;
        public List<string> allowedMonsterTags;

        [Header("Wave Templates")]
        public List<WaveDef> waveDefs;     // aktuell noch konkret, später Constraints
        public int subWaveCount = 5;
        public float interSubWaveDelay = 10f;
        public int maxConCurrentEnemies = 25;

    }
}
