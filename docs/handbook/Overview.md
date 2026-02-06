# Overview

C.H.A.L. is a data-driven auto-battler where runtime behavior is built from Resources (ScriptableObjects, TextAssets) and wired by a small set of core services. The primary orchestrator is `Core/GameManager.cs`, which owns the profile, inventory domain, stats service, codex wiring, and scene transitions.

## Core Ideas
- Resource-first configuration for balance and content.
- Explicit system boundaries with a small, visible set of runtime owners.
- Persistence via snapshots (inventory, codex, stats) separate from profile JSON.

## Entry Scenes
- `01_MainMenu` for the main menu and profile entry.
- `03_Hideout` for management and progression.
- `04_Map` for combat waves and rewards.

## References
- `Core/GameManager.cs` (API: [GameManager](../CHAL/Core/GameManager.md))
- `Core/SaveSystem.cs` (API: [SaveSystem](../CHAL/Core/SaveSystem.md))
- `Core/BalanceManager.cs` (API: [BalanceManager](../CHAL/Core/BalanceManager.md))

## Related
- [Game Loop](GameLoop.md)
- [System Map](SystemMap.md)
- [Scenes and Boot](ScenesAndBoot.md)
