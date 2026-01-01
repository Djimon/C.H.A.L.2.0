// File: Assets/src/CHAL/Systems/Gear/GearModRegistry.cs
using CHAL.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace CHAL.Systems.Items
{
    /// <summary>
    /// Facade registry for all gear mods (Implicits + Affixes).
    /// Internally keeps separate indices, externally offers a unified API.
    /// </summary>
    public sealed class GearModRegistry
    {
        // --- internal indices ---
        private readonly Dictionary<string, ImplicitDef> _implicitById = new(StringComparer.Ordinal);
        private readonly Dictionary<int, List<ImplicitDef>> _implicitByPoolRole = new();

        private readonly Dictionary<string, AffixDef> _affixById = new(StringComparer.Ordinal);
        private readonly Dictionary<AffixFamily, List<AffixDef>> _affixByFamily = new();

        // --- ctor builds everything once (same pattern as current ImplicitRegistry) ---
        public GearModRegistry()
        {
            //                           data\Items\gear\Implicits
            BuildImplicitsFromResources("data/Items/gear/Implicits");
            BuildAffixesFromResources("data/Items/gear/Affixes");

            DebugManager.Log($"[GearModRegistry] Loaded Implicits: {_implicitById.Count} / Affixes: {_affixById.Count}",DebugManager.EDebugLevel.Production,"System");

            ExportModsCsv("../Export/GearModIndex.csv");
        }

        // =====================================================================
        // PUBLIC API (unified facade)
        // =====================================================================


        public void ExportModsCsv(string outputPath)
        {
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                DebugManager.Warning("[GearModRegistry] ExportModsCsv: outputPath is null/empty.");
                return;
            }

            try
            {
                // If relative path: interpret relative to Assets folder
                var finalPath = Path.IsPathRooted(outputPath)
                    ? outputPath
                    : Path.GetFullPath(Path.Combine(Application.dataPath, outputPath));

                var rows = new List<ModCsvRow>(_implicitById.Count + _affixById.Count);

                // ---- Implicits ----
                foreach (var kv in _implicitById)
                {
                    var d = kv.Value;
                    if (d == null) continue;

                    rows.Add(new ModCsvRow
                    {
                        type = "implicit",
                        id = kv.Key, // use dictionary key -> robust even if field names change
                        target = d.Target.ToString(),
                        valueKind = d.ValueKind.ToString(),
                        membership = ToPipe(d.PoolMembership),
                        categoryOrRole = d.Role.ToString(),
                        allowedGearTypes = GearTypesToPipe(d.AllowedGearTypes)
                    });
                }

                // ---- Affixes ----
                foreach (var kv in _affixById)
                {
                    var d = kv.Value;
                    if (d == null) continue;

                    rows.Add(new ModCsvRow
                    {
                        type = "affix",
                        id = kv.Key, // use dictionary key
                        target = d.Target.ToString(),
                        valueKind = d.ValueKind.ToString(),
                        membership = ToPipe(d.FamilyMembership),
                        categoryOrRole = d.Category.ToString(),
                        allowedGearTypes = GearTypesToPipe(d.AllowedGearTypes)
                    });
                }

                // Sort: type -> categoryOrRole -> target -> id
                rows.Sort(ModCsvRowComparer);

                var sb = new StringBuilder(64 * 1024);
                sb.AppendLine("type,id,target,valueKind,membership,categoryOrRole,allowedGearTypes");

                for (int i = 0; i < rows.Count; i++)
                {
                    var r = rows[i];
                    sb.Append(Csv(r.type)).Append(',')
                      .Append(Csv(r.id)).Append(',')
                      .Append(Csv(r.target)).Append(',')
                      .Append(Csv(r.valueKind)).Append(',')
                      .Append(Csv(r.membership)).Append(',')
                      .Append(Csv(r.categoryOrRole)).Append(',')
                      .Append(Csv(r.allowedGearTypes)).AppendLine();
                }

                var dir = Path.GetDirectoryName(finalPath);
                if (!string.IsNullOrWhiteSpace(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(finalPath, sb.ToString(), Encoding.UTF8);
                DebugManager.Log($"[GearModRegistry] Exported gear mods CSV: {finalPath}", DebugManager.EDebugLevel.Production, "System");
            }
            catch (Exception ex)
            {
                DebugManager.Warning($"[GearModRegistry] ExportModsCsv failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

        /// <summary>
        /// Attempts to retrieve an implicit definition by its identifier.
        /// Returns false if the identifier is null or empty, or if the definition is not found.
        /// </summary>
        /// <param name="implicitId">The identifier of the implicit definition to retrieve.</param>
        /// <param name="def">The output parameter that will hold the implicit definition if found.</param>
        /// <returns>True if the implicit definition was found; otherwise, false.</returns>
        public bool TryGetImplicit(string implicitId, out ImplicitDef def)
        {
            def = null;
            if (string.IsNullOrEmpty(implicitId)) return false;
            return _implicitById.TryGetValue(implicitId, out def) && def != null;
        }

/// <summary>
/// Attempts to retrieve an affix definition by its identifier.
/// Returns false if the identifier is null or empty, or if the affix is not found.
/// </summary>
/// <param name="affixId">The identifier of the affix to retrieve.</param>
/// <param name="def">The output parameter that will hold the affix definition if found.</param>
/// <returns>True if the affix was found; otherwise, false.</returns>
        public bool TryGetAffix(string affixId, out AffixDef def)
        {
            def = null;
            if (string.IsNullOrEmpty(affixId)) return false;
            return _affixById.TryGetValue(affixId, out def) && def != null;
        }

        // IMPORTANT:
        // - This lookup expects a SINGLE pool flag (e.g. Melee=1, Ranged=2, Caster=4, Neutral=8),
        //   NOT a multi-flag mask (e.g. Melee|Neutral = 9).
        // - This is NOT a limitation for multi-pool implicits:
        //   during Build(), each ImplicitDef with multi-pool membership is already indexed into
        //   every matching single-pool bucket via EnumeratePools().
        // - If we ever want "union" queries (mask with multiple bits), we must implement an
        //   EnumeratePools(mask) + merge/dedupe across buckets.
/// <summary>
/// Retrieves implicit candidates based on the specified gear type and role.
/// Clears the provided buffer and populates it with matching implicit definitions.
/// </summary>
/// <param name="gearType">The type of gear to filter candidates.</param>
/// <param name="poolMaskAsInt">An integer representing the pool mask.</param>
/// <param name="roleAsInt">An integer representing the role.</param>
/// <param name="buffer">The list to store the resulting implicit definitions.</param>
        public void GetImplicitCandidates(GearType gearType, int poolMaskAsInt, int roleAsInt, List<ImplicitDef> buffer)
        {
            buffer.Clear();

            int key = MakePoolRoleKey(poolMaskAsInt, roleAsInt);
            if (!_implicitByPoolRole.TryGetValue(key, out var list) || list == null || list.Count == 0)
                return;

            for (int i = 0; i < list.Count; i++)
            {
                var d = list[i];
                if (d == null) continue;
                if (!d.Allows(gearType)) continue;
                buffer.Add(d);
            }
        }

        /// <summary>
        /// Writes affix candidates into buffer (clears first).
        /// Family is the "selection axis" (no role, homogeneous slots).
        /// </summary>
        public void GetAffixCandidates(AffixFamily family, GearType gearType, List<AffixDef> buffer)
        {
            buffer.Clear();

            if (!_affixByFamily.TryGetValue(family, out var list) || list == null || list.Count == 0)
                return;

            for (int i = 0; i < list.Count; i++)
            {
                var d = list[i];
                if (d == null) continue;
                if (!d.Allows(gearType)) continue;
                buffer.Add(d);
            }
        }

        // =====================================================================
        // BUILD: IMPLICITS
        // =====================================================================

        private void BuildImplicitsFromResources(string resourcesPath)
        {
            _implicitById.Clear();
            _implicitByPoolRole.Clear();

            if (string.IsNullOrWhiteSpace(resourcesPath))
            {
                DebugManager.Error("[GearModRegistry] BuildImplicitsFromResources: path is empty.", "System");
                return;
            }

            var all = Resources.LoadAll<ImplicitDef>(resourcesPath);
            if (all == null || all.Length == 0)
            {
                DebugManager.Warning($"[GearModRegistry] No ImplicitDef found at Resources/{resourcesPath}", "System");
                return;
            }

            for (int i = 0; i < all.Length; i++)
            {
                var d = all[i];
                if (d == null) continue;

                var id = (d.ImplicitId ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(id)) continue;

                if (_implicitById.ContainsKey(id))
                {
                    DebugManager.Warning($"[GearModRegistry] Duplicate ImplicitId '{id}' ignored (asset '{d.name}').", "System");
                    continue;
                }

                _implicitById[id] = d;

                foreach (var pool in d.EnumeratePools())
                {
                    int key = MakePoolRoleKey((int)pool, (int)d.Role);
                    if (!_implicitByPoolRole.TryGetValue(key, out var bucket))
                    {
                        bucket = new List<ImplicitDef>(16);
                        _implicitByPoolRole[key] = bucket;
                    }
                    bucket.Add(d);
                }
            }
        }

        private void BuildAffixesFromResources(string resourcesPath)
        {
            _affixById.Clear();
            _affixByFamily.Clear();

            if (string.IsNullOrWhiteSpace(resourcesPath))
            {
                DebugManager.Error("[GearModRegistry] BuildAffixesFromResources: path is empty.", "System");
                return;
            }

            var all = Resources.LoadAll<AffixDef>(resourcesPath);
            if (all == null || all.Length == 0)
            {
                DebugManager.Warning($"[GearModRegistry] No AffixDef found at Resources/{resourcesPath}", "System");
                return;
            }

            for (int i = 0; i < all.Length; i++)
            {
                var d = all[i];
                if (d == null) continue;

                var id = (d.AffixId ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(id)) continue;

                if (_affixById.ContainsKey(id))
                {
                    DebugManager.Warning($"[GearModRegistry] Duplicate AffixId '{id}' ignored (asset '{d.name}').", "System");
                    continue;
                }

                _affixById[id] = d;

                foreach (var fam in d.EnumerateFamilies())
                {
                    if (!_affixByFamily.TryGetValue(fam, out var bucket))
                    {
                        bucket = new List<AffixDef>(16);
                        _affixByFamily[fam] = bucket;
                    }
                    bucket.Add(d);
                }
            }
        }

       
        private static int MakePoolRoleKey(int poolMask, int role)
            => (poolMask << 8) ^ role;


        // ==========================
        // Internals (CSV helpers)
        // ==========================

        private struct ModCsvRow
        {
            public string type;
            public string id;
            public string target;
            public string valueKind;
            public string membership;
            public string categoryOrRole;
            public string allowedGearTypes;
        }

        private static int ModCsvRowComparer(ModCsvRow a, ModCsvRow b)
        {
            int c;

            c = StringComparer.OrdinalIgnoreCase.Compare(a.type, b.type);
            if (c != 0) return c;

            c = StringComparer.OrdinalIgnoreCase.Compare(a.categoryOrRole, b.categoryOrRole);
            if (c != 0) return c;

            c = StringComparer.OrdinalIgnoreCase.Compare(a.target, b.target);
            if (c != 0) return c;

            return StringComparer.OrdinalIgnoreCase.Compare(a.id, b.id);
        }

        private static string Csv(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            var needsQuote = s.Contains(',') || s.Contains('"') || s.Contains('\n') || s.Contains('\r');
            if (!needsQuote) return s;
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        }

        private static string GearTypesToPipe(GearType[] allowed)
        {
            if (allowed == null || allowed.Length == 0) return "*";

            // Deterministic, no spaces
            var sb = new StringBuilder(64);
            for (int i = 0; i < allowed.Length; i++)
            {
                if (i > 0) sb.Append('|');
                sb.Append(allowed[i].ToString());
            }
            return sb.ToString();
        }

        private static string ToPipe(ImplicitPoolBitMask mask)
        {
            if (mask == 0) return "None";

            var sb = new StringBuilder(32);
            bool first = true;

            foreach (ImplicitPoolBitMask v in Enum.GetValues(typeof(ImplicitPoolBitMask)))
            {
                if (v == 0) continue;
                if ((mask & v) == 0) continue;

                if (!first) sb.Append('|');
                sb.Append(v.ToString());
                first = false;
            }

            return first ? "None" : sb.ToString();
        }

        private static string ToPipe(AffixFamilyBitMask mask)
        {
            if (mask == 0) return "None";

            var sb = new StringBuilder(32);
            bool first = true;

            foreach (AffixFamilyBitMask v in Enum.GetValues(typeof(AffixFamilyBitMask)))
            {
                if (v == 0) continue;
                if ((mask & v) == 0) continue;

                if (!first) sb.Append('|');
                sb.Append(v.ToString());
                first = false;
            }

            return first ? "None" : sb.ToString();
        }

    }
}
