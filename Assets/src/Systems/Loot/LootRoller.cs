using System.Collections.Generic;
using UnityEngine;

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

        //TODO: Refactor use a kinf of WaveComposition (all details of numbers and difficulty and other modifiers of the last wave)
        //  also change form tag-List to Dictionary<Tag,Count>
        //   --> public List<string> RollLoot(Dictionary<string,int> enemyTagCounts, WaveComposition wave)

        /// <summary>
        /// Rollt Loot für eine Gegner-Welle.
        /// </summary>
        /// <param name="enemyTags">Tags der Gegner (z.B. insect, swarm, lvl3)</param>
        /// <param name="spawns">Anzahl Spawns</param>
        /// <param name="normals">Anzahl Normale</param>
        /// <param name="magics">Anzahl Magic</param>
        /// <param name="elites">Anzahl Elites</param>
        /// <param name="bosses">Anzahl Bosse</param>
        /// <param name="champions">Anzahl Champions</param>
        /// <param name="level">Level der Welle</param>
        /// <param name="difficulty">Difficulty-Multiplikator (0.9/1.0/1.2)</param>
        public List<string> RollLoot(IReadOnlyList<string> enemyTags, int spawns, int normals, int magics, int elites, int bosses, int champions, int level, float difficulty)
        {
            var finalLoot = new List<string>();

            // 1) Budget berechnen
            int B = LootBudgetCalculator.CalculateBudget(spawns, normals, magics, elites, bosses, champions, level, difficulty);
            int U = 0;

            // 2) Kandidaten sammeln
            var merged = _rules.GetMergedForTags(enemyTags);

            // 3) Normaler Loot
            foreach (var drop in merged.drops)
            {
                // --- Chance berechnen ---
                float pBase = drop.chance ?? 0f;
                if (drop.chancesArray != null && drop.chancesArray.Length > 0)
                {
                    // falls mehrere Chancen angegeben, nimm zufällig eine (Varianz)
                    pBase = drop.chancesArray[Random.Range(0, drop.chancesArray.Length)];
                }

                // Unlucky-Boost
                float multUnlucky = _unlucky.GetMultiplier(drop.rarity);
                float pPre = pBase * multUnlucky;

                // Budget-Modulator
                float mBudget = LootBudgetModulator.GetModifier(U, drop.lootValue, B, drop.rarity);

                // Effektive Chance
                float pEff = Mathf.Clamp(pPre * mBudget, 0f, 100f);

                Debug.Log($"[LootRoller]: effecitve chance for {drop.itemId}:{drop.rarity} is {pEff} (BudgetoverflowMultiplier:{mBudget}, UnluckyProt:{multUnlucky}, pBase{pBase}");

                // --- Roll ---
                float roll = Random.Range(0f, 100f);
                if (roll < pEff)
                {
                    // Drop!
                    finalLoot.Add(drop.itemId);
                    U += drop.lootValue;
                    _unlucky.OnDrop(drop.rarity);

                    Debug.Log($"[LootRoller]: {drop.itemId} dropped!");
                }
                else
                {
                    _unlucky.OnFail(drop.rarity);
                    Debug.Log($"[LootRoller]: No Drop :(");
                }
            }

            // 4) Secret Rules → on top, budgetfrei
            var secretDrops = _rules.GetSecretDrops(enemyTags);
            foreach (var sd in secretDrops)
            {
                float chance = sd.chance;
                float roll = Random.Range(0f, 100f);
                if (roll < chance)
                {
                    finalLoot.Add(sd.itemId);
                }
            }

            // 5) Post-Processing: minDrops, maxDrops, rarityGuarantees
            ApplyPostProcessing(merged, finalLoot);

            return finalLoot;
        }

        private void ApplyPostProcessing(MergedLoot merged, List<string> loot)
        {
            // minDrops
            while (loot.Count < merged.minDrops)
            {
                // Zieh erneut aus dem kompletten Pool
                var pick = merged.drops[Random.Range(0, merged.drops.Count)];
                loot.Add(pick.itemId);
                Debug.Log($"[LootRoller] minDrops erzwungen → {pick.itemId}");
            }

            // maxDrops
            if (merged.maxDrops > 0 && loot.Count > merged.maxDrops)
            {
                loot.RemoveRange(merged.maxDrops, loot.Count - merged.maxDrops);
            }

            // rarityGuarantees (vereinfacht: prüfe nur ob vorhanden, sonst add Dummy)
            foreach (var kv in merged.rarityGuarantees)
            {
                var rarity = kv.Key;
                int min = kv.Value;

                int count = 0;
                foreach (var itemId in loot)
                {
                    if (ItemRegistry.Instance.GetRarity(itemId) == rarity)
                        count++;
                }

                while (count < min)
                {
                    // Zieh erneut, aber nur aus Items dieser Rarity
                    var candidates = merged.drops.FindAll(d => d.rarity == rarity);
                    if (candidates.Count > 0)
                    {
                        var pick = candidates[Random.Range(0, candidates.Count)];
                        loot.Add(pick.itemId);
                        Debug.Log($"[LootRoller] rarityGuarantee erzwungen → {pick.itemId}");
                    }
                    else
                    {
                        Debug.LogWarning($"[LootRoller] Keine Kandidaten für guarantee {rarity} gefunden!");
                        break; // raus, um Endlosschleifen zu verhindern
                    }
                    count++;
                }
            }
        }
    }
}
