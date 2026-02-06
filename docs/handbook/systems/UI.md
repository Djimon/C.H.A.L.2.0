# UI

## Responsibilities
- UI Toolkit screens and flows.
- Docking and layout for inventories and panels.

## Key Types
- `UIDockingManager`
- `IDockableView`
- `InventoryView`
- `IngameUI`

## Assets
- UXML: `UI/uxml`
- USS: `UI/uss`

## Flow
- Main menu and map selection trigger GameManager transitions.
- Reward screens are shown by MapManager.
- HeroSelectionUI handles team selection before a map wave.
- HeroLoadout has UXML/USS layout assets but no controller script yet.

## References
- `UI/misc/UIDockingManager.cs` (API: [UIDockingManager](../../CHAL/UI/UIDockingManager.md))
- `UI/misc/IDockableView.cs` (API: [IDockableView](../../CHAL/UI/IDockableView.md))
- `UI/InventoryView.cs` (API: [InventoryView](../../CHAL/UI/InventoryView.md))
- `UI/misc/InGameUI.cs` (API: [InGameUI](../../CHAL/UI/InGameUI.md))
- `UI/MainMenuUI.cs` (API: [MainMenuUI](../../CHAL/UI/MainMenuUI.md))
- `UI/MapSelectionIUI.cs` (API: [MapSelectionIUI](../../CHAL/UI/MapSelectionIUI.md))
- `UI/CharacterCreationUI.cs` (API: [CharacterCreationUI](../../CHAL/UI/CharacterCreationUI.md))
- `UI/HeroSelectionUI.cs` (API: [HeroSelectionUI](../../CHAL/UI/HeroSelectionUI.md))
- `UI/uxml/HeroLoadout.uxml`
- `UI/uss/HeroLoadout.uss`
- `UI/WaveRewardUI.cs` (API: [WaveRewardUI](../../CHAL/UI/WaveRewardUI.md))
- `UI/MapRewardUI.cs` (API: [MapRewardUI](../../CHAL/UI/MapRewardUI.md))

## Related
- [Game Loop](../GameLoop.md)
- [Scenes and Boot](../ScenesAndBoot.md)
- [Heroes and Loadouts](Heroes.md)
