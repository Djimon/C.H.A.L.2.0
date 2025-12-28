using CHAL.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CHAL.Systems.Skill
{
    public sealed class SkillRegistry : ScriptableObject
    {
        private static SkillRegistry _instance;
/// <summary>
/// Gets the singleton instance of the SkillRegistry.
/// </summary>
        public static SkillRegistry Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = CreateInstance<SkillRegistry>();
                    _instance.Reload();
                }
                return _instance;
            }
        }

        // IMPORTANT: adjust to your actual folder under Resources/
        // e.g. Resources/data/Skills/...
        private const string ResourcesPath = "data/Skills";

        private readonly Dictionary<string, SkillModuleDef> _byId = new();

/// <summary>
/// Reloads the skill definitions from resources.
/// </summary>
/// <summary>
/// Reloads the skill definitions from resources.
/// </summary>
        public void Reload()
        {
            _byId.Clear();

            var defs = Resources.LoadAll<SkillModuleDef>(ResourcesPath);
            foreach (var def in defs)
            {
                if (def == null || string.IsNullOrWhiteSpace(def.SkillId))
                {
                    DebugManager.Warning($"[SkillRegistry] Skipping invalid SkillId in asset '{def?.name}'", "System");
                    continue;
                }

                if (_byId.ContainsKey(def.SkillId))
                {
                    DebugManager.Warning($"[SkillRegistry] Duplicate SkillId '{def.SkillId}' in asset '{def.name}'", "System");
                    continue;
                }

                _byId.Add(def.SkillId, def);
            }

            ExportItemIndexCsv("../SkillIndex.csv");

            //TODO: Do some validations?

            DebugManager.Log($"[SkillRegistry] Loaded: {_byId.Count} skills from Resources/{ResourcesPath}",
                DebugManager.EDebugLevel.Production, "System");
        }

/// <summary>
/// Retrieves a skill module definition by its unique identifier.
/// </summary>
/// <param name="skillId">The unique identifier of the skill.</param>
/// <returns>The skill module definition if found; otherwise, null.</returns>
        public SkillModuleDef GetById(string skillId)
        {
            return _byId.TryGetValue(skillId, out var def) ? def : null;
        }

        public bool TryGet(string skillId, out SkillModuleDef def) => _byId.TryGetValue(skillId, out def);

        public IEnumerable<string> GetAllSkillIds() => _byId.Keys;
        public IEnumerable<SkillModuleDef> GetAllSkills() => _byId.Values;

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorAutoReload()
        {
            if (!Application.isPlaying)
                Instance?.Reload();
        }

        internal void TriggertInstanc()
        {
            DebugManager.Log("trigger Instance form Skillregistry");
        }
#endif

        public void ExportItemIndexCsv(string outputPath)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                DebugManager.Warning("[SkillRegistry] ExportSkillIndexCsv: outputPath is null/empty.");
                return;
            }

            try
            {
                // If relative path: interpret relative to project folder (next to Assets)
                var finalPath = Path.IsPathRooted(outputPath)
                    ? outputPath
                    : Path.GetFullPath(Path.Combine(Application.dataPath, outputPath));

                var sb = new StringBuilder(64 * 1024);
                sb.AppendLine("Attribute,Tier,SkillId,Type,Core");

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
                        var attribute = def.AttributeAffinity.ToString();  // if itemType is enum; if string: just def.itemType
                        var skilltype = def.SkillType.ToString();      // if rarity is enum; if string: just def.rarity
                        var skillId = kv.Key;
                        var tier = def.minRequiredTier;
                        var core = def.defualtCore;
                        return new { attribute, skillId, skilltype, tier, core };
                    })
                    .OrderBy(r => r.attribute, StringComparer.OrdinalIgnoreCase)
                    .ThenBy(r => r.tier.ToString(), StringComparer.OrdinalIgnoreCase)
                    .ThenBy(r => r.skillId, StringComparer.OrdinalIgnoreCase);

                foreach (var r in rows)
                {
                    sb.Append(Csv(r.attribute)).Append(',')
                      .Append(Csv(r.tier.ToString())).Append(',')
                      .Append(Csv(r.skillId)).Append(',')
                      .Append(Csv(r.skilltype)).Append(',')                      
                      .Append(Csv(r.core.ToString())).Append(',')
                      .AppendLine();
                }

                var dir = Path.GetDirectoryName(finalPath);
                if (!string.IsNullOrWhiteSpace(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(finalPath, sb.ToString(), Encoding.UTF8);
                DebugManager.Log($"[SkillRegistry] Exported skill index CSV: {finalPath}", DebugManager.EDebugLevel.Production, "System");
            }
            catch (Exception ex)
            {
                DebugManager.Warning($"[SkillRegistry] ExportSkillIndexCsv failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

    }
}
