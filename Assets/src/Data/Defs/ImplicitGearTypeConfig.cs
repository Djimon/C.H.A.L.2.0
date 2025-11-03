using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "ImplicitGearTypeConfig", menuName = "Data/ImplicitGearTypeConfig")]
    public class ImplicitGearTypeConfig : ScriptableObject
    {

        private static readonly string[] DefaultImplicitIds =
        {
            "dmg_pct",
            "thorns_flat",      // bewusst genau so gelassen, falls du die ID schon nutzt
            "phys_dmg_flat",
            "elem_dmg_flat",
            "armor_pct",
            "elem_resist_pct",
            "dodge_pct",
            "barrier_pct",
            "armor_flat",
            "barrier_flat",
            "life_pct",
            "life_flat",
            "item_rarity_pct",
            "move_speed_pct",
        };


        public List<GearTypePool> Pools = new List<GearTypePool>
        {
            
            new GearTypePool{ GearType = GearType.Head,   Entries = new List<ImplicitWeight>() },
            new GearTypePool{ GearType = GearType.Chest,  Entries = new List<ImplicitWeight>() },
            new GearTypePool{ GearType = GearType.Gloves, Entries = new List<ImplicitWeight>() },
            new GearTypePool{ GearType = GearType.Legs,   Entries = new List<ImplicitWeight>() },
            new GearTypePool{ GearType = GearType.Boots,  Entries = new List<ImplicitWeight>() },
            new GearTypePool{ GearType = GearType.Amulet, Entries = new List<ImplicitWeight>() },
        };


        private void OnValidate()
        {
            if (Pools == null) return;

            // Duplikate pro GearType vermeiden, negative Gewichte clampen, IDs trimmen.
            var seenPerType = new Dictionary<GearType, HashSet<string>>();

            for (int p = 0; p < Pools.Count; p++)
            {
                var pool = Pools[p];
                if (!seenPerType.TryGetValue(pool.GearType, out var seen))
                {
                    seen = new HashSet<string>();
                    seenPerType[pool.GearType] = seen;
                }

                if (pool.Entries == null) continue;

                for (int i = 0; i < pool.Entries.Count; i++)
                {
                    var e = pool.Entries[i];
                    e.ImplicitId = (e.ImplicitId ?? string.Empty).Trim();
                    if (e.Weight < 0) e.Weight = 0;

                    // Warnungen bei "ungewöhnlichen" IDs (z.B. Sonderzeichen/Leerzeichen)
                    if (!IsValidId(e.ImplicitId))
                    {
                        DebugManager.Warning($"[ImplicitPoolsDef] Ungewöhnliche ImplicitId '{e.ImplicitId}' im Pool {pool.GearType}. " +
                                         "Empfohlenes Format: lower_snake_case (a-z, 0-9, _).", this);
                    }

                    // Deduplizieren (gleiche ID innerhalb eines GearType nur einmal)
                    if (!string.IsNullOrEmpty(e.ImplicitId))
                    {
                        if (seen.Contains(e.ImplicitId))
                        {
                            DebugManager.Warning($"[ImplicitPoolsDef] Doppelte ImplicitId '{e.ImplicitId}' im Pool {pool.GearType} – wird ignoriert.", this);
                            e.Weight = 0;
                        }
                        else
                        {
                            seen.Add(e.ImplicitId);
                        }
                    }

                    pool.Entries[i] = e;
                }

                // Fehlende Default-IDs hinzufügen (Weight = 0)
                for (int d = 0; d < DefaultImplicitIds.Length; d++)
                {
                    string id = DefaultImplicitIds[d];
                    if (!seen.Contains(id))
                    {
                        pool.Entries.Add(new ImplicitWeight { ImplicitId = id, Weight = 0 });
                        seen.Add(id);
                    }
                }

                Pools[p] = pool;
            }


        }

        private static bool IsValidId(string id)
        {
            // erlaubt: a-z, 0-9, _
            if (string.IsNullOrEmpty(id)) return false;
            for (int i = 0; i < id.Length; i++)
            {
                char c = id[i];
                bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                if (!ok) return false;
            }
            return true;
        }
    }

    [Serializable]
    public struct GearTypePool
    {
        public GearType GearType;
        public List<ImplicitWeight> Entries;
    }

    [Serializable]
    public struct ImplicitWeight
    {
        public string ImplicitId;
        public int Weight;
    }
}
