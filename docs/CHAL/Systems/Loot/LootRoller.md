# CHAL.Systems.Loot.LootRoller

_Automatically generated/updated from `Assets/src/Systems/Loot/LootRoller.cs`._

```csharp
// Documentation for: Assets/src/Systems/Loot/LootRoller.cs
```

1) Purpose
- Defines a LootRoller class responsible for calculating and emitting loot when monsters die and during wave finalization.
- Uses LootRulesService and UnluckyProtection to drive tag-based drops, secret drops, and budget/unlucky adjustments.
- Provides gold and XP calculation helpers for monsters.

2) Public API
- Namespace/Module
  - CHAL.Systems.Loot

- Types
  - public sealed class LootRoller
    - Fields
      - private readonly LootRulesService _rules
        - Loot rule resolution/merging service.
      - private readonly UnluckyProtection _unlucky
        - Tracks unlucky adjustments per drop and per attempt.
    - Constructors
      - public LootRoller(LootRulesService rules,UnluckyProtection unlucky)
        - Initializes internal services.
    - Public methods
      - public List<LootResultEntry> RollLootForMonster(EnemyDef def, EnemyStruct monster, WaveLootContext ctx)
        - Rolls loot for a single monster at death.
        - Returns list of LootResultEntry; side effects include adding entries to ctx.Drops and logs.
      - public void FinalizeWave(WaveLootContext ctx)
        - Wave-end loot enforcement: ensures minimum drops and rarity guarantees; can add entries to ctx.Drops; may adjust ctx.SpentBudget.
      - public int Roll GoldForMonster(EnemyStruct enemy, int maplvl)
        - Computes gold reward using rank-based modifier and map level.
      - public int RollXPForMonster(EnemyStruct enemy, int mapLevel, MapDifficulty difficulty, int waveLevel)
        - Computes XP reward using rank-based base multiplier, difficulty bonus, map level, and wave level.

    - Private methods
      - private void ExecuteDrop(EnemyStruct monster, WaveLootContext ctx, List<LootResultEntry> results, string tag, LootDrop drop, float pBase)
        - Core drop evaluation: applies unlucky multiplier, budget modifier, and random roll.
        - Side effects: may add a LootResultEntry to results and ctx.Drops; updates ctx.SpentBudget; notifies Unlucky; logs.
        - Invoked by RollLootForMonster for each drop/pBase combination (or single pBase if no chancesArray).
    - Notes on behavior
      - Uses effectiveTags derived from monster baseTags and bonusTags (case-insensitive distinct).
      - For each roll, selects a random tag, fetches merged rules for that tag, and processes all drops in that pool.
      - If a drop defines chancesArray, iterates through each pBase; otherwise uses drop.chance (default 0f if absent).
      - Rolls secret drops per monster using _rules.GetSecretDrops(effectiveTags).
      - FinalizeWave interacts with mergedWave from _rules.GetMergedForTags(allTags), and uses mergedWave.drops, mergedWave.minDrops, and mergedWave.rarityGuarantees.

3) Key Behavior & Side Effects
- LootRoller.RollLootForMonster
  - Build effectiveTags = baseTags + monster.bonusTags (distinct, case-insensitive).
  - If effectiveTags null/empty: return empty results.
  - Determine rolls = BalanceManager.Instance.Config.loot.rankMultipliers.GetMultiplier(monster.Rank).
  - For each roll:
    - Pick a random tag from effectiveTags.
    - Get merged rules for that tag via _rules.GetMergedForTags([tag]).
    - For each drop in merged.drops:
      - If drop.chancesArray present: for each element, call ExecuteDrop(monster, ctx, results, tag, drop, pBase).
      - Else: call ExecuteDrop(monster, ctx, results, tag, drop, drop.chance ?? 0f).
    - Secret drops: for each sd in _rules.GetSecretDrops(effectiveTags):
      - Roll 0..100; if roll < sd.chance, add LootResultEntry with EnemyId, PickedTag = sd.sourceTag, ItemId = sd.itemId, quantity = sd.quantity; add to ctx.Drops; log.
- LootRoller.ExecuteDrop
  - Compute multUnlucky = _unlucky.GetMultiplier(drop.rarity).
  - pPre = pBase * multUnlucky.
  - mBudget = LootBudgetModulator.GetModifier(ctx.SpentBudget, drop.lootValue, ctx.TotalBudget, drop.rarity).
  - pEff = clamp(pPre * mBudget, 0f, 100f).
  - Roll, if < pEff: create LootResultEntry; add to results and ctx.Drops; ctx.SpentBudget += drop.lootValue; _unlucky.OnDrop(drop.rarity); log.
  - Else: _unlucky.OnFail(drop.rarity).
- LootRoller.FinalizeWave
  - allTags = ctx.Wave.Monsters.SelectMany(m => m.bonusTags).Distinct().ToArray();
  - mergedWave = _rules.GetMergedForTags(allTags).
  - MinDrops safekeep: while ctx.Drops.Count < mergedWave.minDrops
    - pick = random from mergedWave.drops
    - entry: EnemyId = "WaveBonus", PickedTag = "Failsafe", ItemId = pick.itemId, quantity = pick.quantity
    - ctx.Drops.Add(entry); ctx.SpentBudget += pick.lootValue; _unlucky.OnDrop(pick.rarity); log.
  - RarityGuarantees: for each (rarity, min) in mergedWave.rarityGuarantees
    - count = number of Drops with ItemId having that rarity (via ItemRegistry.Instance.GetRarity)
    - while count < min
      - candidates = mergedWave.drops where d.rarity == rarity
      - if none, break
      - pick = random from candidates
      - entry: EnemyId = "WaveBonus", PickedTag = "Guarantee", ItemId = pick.itemId, quantity = pick.quantity
      - ctx.Drops.Add(entry); log
- LootRoller.RollGoldForMonster
  - Determines baseModifier from rank using BalanceManager.Instance.Config.loot.rankMultipliers
  - Returns Mathf.RoundToInt(curr.baseGoldReward * baseModifier + maplvl * curr.goldPerLevel)
- LootRoller.RollXPForMonster
  - Gets econ and enemyscaling from BalanceManager.Instance.Config
  - baseXp based on enemy.Rank using enemyscaling.{spawn,normal,magic,elite,boss,champion}.xpMultiplier
  - difficultyBonus based on MapDifficulty: Stable=1, Strained=3, Volatile=10, Chaos=50
  - scaled = econ.xp.baseXpReward * baseXp * difficultyBonus + (1f + (mapLevel-1) * econ.xp.xpPerLevel * (waveLevel * 0.1f))
  - Returns Mathf.RoundToInt(scaled)

4) Constraints & Failure Modes
- Guard: effectiveTags is null or empty → RollLootForMonster returns with no results.
- Assumes non-empty mergedWave.drops in FinalizeWave when enforcing minDrops; no explicit guard against an empty drops list.
- Secret drops handling in RollLootForMonster uses per-monster secret drop rules; no global wave secret drops unless invoked via FinalizeWave (commented-out section shows Wave-wide secret rules are currently disabled).
- “TODO: Implement Luck” indicates intended but unimplemented luck-based modifications in this file.
- Uses UnityEngine.Random for RNG; no explicit thread-safety guarantees.
- Writes to ctx.Drops and mutates ctx.SpentBudget and ctx.TotalBudget; side effects affect subsequent rolls within the same wave/context.

5) Example
- Not derived or provided by this file; no standalone usage snippet included.

6) Unknowns
- Exact definitions and contracts of:
  - LootRulesService (GetMergedForTags, GetSecretDrops, data structures of merged drops, minDrops, rarityGuarantees)
  - UnluckyProtection (GetMultiplier, OnDrop, OnFail)
  - WaveLootContext (Drops collection, SpentBudget, TotalBudget, Wave, etc.)
  - EnemyDef, EnemyStruct, EnemyRank, and how baseTags/bonusTags are structured
  - LootDrop (fields: itemId, quantity, rarity, drop.chance, drop.chancesArray, lootValue, etc.)
  - LootResultEntry (fields: EnemyId, PickedTag, ItemId, quantity)
  - Wave and its properties used in FinalizeWave (Monsters, etc.)
  - ItemRegistry.GetRarity(...) and how rarity is defined/compares
  - BalanceManager.Config structures for economy, loot, and rankMultipliers
  - LootBudgetModulator.GetModifier(...) behavior and inputs
  - DebugManager.Log, Debug levels and logging side effects
- Exact behavior for empty or missing data in any of the above, beyond what is explicit in this file.
