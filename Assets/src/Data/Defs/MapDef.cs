using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "MapDef", menuName = "Data/Map Definition")]
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

        [Header("Enemy Pools")]
        public List<EnemyDef> allowedEnemies;
        public List<string> allowedModifiers;

        [Header("Wave Templates")]
        public List<WaveDef> waveDefs;     // aktuell noch konkret, später Constraints
    }
}
