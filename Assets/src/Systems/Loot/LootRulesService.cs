using CHAL.Data;
using CHAL.Systems.Enemy;
using CHAL.Systems.Items;
using CHAL.Systems.Loot.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CHAL.Systems.Loot
{
    public sealed class LootRulesService
    {
        private readonly Dictionary<string, LootRule> _byTag = new();
        private readonly List<SpecialRule> _secretRules = new();

        //NORMALE RULES
/// <summary>
/// Loads all loot rules from the specified resources.
/// </summary>
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
                        DebugManager.Warning($"[LootRules] Duplicate tag '{rule.tag}' in {ta.name}, will be overwritten.");
                    }
                    _byTag[rule.tag] = rule;
                }
                catch (System.Exception ex)
                {
                    DebugManager.Error($"[LootRules] Error in {ta.name}: {ex.Message}");
                }
            }
            DebugManager.Log($"[LootRules] Loaded: {_byTag.Count} tag rules",DebugManager.EDebugLevel.Dev,"System");

            LoadSecretRules();

            ItemRegistry.Instance.ValidateUnusedItems();


            //DumpAllKnownTagsOnce();

            WarnIfMissingLootRulesForAllMonsterTags();
        }

        private void WarnIfMissingLootRulesForAllMonsterTags()
        {
            MonsterTagRegistry.Instance.LoadAll();

            var allTags = MonsterTagRegistry.Instance.All;
            int missing = 0;

#if UNITY_EDITOR
            
#endif

            foreach (var tagDef in allTags)
            {
                if (tagDef == null) continue;

                var id = (tagDef.tagId ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(id)) continue;

                if (_byTag.ContainsKey(id))
                    continue;

                missing++;
                DebugManager.Warning($"[LootRules] No LootRule for MonsterTag '{id}' (category={tagDef.category})", "Validation");

#if UNITY_EDITOR
                var misisng_path = "Assets/Resources/data/LootRules/missing";
                if (!Directory.Exists(misisng_path))
                    Directory.CreateDirectory(misisng_path);

                try
                {
                    var fileName = SanitizeFileName(id) + ".json";
                    var fullPath = Path.Combine(misisng_path, fileName);

                    if (!File.Exists(fullPath))
                    {
                        File.WriteAllText(fullPath, BuildMissingLootRuleJson(id));
                        DebugManager.Log($"[LootRules] Created placeholder LootRule: {fullPath}", DebugManager.EDebugLevel.Dev, "System");
                    }
                }
                catch (System.Exception ex)
                {
                    DebugManager.Warning($"[LootRules] Failed to create placeholder LootRule for '{id}': {ex.Message}", "Validation");
                }
#endif
            }

            if (missing > 0)
                DebugManager.Warning($"[LootRules] Missing LootRules for {missing} MonsterTags", "Validation");

        }

        private void DumpAllKnownTagsOnce()
        {
            try
            {
                // Sammeln mit Source-Info
                var buckets = new Dictionary<string, HashSet<string>>(System.StringComparer.OrdinalIgnoreCase);

                void AddTag(string tag, string source)
                {
                    tag = NormalizeTag(tag);
                    if (string.IsNullOrEmpty(tag)) return;

                    if (!buckets.TryGetValue(tag, out var set))
                    {
                        set = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
                        buckets[tag] = set;
                    }
                    set.Add(source);
                }

                // 1) LootRule Tags (aus geladenen JSONs)
                foreach (var kv in _byTag)
                    AddTag(kv.Key, "lootrules:tag");

                // 2) EnemyDef.baseTags
                var enemies = Resources.LoadAll<EnemyDef>("data/Enemies");
                foreach (var e in enemies)
                {
                    if (e == null || e.baseTags == null) continue;
                    foreach (var t in e.baseTags)
                        AddTag(t, $"enemydef:{e.enemyId}");
                }

                // 3) MapDef.allowedModifiers
                var maps = Resources.LoadAll<MapDef>("data/Maps");
                foreach (var m in maps)
                {
                    if (m == null || m.allowedMonsterTags == null) continue;
                    foreach (var t in m.allowedMonsterTags)
                        AddTag(t, $"mapdef:{m.mapId}");
                }

                // Export
                var exportDir = Path.Combine(Application.dataPath, "../");
                if (!Directory.Exists(exportDir)) Directory.CreateDirectory(exportDir);

                var exportPath = Path.Combine(exportDir, "monster_tags_dump.csv");

                var lines = new List<string>{"tagId,sources"};

                foreach (var tag in buckets.Keys.OrderBy(x => x))
                {
                    var sources = string.Join("|", buckets[tag].OrderBy(x => x));
                    lines.Add($"{tag},{sources}");
                }

                File.WriteAllLines(exportPath, lines);

                DebugManager.Log($"[TagDump] Wrote {buckets.Count} tags to {exportPath}", DebugManager.EDebugLevel.Dev, "System");

            }
            catch (System.Exception ex)
            {
                DebugManager.Error($"[TagDump] Failed: {ex.Message}");
            }
        }

        private static string NormalizeTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return null;
            return tag.Trim(); // absichtlich kein ToLower() -> falls du Groß/Klein behalten willst
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
                    Rarity r = kv.rarity;
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

                if (!ItemRegistry.Instance.EnsureExistsAndMarkUsed(d.itemId, "lootrules", $"file={sourceName}, tag={dto.tag}", out var def))
                    throw new System.Exception($"Ungültige itemId '{d.itemId}'");

                if (def == null)
                    throw new System.Exception($"Item '{d.itemId}' konnte nicht aufgelöst werden (file={sourceName})");


                var drop = new LootDrop
                {
                    itemId = d.itemId,
                    quantity = Mathf.Max(1, d.quantity),
                    chance = (d.chances == null || d.chances.Length == 0) ? d.chance : (float?)null,
                    chancesArray = (d.chances != null && d.chances.Length > 0) ? d.chances : null,
                    rarity = def.rarity,
                    lootValue = def.lootValue,
                    sourceTag = dto.tag
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


/// <summary>
/// Tries to get a loot rule associated with the specified tag.
/// </summary>
/// <param name="tag">The tag to look up the loot rule.</param>
/// <param name="rule">The loot rule associated with the tag, if found.</param>
/// <returns>True if the rule was found; otherwise, false.</returns>
        public bool TryGetRule(string tag, out LootRule rule) => _byTag.TryGetValue(tag, out rule);

        // Merge-Policy: 
        // - drops: concat in Reihenfolge der Tags (stabil)
        // - minDrops/maxDrops: nehmen das MAX über alle beteiligten Tags (0 wird ignoriert)
        // - rarityGuarantees: pro Rarity das MAX über alle Tags
/// <summary>
/// Merges loot based on the specified tags.
/// </summary>
/// <param name="tags">The collection of tags to filter the loot.</param>
/// <returns>The merged loot corresponding to the provided tags.</returns>
        public MergedLoot GetMergedForTags(IEnumerable<string> tags)
        {
            var merged = new MergedLoot();

            foreach (var tag in tags)
            {
                if (!_byTag.TryGetValue(tag, out var rule))
                {
                    DebugManager.Warning($"[LootRules] No rule found for tag '{tag}'");
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

/// <summary>
/// Merges loot for a given wave composition of monsters.
/// </summary>
/// <param name="wave">The wave composition containing the monsters.</param>
/// <returns>The merged loot from the specified wave.</returns>
        public MergedLoot GetMergedForWave(WaveComposition wave)
        {
            var merged = new MergedLoot();

            foreach (var monster in wave.Monsters)
            {
                for (int i = 0; i < monster.Count; i++)
                {
                    foreach (var tag in monster.bonusTags)
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
                    DebugManager.Error($"[SecretRules] Error in {ta.name}: {ex.Message}");
                }
            }
            DebugManager.Log($"[SecretRules] Loaded: {_secretRules.Count} rules",DebugManager.EDebugLevel.Dev,"System");
        }

/// <summary>
/// Retrieves a list of secret loot drops based on the specified monster tags.
/// </summary>
/// <param name="monsterTags">The tags of the monsters to match against.</param>
/// <returns>A list of loot drop data transfer objects.</returns>
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


#if UNITY_EDITOR
        private static string SanitizeFileName(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "missing_tag";
            foreach (var c in Path.GetInvalidFileNameChars())
                s = s.Replace(c, '_');
            return s.Trim();
        }

        private static string BuildMissingLootRuleJson(string tagId)
        {
            // Genau im Stil, den du wolltest
            return
        $@"{{
  ""tag"": ""{tagId}"",
  ""drops"": [
    {{ ""itemId"": ""part:none"", ""chance"": 100, ""quantity"": 1 }}
  ],
  ""minDrops"": 1,
  ""maxDrops"": 1,
  ""rarityGuarantees"": []
}}";
        }
#endif
    }
}
