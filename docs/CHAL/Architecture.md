# Architecture Map

## 1) Context (C4: System Context)
```mermaid
flowchart LR
  Player[(Player)]
  Unity[Unity Runtime]
  CHAL[CHAL Game]
  Files[(Save Files / JSON / CSV)]
  Player --> Unity --> CHAL
  CHAL <---> Files
```

## 2) High-Level Components

- Core: GameManager, SceneRouter, DebugManager
- Systems: Loot, Wave, Research, Crafting, Save, Inventory, Balance, Skills
- Data: ScriptableObjects + JSON DTOs, Registries, Validation
- UI: UI Toolkit Screens, Docking, HUD, Overlays

## 3) GameFlow (Sequence)
```mermaid
sequenceDiagram
  participant P as Player
  participant MM as MainMenu
  participant MAP as MapSelection
  participant W as WaveManager
  participant R as RewardScreen
  participant H as Hideout
  participant RS as ResearchUI

  P->>MM: Start
  MM->>MAP: Choose Map
  MAP->>W: Load Waves
  W->>W: Combat / Drops / Timer
  W->>R: OnWaveEnd (if no loot pending)
  R->>H: Continue
  H->>RS: Spend Research / Craft / Manage
  RS-->>MAP: Unlocks affect available maps
```

## 4) Events & Services

- Event flows (excerpt):
    - EnemyKilled(EnemyRank, Tags) → ResearchEventBridge → ResearchService.Progress
    - WaveCompleted → RewardService.Show → SaveService.QueueAutosave
    - ItemCrafted(ItemId) → ResearchEventBridge → ResearchService.Progress
- Services:
    - **SaveService** (persists profiles, versioning/migration)
    - **ResearchService** (node state, unlocks, progress formulas)
    - **LootService** (drops, rarity, unlucky-protection)
    - **BalanceManager** (numbers from SO/JSON, deterministic seeds)

## 5) Data Flows
```mermaid
flowchart TB
  SO[ScriptableObjects] --> REG[Registries]
  JSON[JSON DTOs] --> REG
  REG --> SVC[Runtime Services]
  SVC --> UI[UI Views]
  SVC --> SAVE[SaveService]
  SAVE --> FILES[(Files)]
```

## 6) Dependency Rules

- UI knows Services (via controller), not vice versa.
- Services know Registries/DTOs, not UI.
- Debug/logs only via DebugManager (see Conventions).

## 7) Extensibility

- New systems → own Service + Registry + UI Controller.
- New data → define DTO + validation + CSV report schema.
