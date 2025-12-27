// File: Assets/src/CHAL/Systems/Gear/ImplicitRegistry.cs
using System;
using System.Collections.Generic;

using CHAL.Data;

namespace CHAL.Systems.Items
{
    public sealed class ImplicitRegistry
    {
        private readonly Dictionary<string, ImplicitDef> _byId = new(StringComparer.Ordinal);
        private readonly Dictionary<PoolRoleKey, List<ImplicitDef>> _byPoolRole = new();

        public ImplicitRegistry(ImplicitRegistryDef def)
        {
            if (def == null || def.Implicits == null)
            {
                DebugManager.Error("[ImplicitRegistry] RegistryDef missing or empty.", "System");
                return;
            }

            for (int i = 0; i < def.Implicits.Count; i++)
            {
                var d = def.Implicits[i];
                if (d == null) continue;

                var id = (d.Id ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(id)) continue;

                if (_byId.ContainsKey(id))
                {
                    DebugManager.Warning($"[ImplicitRegistry] Duplicate Id '{id}' ignored (asset '{d.name}').", "System");
                    continue;
                }

                _byId[id] = d;

                var key = new PoolRoleKey(d.Pool, d.Role);
                if (!_byPoolRole.TryGetValue(key, out var list))
                {
                    list = new List<ImplicitDef>(16);
                    _byPoolRole[key] = list;
                }
                list.Add(d);
            }
        }

        public bool TryGet(string implicitId, out ImplicitDef def)
        {
            def = null;
            if (string.IsNullOrEmpty(implicitId)) return false;
            return _byId.TryGetValue(implicitId, out def) && def != null;
        }

        /// <summary>
        /// Returns candidates for Pool+Role, filtered by AllowedGearTypes.
        /// Result list is written into <paramref name="buffer"/> (cleared first).
        /// </summary>
        public void GetCandidates(ImplicitPool pool, ImplicitRole role, GearType gearType, List<ImplicitDef> buffer)
        {
            buffer.Clear();

            var key = new PoolRoleKey(pool, role);
            if (!_byPoolRole.TryGetValue(key, out var all) || all == null || all.Count == 0)
                return;

            for (int i = 0; i < all.Count; i++)
            {
                var d = all[i];
                if (d == null) continue;
                if (!d.Allows(gearType)) continue;
                buffer.Add(d);
            }
        }

        private readonly struct PoolRoleKey : IEquatable<PoolRoleKey>
        {
            public readonly ImplicitPool Pool;
            public readonly ImplicitRole Role;

            public PoolRoleKey(ImplicitPool pool, ImplicitRole role)
            {
                Pool = pool;
                Role = role;
            }

            public bool Equals(PoolRoleKey other) => Pool == other.Pool && Role == other.Role;
            public override bool Equals(object obj) => obj is PoolRoleKey other && Equals(other);
            public override int GetHashCode() => ((int)Pool * 397) ^ (int)Role;
        }
    }
}
