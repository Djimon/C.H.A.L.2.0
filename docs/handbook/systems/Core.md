# Core

## Responsibilities
- Bootstrap global state and singletons.
- Load and save profile, inventory, stats, and codex snapshots.
- Own scene transitions and game state.

## Key Runtime State
- `GameManager.Profile` (PlayerProfile)
- `GameManager.Inventory` (InventoryDomain)
- `GameManager.Stats` (StatisticsService)
- `GameManager.codexService` and unlock wiring

## Entry Points
- `StartNewGame(profile)`
- `ContinueGame()`
- `StartMap(sceneName, mapDef)`
- `SaveGame()`

## Persistence
- `SaveSystem.Save(Profile)` writes profile JSON and inventory snapshots.
- `SaveSystem.SaveCodex` and `SaveSystem.SaveStatistics` persist codex and stats.

## Resources
- `config/GameBalanceConfig`
- `config/GameSaveConfig`
- `data/Codex` and `data/Codex/Deeds`

## References
- `Core/GameManager.cs` (API: [GameManager](../../CHAL/Core/GameManager.md))
- `Core/SaveSystem.cs` (API: [SaveSystem](../../CHAL/Core/SaveSystem.md))
- `Core/BalanceManager.cs` (API: [BalanceManager](../../CHAL/Core/BalanceManager.md))
- `Core/PlayerProfile.cs` (API: [PlayerProfile](../../CHAL/Data/PlayerProfile.md))

## Related
- [Scenes and Boot](../ScenesAndBoot.md)
- [Save and Load](../SaveLoad.md)
- [Data Pipeline](../DataPipeline.md)
