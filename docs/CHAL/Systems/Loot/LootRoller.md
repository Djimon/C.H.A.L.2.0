# CHAL.Systems.Loot.LootRoller

_Automatically generated/updated from `Assets/src/Systems/Loot/LootRoller.cs`._

1) Purpose
- Defines a sealed LootRoller class to compute loot outcomes for monsters and waves.
- Uses LootRulesService (rules) and UnluckyProtection (unlucky) to influence drops.
- Provides public loot/gold/xp calculation APIs and wave finalization behavior.

2) Public API
- Namespace: CHAL.Systems.Loot
- Types
  - public sealed LootRoller
    - public LootRoller(LootRulesService rules,UnluckyProtection unlucky)
      - ctor wiring: stores rules and unlucky instances
    - public List<LootResultEntry> RollLootForMonster(EnemyDef def, EnemyStruct monster, WaveLootContext ctx)
      - Returns generated loot entries for a single monster; may modify ctx.Drops and ctx.SpentBudget; may log through DebugManager
    - public void FinalizeWave(WaveLootContext ctx)
      - Enforces minimum drops, rarity guarantees for the wave; may modify ctx.Drops, ctx.SpentBudget; logs mid/low-level events
    - public int RollGoldForMonster(EnemyStruct enemy, int maplvl)
      - Computes gold reward based on rank modifiers and map level
    - public int RollXPForMonster(EnemyStruct enemy, int mapLevel, MapDifficulty difficulty, int waveLevel)
      - Computes XP reward based on rank scaling, difficulty, map level, and wave level

3) Key Behavior & Side Effects
- RollLootForMonster(EnemyDef def, EnemyStruct monster, WaveLootContext ctx)
  - Builds effectiveTags from def.baseTags plus monster.bonusTags (case-insensitive distinct)
  - Early return if effectiveTags is null or empty
  - Determines roll count from BalanceManager.Instance.Config.loot.rankMultipliers.GetMultiplier(monster.Rank)
  - For each roll:
    - Picks a random tag from effectiveTags
    - Loads merged drop rules via _rules.GetMergedForTags(new[] { tag })
    - For each drop in merged.drops:
      - If drop.chancesArray present and non-empty:
        - For each pBase in drop.chancesArray: ExecuteDrop(monster, ctx, results, tag, drop, pBase)
      - Else:
        - Use pBase = drop.chance ?? 0f; ExecuteDrop(monster, ctx, results, tag, drop, pBase)
    - Loads secret drops via _rules.GetSecretDrops(effectiveTags)
    - For each sd:
      - Roll 0–100; if roll < sd.chance, create LootResultEntry with EnemyId = monster.EnemyId, PickedTag = sd.sourceTag, ItemId = sd.itemId, quantity = sd.quantity; add to results and ctx.Drops; log
- private void ExecuteDrop(EnemyStruct monster, WaveLootContext ctx, List<LootResultEntry> results, string tag, LootDrop drop, float pBase)
  - Computes multUnlucky from _unlucky.GetMultiplier(drop.rarity)
  - pPre = pBase * multUnlucky
  - mBudget = LootBudgetModulator.GetModifier(ctx.SpentBudget, drop.lootValue, ctx.TotalBudget, drop.rarity)
  - pEff = clamp(pPre * mBudget, 0f, 100f)
  - Roll 0–100; if roll < pEff:
    - Create LootResultEntry with EnemyId = monster.EnemyId, PickedTag = tag, ItemId = drop.itemId, quantity = drop.quantity
    - Add to results and ctx.Drops
    - ctx.SpentBudget += drop.lootValue
    - _unlucky.OnDrop(drop.rarity)
    - Debug log
  - else:
    - _unlucky.OnFail(drop.rarity)
- FinalizeWave(WaveLootContext ctx)
  - allTags = ctx.Wave.Monsters.SelectMany(m => m.bonusTags).Distinct().ToArray()
  - mergedWave = _rules.GetMergedForTags(allTags)
  - MinDrops Failsafe:
    - While ctx.Drops.Count < mergedWave.minDrops:
      - pick = random from mergedWave.drops
      - entry: EnemyId = "WaveBonus", PickedTag = "Failsafe", ItemId = pick.itemId, quantity = pick.quantity
      - ctx.Drops.Add(entry); ctx.SpentBudget += pick.lootValue; _unlucky.OnDrop(pick.rarity)
      - Debug log
  - RarityGuarantees:
    - For each (rarity, min) in mergedWave.rarityGuarantees:
      - count = number of ctx.Drops with ItemId whose rarity matches rarity (via ItemRegistry.Instance.GetRarity)
      - While count < min:
        - candidates = mergedWave.drops.FindAll(d => d.rarity == rarity)
        - If candidates is empty, break
        - pick = random from candidates
        - entry: EnemyId = "WaveBonus", PickedTag = "Guarantee", ItemId = pick.itemId, quantity = pick.quantity
        - ctx.Drops.Add(entry)
        - Debug log
        - count++
  - SecretRules (Wave-wide) currently commented out; no active effect in this method
- RollGoldForMonster(EnemyStruct enemy, int maplvl)
  - Determines baseModifier by rank using a switch on enemy.Rank
  - Returns RoundToInt(curr.baseGoldReward * baseModifier + maplvl * curr.goldPerLevel)
- RollXPForMonster(EnemyStruct enemy, int mapLevel, MapDifficulty difficulty, int waveLevel)
  - Reads econ and enemyscaling from BalanceManager.Config
  - baseXp determined by rank via enemyscaling.*.xpMultiplier
  - difficultyBonus mapped from MapDifficulty
  - scaled = econ.xp.baseXpReward * baseXp * difficultyBonus + (1f + (mapLevel-1) * econ.xp.xpPerLevel * (waveLevel * 0.1f))
  - Returns RoundToInt(scaled)

4) Constraints & Failure Modes
- Null/empty handling
  - If effectiveTags is empty, RollLootForMonster returns without adding loot
  - drop.chancesArray may be null or empty; code guards accordingly
- Dependency assumptions
  - _rules.GetMergedForTags and _rules.GetSecretDrops must provide non-null merged and secret drop lists; no explicit null checks after call
- Potential edge cases
  - If mergedWave.drops is empty, MinDrops and other loops could fail or throw
  - If drop.chance and drop.lootValue are null/missing, behavior relies on defaults (e.g., pBase default 0f)
- Side effects
  - Creates and appends LootResultEntry items to both local results and ctx.Drops
  - Modifies ctx.SpentBudget
  - Triggers _unlucky.OnDrop or _unlucky.OnFail
  - Logs via DebugManager
- Concurrency/threading
  - Not addressed; code assumes single-threaded Unity context
- Performance
  - Nested loops over rolls, drops, and chances; complexity scales with configured rules and tags

5) Example
- Not derivable from the file; no self-contained usage example present.

6) Unknowns
- Definitions and behavior of:
  - LootRulesService, UnluckyProtection, WaveLootContext, EnemyDef, EnemyStruct, LootDrop, LootResultEntry
  - BalanceManager, LootBudgetModulator, ItemRegistry, DebugManager
- Exact constraints of mergedWave and mergedWave.drops (structure, required fields)
- Details of how bonusTags/baseTags are populated and how rarity, lootValue, and quantity semantics are defined
- Any external side effects of DebugManager logging beyond message emission
