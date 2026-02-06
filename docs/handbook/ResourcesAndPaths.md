# Resources and Paths

## Layout
- `Resources/config`: `GameBalanceConfig`, `GameSaveConfig`, `DebugConfig`, `RuneForgeConfig`.
- `Resources/data`: archetypes, codex, enemies, heroes, inventory, items, loot rules, map (singular), recipes, skills.

## Key Loads
- `config/GameBalanceConfig`
- `config/GameSaveConfig`
- `data/Codex` and `data/Codex/Deeds`
- `data/Items`
- `data/MonsterTags`
- `data/LootRules` and `data/LootComboRules`

## Known Mismatches
- Maps are loaded from `data/Maps` in code, but the folder is `Resources/data/Map` (singular).
- CheatMenu loads `data/Research/*`, but Resources uses `data/Codex/*`.

## Guidance
- Treat resource paths as case-sensitive to avoid platform-specific issues.

## References
- `Core/GameManager.cs` (API: [GameManager](../CHAL/Core/GameManager.md))
- `Core/BalanceManager.cs` (API: [BalanceManager](../CHAL/Core/BalanceManager.md))
- `Core/SaveSystem.cs` (API: [SaveSystem](../CHAL/Core/SaveSystem.md))
- `Systems/Loot/LootRulesService.cs` (API: [LootRulesService](../CHAL/Systems/Loot/LootRulesService.md))
- `UI/CheatMenuController.cs` (API: [CheatMenuController](../CHAL/UI/CheatMenuController.md))

## Related
- [Data Pipeline](DataPipeline.md)
