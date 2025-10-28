# Assets/src/Systems/Loot/LootRoller.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `LootRoller` class for managing loot generation in a game.
- Provides methods for rolling loot and experience points (XP) for monsters.

## Public API
- Namespace: `CHAL.Systems.Loot`
- Types
  - `public sealed class LootRoller`
    - **Public methods:**
      - `public LootRoller(LootRulesService rules, UnluckyProtection unlucky)`
      - `public List<LootResultEntry> RollLootForMonster(EnemyDef def, EnemyStruct monster, WaveLootContext ctx) : List<LootResultEntry>`
      - `public void FinalizeWave(WaveLootContext ctx)`
      - `public int RollGoldForMonster(EnemyStruct enemy, int maplvl) : int`
      - `public int RollXPForMonster(EnemyStruct enemy, int mapLevel, MapDifficulty difficulty, int waveLevel) : int`

## Key Behavior & Side Effects
- `RollLootForMonster`: Generates loot based on monster attributes and context, utilizing random rolls and defined rules.
- `FinalizeWave`: Ensures minimum drops and rarity guarantees at the end of a wave.
- `RollGoldForMonster`: Calculates gold rewards based on enemy rank and map level.
- `RollXPForMonster`: Calculates XP rewards based on enemy rank, map level, difficulty, and wave level.

## Constraints & Failure Modes
- Handles cases where effective tags are null or empty by returning an empty result list.
- Uses random number generation for loot drops, which may lead to variability in results.
- Ensures that the number of drops meets minimum requirements through a failsafe mechanism.

## Example
```csharp
var lootRoller = new LootRoller(rulesService, unluckyProtection);
var lootResults = lootRoller.RollLootForMonster(enemyDef, monsterStruct, waveLootContext);
```

## Unknowns
- The implementation details of `LootRulesService`, `UnluckyProtection`, `LootResultEntry`, `EnemyDef`, `EnemyStruct`, `WaveLootContext`, and other referenced types are not provided in this file.
```
