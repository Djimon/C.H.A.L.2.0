# CHAL.Systems.Loot.LootRoller

_Automatically generated/updated from `Assets/src/Systems/Loot/LootRoller.cs`._

# Purpose
- Defines the `LootRoller` class responsible for rolling loot and experience for monsters in the game.

# Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - `public sealed class LootRoller`
    - **Public methods:**
      - `public LootRoller(LootRulesService rules, UnluckyProtection unlucky)`
      - `public List<LootResultEntry> RollLootForMonster(EnemyDef def, EnemyStruct monster, WaveLootContext ctx)`
      - `public void FinalizeWave(WaveLootContext ctx)`
      - `public int RollGoldForMonster(EnemyStruct enemy, int maplvl)`
      - `public int RollXPForMonster(EnemyStruct enemy, int mapLevel, MapDifficulty difficulty, int waveLevel)`

# Key Behavior & Side Effects
- `RollLootForMonster`: Rolls loot based on monster's tags and applies unlucky protection. Adds loot entries to the context.
- `FinalizeWave`: Ensures minimum drops and rarity guarantees are met at the end of a wave.
- `RollGoldForMonster`: Calculates gold dropped based on monster rank and map level.
- `RollXPForMonster`: Calculates experience points based on monster rank, map level, difficulty, and wave level.

# Constraints & Failure Modes
- If `effectiveTags` is null or empty in `RollLootForMonster`, an empty list is returned.
- In `FinalizeWave`, if the count of drops is less than `minDrops`, additional drops are added until the condition is met.
- Uses random number generation for loot drops, which may lead to variability in results.

# Example
```csharp
var lootRoller = new LootRoller(rulesService, unluckyProtection);
var lootResults = lootRoller.RollLootForMonster(enemyDef, monsterStruct, waveLootContext);
```

# Unknowns
- The implementation details of `LootRulesService`, `UnluckyProtection`, `LootResultEntry`, `EnemyDef`, `EnemyStruct`, `WaveLootContext`, and other referenced types are not provided in this file.

