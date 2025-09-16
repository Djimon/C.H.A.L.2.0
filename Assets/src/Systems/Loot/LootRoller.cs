using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Items;
using CHAL.Systems.Loot.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

namespace CHAL.Systems.Loot
{
    public sealed class LootRoller
    {
        private readonly LootRulesService _rules;
        private readonly UnluckyProtection _unlucky;

        public LootRoller(LootRulesService rules, UnluckyProtection unlucky)
        {
            _rules = rules;
            _unlucky = unlucky;
        }

        /// <summary>
        /// Haupt-Einstieg: rollt Loot für eine komplette Welle.
        /// </summary>
        public List<LootResultEntry> RollLoot(WaveComposition wave)
        {
            var finalLoot = new List<LootResultEntry>();

            // 1) Budget berechnen
            int B = LootBudgetCalculator.CalculateBudget(
                wave.TotalSpawns, wave.TotalNormals, wave.TotalMagics,
                wave.TotalElites, wave.TotalBosses, wave.TotalChampions,
                wave.Level, wave.Difficulty
            );
            int U = 0; // bisher verbrauchtes Budget

            // 2) Normale Drops für jede Monster-Instanz
            foreach (var monster in wave.Monsters)
            {
                for (int i = 0; i < monster.Count; i++)
                {
                    if (monster.Tags == null || monster.Tags.Count == 0)
                        continue;

                    // zufällig einen Tag picken
                    var tag = monster.Tags[Random.Range(0, monster.Tags.Count)];

                    // Regel für diesen Tag laden
                    var merged = _rules.GetMergedForTags(new[] { tag });

                    // Würfelprozess für jeden Drop-Kandidaten
                    foreach (var drop in merged.drops)
                    {
                        float pBase = drop.chance ?? 0f;
                        if (drop.chancesArray != null && drop.chancesArray.Length > 0)
                        {
                            pBase = drop.chancesArray[Random.Range(0, drop.chancesArray.Length)];
                        }

                        float multUnlucky = _unlucky.GetMultiplier(drop.rarity);
                        float pPre = pBase * multUnlucky;

                        float mBudget = LootBudgetModulator.GetModifier(U, drop.lootValue, B, drop.rarity);
                        float pEff = Mathf.Clamp(pPre * mBudget, 0f, 100f);

                        //DebugManager.Log($"{drop.itemId}: {pEff}% (base:{pBase}, unlucky:{multUnlucky}, budget:{mBudget})", DebugManager.EDebugLevel.Debug, "Loot");

                        float roll = Random.Range(0f, 100f);
                        if (roll < pEff)
                        {
                            finalLoot.Add(new LootResultEntry 
                                {
                                    EnemyId = monster.EnemyId,
                                    PickedTag = tag,
                                    ItemId = drop.itemId
                                });
                            U += drop.lootValue;
                            DebugManager.Log($"Item: {drop.itemId}({drop.rarity}) dropped (chance: {pEff})", DebugManager.EDebugLevel.Dev, "Loot");
                            _unlucky.OnDrop(drop.rarity);
                        }
                        else
                        {
                            _unlucky.OnFail(drop.rarity);
                        }
                    }

                    // 3) Secret Rules prüfen – für dieses eine Monster
                    var secretDrops = _rules.GetSecretDrops(monster.Tags);
                    foreach (var sd in secretDrops)
                    {
                        float roll = Random.Range(0f, 100f);
                        if (roll < sd.chance)
                        {
                            finalLoot.Add(new LootResultEntry
                            {
                                EnemyId = monster.EnemyId,
                                PickedTag = string.Join(",", monster.Tags), // alle Tags für SecretRule
                                ItemId = sd.itemId
                            });
                            DebugManager.Log($"Secret Drop:{sd.itemId}({ItemRegistry.Instance.GetRarity(sd.itemId)}) - chance was {sd.chance}%", DebugManager.EDebugLevel.Test, "Loot");
                        }
                    }
                }
            }

            // 4) Post-Processing: minDrops, maxDrops, rarityGuarantees
            //    (auf Basis der gesamten Welle, nicht einzelner Monster)
            var mergedWave = _rules.GetMergedForWave(wave);
            ApplyPostProcessing(mergedWave, finalLoot);

            return finalLoot;
        }

        private void ApplyPostProcessing(MergedLoot merged, List<LootResultEntry> loot)
        {
            // minDrops
            while (loot.Count < merged.minDrops)
            {
                var pick = merged.drops[Random.Range(0, merged.drops.Count)];
                loot.Add(new LootResultEntry
                {
                    EnemyId = "PostProcess",
                    PickedTag = "rule:minDrops",
                    ItemId = pick.itemId
                });
                DebugManager.Log($"added {pick.itemId}({pick.rarity}) to reach minDrop", DebugManager.EDebugLevel.Dev, "Loot");
            }

            // maxDrops
            SmartTrim(merged, loot, BalanceManager.Instance.Config);

            // rarityGuarantees
            foreach (var kv in merged.rarityGuarantees)
            {
                var rarity = kv.Key;
                int min = kv.Value;

                int count = loot.Count(entry => ItemRegistry.Instance.GetRarity(entry.ItemId) == rarity);

                while (count < min)
                {
                    var candidates = merged.drops.FindAll(d => d.rarity == rarity);
                    if (candidates.Count == 0) break;

                    var pick = candidates[Random.Range(0, candidates.Count)];
                    loot.Add(new LootResultEntry
                    {
                        EnemyId = "PostProcess",
                        PickedTag = "rule:guaranteeDrops",
                        ItemId = pick.itemId
                    });
                    DebugManager.Log($"added {pick.itemId}({pick.rarity}) as guaranteed drop.", DebugManager.EDebugLevel.Dev, "Loot");
                    count++;
                }
            }
        }

        private void SmartTrim(MergedLoot merged, List<LootResultEntry> loot, GameBalanceConfig balance)
        {
            if (merged.maxDrops <= 0 || loot.Count <= merged.maxDrops)
                return;

            var trimWeights = balance.loot.trim;

            // so lange wir zu viele Drops haben → zufällig Items entfernen
            while (loot.Count > merged.maxDrops)
            {
                // Gewichte aufstellen
                var weightedList = new List<LootResultEntry>();

                foreach (var entry in loot)
                {
                    var rarity = ItemRegistry.Instance.GetRarity(entry.ItemId);
                    float weight = rarity switch
                    {
                        Rarity.Common => trimWeights.common,
                        Rarity.Uncommon => trimWeights.uncommon,
                        Rarity.Rare => trimWeights.rare,
                        Rarity.Epic => trimWeights.epic,
                        Rarity.Legendary => trimWeights.legendary,
                        _ => 1f
                    };

                    int slots = Mathf.CeilToInt(weight * 100);
                    for (int i = 0; i < slots; i++)
                        weightedList.Add(entry);
                }

                if (weightedList.Count > 0)
{
                    var removeEntry = weightedList[Random.Range(0, weightedList.Count)];
                    loot.Remove(removeEntry);
                    DebugManager.Log($"SmartTrim removed {removeEntry.ItemId}({ItemRegistry.Instance.GetRarity(removeEntry.ItemId)}) to reach configured maxDrop", DebugManager.EDebugLevel.Dev, "Loot");
                }
                else
                {
                    // Fallback: uniform random, falls alle Weights 0 sind
                    int idx = Random.Range(0, loot.Count);
                    var removeEntry = loot[idx];
                    loot.RemoveAt(idx);
                    DebugManager.Log($"SmartTrim removed {removeEntry.ItemId}({ItemRegistry.Instance.GetRarity(removeEntry.ItemId)}) to reach configured maxDrop", DebugManager.EDebugLevel.Dev, "Loot");
                }

            }
        }
    
        
    }
}
