# C.H.A.L. — Overview & Getting Started

**Goal**  
Auto-battler/ARPG hybrid with data-driven architecture (SO/JSON), research gating, modular loot & crafting, UI Toolkit, and a custom debug/logging policy (see 00_REGELN).

**Key Themes**
- Gameplay loop: MainMenu → Map → Waves → Rewards → Hideout → Research/Forge → Map
- Data flow: ScriptableObjects & JSON DTOs → Registries → Runtime Services
- Strict logging rules via DebugManager (no UnityEngine.Debug.*)

## Repo Structure (Short)
```text
Assets
└─ src
   ├─ Core
   ├─ Systems
   │  ├─ Loot
   │  ├─ Wave
   │  ├─ Research
   │  ├─ Crafting
   │  ├─ Save
   │  ├─ Inventory
   │  ├─ Balance
   │  └─ UI
   ├─ Data
   │  ├─ Items
   │  ├─ Recipes
   │  ├─ Research
   │  └─ Maps
   └─ Resources
      ├─ Registries
      ├─ Themes
      └─ Localization
UI Toolkit
├─ UXML
└─ USS
Docs
```

## Build & Start (Unity 6)
1. Open Unity 6 → start `Scenes/MainMenu.unity`.  
2. Debug shortcuts:  
   - `Ctrl+Alt+D`: Debug Overlay  
   - `F8`: Force Save Profile  
   - `Shift+R`: Open Research UI  
3. The first profile is created on first start; see `Docs/SaveSystem.md` for storage location.

## Designer Entry Points
- **Items**: `Docs/Designer/Items.md`
- **Recipes**: `Docs/Designer/Recipes.md`
- **Research**: `Docs/Designer/ResearchTree.md`
- **Waves/Maps**: `Docs/Designer/MapsAndWaves.md`

## Other Key Documents
- Architecture map: `Docs/Architecture.md`
- Lifecycle/Stateflow: `Docs/Lifecycle.md`
- Logging/Errors/Performance/Tests: `Docs/Conventions.md`
- Save/Load + Migration + Crypto: `Docs/SaveSystem.md`
- ItemRegistry reports (CSV): `Docs/Validation.md`
- UI flows & docking: `Docs/UI-Flow.md`

## Manual Docs
- Handbook entry: `docs/handbook/README.md`
