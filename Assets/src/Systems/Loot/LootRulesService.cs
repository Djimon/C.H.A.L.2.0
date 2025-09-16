using CHAL.Data;
using CHAL.Systems.Items;
using CHAL.Systems.Loot.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CHAL.Systems.Loot
{
    public sealed class LootRulesService
    {
        private readonly Dictionary<string, LootRule> _byTag = new();
        private readonly List<SpecialRule> _secretRules = new();

        //NORMALE RULES
        public void LoadAll()
        {
            _byTag.Clear();
            var assets = Resources.LoadAll<TextAsset>("data/LootRules");
            foreach (var ta in assets)
            {
                try
                {
                    var dto = JsonUtility.FromJson<LootRuleDto>(ta.text);
                    var rule = ToRule(dto, ta.name);
                    if (_byTag.ContainsKey(rule.tag))
                    {
                        Debug.LogWarning($"[LootRules] Duplicate tag '{rule.tag}' in {ta.name}, wird überschrieben.");
                    }
                    _byTag[rule.tag] = rule;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[LootRules] Fehler in {ta.name}: {ex.Message}");
                }
            }
            Debug.Log($"[LootRules] Geladen: {_byTag.Count} Tag-Rules");

            LoadSecretRules();
        }

        private LootRule ToRule(LootRuleDto dto, string sourceName)
        {
            if (string.IsNullOrWhiteSpace(dto.tag))
                throw new System.Exception("tag fehlt");

            var rule = new LootRule { tag = dto.tag, minDrops = dto.minDrops, maxDrops = dto.maxDrops };

            // rarityGuarantees
            if (dto.rarityGuarantees != null)
            {
                foreach (var kv in dto.rarityGuarantees)
                {
                    if (!System.Enum.TryParse<Rarity>(kv.rarity, out var r))
                        throw new System.Exception($"rarityGuarantees: unbekannte Rarity '{kv.rarity}'");
                    rule.rarityGuarantees[r] = Mathf.Max(0, kv.min);
                }
            }

            // drops
            if (dto.drops == null || dto.drops.Length == 0)
                throw new System.Exception("drops leer");

            foreach (var d in dto.drops)
            {
                if (!ItemKey.TryParse(d.itemId, out _))
                    throw new System.Exception($"Ungültige itemId '{d.itemId}'");

                if (!ItemRegistry.Instance.TryGet(d.itemId, out var def))
                    throw new System.Exception($"Item '{d.itemId}' nicht in ItemRegistry gefunden");

                var drop = new LootDrop
                {
                    itemId = d.itemId,
                    quantity = Mathf.Max(1, d.quantity),
                    chance = (d.chances == null || d.chances.Length == 0) ? d.chance : (float?)null,
                    chancesArray = (d.chances != null && d.chances.Length > 0) ? d.chances : null,
                    rarity = def.rarity,
                    lootValue = def.lootValue
                };

                // Validierung: chance ODER chancesArray
                if (drop.chance is float c && (c < 0 || c > 100))
                    throw new System.Exception($"chance {c} außerhalb [0,100] für '{d.itemId}'");
                if (drop.chancesArray != null)
                {
                    foreach (var cc in drop.chancesArray)
                    {
                        if (cc < 0 || cc > 100) throw new System.Exception($"chances[] Wert {cc} außerhalb [0,100] für '{d.itemId}'");
                    }
                }
                if (drop.chance == null && drop.chancesArray == null)
                    throw new System.Exception($"Weder chance noch chances[] gesetzt für '{d.itemId}'");

                rule.drops.Add(drop);
            }

            return rule;
        }

        public bool TryGetRule(string tag, out LootRule rule) => _byTag.TryGetValue(tag, out rule);

        // Merge-Policy: 
        // - drops: concat in Reihenfolge der Tags (stabil)
        // - minDrops/maxDrops: nehmen das MAX über alle beteiligten Tags (0 wird ignoriert)
        // - rarityGuarantees: pro Rarity das MAX über alle Tags
        public MergedLoot GetMergedForTags(IEnumerable<string> tags)
        {
            var merged = new MergedLoot();

            foreach (var tag in tags)
            {
                if (!_byTag.TryGetValue(tag, out var rule))
                {
                    Debug.LogWarning($"[LootRules] Keine Rule für tag '{tag}' gefunden");
                    continue;
                }

                merged.drops.AddRange(rule.drops);

                if (rule.minDrops > 0)
                    merged.minDrops = Mathf.Max(merged.minDrops, rule.minDrops);
                if (rule.maxDrops > 0)
                    merged.maxDrops = Mathf.Max(merged.maxDrops, rule.maxDrops);

                foreach (var kv in rule.rarityGuarantees)
                {
                    var r = kv.Key; var min = kv.Value;
                    if (!merged.rarityGuarantees.TryGetValue(r, out var cur)) cur = 0;
                    merged.rarityGuarantees[r] = Mathf.Max(cur, min);
                }
            }

            return merged;
        }

        public MergedLoot GetMergedForWave(WaveComposition wave)
        {
            var merged = new MergedLoot();

            foreach (var monster in wave.Monsters)
            {
                for (int i = 0; i < monster.Count; i++)
                {
                    foreach (var tag in monster.Tags)
                    {
                        if (!_byTag.TryGetValue(tag, out var rule))
                            continue;

                        merged.drops.AddRange(rule.drops);

                        if (rule.minDrops > 0)
                            merged.minDrops += rule.minDrops; // Summe
                        if (rule.maxDrops > 0)
                            merged.maxDrops += rule.maxDrops; // Summe

                        foreach (var kv in rule.rarityGuarantees)
                        {
                            var rarity = kv.Key;
                            int min = kv.Value;
                            if (!merged.rarityGuarantees.TryGetValue(rarity, out var cur)) cur = 0;
                            merged.rarityGuarantees[rarity] = cur + min; // Summierung
                        }
                    }
                }
            }

            return merged;
        }

        private void LoadSecretRules()
        {
            _secretRules.Clear();
            var assets = Resources.LoadAll<TextAsset>("data/LootComboRules");
            foreach (var ta in assets)
            {
                try
                {
                    var wrapper = JsonUtility.FromJson<SpecialRulesWrapper>(ta.text);
                    if (wrapper?.rules == null) continue;
                    _secretRules.AddRange(wrapper.rules);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[SecretRules] Fehler in {ta.name}: {ex.Message}");
                }
            }
            Debug.Log($"[SecretRules] Geladen: {_secretRules.Count} Regeln");
        }

        public List<LootDropDto> GetSecretDrops(IEnumerable<string> monsterTags)
        {
            var extras = new List<LootDropDto>();

            foreach (var rule in _secretRules)
            {
                if (MatchesAll(monsterTags, rule.tags))
                {
                    extras.AddRange(rule.drops);
                }
            }

            return extras;
        }

        private bool MatchesAll(IEnumerable<string> presentTags, IEnumerable<string> requiredTags)
        {
            foreach (var tag in requiredTags)
            {
                if (!presentTags.Contains(tag))
                    return false;
            }
            return true;
        }
    }
}