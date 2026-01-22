# Assets/src/UI/CodexScreenController.cs

_Automatically generated/updated from `Assets/src/UI/CodexScreenController.cs`._

# Purpose
- Defines the `CodexScreenController` class for managing the UI of the Codex screen in the game.
- Handles UI interactions, updates, and binds to the Codex service.

# Public API
- Namespace: `CHAL.Systems.UI`
- Types
  - `sealed class CodexScreenController : IngameUI`
    - Public fields/properties: None
    - Public methods: None

# Key Behavior & Side Effects
- Initializes UI components and binds events in `Awake()`.
- Refreshes UI on Codex changes via `OnCodexChanged()`.
- Handles button clicks for unlocking slots, activating deeds, and claiming rewards.
- Updates UI elements based on the current state of the Codex.

# Constraints & Failure Modes
- If `root`, `GameManager.Instance`, or `codexService` is null during initialization, the controller is disabled.
- UI updates are contingent on the state of the Codex and may not reflect changes if the Codex is not properly initialized.

# Example
```csharp
// Example usage of CodexScreenController would be within the Unity environment,
// where it is automatically instantiated and managed by the Unity lifecycle.
```

# Unknowns
- The exact behavior of the `CodexService` and its methods is not defined in this file.
- The structure and contents of `DeedVM`, `ChapterVM`, and `GroupVM` are not detailed here.

