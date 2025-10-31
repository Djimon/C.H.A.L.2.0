# CHAL.Systems.Loot.LootRoller

_Automatically generated/updated from `Assets/src/Systems/Loot/LootRoller.cs`._

# LootRoller.cs

## Purpose
- Defines the `LootRoller` class for handling loot generation in a game.
- Provides methods to roll loot for monsters, finalize loot at the end of a wave, and calculate gold and experience rewards.

## Public API
- Namespace: `CHAL.Systems.Loot`
- Types:
  - `public sealed class LootRoller`
    - **Public Fields/Properties**: None
    - **Public Methods**:
      - `public LootRoller(LootRulesService rules, UnluckyProtection unlucky)`
      - `public List<LootResultEntry> RollLootForMonster(EnemyDef def, EnemyStruct monster, WaveLootContext ctx)`
        - Returns a list of loot entries for the specified monster.
      - `public void FinalizeWave(WaveLootContext ctx)`
        - Finalizes loot for the wave, ensuring minimum drops and rarity guarantees.
      - `public int RollGoldForMonster(EnemyStruct enemy, int maplvl)`
        - Returns the amount of gold dropped by the specified enemy.
      - `public int RollXPForMonster(EnemyStruct enemy, int mapLevel, MapDifficulty difficulty, int waveLevel)`
        - Returns the experience points awarded for defeating the specified enemy.

## Key Behavior & Side Effects
- `RollLootForMonster` generates loot based on monster tags and applies rules from `LootRulesService`.
- Uses random number generation to determine loot drops and secret drops.
- `FinalizeWave` ensures that minimum loot drops are met and applies rarity guarantees.
- Updates the `WaveLootContext` with drops and modifies the spent budget.
- Logs drop events and secret drops for debugging purposes.

## Constraints & Failure Modes
- If no effective tags are found for a monster, an empty list is returned from `RollLootForMonster`.
- The method `ExecuteDrop` handles cases where drop chances are null or empty.
- The `FinalizeWave` method ensures that minimum drops are added if not met, but does not reset the unlucky state for guaranteed drops.

## Example
```csharp
var lootRoller = new LootRoller(rulesService, unluckyProtection);
var lootEntries = lootRoller.RollLootForMonster(enemyDef, monsterStruct, waveContext);
```

## Unknowns
- The implementation details of `LootRulesService`, `UnluckyProtection`, and other referenced types are not provided in this file.
- The exact structure of `LootResultEntry`, `EnemyDef`, `EnemyStruct`, and `WaveLootContext` is not defined here.

