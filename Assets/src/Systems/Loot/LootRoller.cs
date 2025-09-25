using CHAL.Core;
using CHAL.Data;
using CHAL.Systems.Items;
using CHAL.Systems.Loot;
using CHAL.Systems.Loot.Models;
using CHAL.Systems.Wave;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CHAL.Data.GameBalanceConfig;

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

                    
                    if (drop.chancesArray != null && drop.chancesArray.Length > 0)
                    {
                        for (int i = 0; i < drop.chancesArray.Length; i++)
                        {
                            float pBase = drop.chancesArray[i];
                            ExecuteDrop(monster, ctx, results, tag, drop, pBase);
                        }
                    }
                    else {
                        float pBase = drop.chance ?? 0f;
                        ExecuteDrop(monster, ctx, results, tag, drop, pBase);
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
                            ItemId = sd.itemId,
                            quantity = sd.quantity
                        };
                        results.Add(entry);
                        ctx.Drops.Add(entry);
                        //No Unlucky reset

                        DebugManager.Log($"Secret drop {sd.itemId} from {monster.EnemyId}",DebugManager.EDebugLevel.Test,"Loot");
                    }
                }
            }

            return results;
        }

        private void ExecuteDrop(EnemyInstance monster, WaveLootContext ctx, List<LootResultEntry> results, string tag, LootDrop drop, float pBase)
        {
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
                    ItemId = drop.itemId,
                    quantity = drop.quantity,
                };
                results.Add(entry);
                ctx.Drops.Add(entry);

                ctx.SpentBudget += drop.lootValue;
                _unlucky.OnDrop(drop.rarity);

                DebugManager.Log($"{drop.itemId} ({drop.quantity}x) dropped from {monster.EnemyId} ({monster.Rank}) via tag:{tag}", DebugManager.EDebugLevel.Test, "Loot");
            }
            else
            {
                _unlucky.OnFail(drop.rarity);
            }
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
                var entry = new LootResultEntry { 
                        EnemyId = "WaveBonus",
                        PickedTag = "Failsafe",
                        ItemId = pick.itemId,
                        quantity = pick.quantity};

                ctx.Drops.Add(entry);
                ctx.SpentBudget += pick.lootValue;
                _unlucky.OnDrop(pick.rarity);
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
                    var entry = new LootResultEntry {
                            EnemyId = "WaveBonus",
                            PickedTag = "Guarantee",
                            ItemId = pick.itemId,
                            quantity = pick.quantity};

                    ctx.Drops.Add(entry);
                    //No Unlucky reset
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


        public int RollGoldForMonster(EnemyInstance enemy, int maplvl)
        {
            var rank = enemy.Rank;
            var curr = BalanceManager.Instance.Config.economy.currencies;
            var loot = BalanceManager.Instance.Config.loot;

            int baseModifier = rank switch
            {
                EnemyRank.Spawn => loot.rankMultipliers.spawn,
                EnemyRank.Normal => loot.rankMultipliers.normal,
                EnemyRank.Magic => loot.rankMultipliers.magic,
                EnemyRank.Elite => loot.rankMultipliers.elite,
                EnemyRank.Boss => loot.rankMultipliers.boss,
                EnemyRank.Champion => loot.rankMultipliers.champion,
                _ => 1
            };

            return Mathf.RoundToInt(curr.baseGoldReward * baseModifier  + (maplvl) * curr.goldPerLevel);

        }

        public int RollXPForMonster(EnemyInstance enemy, int mapLevel, MapDifficulty difficulty, int waveLevel)
        {
            var econ = BalanceManager.Instance.Config.economy;
            var enemyscaling = BalanceManager.Instance.Config.enemies.rankScaling;
            var rank = enemy.Rank;

            float baseXp = rank switch
            {
                EnemyRank.Spawn => enemyscaling.spawn.xpMultiplier,
                EnemyRank.Normal => enemyscaling.normal.xpMultiplier,
                EnemyRank.Magic => enemyscaling.magic.xpMultiplier,
                EnemyRank.Elite => enemyscaling.elite.xpMultiplier,
                EnemyRank.Boss => enemyscaling.boss.xpMultiplier,
                EnemyRank.Champion => enemyscaling.champion.xpMultiplier,
                _ => 1
            };

            int difficultyBonus = difficulty switch
            { 
                MapDifficulty.Stable => 1,
                MapDifficulty.Strained => 3,
                MapDifficulty.Volatile => 10,
                MapDifficulty.Chaos => 50,
                _ => 1
            };


            float scaled = econ.xp.baseXpReward * baseXp * difficultyBonus
                    + (1f + (mapLevel-1) * econ.xp.xpPerLevel *(waveLevel * 0.1f));
              
            return Mathf.RoundToInt(scaled);
        }
    }
}
