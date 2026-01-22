# Assets/src/UI/CodexScreenController.cs

_Automatically generated/updated from `Assets/src/UI/CodexScreenController.cs`._

# Purpose
- Defines the `CodexScreenController` class for managing the Codex UI in the game.
- Handles UI interactions, updates, and binds to the `CodexService`.

# Public API
- Namespace: `CHAL.Systems.UI`
- Types
  - `public sealed class CodexScreenController : IngameUI`
    - Public fields/properties: None
    - Public methods: None

# Key Behavior & Side Effects
- Initializes UI components and binds events in `Awake()`.
- Refreshes UI on codex changes via `OnCodexChanged()`.
- Handles button clicks for activating deeds and claiming rewards.

# Constraints & Failure Modes
- If `root`, `GameManager.Instance`, or `GameManager.codexService` is null during initialization, the controller is disabled.
- UI elements are cleared and rebuilt based on the current state of the codex and selected deeds.

# Example
```csharp
// Example of how to instantiate and use CodexScreenController
CodexScreenController controller = new CodexScreenController();
controller.Awake(); // Initializes the controller and binds UI
```

# Unknowns
- None

