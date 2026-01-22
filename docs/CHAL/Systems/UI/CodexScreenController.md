# Assets/src/UI/CodexScreenController.cs

_Automatically generated/updated from `Assets/src/UI/CodexScreenController.cs`._

# Purpose
- Defines the `CodexScreenController` class for managing the UI of the Codex screen in the game.
- Handles user interactions, updates UI elements based on the state of the Codex, and communicates with the `CodexService`.

# Public API
- Namespace: `CHAL.Systems.UI`
- Types
  - `public sealed class CodexScreenController : IngameUI`
    - Public fields/properties: None
    - Public methods:
      - `protected override void Awake()`
      - `private void OnDestroy()`
      - `private void OnActivateClicked()`
      - `private void OnClaimClicked()`

# Key Behavior & Side Effects
- Initializes UI components and binds events in `Awake()`.
- Refreshes UI elements when the Codex state changes via `OnCodexChanged()`.
- Updates the UI based on user interactions with buttons and slots.
- Claims deeds and activates focus slots based on user actions.

# Constraints & Failure Modes
- If `root`, `GameManager.Instance`, or `GameManager.codexService` is null during initialization, the controller disables itself.
- UI elements are cleared and updated based on the current state of the Codex and user selections.
- Handles potential null references when accessing UI elements and Codex data.

# Example
```csharp
var codexScreenController = new CodexScreenController();
codexScreenController.Awake();
```

# Unknowns
- The exact structure and behavior of `DeedVM`, `ChapterVM`, and `GroupVM` are not defined in this file.
- The implementation details of `CodexService` and its methods are not provided.

