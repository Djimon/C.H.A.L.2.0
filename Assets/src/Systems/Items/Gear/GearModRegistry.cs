// File: Assets/src/CHAL/Systems/Gear/GearModRegistry.cs
using System;
using System.Collections.Generic;
using CHAL.Data;

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
        public GearModRegistry(ImplicitRegistryDef implicitsDef, AffixRegistryDef affixesDef)
        {
            BuildImplicits(implicitsDef);
            BuildAffixes(affixesDef);
        }

        // =====================================================================
        // PUBLIC API (unified facade)
        // =====================================================================

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

        private void BuildImplicits(ImplicitRegistryDef def)
        {
            _implicitById.Clear();
            _implicitByPoolRole.Clear();

            if (def == null || def.Implicits == null)
            {
                DebugManager.Error("[GearModRegistry] ImplicitRegistryDef missing or empty.", "System");
                return;
            }

            var list = def.Implicits;
            for (int i = 0; i < list.Count; i++)
            {
                var d = list[i];
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

        private static int MakePoolRoleKey(int poolMask, int role)
            => (poolMask << 8) ^ role;

        // =====================================================================
        // BUILD: AFFIXES
        // =====================================================================

        private void BuildAffixes(AffixRegistryDef def)
        {
            _affixById.Clear();
            _affixByFamily.Clear();

            if (def == null || def.Affixes == null)
            {
                DebugManager.Error("[GearModRegistry] AffixRegistryDef missing or empty.", "System");
                return;
            }

            var list = def.Affixes;
            for (int i = 0; i < list.Count; i++)
            {
                var d = list[i];
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
    }
}
