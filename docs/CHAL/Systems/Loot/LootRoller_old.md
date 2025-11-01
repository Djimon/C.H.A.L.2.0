# CHAL.Systems.Loot.LootRoller_old

_Automatically generated/updated from `Assets/src/Systems/Loot/LootRoller_old.cs`._

```text
1) Purpose
- Defines a class LootRoller_old that orchestrates loot generation for a WaveComposition, using LootRulesService and UnluckyProtection.
- Performs normal drops per monster (when bonus tags exist), secret rule drops, and post-processing (min drops and guaranteed rarities).
- Encapsulates helper logic for post-processing and (currently unused) smart trimming.

2) Public API
- Namespace/module: CHAL.Systems.Loot

- Type: public sealed class LootRoller_old
  - Public LootRoller_old(LootRulesService rules, UnluckyProtection unlucky)
  - Public List<LootResultEntry> RollLoot(WaveComposition wave)

Note: Private members are not part of the public API surface.

3) Key Behavior & Side Effects
- RollLoot workflow:
  - Compute initial budget B via LootBudgetCalculator.CalculateBudget using wave totals and level/difficulty; initialize U = 0.
  - For each monster in wave.Monsters:
    - For each instance (i < monster.Count):
      - If monster.bonusTags is null or empty, skip to next instance.
      - Pick a random bonus tag from monster.bonusTags.
      - Load rule-set for that tag: merged = _rules.GetMergedForTags(new[] { tag }).
      - For each drop in merged.drops:
        - pBase = drop.chance ?? 0f; if drop.chancesArray present and non-empty, pBase = random from that array.
        - multUnlucky = _unlucky.GetMultiplier(drop.rarity).
        - pPre = pBase * multUnlucky.
        - mBudget = LootBudgetModulator.GetModifier(U, drop.lootValue, B, drop.rarity).
        - pEff = clamp(pPre * mBudget, 0f, 100f).
        - Roll roll ∈ [0, 100). If roll < pEff:
          - Add LootResultEntry { EnemyId = monster.EnemyId, PickedTag = tag, ItemId = drop.itemId } to finalLoot.
          - U += drop.lootValue.
          - Debug/log and call _unlucky.OnDrop(drop.rarity).
        - Else:
          - _unlucky.OnFail(drop.rarity).
      - Secret rules for this monster:
        - var secretDrops = _rules.GetSecretDrops(monster.bonusTags).
        - For each sd in secretDrops: roll; if roll < sd.chance, add LootResultEntry { EnemyId = monster.EnemyId, PickedTag = string.Join(",", monster.bonusTags), ItemId = sd.itemId }; log.
  - Post-processing:
    - mergedWave = _rules.GetMergedForWave(wave).
    - ApplyPostProcessing(mergedWave, finalLoot).
- ApplyPostProcessing behavior:
  - minDrops: While loot.Count < merged.minDrops, pick a random drop from merged.drops and append a LootResultEntry with EnemyId = "PostProcess:minDrops", PickedTag = pick.sourceTag, ItemId = pick.itemId; log.
  - Optional maxDrops / trimming: SmartTrim is defined but not invoked (commented reference present).
  - rarityGuarantees: For each (rarity, min) in merged.rarityGuarantees:
    - Count current drops of that rarity in loot.
    - While count < min:
      - candidates = merged.drops with d.rarity == rarity; if none, break.
      - pick a random candidate; add LootResultEntry { EnemyId = "PostProcess:Guarantees", PickedTag = pick.sourceTag, ItemId = pick.itemId }; log; count++.

- SmartTrim behavior (defined but not active in RollLoot):
  - If merged.maxDrops > 0 and loot.Count > merged.maxDrops:
    - Build a weighted list of current loot entries based on rarity-specific trim weights (balance.loot.trim.*).
    - If weightedList non-empty, remove a randomly selected entry from weightedList; log.
    - Otherwise, remove a random entry uniformly as fallback.

- Side effects:
  - Random number generation via UnityEngine.Random.
  - Modifies internal budget U and logs several events via DebugManager.Log.
  - Calls _unlucky.OnDrop, _unlucky.OnFail, and may call _unlucky.GetMultiplier (purely read/compute).
  - Potentially adds entries with special EnemyId strings (PostProcess:minDrops, PostProcess:Guarantees).

4) Constraints & Failure Modes
- Guard conditions:
  - Drops only processed when monster.bonusTags is non-null and non-empty.
- Potential pitfalls:
  - If merged.drops is empty and minDrops > current loot size, ApplyPostProcessing minDrops loop could access merged.drops[Random.Range(0, merged.drops.Count)] and fail.
  - SmartTrim is defined but not invoked; maxDrops behavior is not active in this implementation.
  - Random-based selections rely on UnityEngine.Random; behavior can vary between runs.
- Performance considerations:
  - Nested loops over wave monsters, per-monster drops, and per-drop logic; could be heavy for large waves.
- Nullability/assumptions:
  - Requires external services (LootRulesService, UnluckyProtection) to be provided in constructor.
  - Accesses external registries (ItemRegistry) and DebugManager; assumes these are initialized.
- Threading:
  - All logic executes synchronously on the caller’s thread; no explicit synchronization or async handling.

6) Unknowns
- Exact structure/definition of:
  - LootResultEntry (beyond fields used here: EnemyId, PickedTag, ItemId).
  - MergedLoot (fields used: minDrops, drops, rarityGuarantees; drop structure includes sourceTag, itemId, rarity, chance, chancesArray, lootValue).
  - Drop type fields: itemId, rarity, chance, chancesArray, lootValue, sourceTag.
  - WaveComposition and Monster types (TotalSpawns, TotalNormals, TotalMagics, TotalElites, TotalBosses, TotalChampions, Level, Difficulty, Monsters; Monster.Count, Monster.bonusTags, Monster.EnemyId).
  - Behavior/contents of LootRulesService.GetMergedForTags, GetSecretDrops, GetMergedForWave.
  - LootBudgetCalculator and LootBudgetModulator logic and their interaction with B, U, and drop.lootValue.
  - Debug levels and exact formatting of DebugManager.Log calls.
- Whether any of the external calls can throw exceptions or rely on specific invariants.
```
