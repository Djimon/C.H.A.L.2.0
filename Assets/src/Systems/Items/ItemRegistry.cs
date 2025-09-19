using CHAL.Data;
using CHAL.Systems.Loot.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CHAL.Systems.Items
{
    public sealed class ItemRegistry : ScriptableObject
    {
        // Optional: ein zentrales Asset, in das du NICHTS einträgst – dient nur als Loader-Entry.
        private static ItemRegistry _instance;
        public static ItemRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = CreateInstance<ItemRegistry>();
                    _instance.Reload();
                }
                return _instance;
            }
        }

        private readonly Dictionary<string, ItemDef> _byId = new();

        public void Reload()
        {
            _byId.Clear();
            // Alle ItemDef-Assets unter Resources/Items/ laden
            var defs = Resources.LoadAll<ItemDef>("data/Items");
            foreach (var def in defs)
            {
                if (string.IsNullOrWhiteSpace(def.itemId) || !ItemKey.TryParse(def.itemId, out _))
                {
                    DebugManager.Warning($"[ItemRegistry] Skip ungültige ID in {def.name}");
                    continue;
                }
                if (_byId.ContainsKey(def.itemId))
                {
                    DebugManager.Warning($"[ItemRegistry] Duplicate ItemId '{def.itemId}' in {def.name}");
                    continue;
                }
                _byId.Add(def.itemId, def);
            }
            DebugManager.Log($"[ItemRegistry] Geladen: {_byId.Count} Items",DebugManager.EDebugLevel.Production,"System");


            var mod_part_map = LoadModulePartMap();
            ValidateModulePartMap(mod_part_map);

        }

        private Dictionary<string, string[]> LoadModulePartMap()
        {
            TextAsset json = Resources.Load<TextAsset>("data/Items/ModulePartMap");
            if (json == null)
            {
                DebugManager.Warning("[ItemRegistry] Keine ModulePartMap gefunden!");
                return null;
            }

            var wrapper = JsonUtility.FromJson<ModulePartMapWrapper>(json.text);
            var _modulePartMap = wrapper.ToDictionary();
            DebugManager.Log($"[ItemRegistry] ModulePartMap geladen mit {_modulePartMap.Count} Modulen", DebugManager.EDebugLevel.Production, "System");
            return _modulePartMap;
        }

        private void ValidateModulePartMap(Dictionary<string, string[]> _modulePartMap)
        {
            List<string> errors = new();

            // Check: jedes Modul existiert
            foreach (var module in _modulePartMap.Keys)
            {
                if (!_byId.ContainsKey(module))
                {
                    CreatePlaceholderitem(module);
                    errors.Add($"Module {module} existiert nicht in ItemRegistry!");
                }      
            }

            // Check: jedes Part im Mapping existiert
            foreach (var parts in _modulePartMap.Values)
            {
                foreach (var part in parts)
                {
                    if (!_byId.ContainsKey(part))
                    {
                        CreatePlaceholderitem(part);
                        errors.Add($"Part {part} existiert nicht in ItemRegistry!");
                    }      
                }
            }

            // Check: jedes Modul hat mindestens 1 Part
            foreach (var kv in _modulePartMap)
            {
                if (kv.Value == null || kv.Value.Length == 0)
                    errors.Add($"Module {kv.Key} hat keine Parts!");
            }

            // Check: gibt es Parts, die in keinem Modul vorkommen?
            var allMappedParts = new HashSet<string>(_modulePartMap.Values.SelectMany(v => v));
            foreach (var item in _byId.Keys.Where(id => id.StartsWith("part:")))
            {
                if (!allMappedParts.Contains(item))
                    errors.Add($"Part {item} wird in keinem Modul verwendet!");
            }

            if (errors.Count > 0)
            {
                foreach (var e in errors)
                    DebugManager.Error(e, "Validation");

                string path = Path.Combine(Application.dataPath, "../ModulePartValidation.csv");
                File.WriteAllLines(path, errors);
                DebugManager.Log($"[ItemRegistry] Validierungsreport gespeichert: {path}", DebugManager.EDebugLevel.Production, "System");

            }
            else
            {
                DebugManager.Log("[ItemRegistry] ModulePartMap vollständig valide", DebugManager.EDebugLevel.Production, "System");
            }
        }


        public bool TryGet(string itemId, out ItemDef def) => _byId.TryGetValue(itemId, out def);
        public Rarity GetRarity(string itemId) => _byId.TryGetValue(itemId, out var d) ? d.rarity : Rarity.Common;
        public int GetLootValue(string itemId) => _byId.TryGetValue(itemId, out var d) ? d.lootValue : 0;
        public bool Exists(string itemId) => _byId.ContainsKey(itemId);

        public IEnumerable<ItemDef> GetAllItemsByType(string typePrefix)
        {
            foreach (var kv in _byId)
            {
                if (kv.Key.StartsWith(typePrefix + ":", System.StringComparison.OrdinalIgnoreCase))
                    yield return kv.Value;
            }
        }

        public void CreatePlaceholderitem(string itemId)
        {
            var prefix = itemId.Split(':')[0];
            var folder = $"Assets/Resources/data/Items/{prefix}/missing";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var assetPath = $"{folder}/{itemId.Replace(":", "_")}.asset";

            var def = ScriptableObject.CreateInstance<ItemDef>();
            def.itemId = itemId;
            def.description = "Placeholder Item – auto-generated.";
            def.rarity = Rarity.Common;
            def.lootValue = 0;

            AssetDatabase.CreateAsset(def, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            DebugManager.Log($"[ItemRegistry] Placeholder für '{itemId}' angelegt unter {assetPath}");
        }
    }
}
