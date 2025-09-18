using CHAL.Data;
using System.Collections.Generic;
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
    }
}
