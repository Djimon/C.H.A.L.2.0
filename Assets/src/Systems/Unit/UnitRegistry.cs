using CHAL.Data;
using CHAL.Systems.Items;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Unit
{

    //[CreateAssetMenu(fileName = "UnitRegistry", menuName = "Config/UnitRegistry")]
    public sealed class UnitRegistry : ScriptableObject
    {
        private static UnitRegistry _instance;
        public static UnitRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = CreateInstance<UnitRegistry>();
                    _instance.Reload();
                }
                return _instance;
            }
        }

        private readonly Dictionary<string, HeroDef> _HeroById = new();
        private readonly Dictionary<string, EnemyDef> _EnemyById = new();

        public void Reload()
        {
            _HeroById.Clear();
            _EnemyById.Clear();

            // Alle Def-Assets unter Resources/ laden
            var herodefs = Resources.LoadAll<HeroDef>("data/Heroes");
            foreach (var def in herodefs)
            {
                if (string.IsNullOrWhiteSpace(def.HeroId))
                {
                    DebugManager.Warning($"[UnitRegistry] Skipping invalid ID in {def.name}");
                    continue;
                }
                if (_HeroById.ContainsKey(def.HeroId))
                {
                    DebugManager.Warning($"[UnitRegistry] Duplicate HeroID '{def.HeroId}' in {def.name}");
                    continue;
                }

                DebugManager.Log($"[UnitRegistry] Hero Loaded: {def.HeroId}", DebugManager.EDebugLevel.Debug, "System");
                _HeroById.Add(def.HeroId, def);

            }
            DebugManager.Log($"[UnitRegistry] Loaded: {_HeroById.Count} Heroes", DebugManager.EDebugLevel.Production, "System");


            //Alle degs aus Enemies laden
            var enemydefs = Resources.LoadAll<EnemyDef>("data/Enemies");
            //DebugManager.DebugLog($"enemys?:{enemydefs.Length}");
            foreach (var def in enemydefs)
            {
                //Debug.Log($"?{def.displayNameKey}");
                if (string.IsNullOrWhiteSpace(def.enemyId))
                {
                    DebugManager.Warning($"[UnitRegistry] Skipping invalid ID in {def.name}");
                    continue;
                }
                if (_EnemyById.ContainsKey(def.enemyId))
                {
                    DebugManager.Warning($"[UnitRegistry] Duplicate Enemy '{def.enemyId}' in {def.name}");
                    continue;
                }
                DebugManager.Log($"[UnitRegistry] Enemy Loaded: {def.enemyId}", DebugManager.EDebugLevel.Debug, "System");
                _EnemyById.Add(def.enemyId, def);
            }

            DebugManager.Log($"[UnitRegistry] Loaded: {_EnemyById.Count} enemies", DebugManager.EDebugLevel.Production, "System");

            var enemycount = Mathf.Max(0, _EnemyById.Count);
            DebugManager.Log($"{enemycount} enemies loaded", DebugManager.EDebugLevel.Production, "System");

        }

        public HeroDef GetHeroById(string id)
        {
            return _HeroById.TryGetValue(id, out var def) ? def : null;
        }

        public EnemyDef GetEnemyByID(string id)
        {
            return _EnemyById.TryGetValue(id, out var def) ? def : null;
        }

        public IEnumerable<string> GetAllHeroIds() => _HeroById.Keys;
        public IEnumerable<string> GetAllEnemyIds() => _EnemyById.Keys;
        public IEnumerable<HeroDef> GetAllHeroes() => _HeroById.Values;
        public IEnumerable<EnemyDef> GetAllEnemies() => _EnemyById.Values;

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorAutoReload()
        {
            if (!Application.isPlaying)
            {
                Instance?.Reload();
            }
        }
#endif
    }
}
