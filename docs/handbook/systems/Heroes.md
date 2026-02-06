# Heroes and Loadouts

## Responsibilities
- Manage the hero roster and per-hero progression data.
- Select a team for each map run.
- Spawn runtime hero instances for combat.
- Maintain hero gear and socket inventories.

## Data Model
- `HeroDef` defines base stats, prefab, and skill defaults.
- `HeroCatalog` provides the ordered list of all heroes for UI.
- `PlayerProfile` stores `UnlockedHeroes` and `HeroesData` (per-hero progress).
- `HeroProgressData` persists level, XP, orbit points, and unlocked sockets.

## Team Selection (Map Entry)
- `HeroSelectionUI` builds a selectable roster from `PlayerProfile.GetUnlockedHeroes()`.
- Slot count is driven by `MapDef.heroSlots`.
- On start, the UI passes the selected hero IDs to `MapManager.SetSelectedHeroes()`.
- `MapManager.StartWave()` spawns hero prefabs at map spawn points.

## Runtime Hero Instances
- `HeroController` owns combat behavior and skill rotation.
- `HeroInstance` holds stats, HP, modifiers, XP, and socket unlock counts.
- `MapManager` passes `HeroProgressData` into `HeroController.Init()` so progress is applied.

## Skill Binding (Hero -> SkillInstance)
- `HeroController.RebuildSocketedSkills()` builds the runtime skill list for the hero.
- It always adds a fallback attack from `HeroDef.fallBackAttack` if present.
- For each module item in `debugSocketSkills`, it resolves the `SkillModuleDef` via `SkillRegistry` and creates a `SkillInstance`.
- Each `SkillInstance` is then used by the combat system (`SkillExecutor`, `CombatCalculator`, `DamageImpact`).

## Current Gap (Loadout -> Skills)
- Hero socket inventories (`hero:{id}:sockets`) exist and can be manipulated via `HeroLoadoutService`.
- However, `HeroController` currently reads from `debugSocketSkills`, not from socket inventory instances.
- There is no live bridge yet that turns socket inventory contents into `SkillInstance` objects for combat.

**Temporary Note**
- Current skill binding uses `debugSocketSkills` as a test-time bridge until a loadout UI exists.
- This is an intentional workaround for fast testing and does not reflect final loadout wiring.


## Loadout Inventories
- Hero loadouts are stored as dedicated inventory instances:
- Gear: `hero:{HeroId}:gear`
- Sockets: `hero:{HeroId}:sockets`
- `GameManager.EnsureHeroLoadoutInventoriesFromProfile()` creates these for unlocked heroes.
- `RepairHeroSocketOverflowToPlayerGear()` moves modules from locked sockets back to player gear.

## Equip and Socket Operations
- `HeroLoadoutService.TryEquipGear` and `TryUnequipGear` move gear between player inventory and hero gear slots.
- `HeroLoadoutService.TrySocketModule` and `TryUnsocketModule` move modules between player inventory and hero sockets.
- Replace cases return the existing item to the player inventory if space allows.

## Progression and XP
- `MapManager.GrantHeroXpForWave()` distributes wave XP across selected heroes.
- A temporary `HeroInstance` is used to apply XP and write back into `HeroProgressData`.
- Orbit points and socket unlock counts live on `HeroProgressData` and `HeroInstance`.

## UI Notes
- `HeroSelectionUI` is the live UI for team selection.
- `HeroLoadout.uxml` and `HeroLoadout.uss` exist, but there is no controller script yet.

## Known Limitations
- Orbit system is not implemented beyond points storage.
- Socket unlock thresholds are TODO and currently minimal.
- Socketed skills are not yet sourced from hero socket inventories.

## References
- `Data/Defs/HeroDef.cs` (API: [HeroDef](../../CHAL/Data/HeroDef.md))
- `Systems/Heroes/HeroCatalog.cs` (API: [HeroCatalog](../../CHAL/Data/HeroCatalog.md))
- `Core/PlayerProfile.cs` (API: [PlayerProfile](../../CHAL/Data/PlayerProfile.md))
- `Systems/Heroes/HeroProgressData.cs` (API: [HeroProgressData](../../global/HeroProgressData.md))
- `Systems/Heroes/HeroController.cs` (API: [HeroController](../../CHAL/Systems/Hero/HeroController.md))
- `Systems/Heroes/HeroInstance.cs` (API: [HeroInstance](../../CHAL/Systems/Hero/HeroInstance.md))
- `Systems/Heroes/HeroLoadoutService.cs` (API: [HeroLoadoutService](../../CHAL/Systems/Hero/HeroLoadoutService.md))
- `Systems/Skills/SkillInstance.cs` (API: [SkillInstance](../../CHAL/Systems/Skill/SkillInstance.md))
- `Systems/Skills/SkillExecuter.cs` (API: [SkillExecuter](../../CHAL/Systems/Skill/SkillExecuter.md))
- `Systems/Map/MapManager.cs` (API: [MapManager](../../CHAL/Systems/Map/MapManager.md))
- `UI/HeroSelectionUI.cs` (API: [HeroSelectionUI](../../CHAL/UI/HeroSelectionUI.md))
- `UI/uxml/HeroLoadout.uxml`
- `UI/uss/HeroLoadout.uss`

## Related
- [Map and Waves](MapWave.md)
- [Inventory](Inventory.md)
- [Skills and Combat](SkillsCombat.md)
- [UI](UI.md)
