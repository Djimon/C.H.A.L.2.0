using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Items;
using CHAL.Systems.Loot;
using CHAL.Systems.Loot.Models;
using CHAL.Systems.Wave;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CHAL.Systems.Loot
{
    public sealed class LootRoller
    {
        private readonly LootRulesService _rules;
        private readonly UnluckyProtection _unlucky;

        public LootRoller(LootRulesService rules,UnluckyProtection unlucky)
        {
            _rules = rules;
            _unlucky = unlucky;
        }

        /// <summary>
        /// Rollt Loot für einen einzelnen Gegner bei dessen Tod.
        /// Nutzt Budget & Unlucky aus dem WaveContext.
        /// </summary>
        public List<LootResultEntry> RollLootForMonster(EnemyInstance monster, WaveLootContext ctx)
        {
            var results = new List<LootResultEntry>();

            if (monster.Tags == null || monster.Tags.Count == 0)
                return results;

            // 1. Multiplikator abhängig vom Rank bestimmen
            int rolls = BalanceManager.Instance.Config.loot.rankMultipliers.GetMultiplier(monster.Rank);

            for (int r = 0; r < rolls; r++)
            {
                // 2. Zufälligen Tag picken
                var tag = monster.Tags[Random.Range(0, monster.Tags.Count)];

                // 3. Regel für diesen Tag laden
                var merged = _rules.GetMergedForTags(new[] { tag });

                // 4. RNG-Loop über alle Drops im Pool
                foreach (var drop in merged.drops)
                {
                    float pBase = drop.chance ?? 0f;
                    if (drop.chancesArray != null && drop.chancesArray.Length > 0)
                        pBase = drop.chancesArray[Random.Range(0, drop.chancesArray.Length)];

                    // Unlucky / Budget
                    float multUnlucky = _unlucky.GetMultiplier(drop.rarity);
                    float pPre = pBase * multUnlucky;

                    float mBudget = LootBudgetModulator.GetModifier(ctx.SpentBudget, drop.lootValue, ctx.TotalBudget, drop.rarity);
                    float pEff = Mathf.Clamp(pPre * mBudget, 0f, 100f);

                    float roll = Random.Range(0f, 100f);
                    if (roll < pEff)
                    {
                        var entry = new LootResultEntry
                        {
                            EnemyId = monster.EnemyId,
                            PickedTag = tag,
                            ItemId = drop.itemId
                        };
                        results.Add(entry);
                        ctx.Drops.Add(entry);

                        ctx.SpentBudget += drop.lootValue;
                        _unlucky.OnDrop(drop.rarity);

                        DebugManager.Log($"{drop.itemId} dropped from {monster.EnemyId} ({monster.Rank}) via tag:{tag}",DebugManager.EDebugLevel.Test,"Loot");
                    }
                    else
                    {
                        _unlucky.OnFail(drop.rarity);
                    }
                }

                // 5. SecretDrops pro Monster
                var secretDrops = _rules.GetSecretDrops(monster.Tags);
                foreach (var sd in secretDrops)
                {
                    float roll = Random.Range(0f, 100f);
                    if (roll < sd.chance)
                    {
                        var entry = new LootResultEntry
                        {
                            EnemyId = monster.EnemyId,
                            PickedTag = sd.sourceTag,
                            ItemId = sd.itemId
                        };
                        results.Add(entry);
                        ctx.Drops.Add(entry);

                        DebugManager.Log($"Secret drop {sd.itemId} from {monster.EnemyId}",DebugManager.EDebugLevel.Test,"Loot");
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Wellenabschluss: erzwingt MinDrops, Rarity-Guarantees und globale SecretDrops.
        /// </summary>
        public void FinalizeWave(WaveLootContext ctx)
        {
            var allTags = ctx.Wave.Monsters.SelectMany(m => m.Tags).Distinct().ToArray();
            var mergedWave = _rules.GetMergedForTags(allTags);

            // MinDrops-Failsafe
            while (ctx.Drops.Count < mergedWave.minDrops)
            {
                var pick = mergedWave.drops[Random.Range(0, mergedWave.drops.Count)];
                var entry = new LootResultEntry { EnemyId = "WaveBonus", PickedTag = "Failsafe", ItemId = pick.itemId };
                ctx.Drops.Add(entry);
                DebugManager.Log($"Added {pick.itemId} to reach minDrops",DebugManager.EDebugLevel.Dev,"Loot");
            }

            // RarityGuarantees
            foreach (var kv in mergedWave.rarityGuarantees)
            {
                var rarity = kv.Key;
                int min = kv.Value;
                int count = ctx.Drops.Count(d => ItemRegistry.Instance.GetRarity(d.ItemId) == rarity);

                while (count < min)
                {
                    var candidates = mergedWave.drops.FindAll(d => d.rarity == rarity);
                    if (candidates.Count == 0) break;

                    var pick = candidates[Random.Range(0, candidates.Count)];
                    var entry = new LootResultEntry { EnemyId = "WaveBonus", PickedTag = "Guarantee", ItemId = pick.itemId };
                    ctx.Drops.Add(entry);
                    DebugManager.Log($"Guaranteed {rarity} → {pick.itemId}",DebugManager.EDebugLevel.Dev,"Loot");
                    count++;
                }
            }

            // SecretRules (Wave-wide) // GameDesign-decision -> To easy to get the secrets with this implementation
            //var secretDrops = _rules.GetSecretDrops(allTags);
            //foreach (var sd in secretDrops)
            //{
            //    float roll = Random.Range(0f, 100f);
            //    if (roll < sd.chance)
            //    {
            //        var entry = new LootResultEntry { EnemyId = "WaveBonus", PickedTag = "WaveSecret", ItemId = sd.itemId };
            //        ctx.Drops.Add(entry);
            //        DebugManager.Log($"Wave secret drop {sd.itemId}",DebugManager.EDebugLevel.Test,"Loot");
            //    }
            //}
        }
    }
}
