// File: Assets/src/CHAL/Systems/Gear/AffixRegistry.cs
using System;
using System.Collections.Generic;
using CHAL.Data;

namespace CHAL.Systems.Items
{
    public sealed class AffixRegistry
    {
        private readonly Dictionary<string, AffixDef> _byId = new(StringComparer.Ordinal);
        private readonly Dictionary<FamilyKey, List<AffixDef>> _byFamily = new();

        public AffixRegistry(AffixRegistryDef def)
        {
            if (def == null || def.Affixes == null)
            {
                DebugManager.Error("[AffixRegistry] RegistryDef missing or empty.", "System");
                return;
            }

            for (int i = 0; i < def.Affixes.Count; i++)
            {
                var d = def.Affixes[i];
                if (d == null) continue;

                var id = (d.AffixId ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(id)) continue;

                if (_byId.ContainsKey(id))
                {
                    DebugManager.Warning($"[AffixRegistry] Duplicate Id '{id}' ignored (asset '{d.name}').", "System");
                    continue;
                }

                _byId[id] = d;

                foreach (var family in d.EnumerateFamilies())
                {
                    var key = new FamilyKey(family);
                    if (!_byFamily.TryGetValue(key, out var list))
                    {
                        list = new List<AffixDef>(16);
                        _byFamily[key] = list;
                    }
                    list.Add(d);
                }
            }
        }

        /// <summary>
        /// Tries to get an AffixDef by its affix ID.
        /// Returns false if the ID is null or empty.
        /// </summary>
        public bool TryGet(string affixId, out AffixDef def)
        {
            def = null;
            if (string.IsNullOrEmpty(affixId)) return false;
            return _byId.TryGetValue(affixId, out def) && def != null;
        }

        /// <summary>
        /// Returns candidates for Family, filtered by AllowedGearTypes.
        /// Result list is written into <paramref name="buffer"/> (cleared first).
        /// </summary>
        public void GetCandidates(AffixFamily family, GearType gearType, List<AffixDef> buffer)
        {
            buffer.Clear();

            var key = new FamilyKey(family);
            if (!_byFamily.TryGetValue(key, out var all) || all == null || all.Count == 0)
                return;

            for (int i = 0; i < all.Count; i++)
            {
                var d = all[i];
                if (d == null) continue;
                if (!d.Allows(gearType)) continue;
                buffer.Add(d);
            }
        }

        private readonly struct FamilyKey : IEquatable<FamilyKey>
        {
            public readonly AffixFamily Family;

            public FamilyKey(AffixFamily family)
            {
                Family = family;
            }

/// <summary>
/// Determines whether this instance is equal to another FamilyKey instance.
/// </summary>
/// <param name="other">The FamilyKey instance to compare with.</param>
/// <returns>True if the instances are equal; otherwise, false.</returns>
            public bool Equals(FamilyKey other) => Family == other.Family;
/// <summary>
/// Determines whether the specified object is equal to the current instance.
/// </summary>
/// <param name="obj">The object to compare with the current instance.</param>
/// <returns>True if the specified object is equal to the current instance; otherwise, false.</returns>
            public override bool Equals(object obj) => obj is FamilyKey other && Equals(other);
/// <summary>
/// Returns a hash code for the current instance based on the Family value.
/// </summary>
/// <returns>A hash code as an integer.</returns>
            public override int GetHashCode() => (int)Family;
        }
    }
}
