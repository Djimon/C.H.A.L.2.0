using CHAL.Data;
using CHAL.Systems.Crafting;
using CHAL.Systems.Loot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CHAL.Systems.Items
{
    public sealed class ItemRegistry : ScriptableObject
    {
        // Optional: ein zentrales Asset, in das du NICHTS eintrÃ¤gst â€“ dient nur als Loader-Entry.
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

        private readonly Dictionary<string, bool> _used = new(StringComparer.OrdinalIgnoreCase);
        private string unusedExportPath = Path.Combine(Application.dataPath, "../Export/UnusedItems.csv");

        /// <summary>
        /// Reloads the item definitions from the Resources folder.
        /// </summary>
        public void Reload()
        {
            _byId.Clear();
            _used.Clear();
            // Alle ItemDef-Assets unter Resources/Items/ laden
            var defs = Resources.LoadAll<ItemDef>("data/Items");
            foreach (var def in defs)
            {
                if (string.IsNullOrWhiteSpace(def.itemId) || !ItemKey.TryParse(def.itemId, out _))
                {
                    DebugManager.Warning($"Skipping invalid ID in {def.name}");
                    continue;
                }
                if (_byId.ContainsKey(def.itemId))
                {
                    DebugManager.Warning($"[ItemRegistry] Duplicate ItemId '{def.itemId}' in {def.name}");
                    continue;
                }
                _byId.Add(def.itemId, def);

                _used[def.itemId] = false;
            }
            DebugManager.Log($"[ItemRegistry] Loaded: {_byId.Count} items",DebugManager.EDebugLevel.Production,"System");

            ExportItemIndexCsv("../Export/ItemIndex.csv");

            var mod_part_map = LoadModulePartMap();
            ValidateModulePartMap(mod_part_map);

            var reportPath = Path.Combine(Application.dataPath, "../Export/ModulePartValidation.csv"); 
            ValidateGearAndRecipes(reportPath);

            // NEW: Core coverage
            ValidateCoreCoverage();

       
            // NEW: Unused items audit
            ValidateUnusedItems();

            //TODO: export all items from _byID with Name and rarity as json: grouped by ItemType 

        }

        /// <summary>
        /// Ensures the item exists in registry (creates placeholder if missing) AND marks it as used.
        /// Returns false only if itemId is empty/invalid.
        /// </summary>
        public bool EnsureExistsAndMarkUsed(string itemId, string domain, string context, out ItemDef def)
        {
            def = null;

            if (string.IsNullOrWhiteSpace(itemId) || !ItemKey.TryParse(itemId, out _))
            {
                DebugManager.Warning($"[ItemRegistry] Invalid itemId ref: '{itemId}' (domain={domain}, ctx={context})", "Validation");
                return false;
            }

            // mark used (even if it doesn't exist yet -> placeholder will be created)
            _used[itemId] = true;

            if (_byId.TryGetValue(itemId, out def) && def != null)
                return true;

            // Missing: create placeholder + add to registry so subsequent lookups work
            CreatePlaceholderitem(itemId);

            // Try to load it back into registry in-memory (editor context).
            // NOTE: placeholder asset exists now, but Resources.LoadAll won't refresh _byId automatically.
            // We add a minimal in-memory placeholder entry to avoid null refs.
            var ph = ScriptableObject.CreateInstance<ItemDef>();
            ph.itemId = itemId;
            ph.description = "Placeholder Item: auto-generated (in-memory fallback).";
            ph.rarity = Rarity.Common;
            ph.lootValue = 0;
            _byId[itemId] = ph;
            def = ph;

            DebugManager.Warning($"[ItemRegistry] Missing item ref -> placeholder created: '{itemId}' (domain={domain}, ctx={context})", "Validation");
            return true;
        }

        private void ValidateGearAndRecipes(string reportPath)
        {
            var rows = new List<string>();

            // ============ 1) Gear-Folder unter Resources/data/Items/gear ============
            var gearDefs = Resources.LoadAll<ItemDef>("data/Items/gear");
            if (gearDefs == null || gearDefs.Length == 0)
            {
                rows.Add("warn,gear_folder,empty_or_missing,Resources/data/Items/gear");
            }
            else
            {
                foreach (var g in gearDefs)
                {
                    // leere oder ungÃ¼ltige ID
                    if (string.IsNullOrWhiteSpace(g.itemId))
                    {
                        rows.Add($"warn,gear,id_empty,asset={g.name}");
                        continue;
                    }

                    // Prefix-Check (nur Hinweis, Item ist ggf. trotzdem registriert)
                    if (!g.itemId.StartsWith("gear:", System.StringComparison.OrdinalIgnoreCase))
                    {
                        rows.Add($"warn,gear,wrong_prefix,{g.itemId},asset={g.name}");
                    }

                    // Schon in Registry? (Reload hat _byId befÃ¼llt)
                    if (!_byId.ContainsKey(g.itemId))
                    {
                        rows.Add($"warn,gear,not_in_registry,{g.itemId},asset={g.name}");
                    }
                }
            }

            // ============ 2) Recipes: outputItemId muss existieren ===================
            var recipes = Resources.LoadAll<RecipeDef>("data/Recipes");
            if (recipes == null || recipes.Length == 0)
            {
                rows.Add("warn,recipes,none_found,Resources/data/Recipes");
            }
            else
            {
                foreach (var r in recipes)
                {
                    var outId = r != null ? r.outputItemId : null;

                    if (string.IsNullOrWhiteSpace(outId))
                    {
                        rows.Add($"warn,recipe_output,empty,recipeAsset={r?.name}");
                        continue;
                    }

                    EnsureExistsAndMarkUsed(outId, "recipes", $"recipeAsset={r.name} output", out _);

                    // existiert Output-Item?
                    if (!_byId.ContainsKey(outId))
                    {
                        // analog "missing" kennzeichnen + Kontext anhÃ¤ngen
                        rows.Add($"missing,recipe_output,{outId},recipeAsset={r.name}");
                    }

                    if (r.inputs != null)
                    {
                        for (int i = 0; i < r.inputs.Count; i++)
                        {
                            var inId = r.inputs[i].itemId;
                            if (string.IsNullOrWhiteSpace(inId))
                            {
                                rows.Add($"warn,recipe_input,empty,recipeAsset={r.name},index={i}");
                                continue;
                            }

                            EnsureExistsAndMarkUsed(inId, "recipes", $"recipeAsset={r.name} input[{i}]", out _);

                            if (!_byId.ContainsKey(inId))
                            {
                                // analog "missing" kennzeichnen + Kontext anhÃ¤ngen
                                rows.Add($"missing,recipe_input,{inId},recipeAsset={r.name}");
                            }
                        }
                    }



                }
            }

            // ============ Report schreiben (Append) =================================
            if (rows.Count > 0)
            {
                try
                {
                    if (!File.Exists(reportPath))
                    {
                        // Header nur beim ersten Mal sinnvoll â€“ optional
                        File.WriteAllLines(reportPath, new[] { "level,domain,kind,value,context" });
                    }
                    File.AppendAllLines(reportPath, rows);
                    DebugManager.Log($"Gear/Recipes validation attached: {reportPath}",
                        DebugManager.EDebugLevel.Production, "System");
                }
                catch (System.Exception ex)
                {
                    DebugManager.Warning($"[ItemRegistry] Failed to write validation report: {ex.Message}", "Validation");
                }
            }
            else
            {
                DebugManager.Log("[ItemRegistry] Gear & Recipes valide", DebugManager.EDebugLevel.Production, "System");
            }
        }

        private void ValidateCoreCoverage()
        {
            // Alle cores sammeln
            var coreDefs = _byId.Values
                .Where(d => d != null && !string.IsNullOrEmpty(d.itemId) && d.itemId.StartsWith("core:", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Coverage map
            var covered = new HashSet<DamageType>();

            for (int i = 0; i < coreDefs.Count; i++)
            {
                var d = coreDefs[i];
                if (d.coreData == null) continue; // core ohne coreData -> ignorieren
                covered.Add(d.coreData.defualtDmgType); // (ja, typo im Feldnamen existiert bei dir)
                _used[d.itemId] = true; // cores sind "verwendet" per Definition
            }

            // Für jeden DamageType: mindestens 1 core?
            foreach (DamageType dt in Enum.GetValues(typeof(DamageType)))
            {
                if (covered.Contains(dt)) continue;

                var placeholderId = $"core:missing_{dt.ToString().ToLowerInvariant()}";
                DebugManager.Warning($"[ItemRegistry] No Core covers DamageType '{dt}'. Creating placeholder '{placeholderId}'.", "Validation");

                // placeholder + used mark (damit es nicht gleich im Unused Audit nervt)
                EnsureExistsAndMarkUsed(placeholderId, "core_coverage", $"missing coverage for {dt}", out _);
            }
        }


        private Dictionary<string, string[]> LoadModulePartMap()
        {
            TextAsset json = Resources.Load<TextAsset>("data/Items/ModulePartMap");
            if (json == null)
            {
                DebugManager.Warning("[ItemRegistry] No ModulePartMap found!");
                return null;
            }

            var wrapper = JsonUtility.FromJson<ModulePartMapWrapper>(json.text);
            var _modulePartMap = wrapper.ToDictionary();
            DebugManager.Log($"[ItemRegistry] ModulePartMap loaded with {_modulePartMap.Count} modules", DebugManager.EDebugLevel.Production, "System");
            return _modulePartMap;
        }

        private void ValidateModulePartMap(Dictionary<string, string[]> _modulePartMap)
        {
            List<string> errors = new();

            // Check: jedes Modul existiert
            foreach (var module in _modulePartMap.Keys)
            {

                EnsureExistsAndMarkUsed(module, "module_part_map", "moduleKey", out _);

                if (!_byId.ContainsKey(module))
                {
                    errors.Add($"Module {module} existiert nicht in ItemRegistry!");
                }      
            }

            // Check: jedes Part im Mapping existiert
            foreach (var parts in _modulePartMap.Values)
            {
                foreach (var part in parts)
                {
                    EnsureExistsAndMarkUsed(part, "module_part_map", "mappedPart", out _);

                    if (!_byId.ContainsKey(part))
                    {
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
                    DebugManager.Warning(e, "Validation");

                string path = Path.Combine(Application.dataPath, "../Export/ModulePartValidation.csv");
                File.WriteAllLines(path, errors);
                DebugManager.Log($"[ItemRegistry] Validation report saved: {path}", DebugManager.EDebugLevel.Production, "System");

            }
            else
            {
                DebugManager.Log("[ItemRegistry] ModulePartMap is fully valid", DebugManager.EDebugLevel.Production, "System");
            }
        }

        public void ValidateUnusedItems()
        {
            var unused = new List<string>();

            foreach (var kv in _byId)
            {
                var id = kv.Key;
                if (string.IsNullOrEmpty(id)) continue;

                if (_used.TryGetValue(id, out var isUsed) && isUsed)
                    continue;

                // nicht verwendet
                unused.Add(id);
            }

            if (unused.Count == 0)
            {
                DebugManager.Log("[ItemRegistry] UnusedItems: none", DebugManager.EDebugLevel.Production, "System");
                return;
            }

            // Warnungen (kompakt)
            for (int i = 0; i < unused.Count; i++)
                DebugManager.Warning($"[ItemRegistry] Unused item: {unused[i]}", "Validation");

            // Optional: CSV append
            try
            {
                var rows = unused.Select(id => $"warn,unused_item,{id},no_known_system_reference").ToArray();
                var header = "level,domain,kind,value,context";
                var lines = new List<string>(1 + rows.Length) { header };
                lines.AddRange(rows);

                var dir = Path.GetDirectoryName(unusedExportPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllLines(unusedExportPath, lines);
            }
            catch (Exception ex)
            {
                DebugManager.Warning($"[ItemRegistry] Failed to write unused-items report: {ex.Message}", "Validation");
            }
        }

        /// <summary>
        /// Exports the item index to a CSV file at the specified output path.
        /// </summary>
        /// <param name="outputPath">The path where the CSV file will be saved.</param>
        public void ExportItemIndexCsv(string outputPath)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                DebugManager.Warning("[ItemRegistry] ExportItemIndexCsv: outputPath is null/empty.");
                return;
            }

            try
            {
                // If relative path: interpret relative to project folder (next to Assets)
                var finalPath = Path.IsPathRooted(outputPath)
                    ? outputPath
                    : Path.GetFullPath(Path.Combine(Application.dataPath, outputPath));

                var sb = new StringBuilder(64 * 1024);
                sb.AppendLine("itemType,itemId,rarity");

                static string Csv(string s)
                {
                    if (string.IsNullOrEmpty(s)) return "";
                    // Quote only when needed
                    var needsQuote = s.Contains(',') || s.Contains('"') || s.Contains('\n') || s.Contains('\r');
                    if (!needsQuote) return s;
                    return "\"" + s.Replace("\"", "\"\"") + "\"";
                }

                // Note: adjust these member names if your ItemDef uses different ones.
                // If you want: I can make it reflection-based again, but you asked for simple.
                var rows = _byId
                    .Where(kv => kv.Value != null)
                    .Select(kv =>
                    {
                        var def = kv.Value;
                        var itemType = def.itemType.ToString();  // if itemType is enum; if string: just def.itemType
                        var rarity = def.rarity.ToString();      // if rarity is enum; if string: just def.rarity
                        var itemId = kv.Key;
                        return new { itemType, rarity, itemId };
                    })
                    .OrderBy(r => r.itemType, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(r => r.rarity, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(r => r.itemId, StringComparer.OrdinalIgnoreCase);

                foreach (var r in rows)
                {
                    sb.Append(Csv(r.itemType)).Append(',')
                      .Append(Csv(r.itemId)).Append(',')
                      .Append(Csv(r.rarity)).AppendLine();
                }

                var dir = Path.GetDirectoryName(finalPath);
                if (!string.IsNullOrWhiteSpace(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(finalPath, sb.ToString(), Encoding.UTF8);
                DebugManager.Log($"[ItemRegistry] Exported item index CSV: {finalPath}", DebugManager.EDebugLevel.Production, "System");
            }
            catch (Exception ex)
            {
                DebugManager.Warning($"[ItemRegistry] ExportItemIndexCsv failed: {ex.GetType().Name}: {ex.Message}");
            }
        }


        /// <summary>
        /// Tries to retrieve an item definition based on its identifier.
        /// </summary>
        /// <param name="itemId">The identifier of the item to retrieve.</param>
        /// <param name="def">The item definition if found; otherwise, null.</param>
        /// <returns>True if the item was found; otherwise, false.</returns>
        public bool TryGet(string itemId, out ItemDef def) => _byId.TryGetValue(itemId, out def);
/// <summary>
/// Retrieves the rarity of an item based on its identifier.
/// </summary>
/// <param name="itemId">The identifier of the item to retrieve the rarity for.</param>
/// <returns>The rarity of the item, or Rarity.Common if the item does not exist.</returns>
        public Rarity GetRarity(string itemId) => _byId.TryGetValue(itemId, out var d) ? d.rarity : Rarity.Common;
/// <summary>
/// Retrieves the loot value for a specified item by its identifier.
/// </summary>
/// <param name="itemId">The identifier of the item to retrieve the loot value for.</param>
/// <returns>The loot value of the item, or 0 if the item does not exist.</returns>
        public int GetLootValue(string itemId) => _byId.TryGetValue(itemId, out var d) ? d.lootValue : 0;
/// <summary>
/// Checks if an item exists by its identifier.
/// </summary>
/// <param name="itemId">The identifier of the item to check.</param>
/// <returns>True if the item exists; otherwise, false.</returns>
        public bool Exists(string itemId) => _byId.ContainsKey(itemId);

/// <summary>
/// Retrieves all items that match the specified type prefix.
/// </summary>
/// <param name="typePrefix">The prefix to filter item types.</param>
/// <returns>An enumerable collection of matching item definitions.</returns>
        public IEnumerable<ItemDef> GetAllItemsByType(string typePrefix)
        {
            foreach (var kv in _byId)
            {
                if (kv.Key.StartsWith(typePrefix + ":", System.StringComparison.OrdinalIgnoreCase))
                    yield return kv.Value;
            }
        }

/// <summary>
/// Creates a placeholder item asset with the specified item ID.
/// </summary>
/// <param name="itemId">The unique identifier for the item.</param>
        public void CreatePlaceholderitem(string itemId)
        {
            var prefix = itemId.Split(':')[0];
            var folder = $"Assets/Resources/data/Items/{prefix}/missing";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var assetPath = $"{folder}/{itemId.Replace(":", "_")}.asset";

            var def = ScriptableObject.CreateInstance<ItemDef>();
            def.itemId = itemId;
            def.description = "Placeholder Item: auto-generated.";
            def.rarity = Rarity.Common;
            def.lootValue = 0;

            DebugManager.DevLog($"[ItemRegistry] Created placeholder for '{itemId}' at {assetPath}", "Validation");

            AssetDatabase.CreateAsset(def, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            
        }

/// <summary>
/// Retrieves the item type associated with the specified item ID.
/// </summary>
/// <param name="itemId">The ID of the item.</param>
/// <returns>The item type corresponding to the given item ID.</returns>
        public ItemType GetTypeOf(string itemId)
        {
            return ItemTypeUtils.FromId(itemId);
        }

/// <summary>
/// Checks if the specified item ID matches the given item type.
/// </summary>
/// <param name="itemId">The ID of the item to check.</param>
/// <param name="t">The item type to compare against.</param>
/// <returns>True if the item ID matches the item type; otherwise, false.</returns>
        public bool IsType(string itemId, ItemType t)
        {
            return GetTypeOf(itemId) == t;
        }

/// <summary>
/// Triggers an instance action in the ItemRegistry.
/// </summary>
        public void TriggerInstance()
        {
            DebugManager.Log("trigger Instance form Itemregistry");
        }
    }
}
