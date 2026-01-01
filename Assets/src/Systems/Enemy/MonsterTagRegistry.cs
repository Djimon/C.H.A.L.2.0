using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CHAL.Systems.Enemy
{
    using CHAL.Data;

    public sealed class MonsterTagRegistry
    {
        public static MonsterTagRegistry Instance { get; } = new();

        private readonly Dictionary<string, MonsterTagDef> _byId =
            new(StringComparer.OrdinalIgnoreCase);

        private bool _loaded;

        // Pfad: Assets/Resources/data/MonsterTags/*.asset
        private const string RES_PATH = "data/MonsterTags";

        public void LoadAll(bool force = false)
        {
            if (_loaded && !force) return;

            _byId.Clear();
            _loaded = true;

            var defs = Resources.LoadAll<MonsterTagDef>(RES_PATH);
            for (int i = 0; i < defs.Length; i++)
            {
                var d = defs[i];
                if (d == null) continue;

                var id = (d.tagId ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(id))
                {
                    DebugManager.Warning($"[MonsterTags] Empty tagId in asset '{d.name}'", "Validation");
                    continue;
                }

                if (_byId.ContainsKey(id))
                {
                    DebugManager.Warning($"[MonsterTags] Duplicate tagId '{id}' (asset '{d.name}') - keeping first", "Validation");
                    continue;
                }

                _byId[id] = d;
            }

            DebugManager.Log($"[MonsterTags] Loaded: {_byId.Count} tags", DebugManager.EDebugLevel.Dev, "System");

            var reportPath = Path.Combine(Application.dataPath, "../Export/monsterTags.csv");
            ExportCsv(reportPath);
        }

        public bool TryGet(string tagId, out MonsterTagDef def)
        {
            def = null;
            if (!_loaded) LoadAll();
            if (string.IsNullOrWhiteSpace(tagId)) return false;
            return _byId.TryGetValue(tagId.Trim(), out def);
        }

        public bool IsKnown(string tagId)
        {
            return TryGet(tagId, out _);
        }

        public IReadOnlyCollection<MonsterTagDef> All
        {
            get
            {
                if (!_loaded) LoadAll();
                return _byId.Values;
            }
        }

        /// <summary>
        /// Dev helper: exports current registry snapshot to CSV.
        /// </summary>
        public void ExportCsv(string exportPath)
        {
            if (!_loaded) LoadAll();

            var dir = Path.GetDirectoryName(exportPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var lines = new List<string> { "tagId,category" };
            foreach (var d in _byId.Values.OrderBy(x => x.tagId, StringComparer.OrdinalIgnoreCase))
            {
                lines.Add($"{d.tagId},{d.category}");
            }
            File.WriteAllLines(exportPath, lines);

            DebugManager.Log($"[MonsterTags] Exported CSV: {exportPath}", DebugManager.EDebugLevel.Dev, "System");
        }

        private static string EscapeCsv(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            if (s.Contains(',') || s.Contains('"') || s.Contains('\n'))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }
    }
}
